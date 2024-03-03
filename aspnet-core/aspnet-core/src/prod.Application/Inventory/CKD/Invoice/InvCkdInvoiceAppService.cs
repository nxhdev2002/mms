using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using prod.Authorization;
using prod.Dto;
using prod.HistoricalData;
using prod.Inventory.CKD;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.Invoice;
using prod.Inventory.CKD.Invoice.Dto;
using prod.Inventory.Invoice.Dto;
using prod.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prod.Inventory.Invoice
{
    [AbpAuthorize(AppPermissions.Pages_Ckd_Intransit_Invoice_View)]
    public class InvCkdInvoiceAppService : prodAppServiceBase, IInvCkdInvoiceAppService
    {
        private readonly IDapperRepository<InvCkdInvoice, long> _dapperRepo;
        private readonly IInvCkdInvoiceExcelExporter _invCkdInvoiceListExcelExporter;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IHistoricalDataAppService _historicalDataAppService;

        public InvCkdInvoiceAppService(IDapperRepository<InvCkdInvoice, long> dapperRepo,
                                                        IInvCkdInvoiceExcelExporter invCkdInvoiceListExcelExporter,
                                                        IHistoricalDataAppService historicalDataAppService,
                                                        IInvCkdInvoiceExcelExporter invCkdInvoiceExcelExporter)
        {
            _dapperRepo = dapperRepo;
            _invCkdInvoiceListExcelExporter = invCkdInvoiceListExcelExporter;
            _historicalDataAppService = historicalDataAppService;
        }

        public async Task<List<string>> GetInvoiceHistory(GetInvCkdInvoiceHistoryInput input)
        {
            return await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
        }

        public async Task<FileDto> GetHistoricalDataToExcel(GetInvCkdInvoiceHistoryExcelInput input)
        {
            var data = await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
            return _invCkdInvoiceListExcelExporter.ExportToHistoricalFile(data, input.TableName);
        }

        public async Task<ChangedRecordIdsInvoiceDto> GetChangedRecords()
        {
            var listInvoice = await _historicalDataAppService.GetChangedRecordIds("InvCkdInvoice");
            var ListInvoiceDetail = await _historicalDataAppService.GetChangedRecordIds("InvCkdInvoiceDetails");

            ChangedRecordIdsInvoiceDto result = new ChangedRecordIdsInvoiceDto();
            result.Invoice = listInvoice;
            result.InvoiceDetail = ListInvoiceDetail;
            return result;
        }

        public async Task<PagedResultDto<InvCkdInvoiceDetailsDto>> GetInvoiceDetailsGetbyinvoiceid(GetInvCkdInvoiceDetailDtolInput input)
        {
            string _sql = "Exec INV_CKD_INVOICE_DETAILS_GETBYINVOICEID  @p_invoice_id";

            var result = await _dapperRepo.QueryAsync<InvCkdInvoiceDetailsDto>(_sql, new
            {
                p_invoice_id = input.InvoiceId
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCkdInvoiceDetailsDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<PagedResultDto<InvCkdInvoiceDto>> GetInvoiceSearch(GetInvCkdInvoiceDtolInput input)
        {
            string _sql = "Exec INV_CKD_INVOICE_SEARCH @p_invoice_no, @p_invoice_date_from, @p_invoice_date_to, @p_bill, @p_shipment, @p_container_no, @p_bill_date_from, @p_bill_date_to, @p_order_type_code, @p_supplier_no, @p_CkdPio";

            var filtered = await _dapperRepo.QueryAsync<InvCkdInvoiceDto>(_sql, new
            {
                p_invoice_no = input.InvoiceNo,
                p_invoice_date_from = input.InvoiceDateFrom,
                p_invoice_date_to = input.InvoiceDateTo,
                p_bill = input.BillNo,
                p_shipment = input.ShipmentNo,
                p_container_no = input.ContainerNo,
                p_bill_date_from = input.BillDateFrom,
                p_bill_date_to = input.BillDateTo,
                p_order_type_code = input.OrderTypeCode,
                p_supplier_no = input.SupplierNo,
                p_CkdPio = input.CkdPio
            });
            var results = filtered.ToList();

            var pagedAndFiltered = results.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = filtered.ToList().Count();

            return new PagedResultDto<InvCkdInvoiceDto>(
                totalCount,
                pagedAndFiltered);

            //   return _sqldata.ToList();
        }

        public async Task<FileDto> GetInvoiceExportExcel(GetInvCkdInvoiceDtolInput input)
        {
            string _sql = "Exec INV_CKD_INVOICE_SEARCH @p_invoice_no, @p_invoice_date_from, @p_invoice_date_to, @p_bill, @p_shipment, @p_container_no, @p_bill_date_from, @p_bill_date_to, @p_order_type_code, @p_supplier_no, @p_CkdPio";

            var filtered = await _dapperRepo.QueryAsync<InvCkdInvoiceDto>(_sql, new
            {
                p_invoice_no = input.InvoiceNo,
                p_invoice_date_from = input.InvoiceDateFrom,
                p_invoice_date_to = input.InvoiceDateTo,
                p_bill = input.BillNo,
                p_shipment = input.ShipmentNo,
                p_container_no = input.ContainerNo,
                p_bill_date_from = input.BillDateFrom,
                p_bill_date_to = input.BillDateTo,
                p_order_type_code = input.OrderTypeCode,
                p_supplier_no = input.SupplierNo,
                p_CkdPio = input.CkdPio
            });


            var exportToExcel = filtered.ToList();
            return _invCkdInvoiceListExcelExporter.ExportToFile(exportToExcel);

            //   return _sqldata.ToList();
        }

        public async Task<FileDto> GetInvoiceDetailExportExcel(GetInvCkdInvoiceDetailDtolInput input)
        {

            string _sql = "Exec INV_CKD_INVOICE_DETAILS_GETBYINVOICEID  @p_invoice_id";

            var filtered = await _dapperRepo.QueryAsync<InvCkdInvoiceDetailsDto>(_sql, new
            {
                p_invoice_id = input.InvoiceId
            });

            var results = from o in filtered
                          select new InvCkdInvoiceDetailsDto
                          {
                              PartNo = o.PartNo,
                              UsageQty = o.UsageQty,
                              LotNo = o.LotNo,
                              Fixlot = o.Fixlot,
                              CaseNo = o.CaseNo,
                              ModuleNo = o.ModuleNo,
                              ContainerNo = o.ContainerNo,
                              Renban = o.Renban,
                              SupplierNo = o.SupplierNo,
                              Insurance = o.Insurance,
                              Fi = o.Fi,
                              Fob = o.Fob,
                              Freight = o.Freight,
                              Thc = o.Thc,
                              Cif = o.Cif,
                              Tax = o.Tax,
                              TaxRate = o.TaxRate,
                              Vat = o.Vat,
                              VatRate = o.VatRate,
                              Inland = o.Inland,
                              CeptType = o.CeptType,
                              PartName = o.PartName,
                              CarfamilyCode = o.CarfamilyCode,
                              PartNetWeight = o.PartNetWeight,
                              OrderNo = o.OrderNo,
                              Firmpackingmonth = o.Firmpackingmonth,
                              ReexportCode = o.ReexportCode,
                              Description = o.Description,
                              FobVn = o.FobVn,
                              FreightVn = o.FreightVn,
                              InsuranceVn = o.InsuranceVn,
                              FiVn = o.FiVn,
                              ThcVn = o.ThcVn,
                              CifVn = o.CifVn,
                              TaxVn = o.TaxVn,
                              VatVn = o.VatVn,
                              InvoiceId = o.InvoiceId,
                              InlandVn = o.InlandVn,
                              InvoiceParentId = o.InvoiceParentId,
                              PeriodDate = o.PeriodDate,
                              PeriodId = o.PeriodId,
                              HsCode = o.HsCode,
                              PartnameVn = o.PartnameVn,
                              CarName = o.CarName,
                              PmhistId = o.PmhistId,
                              PreCustomsId = o.PreCustomsId,
                              Ecus5TaxRate = o.Ecus5TaxRate,
                              Ecus5VatRate = o.Ecus5VatRate,
                              Ecus5HsCode = o.Ecus5HsCode,
                              DeclareType = o.DeclareType,
                              IsActive = o.IsActive,
                              Status = o.Status

                          };

            var exportToExcel = results.ToList();
            return _invCkdInvoiceListExcelExporter.ExportToFile2(exportToExcel);
        }

        public async Task<FileDto> GetInvoiceCustomsExportExcel(GetInvCkdInvoiceCustomsDtolInput input)
        {
            string _sql = "Exec INV_CKD_INVOICE_CUSTOM_EXPORT @p_invoice_no, @p_invoice_date_from, @p_invoice_date_to, @p_bill, @p_shipment, @p_container_no, @p_bill_date_from, @p_bill_date_to, @p_order_type_code, @p_supplier_no, @p_CkdPio";

            var filtered = await _dapperRepo.QueryAsync<InvCkdInvoiceCustomsDto>(_sql, new
            {
                p_invoice_no = input.InvoiceNo,
                p_invoice_date_from = input.InvoiceDateFrom,
                p_invoice_date_to = input.InvoiceDateTo,
                p_bill = input.BillNo,
                p_shipment = input.ShipmentNo,
                p_container_no = input.ContainerNo,
                p_bill_date_from = input.BillDateFrom,
                p_bill_date_to = input.BillDateTo,
                p_order_type_code = input.OrderTypeCode,
                p_supplier_no = input.SupplierNo,
                p_CkdPio = input.CkdPio
            });


            var exportToExcel = filtered.ToList();
            return _invCkdInvoiceListExcelExporter.ExportToFile3(exportToExcel);

            //   return _sqldata.ToList();
        }


        public async Task<PagedResultDto<InvCkdInvoiceDetailsByLotDto>> GetDataInvoiceDetailsByLot(InvCkdInvoiceDetailsByLotInput input)
        {
            string _sql = "Exec GET_INVOICE_DETAILS_BY_LOTNO_SEARCH @p_lotno";

            IEnumerable<InvCkdInvoiceDetailsByLotDto> result = await _dapperRepo.QueryAsync<InvCkdInvoiceDetailsByLotDto>(_sql, new
            {
                p_lotno = input.LotNo
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCkdInvoiceDetailsByLotDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetInvoiceDetailsByLotNoToExcel(InvCkdInvoiceDetailsByLotExportInput input)
        {
            string _sql = "Exec GET_INVOICE_DETAILS_BY_LOTNO_SEARCH @p_lotno";

            IEnumerable<InvCkdInvoiceDetailsByLotDto> result = await _dapperRepo.QueryAsync<InvCkdInvoiceDetailsByLotDto>(_sql, new
            {
                p_lotno = input.LotNo
            });

            var exportToExcel = result.ToList();
            return _invCkdInvoiceListExcelExporter.ExportToFileByLotNo(exportToExcel);
        }

    }
}

