using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.PIO.StockIssuing;
using prod.Inventory.PIO.StockTransaction.Dto;
using prod.Inventory.PIO.StockTransaction.Exporting;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Inventory.PIO
{
    [AbpAuthorize(AppPermissions.Pages_PIO_Warehouse_StockIssuing_View)]
    public class InvPIOStockIssuingAppService : prodAppServiceBase, IInvPIOStockIssuingAppService
    {
        private readonly IDapperRepository<InvPIOStockTransaction, long> _dapperRepo;
        private readonly IRepository<InvPIOStockTransaction, long> _repo;
        private readonly InvPIOStockIssuingExcelExporter _pioStockIssuingExcelExporter;


        public InvPIOStockIssuingAppService(IRepository<InvPIOStockTransaction, long> repo,
                                         IDapperRepository<InvPIOStockTransaction, long> dapperRepo,
                                        InvPIOStockIssuingExcelExporter pioStockIssuingExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _pioStockIssuingExcelExporter = pioStockIssuingExcelExporter;
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

            if (listResult.Count > 0)
            {
                listResult[0].GrandQty = listResult.Sum(e => e.Qty);
            }

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvPIOStockTransactionDto>(
                totalCount,
                 pagedAndFiltered
            );
        }

        public async Task<FileDto> GetStockIssuingToExcel(GetInvPIOStockTransactionExportInput input)
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

            var exportToExcel = result.ToList();
            return _pioStockIssuingExcelExporter.ExportToFile(exportToExcel);
        }


    }
}
