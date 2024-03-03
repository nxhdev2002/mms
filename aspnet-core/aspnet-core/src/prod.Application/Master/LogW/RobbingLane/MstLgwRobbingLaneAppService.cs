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
    //  [AbpAuthorize(AppPermissions.Pages_Master_LogW_RobbingLane)]
    public class MstLgwRobbingLaneAppService : prodAppServiceBase, IMstLgwRobbingLaneAppService
    {
        private readonly IDapperRepository<MstLgwRobbingLane, long> _dapperRepo;
        private readonly IRepository<MstLgwRobbingLane, long> _repo;
        private readonly IMstLgwRobbingLaneExcelExporter _calendarListExcelExporter;

        public MstLgwRobbingLaneAppService(IRepository<MstLgwRobbingLane, long> repo,
                                         IDapperRepository<MstLgwRobbingLane, long> dapperRepo,
                                        IMstLgwRobbingLaneExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogW_RobbingLane_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstLgwRobbingLaneDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstLgwRobbingLaneDto input)
        {
            var mainObj = ObjectMapper.Map<MstLgwRobbingLane>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstLgwRobbingLaneDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogW_RobbingLane_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstLgwRobbingLaneDto>> GetAll(GetMstLgwRobbingLaneInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.LaneNo), e => e.LaneNo.Contains(input.LaneNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.LaneName), e => e.LaneName.Contains(input.LaneName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ContNo), e => e.ContNo.Contains(input.ContNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Renban), e => e.Renban.Contains(input.Renban))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierNo), e => e.SupplierNo.Contains(input.SupplierNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ShowOnly), e => e.ShowOnly.Contains(input.ShowOnly))
                .WhereIf(!string.IsNullOrWhiteSpace(input.IsDisabled), e => e.IsDisabled.Contains(input.IsDisabled))
                .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstLgwRobbingLaneDto
                         {
                             Id = o.Id,
                             LaneNo = o.LaneNo,
                             LaneName = o.LaneName,
                             ContNo = o.ContNo,
                             Renban = o.Renban,
                             SupplierNo = o.SupplierNo,
                             ShowOnly = o.ShowOnly,
                             IsDisabled = o.IsDisabled,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstLgwRobbingLaneDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetRobbingLaneToExcel(MstLgwRobbingLaneExportInput input)
        {
            var filtered = _repo.GetAll()
              .WhereIf(!string.IsNullOrWhiteSpace(input.LaneNo), e => e.LaneNo.Contains(input.LaneNo))
              .WhereIf(!string.IsNullOrWhiteSpace(input.LaneName), e => e.LaneName.Contains(input.LaneName))
              .WhereIf(!string.IsNullOrWhiteSpace(input.ContNo), e => e.ContNo.Contains(input.ContNo))
              .WhereIf(!string.IsNullOrWhiteSpace(input.Renban), e => e.Renban.Contains(input.Renban))
              .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierNo), e => e.SupplierNo.Contains(input.SupplierNo))
              .WhereIf(!string.IsNullOrWhiteSpace(input.ShowOnly), e => e.ShowOnly.Contains(input.ShowOnly))
              .WhereIf(!string.IsNullOrWhiteSpace(input.IsDisabled), e => e.IsDisabled.Contains(input.IsDisabled))
              .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive))
              ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var query = from o in pageAndFiltered
                         select new MstLgwRobbingLaneDto
                        {
                            Id = o.Id,
                            LaneNo = o.LaneNo,
                            LaneName = o.LaneName,
                            ContNo = o.ContNo,
                            Renban = o.Renban,
                            SupplierNo = o.SupplierNo,
                            ShowOnly = o.ShowOnly,
                            IsDisabled = o.IsDisabled,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstLgwRobbingLaneConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}

