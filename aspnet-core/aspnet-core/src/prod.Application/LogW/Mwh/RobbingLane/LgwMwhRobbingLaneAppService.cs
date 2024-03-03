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
using prod.LogW.Mwh.Dto;
using prod.LogW.Mwh.Exporting;

namespace prod.LogW.Mwh.RobbingLane
{
    //  [AbpAuthorize(AppPermissions.Pages_LogW_Mwh_RobbingLane)]
    public class LgwMwhRobbingLaneAppService : prodAppServiceBase, ILgwMwhRobbingLaneAppService
    {
        private readonly IDapperRepository<LgwMwhRobbingLane, long> _dapperRepo;
        private readonly IRepository<LgwMwhRobbingLane, long> _repo;
        private readonly ILgwMwhRobbingLaneExcelExporter _calendarListExcelExporter;

        public LgwMwhRobbingLaneAppService(IRepository<LgwMwhRobbingLane, long> repo,
                                         IDapperRepository<LgwMwhRobbingLane, long> dapperRepo,
                                        ILgwMwhRobbingLaneExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<LgwMwhRobbingLaneDto>> GetAll(GetLgwMwhRobbingLaneInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.LaneNo), e => e.LaneNo.Contains(input.LaneNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.LaneName), e => e.LaneName.Contains(input.LaneName))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new LgwMwhRobbingLaneDto
                         {
                             Id = o.Id,
                             LaneNo = o.LaneNo,
                             LaneName = o.LaneName,
                             ContNo = o.ContNo,
                             Renban = o.Renban,
                             SupplierNo = o.SupplierNo,
                             ShowOnly = o.ShowOnly,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<LgwMwhRobbingLaneDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }

        public async Task<FileDto> GetRobbingLaneToExcel(GetLgwMwhRobbingLaneExportInput input)
        {
            var filtered = _repo.GetAll()
               .WhereIf(!string.IsNullOrWhiteSpace(input.LaneNo), e => e.LaneNo.Contains(input.LaneNo))
               .WhereIf(!string.IsNullOrWhiteSpace(input.LaneName), e => e.LaneName.Contains(input.LaneName))
               ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var query = from o in pageAndFiltered
                        select new LgwMwhRobbingLaneDto
                        {
                            Id = o.Id,
                            LaneNo = o.LaneNo,
                            LaneName = o.LaneName,
                            ContNo = o.ContNo,
                            Renban = o.Renban,
                            SupplierNo = o.SupplierNo,
                            ShowOnly = o.ShowOnly,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
