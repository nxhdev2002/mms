using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.PIO.Stock;
using prod.Inventory.PIO.Stock.Dto;
using prod.Inventory.PIO.Stock.Exporting;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Inventory.PIO
{
    [AbpAuthorize(AppPermissions.Pages_PIO_Warehouse_Stock_View)]
    public class InvPIOStockAppService : prodAppServiceBase, IInvPIOStockAppService
    {
        private readonly IDapperRepository<InvPIOStock, long> _dapperRepo;
        private readonly IInvPIOStockExcelExporter _pioStockExcelExporter;

        public InvPIOStockAppService(IDapperRepository<InvPIOStock, long> dapperRepo,
                                        IInvPIOStockExcelExporter pioStockExcelExporter)
        {
            _dapperRepo = dapperRepo;
            _pioStockExcelExporter = pioStockExcelExporter;
        }

        public async Task<PagedResultDto<InvPIOStockDto>> GetAll(GetInvPIOStockInput input)
        {

            string _sql = "Exec INV_PIO_STOCK_SEARCH @p_MktCode, @p_PartNo, @p_WorkingDateFrom, @p_WorkingDateTo";

            IEnumerable<InvPIOStockDto> result = await _dapperRepo.QueryAsync<InvPIOStockDto>(_sql, new
            {
                p_MktCode = input.MktCode,
                p_PartNo = input.PartNo,                
                p_WorkingDateFrom = input.WorkingDateFrom,
                p_WorkingDateTo = input.WorkingDateTo
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvPIOStockDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetStockToExcel(GetInvPIOStockExportInput input)
        {
            string _sql = "Exec INV_PIO_STOCK_SEARCH @p_MktCode, @p_PartNo, @p_WorkingDateFrom, @p_WorkingDateTo";

            IEnumerable<InvPIOStockDto> result = await _dapperRepo.QueryAsync<InvPIOStockDto>(_sql, new
            {
                p_MktCode = input.MktCode,
                p_PartNo = input.PartNo,
                p_WorkingDateFrom = input.WorkingDateFrom,
                p_WorkingDateTo = input.WorkingDateTo
            });

            return _pioStockExcelExporter.ExportToFile(result.ToList());
        }

    }
}
