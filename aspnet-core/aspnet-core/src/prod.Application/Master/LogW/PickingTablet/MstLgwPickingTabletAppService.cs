using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Dapper;
using Microsoft.EntityFrameworkCore;
using NUglify.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Master.LogW.Dto;
using prod.Master.LogW.Exporting;
using prod.Master.LogW.PickingTablet;

namespace prod.Master.LogW
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_LogW_PickingTablet)]
    public class MstLgwPickingTabletAppService : prodAppServiceBase, IMstLgwPickingTabletAppService
    {
        private readonly IDapperRepository<MstLgwPickingTablet, long> _dapperRepo;
        private readonly IRepository<MstLgwPickingTablet, long> _repo;
        private readonly IMstLgwPickingTabletExcelExporter _calendarListExcelExporter;

        public MstLgwPickingTabletAppService(IRepository<MstLgwPickingTablet, long> repo,
                                         IDapperRepository<MstLgwPickingTablet, long> dapperRepo,
                                        IMstLgwPickingTabletExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogW_PickingTablet_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstLgwPickingTabletDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstLgwPickingTabletDto input)
        {
            var mainObj = ObjectMapper.Map<MstLgwPickingTablet>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstLgwPickingTabletDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogW_PickingTablet_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstLgwPickingTabletDto>> GetAll(GetMstLgwPickingTabletInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.PickingTabletId), e => e.PickingTabletId.Contains(input.PickingTabletId))
                .WhereIf(!string.IsNullOrWhiteSpace(input.DeviceIp), e => e.DeviceIp.Contains(input.DeviceIp))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ScanType), e => e.ScanType.Contains(input.ScanType))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstLgwPickingTabletDto
                         {
                             Id = o.Id,
                             PickingTabletId = o.PickingTabletId,
                             DeviceIp = o.DeviceIp,
                             ScanType = o.ScanType,
                             ScanName = o.ScanName,
                             CurrentAction = o.CurrentAction,
                             LotNo = o.LotNo,
                             UpTable = o.UpTable,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstLgwPickingTabletDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetPickingTabletToExcel(MstLgwPickingTabletExportInput input)
        {
            var filtered = _repo.GetAll()
                  .WhereIf(!string.IsNullOrWhiteSpace(input.PickingTabletId), e => e.PickingTabletId.Contains(input.PickingTabletId))
                  .WhereIf(!string.IsNullOrWhiteSpace(input.DeviceIp), e => e.DeviceIp.Contains(input.DeviceIp))
                  .WhereIf(!string.IsNullOrWhiteSpace(input.ScanType), e => e.ScanType.Contains(input.ScanType))
                  ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var query = from o in pageAndFiltered
                         select new MstLgwPickingTabletDto
                        {
                            Id = o.Id,
                            PickingTabletId = o.PickingTabletId,
                            DeviceIp = o.DeviceIp,
                            ScanType = o.ScanType,
                            ScanName = o.ScanName,
                            CurrentAction = o.CurrentAction,
                            LotNo = o.LotNo,
                            UpTable = o.UpTable,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstLgwPickingTabletConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
