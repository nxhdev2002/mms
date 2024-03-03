using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using prod.Authorization;
using prod.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using prod.Inventory.SPP.Cost.Dto;
using prod.Inventory.SPP.Cost.Exporting;

namespace prod.Inventory.SPP.Cost
{
    [AbpAuthorize(AppPermissions.Pages_SPP_Cost_View)]
    public class InvSppCostAppService : prodAppServiceBase, IInvSppCostAppService
    {
        private readonly IDapperRepository<InvSppCost, long> _dapperRepo;
        private readonly IRepository<InvSppCost, long> _repo;
        private readonly IInvSppCostExcelExporter _costExcelExporter;

        public InvSppCostAppService(IRepository<InvSppCost, long> repo,
                                         IDapperRepository<InvSppCost, long> dapperRepo,
                                         IInvSppCostExcelExporter costExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _costExcelExporter = costExcelExporter;
        }

        public async Task<PagedResultDto<InvSppCostDto>> GetAll(GetInvSppCostInput input)
        {
            string _sql = "Exec INV_SPP_COST_SEARCH @p_partno, @p_invoiceno, @p_stock, @p_frommonthyear, @p_tomonthyear";

            IEnumerable<InvSppCostDto> result = await _dapperRepo.QueryAsync<InvSppCostDto>(_sql, new
            {
                p_partno = input.PartNo,
                p_invoiceno = input.InvoiceNo,
                p_stock = input.Stock,
                p_frommonthyear = input.FromMonthYear,
                p_tomonthyear = input.ToMonthYear
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvSppCostDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetCostToExcel(GetInvSppCostExportInput input)
        {
            string _sql = "Exec INV_SPP_COST_SEARCH @p_partno, @p_invoiceno, @p_stock, @p_frommonthyear, @p_tomonthyear";

            IEnumerable<InvSppCostDto> result = await _dapperRepo.QueryAsync<InvSppCostDto>(_sql, new
            {
                p_partno = input.PartNo,
                p_invoiceno = input.InvoiceNo,
                p_stock = input.Stock,
                p_frommonthyear = input.FromMonthYear,
                p_tomonthyear = input.ToMonthYear
            });

            var exportToExcel = result.ToList();
            return _costExcelExporter.ExportToFile(exportToExcel);
        }
    }
}
