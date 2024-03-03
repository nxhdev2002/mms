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
using prod.LogW.Mwh.ContList.Exporting;
using prod.LogW.Mwh.Dto;

namespace prod.LogW.Mwh.ContList
{
    //  [AbpAuthorize(AppPermissions.Pages_LogW_Mwh_ContList)]
    public class LgwMwhContListAppService : prodAppServiceBase, ILgwMwhContListAppService
    {
        private readonly IDapperRepository<LgwMwhContList, long> _dapperRepo;
        private readonly IRepository<LgwMwhContList, long> _repo;
        private readonly ILgwMwhContListExcelExporter _calendarListExcelExporter;

        public LgwMwhContListAppService(IRepository<LgwMwhContList, long> repo,
                                         IDapperRepository<LgwMwhContList, long> dapperRepo,
                                        ILgwMwhContListExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

 


        public async Task<PagedResultDto<LgwMwhContListDto>> GetAll(GetLgwMwhContListInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.ContainerNo), e => e.ContainerNo.Contains(input.ContainerNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Renban), e => e.Renban.Contains(input.Renban))
                .WhereIf(input.DevanningDateFrom.HasValue, t => t.DevanningDate >= input.DevanningDateFrom.Value)
                .WhereIf(input.DevanningDateTo.HasValue, t => t.DevanningDate <= input.DevanningDateTo.Value);


            var pageAndFiltered = filtered.OrderByDescending(s => s.DevanningDate);


            var system = from o in pageAndFiltered
                         select new LgwMwhContListDto
                         {
                             Id = o.Id,
                             ContainerNo = o.ContainerNo,
                             Renban = o.Renban,
                             SupplierNo = o.SupplierNo,
                             DevanningDate = o.DevanningDate,
                             StartDevanningDate = o.StartDevanningDate,
                             FinishDevanningDate = o.FinishDevanningDate,
                             Status = o.Status,
                             ContScheduleId = o.ContScheduleId,
                             Shop = o.Shop,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<LgwMwhContListDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetContListToExcel(GetLgwMwhContListExportInput input)
        {
            var filtered = _repo.GetAll()
              .WhereIf(!string.IsNullOrWhiteSpace(input.ContainerNo), e => e.ContainerNo.Contains(input.ContainerNo))
              .WhereIf(!string.IsNullOrWhiteSpace(input.Renban), e => e.Renban.Contains(input.Renban))
              .WhereIf(input.DevanningDateFrom.HasValue, t => t.DevanningDate >= input.DevanningDateFrom.Value)
              .WhereIf(input.DevanningDateTo.HasValue, t => t.DevanningDate <= input.DevanningDateTo.Value);
            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var query = from o in pageAndFiltered
                        select new LgwMwhContListDto
                        {
                            Id = o.Id,
                            ContainerNo = o.ContainerNo,
                            Renban = o.Renban,
                            SupplierNo = o.SupplierNo,
                            DevanningDate = o.DevanningDate,
                            StartDevanningDate = o.StartDevanningDate,
                            FinishDevanningDate = o.FinishDevanningDate,
                            Status = o.Status,
                            ContScheduleId = o.ContScheduleId,
                            Shop = o.Shop,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
