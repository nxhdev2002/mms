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

namespace prod.Master.LogW
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_LogW_LayoutSetup)]
    public class MstLgwLayoutSetupAppService : prodAppServiceBase, IMstLgwLayoutSetupAppService
    {
        private readonly IDapperRepository<MstLgwLayoutSetup, long> _dapperRepo;
        private readonly IRepository<MstLgwLayoutSetup, long> _repo;
        private readonly IMstLgwLayoutSetupExcelExporter _calendarListExcelExporter;

        public MstLgwLayoutSetupAppService(IRepository<MstLgwLayoutSetup, long> repo,
                                         IDapperRepository<MstLgwLayoutSetup, long> dapperRepo,
                                        IMstLgwLayoutSetupExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogW_LayoutSetup_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstLgwLayoutSetupDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstLgwLayoutSetupDto input)
        {
            var mainObj = ObjectMapper.Map<MstLgwLayoutSetup>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstLgwLayoutSetupDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogW_LayoutSetup_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstLgwLayoutSetupDto>> GetAll(GetMstLgwLayoutSetupInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Zone), e => e.Zone.Contains(input.Zone))
                .WhereIf(input.SubScreenNo.HasValue , t => t.SubScreenNo == input.SubScreenNo)
                .WhereIf(!string.IsNullOrWhiteSpace(input.CellType), e => e.CellType.Contains(input.CellType))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstLgwLayoutSetupDto
                         {
                             Id = o.Id,
                             Zone = o.Zone,
                             SubScreenNo = o.SubScreenNo,
                             ScreenArea = o.ScreenArea,
                             CellName = o.CellName,
                             CellType = o.CellType,
                             NumRow = o.NumRow,
                             ColumnName = o.ColumnName,
                             IsDisabled = o.IsDisabled,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstLgwLayoutSetupDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetLayoutSetupToExcel(MstLgwLayoutSetupExportInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Zone), e => e.Zone.Contains(input.Zone))
                .WhereIf(input.SubScreenNo.HasValue, t => t.SubScreenNo == input.SubScreenNo)
                .WhereIf(!string.IsNullOrWhiteSpace(input.CellType), e => e.CellType.Contains(input.CellType))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var query = from o in pageAndFiltered
                         select new MstLgwLayoutSetupDto
                        {
                            Id = o.Id,
                            Zone = o.Zone,
                            SubScreenNo = o.SubScreenNo,
                            ScreenArea = o.ScreenArea,
                            CellName = o.CellName,
                            CellType = o.CellType,
                            NumRow = o.NumRow,
                            ColumnName = o.ColumnName,
                            IsDisabled = o.IsDisabled,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstLgwLayoutSetupConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
