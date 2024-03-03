using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.PIO.StockRundownTransaction.Dto;
using prod.Inventory.PIO.StockRundownTransaction.Exporting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prod.Inventory.PIO.StockRundownTransaction
{

    [AbpAuthorize(AppPermissions.Pages_PIO_Warehouse_StockRundownTransaction_View)]
    public class InvPIOStockRundownTransactionAppService : prodAppServiceBase, IInvPIOStockRundownTransactionAppService
    {
        private readonly IDapperRepository<InvPIOStockRundownTransaction, long> _dapperRepo;
        private readonly IInvPIOStockRundownTransactionExcelExporter _pioStockRundownExcelExporter;

        public InvPIOStockRundownTransactionAppService(IDapperRepository<InvPIOStockRundownTransaction, long> dapperRepo,
                                                        IInvPIOStockRundownTransactionExcelExporter pioStockRundownExcelExporter)
        {
            _dapperRepo = dapperRepo;
            _pioStockRundownExcelExporter = pioStockRundownExcelExporter;
        }


        public async Task<PagedResultDto<InvPIOStockRundownTransactionDto>> GetAll(GetInvPIOStockRundownTransactionInput input)
        {
            string _sql = "Exec INV_PIO_STOCK_RUNDOWN_TRANSACTION_SEARCH @p_PartNo, @p_MktCode, @p_WorkingDateFrom, @p_WorkingDateTo";

            IEnumerable<InvPIOStockRundownTransactionDto> result = await _dapperRepo.QueryAsync<InvPIOStockRundownTransactionDto>(_sql, new
            {
                p_PartNo = input.PartNo,
                p_MktCode = input.MktCode,
                p_WorkingDateFrom = input.WorkingDateFrom,
                p_WorkingDateTo = input.WorkingDateTo
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvPIOStockRundownTransactionDto>(
                totalCount,
                pagedAndFiltered);
        }


        public async Task<FileDto> GetStockRundownTransactionToExcel(GetInvPIOStockRundownTransactionExportInput input)
        {
            string _sql = "Exec INV_PIO_STOCK_RUNDOWN_TRANSACTION_SEARCH @p_PartNo, @p_MktCode, @p_WorkingDateFrom, @p_WorkingDateTo";

            IEnumerable<InvPIOStockRundownTransactionDto> result = await _dapperRepo.QueryAsync<InvPIOStockRundownTransactionDto>(_sql, new
            {
                p_PartNo = input.PartNo,
                p_MktCode = input.MktCode,
                p_WorkingDateFrom = input.WorkingDateFrom,
                p_WorkingDateTo = input.WorkingDateTo
            });

            return _pioStockRundownExcelExporter.ExportToFile(result.ToList());
        }
    }
}
