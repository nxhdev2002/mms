using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using prod.Dto;
using prod.Inventory.SPP.CostOfSaleSummary.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using prod.Authorization;
using prod.Inventory.SPP.CostOfSaleSummary.Exporting;

namespace prod.Inventory.SPP.CostOfSaleSummary
{
    [AbpAuthorize(AppPermissions.Pages_SPP_CostOfSaleSummary_View)]
    public class InvSppCostOfSaleSummaryAppService : prodAppServiceBase, IInvSppCostOfSaleSummaryAppService
    {
        private readonly IDapperRepository<InvSppCostOfSaleSummary, long> _dapperRepo;
        private readonly IRepository<InvSppCostOfSaleSummary, long> _repo;
        private readonly IInvSppCostOfSaleSummaryExcelExporter _costOfSaleSummaryExcelExporter;

        public InvSppCostOfSaleSummaryAppService(IRepository<InvSppCostOfSaleSummary, long> repo,
                                         IDapperRepository<InvSppCostOfSaleSummary, long> dapperRepo,
                                         IInvSppCostOfSaleSummaryExcelExporter costOfSaleSummaryExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _costOfSaleSummaryExcelExporter = costOfSaleSummaryExcelExporter;
        }

        public async Task<PagedResultDto<InvSppCostOfSaleSummaryDto>> GetAll(GetInvSppCostOfSaleSummaryInput input)
        {
            string _sql = "Exec INV_SPP_COSTOFSALESUMMARY_SEARCH @p_customerno";

            IEnumerable<InvSppCostOfSaleSummaryDto> result = await _dapperRepo.QueryAsync<InvSppCostOfSaleSummaryDto>(_sql, new
            {
                p_customerno = input.CustomerNo,
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvSppCostOfSaleSummaryDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetCostOfSummaryToExcel(GetInvSppCostOfSaleSummaryExportInput input)
        {
            string _sql = "Exec INV_SPP_COSTOFSALESUMMARY_SEARCH @p_customerno";

            IEnumerable<InvSppCostOfSaleSummaryDto> result = await _dapperRepo.QueryAsync<InvSppCostOfSaleSummaryDto>(_sql, new
            {
                p_customerno = input.CustomerNo,
            });

            var exportToExcel = result.ToList();
            return _costOfSaleSummaryExcelExporter.ExportToFile(exportToExcel);
        }
    }
}
