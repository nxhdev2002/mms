using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.PIO.StockRundownTransaction.Dto;
using prod.Inventory.PIO.StockTransaction.Dto;
using prod.Inventory.PIO.StockTransaction.Exporting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prod.Inventory.PIO.StockTransaction
{
    [AbpAuthorize(AppPermissions.Pages_PIO_Warehouse_StockTransaction_View)]
    public class InvPIOStockTransactionAppService : prodAppServiceBase, IInvPIOStockTransactionAppService
    {
        private readonly IDapperRepository<InvPIOStockTransaction, long> _dapperRepo;
        private readonly IRepository<InvPIOStockTransaction, long> _repo;
        private readonly IInvPIOStockTransactionExcelExporter _calendarListExcelExporter;

        public InvPIOStockTransactionAppService(IRepository<InvPIOStockTransaction, long> repo,
                                         IDapperRepository<InvPIOStockTransaction, long> dapperRepo,
                                        IInvPIOStockTransactionExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<InvPIOStockTransactionDto>> GetAll(GetInvPIOStockTransactionInput input)
        {
            string _sql = "Exec INV_PIO_STOCK_TRANSACTION_ISSUING_SEARCH @p_partNo, @p_mktcode, @p_vinno, @p_workingdatefrom, @p_workingdateto";

            IEnumerable<InvPIOStockTransactionDto> result = await _dapperRepo.QueryAsync<InvPIOStockTransactionDto>(_sql, new
            {
                p_PartNo = input.PartNo,
                p_mktcode = input.MktCode,
                p_vinno = input.VinNo,
                p_workingdatefrom = input.WorkingDateFrom,
                p_workingdateto = input.WorkingDateTo

            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvPIOStockTransactionDto>(
                totalCount,
                pagedAndFiltered);
        }


        public async Task<FileDto> GetStockTransactionToExcel(GetInvPIOStockTransactionExportInput input)
        {
            string _sql = "Exec INV_PIO_STOCK_TRANSACTION_ISSUING_SEARCH @p_partNo, @p_mktcode, @p_vinno, @p_workingdatefrom, @p_workingdateto";

            IEnumerable<InvPIOStockTransactionDto> result = await _dapperRepo.QueryAsync<InvPIOStockTransactionDto>(_sql, new
            {
                p_PartNo = input.PartNo,
                p_mktcode = input.MktCode,
                p_vinno = input.VinNo,
                p_workingdatefrom = input.WorkingDateFrom,
                p_workingdateto = input.WorkingDateTo

            });

            var listResult = result.ToList();
            return _calendarListExcelExporter.ExportToFile(listResult);
        }
    }
}
