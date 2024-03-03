using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.CKD.Exporting;
using prod.Inventory.CKD.ReceivingPhysicalStock.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prod.Inventory.CKD
{
    [AbpAuthorize(AppPermissions.Pages_Ckd_Physical_ReceivingPhysicalStock_View)]
    public class InvCkdReceivingPhysicalStockAppService : prodAppServiceBase, IInvCkdReceivingPhysicalStockAppService
    {
        private readonly IDapperRepository<InvCkdPhysicalStockTransaction, long> _dapperRepo;
        private readonly IInvCkdStockReceivingExcelExporter _receivingPhysicalStockListExcelExporter;
        public InvCkdReceivingPhysicalStockAppService(IDapperRepository<InvCkdPhysicalStockTransaction, long> dapperRepo,
                                                        IInvCkdStockReceivingExcelExporter receivingPhysicalStockListExcelExporter)
        {
            _dapperRepo = dapperRepo;
            _receivingPhysicalStockListExcelExporter = receivingPhysicalStockListExcelExporter;
        }
        public async Task<PagedResultDto<InvCkdReceivingPhysicalStockDto>> GetDataReceivingPhysicalStock(InvCkdReceivingPhysicalStockInputDto input)
        {
            string _sql = "Exec INV_CKD_PHYSICAL_STOCK_RECEIVING_SEARCH @p_part_no,@p_workingdatefrom,@p_workingdateto ,@p_cfc ,@p_supplier_no, @p_lot_no, @p_mode, @p_period_id ";

            IEnumerable<InvCkdReceivingPhysicalStockDto> result = await _dapperRepo.QueryAsync<InvCkdReceivingPhysicalStockDto>(_sql, new
            {
                p_part_no = input.PartNo,
                p_workingdatefrom = input.WorkingDateFrom,
                p_workingdateto = input.WorkingDateTo,
                p_cfc = input.Cfc,
                p_supplier_no = input.SupplierNo,
                p_lot_no = input.LotNo,
                p_mode = input.p_mode,
                p_period_id = input.PeriodId
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            if (listResult.Count > 0) listResult[0].GrandTotal = listResult.Sum(e => e.Qty);

            return new PagedResultDto<InvCkdReceivingPhysicalStockDto>(
                totalCount,
                pagedAndFiltered);
        }
        public async Task CallReceivingPhysicalStock()
        {
            Dapper.SqlMapper.Settings.CommandTimeout = 900;
            string _sql = "Exec JOB_INV_CKD_PHYSICAL_STOCK_TRANSACTION_RECEIVE_CREATE ";
            await _dapperRepo.ExecuteAsync(_sql);
        }

        public async Task<FileDto> GetReceivingPhysicalStockToExcel(InvCkdReceivingPhysicalStockInputDto input)
        {
            string _sql = "Exec INV_CKD_PHYSICAL_STOCK_RECEIVING_SEARCH @p_part_no,@p_workingdatefrom,@p_workingdateto ,@p_cfc ,@p_supplier_no, @p_lot_no, @p_mode, @p_period_id ";

            IEnumerable<InvCkdReceivingPhysicalStockDto> result = await _dapperRepo.QueryAsync<InvCkdReceivingPhysicalStockDto>(_sql, new
            {
                p_part_no = input.PartNo,
                p_workingdatefrom = input.WorkingDateFrom,
                p_workingdateto = input.WorkingDateTo,
                p_cfc = input.Cfc,
                p_supplier_no = input.SupplierNo,
                p_lot_no = input.LotNo,
                p_mode = input.p_mode,
                p_period_id = input.PeriodId
            });
            var exportToExcel = result.ToList();
            return _receivingPhysicalStockListExcelExporter.GetReceivingPhysicalStockToExcel(exportToExcel);

        }


        public async Task<FileDto> GetReceivingPhysicalStockDetailsToExcel(InvCkdReceivingPhysicalStockInputDto input)
        {
            Dapper.SqlMapper.Settings.CommandTimeout = 900;

            string _sql = "Exec INV_CKD_PHYSICAL_STOCK_RECEIVING_EXPORT_DETAILS_DATA @p_part_no,@p_workingdatefrom,@p_workingdateto ,@p_cfc ,@p_supplier_no, @p_lot_no, @p_mode, @p_period_id ";

            IEnumerable<InvCkdReceivingPhysStockDetailsDataDto> result = await _dapperRepo.QueryAsync<InvCkdReceivingPhysStockDetailsDataDto>(_sql, new
            {
                p_part_no = input.PartNo,
                p_workingdatefrom = input.WorkingDateFrom,
                p_workingdateto = input.WorkingDateTo,
                p_cfc = input.Cfc,
                p_supplier_no = input.SupplierNo,
                p_lot_no = input.LotNo,
                p_mode = input.p_mode,
                p_period_id = input.PeriodId
            });
            var exportToExcel = result.ToList();
            return _receivingPhysicalStockListExcelExporter.ExportToFileDetailsData(exportToExcel);

        }

    }
}
