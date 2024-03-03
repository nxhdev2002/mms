using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.PIO.StockIssuing;
using prod.Inventory.PIO.StockReceiving.Dto;
using prod.Inventory.PIO.StockReceiving.Exporting;
using prod.Inventory.PIO.StockTransaction.Dto;
using prod.Inventory.PIO.StockTransaction.Exporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.PIO.StockReceiving
{
    [AbpAuthorize(AppPermissions.Pages_PIO_Warehouse_StockReceiving_View)]
    public class InvPIOStockReceivingAppService : prodAppServiceBase, IInvPIOStockReceivingAppService
    {
        private readonly IDapperRepository<InvPIOStockReceiving, long> _dapperRepo;
        private readonly IRepository<InvPIOStockReceiving, long> _repo;
        private readonly InvPIOStockReceivingExcelExporter _calendarListExcelExporter;


        public InvPIOStockReceivingAppService (IRepository<InvPIOStockReceiving, long> repo,
                                         IDapperRepository<InvPIOStockReceiving, long> dapperRepo,
                                        InvPIOStockReceivingExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter; ;
        }
        public async Task<PagedResultDto<InvPIOStockReceivingDto>> GetAll(GetInvPIOStockReceivingInput input)
        {
            string _sql = "Exec INV_PIO_STOCK_TRANSACTION_RECEIVING_SEARCH @p_partNo, @p_mktcode, @p_vinno, @p_workingdatefrom, @p_workingdateto";

            IEnumerable<InvPIOStockReceivingDto> result = await _dapperRepo.QueryAsync<InvPIOStockReceivingDto>(_sql, new
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

            return new PagedResultDto<InvPIOStockReceivingDto>(
                totalCount,
                 pagedAndFiltered
            );
        }
        public async Task<FileDto> GetStockReceivingToExcel(GetInvPIOStockReceivingExportInput input)
        {
            string _sql = "Exec INV_PIO_STOCK_TRANSACTION_RECEIVING_SEARCH @p_partNo, @p_mktcode, @p_vinno, @p_workingdatefrom, @p_workingdateto";

            IEnumerable<InvPIOStockReceivingDto> result = await _dapperRepo.QueryAsync<InvPIOStockReceivingDto>(_sql, new
            {
                p_PartNo = input.PartNo,
                p_mktcode = input.MktCode,
                p_vinno = input.VinNo,
                p_workingdatefrom = input.WorkingDateFrom,
                p_workingdateto = input.WorkingDateTo

            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
