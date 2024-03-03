using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using prod.Authorization;
using prod.Dto;
using prod.HistoricalData;
using prod.Inventory.CKD.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Inventory.CKD
{
    [AbpAuthorize(AppPermissions.Pages_Ckd_Warehouse_PartManagement_View)]
    public class InvCkdPartManagementAppService : prodAppServiceBase, IInvCkdPartManagementAppService
    {
        private readonly IDapperRepository<InvCkdShipment, long> _dapperRepo;
        private readonly IRepository<InvCkdShipment, long> _repo;
        private readonly IInvCkdPartManagementExcelExporter _partMngExcelExporter;
        private readonly IHistoricalDataAppService _historicalDataAppService;


        public InvCkdPartManagementAppService(IRepository<InvCkdShipment, long> repo,
                                         IDapperRepository<InvCkdShipment, long> dapperRepo,
                                        IInvCkdPartManagementExcelExporter partMngExcelExporter,
                                        IHistoricalDataAppService historicalDataAppService
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _partMngExcelExporter = partMngExcelExporter;
            _historicalDataAppService = historicalDataAppService;
        }

        public async Task<List<string>> GetPartManagementHistory(GetInvCkdPartManagementHistoryInput input)
        {
            return await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
        }

        public async Task<FileDto> GetHistoricalDataToExcel(GetInvCkdPartManagementHistoryExcelInput input)
        {
            var data = await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
            return _partMngExcelExporter.ExportToHistoricalFile(data);
        }

        public async Task<List<long?>> GetChangedRecords()
        {
            var data = await _historicalDataAppService.GetChangedRecordIds("InvCkdInvoiceDetails");
            return data;
        }

        public async Task<PagedResultDto<InvCkdPartManagementDto>> GetAll(GetInvCkdPartManagementInput input)
        {
            Dapper.SqlMapper.Settings.CommandTimeout = 900;
            string _sql = "Exec INV_CKD_PART_LIST_MANAGEMENT_SEARCH @p_part_no, @p_cfc, @p_supplier_no, @p_invoice_no, " +
                "@p_invoice_date_from, @p_invoice_date_to, @p_bill_no, @p_shipment_no, @p_container_no, @p_renban, " +
                "@p_port_date_from, @p_port_date_to, @p_receive_date_from, @p_receive_date_to, @p_radio, @p_module_case, " +
                "@p_unpacking_date_from, @p_unpacking_date_to, @p_storage_code, @p_bill_date_from, @p_bill_date_to, @p_orderno,@p_ordertype_code,@p_goodstype_code,@p_CkdPio,@p_firmpacking_date_from, @p_firmpacking_date_to, @p_lotno";

            IEnumerable<InvCkdPartManagementDto> result = await _dapperRepo.QueryAsync<InvCkdPartManagementDto>(_sql, new
            {
                p_part_no = input.PartNo,
                p_cfc = input.Cfc,
                p_supplier_no = input.SupplierNo,
                p_invoice_no = input.InvoiceNo,
                p_invoice_date_from = input.InvoiceDateFrom,
                p_invoice_date_to = input.InvoiceDateTo,
                p_bill_no = input.BillofladingNo,
                p_shipment_no = input.ShipmentNo,
                p_container_no = input.ContainerNo,
                p_renban = input.Renban,
                p_port_date_from = input.PortDateFrom,
                p_port_date_to = input.PortDateTo,
                p_receive_date_from = input.ReceiveDateFrom,
                p_receive_date_to = input.ReceiveDateTo,
                p_radio = input.radio,
                p_module_case = input.ModuleCaseNo,
                p_unpacking_date_from = input.UnpackingDateFrom,
                p_unpacking_date_to = input.UnpackingDateTo,
                p_storage_code = input.StorageLocationCode,
                p_bill_date_from = input.BillDateFrom,
                p_bill_date_to = input.BillDateTo,
                p_orderno = input.OrderNo,
                p_ordertype_code = input.OrderTypeCode,
                p_goodstype_code = input.GoodsTypeCode,
                p_CkdPio = input.CkdPio,
                p_firmpacking_date_from = input.FirmPackingDateFrom,
                p_firmpacking_date_to = input.FirmPackingDateTo,
                p_lotno = input.LotNo,
            });

            List<InvCkdPartManagementDto> listResult = result.ToList();
            if (listResult.Count > 0)
            {
                listResult[0].TotalQty = 0;
                listResult[0].TotalFob = 0;
                listResult[0].TotalCif = 0;
                listResult[0].TotalFreight = 0;
                listResult[0].TotalInsurance = 0;
                listResult[0].TotalTax = 0;
                listResult[0].TotalAmount = 0;
                foreach (InvCkdPartManagementDto item in listResult)
                {
                    listResult[0].TotalQty += item.UsageQty * 1;
                    listResult[0].TotalFob += item.UsageQty * item.Fob;
                    listResult[0].TotalCif += item.UsageQty * item.Cif;
                    listResult[0].TotalFreight += item.UsageQty * item.Freight;
                    listResult[0].TotalInsurance += item.UsageQty * item.Insurance;
                    listResult[0].TotalTax += item.UsageQty * item.Tax;
                    listResult[0].TotalAmount += item.UsageQty * item.Amount;
                }
            }

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCkdPartManagementDto>(
                totalCount,
                pagedAndFiltered
            );
        }


        public async Task<FileDto> GetPartManagementToExcel(InvCkdPartManagementExportInput input)
        {
            Dapper.SqlMapper.Settings.CommandTimeout = 900;
            string _sql = "Exec INV_CKD_PART_LIST_MANAGEMENT_SEARCH @p_part_no, @p_cfc, @p_supplier_no, @p_invoice_no, " +
                "@p_invoice_date_from, @p_invoice_date_to, @p_bill_no, @p_shipment_no, @p_container_no, @p_renban, " +
                "@p_port_date_from, @p_port_date_to, @p_receive_date_from, @p_receive_date_to, @p_radio, @p_module_case, " +
                "@p_unpacking_date_from, @p_unpacking_date_to, @p_storage_code, @p_bill_date_from, @p_bill_date_to, @p_orderno,@p_ordertype_code,@p_goodstype_code, @p_CkdPio,@p_firmpacking_date_from, @p_firmpacking_date_to, @p_lotno";

            IEnumerable<InvCkdPartManagementDto> result = await _dapperRepo.QueryAsync<InvCkdPartManagementDto>(_sql, new
            {
                p_part_no = input.PartNo,
                p_cfc = input.Cfc,
                p_supplier_no = input.SupplierNo,
                p_invoice_no = input.InvoiceNo,
                p_invoice_date_from = input.InvoiceDateFrom,
                p_invoice_date_to = input.InvoiceDateTo,
                p_bill_no = input.BillofladingNo,
                p_shipment_no = input.ShipmentNo,
                p_container_no = input.ContainerNo,
                p_renban = input.Renban,
                p_port_date_from = input.PortDateFrom,
                p_port_date_to = input.PortDateTo,
                p_receive_date_from = input.ReceiveDateFrom,
                p_receive_date_to = input.ReceiveDateTo,
                p_radio = input.radio,
                p_module_case = input.ModuleCaseNo,
                p_unpacking_date_from = input.UnpackingDateFrom,
                p_unpacking_date_to = input.UnpackingDateTo,
                p_storage_code = input.StorageLocationCode,
                p_bill_date_from = input.BillDateFrom,
                p_bill_date_to = input.BillDateTo,
                p_orderno = input.OrderNo,
                p_ordertype_code = input.OrderTypeCode,
                p_goodstype_code = input.GoodsTypeCode,
                p_CkdPio = input.CkdPio,
                p_firmpacking_date_from = input.FirmPackingDateFrom,
                p_firmpacking_date_to = input.FirmPackingDateTo,
                p_lotno = input.LotNo,
            });

            var exportToExcel = result.ToList();
            return _partMngExcelExporter.ExportToFile(exportToExcel);
        }


        public async Task<FileDto> GetPartManagementReportToExcel(InvCkdPartManagementExportInput input)
        {
            Dapper.SqlMapper.Settings.CommandTimeout = 900;
            string _sql = "Exec INV_CKD_PART_LIST_MANAGEMENT_REPORT @p_part_no, @p_cfc, @p_supplier_no, @p_invoice_no, " +
                "@p_invoice_date_from, @p_invoice_date_to, @p_bill_no, @p_shipment_no, @p_container_no, @p_renban, " +
                "@p_port_date_from, @p_port_date_to, @p_receive_date_from, @p_receive_date_to, @p_radio, @p_module_case, " +
                "@p_unpacking_date_from, @p_unpacking_date_to, @p_storage_code, @p_bill_date_from, @p_bill_date_to, @p_orderno,@p_ordertype_code,@p_goodstype_code, @p_CkdPio,@p_firmpacking_date_from, @p_firmpacking_date_to, @p_lotno";

            IEnumerable<InvCkdPartManagementReportDto> result = await _dapperRepo.QueryAsync<InvCkdPartManagementReportDto>(_sql, new
            {
                p_part_no = input.PartNo,
                p_cfc = input.Cfc,
                p_supplier_no = input.SupplierNo,
                p_invoice_no = input.InvoiceNo,
                p_invoice_date_from = input.InvoiceDateFrom,
                p_invoice_date_to = input.InvoiceDateTo,
                p_bill_no = input.BillofladingNo,
                p_shipment_no = input.ShipmentNo,
                p_container_no = input.ContainerNo,
                p_renban = input.Renban,
                p_port_date_from = input.PortDateFrom,
                p_port_date_to = input.PortDateTo,
                p_receive_date_from = input.ReceiveDateFrom,
                p_receive_date_to = input.ReceiveDateTo,
                p_radio = input.radio,
                p_module_case = input.ModuleCaseNo,
                p_unpacking_date_from = input.UnpackingDateFrom,
                p_unpacking_date_to = input.UnpackingDateTo,
                p_storage_code = input.StorageLocationCode,
                p_bill_date_from = input.BillDateFrom,
                p_bill_date_to = input.BillDateTo,
                p_orderno = input.OrderNo,
                p_ordertype_code = input.OrderTypeCode,
                p_goodstype_code = input.GoodsTypeCode,
                p_CkdPio = input.CkdPio,
                p_firmpacking_date_from = input.FirmPackingDateFrom,
                p_firmpacking_date_to = input.FirmPackingDateTo,
                p_lotno = input.LotNo,
            });

            var exportToExcel = result.ToList();
            return _partMngExcelExporter.ExportReportToFile(exportToExcel);
        }

    }
}
