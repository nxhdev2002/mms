using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using prod.Authorization;
using prod.Dto;
using prod.HistoricalData;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.Exporting;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Inventory.CKD
{
    [AbpAuthorize(AppPermissions.Pages_Ckd_Warehouse_ContainerList_View)]
    public class InvCkdContainerListAppService : prodAppServiceBase, IInvCkdContainerListAppService
    {
        private readonly IDapperRepository<InvCkdContainerList, long> _dapperRepo;
        private readonly IRepository<InvCkdContainerList, long> _repo;
        private readonly IInvCkdContainerListExcelExporter _containerListExcelExporter;
        private readonly IHistoricalDataAppService _historicalDataAppService;

        public InvCkdContainerListAppService(IRepository<InvCkdContainerList, long> repo,
                                         IDapperRepository<InvCkdContainerList, long> dapperRepo,
                                        IInvCkdContainerListExcelExporter containerListExcelExporter,
                                        IHistoricalDataAppService historicalDataAppService
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _containerListExcelExporter = containerListExcelExporter;
            _historicalDataAppService = historicalDataAppService;
        }

        public async Task<List<string>> GetContainerListHistory(GetInvCkdPartListHistoryInput input)
        {
            return await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
        }

        public async Task<FileDto> GetHistoricalDataToExcel(GetInvCkdPartListHistoryExcelInput input)
        {
            var data = await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
            return _containerListExcelExporter.ExportToHistoricalFile(data);
        }

        public async Task<List<long?>> GetChangedRecords()
        {
            return  await _historicalDataAppService.GetChangedRecordIds("InvCkdContainerList");
        }

        public async Task<PagedResultDto<InvCkdContainerListDto>> GetAll(GetInvCkdContainerListInput input)
        {
            Dapper.SqlMapper.Settings.CommandTimeout = 900;
            string _sql = "Exec INV_CKD_CONTAINERLIST_SEARCH @p_container_no, @p_renban, @p_supplier_no, @p_haisen_no, @p_bill_of_lading_no, @p_invoice_no, " +
                "@p_ordertype_code, @p_port_date_from, @p_port_date_to, @p_receive_date_from, @p_receive_date_to, @p_goodstype_code, @p_radio, @p_bill_date_from, @p_bill_date_to, @p_CkdPio,@p_lotno";

            IEnumerable<InvCkdContainerListDto> result = await _dapperRepo.QueryAsync<InvCkdContainerListDto>(_sql, new
            {
                p_container_no = input.ContainerNo,
                p_renban = input.Renban,
                p_supplier_no = input.SupplierNo,
                p_haisen_no = input.HaisenNo,
                p_bill_of_lading_no = input.BillOfLadingNo,
                p_invoice_no = input.InvoiceNo,
                p_ordertype_code = input.OrderTypeCode,
                p_port_date_from = input.PortDateFrom,
                p_port_date_to = input.PortDateTo,
                p_receive_date_from = input.ReceiveDateFrom,
                p_receive_date_to = input.ReceiveDateTo,
                p_goodstype_code = input.GoodsTypeCode,
                p_radio = input.radio,
                p_bill_date_from = input.BillDateFrom,
                p_bill_date_to = input.BillDateTo,
                p_CkdPio = input.CkdPio,
                p_lotno = input.LotNo,
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            if (listResult.Count > 0)
            {
                listResult[0].GrandFob = listResult.Sum(e => e.Fob);
                listResult[0].GrandAmount = listResult.Sum(e => e.Amount);
                listResult[0].GrandCif = listResult.Sum(e => e.Cif);
                listResult[0].GrandFreight = listResult.Sum(e => e.Freight);
                listResult[0].GrandInsurance = listResult.Sum(e => e.Insurance);
                listResult[0].GrandTax = listResult.Sum(e => e.Tax);
                listResult[0].GrandOverDEM = listResult.Sum(e => e.OverDEM);
                listResult[0].GrandOverDET = listResult.Sum(e => e.OverDET);
                listResult[0].GrandOverDEMDET = listResult.Sum(e => e.OverDEMDET);
                listResult[0].GrandOverFeeDEM = listResult.Sum(e => e.OverFeeDEM);
                listResult[0].GrandOverFeeDET = listResult.Sum(e => e.OverFeeDET);
                listResult[0].GrandOverFeeDEMDET = listResult.Sum(e => e.OverFeeDEMDET);
            }

            return new PagedResultDto<InvCkdContainerListDto>(
                totalCount,
                pagedAndFiltered);
        }


        public async Task<FileDto> GetContainerListToExcel(InvCkdContainerListExportInput input)
        {
            Dapper.SqlMapper.Settings.CommandTimeout = 900;
            string _sql = "Exec INV_CKD_CONTAINERLIST_SEARCH @p_container_no, @p_renban, @p_supplier_no, @p_haisen_no, @p_bill_of_lading_no, @p_invoice_no, " +
               "@p_ordertype_code, @p_port_date_from, @p_port_date_to, @p_receive_date_from, @p_receive_date_to, @p_goodstype_code, @p_radio, @p_bill_date_from, @p_bill_date_to,@p_CkdPio,@p_lotno";

            IEnumerable<InvCkdContainerListDto> result = await _dapperRepo.QueryAsync<InvCkdContainerListDto>(_sql, new
            {
                p_container_no = input.ContainerNo,
                p_renban = input.Renban,
                p_supplier_no = input.SupplierNo,
                p_haisen_no = input.HaisenNo,
                p_bill_of_lading_no = input.BillOfLadingNo,
                p_invoice_no = input.InvoiceNo,
                p_ordertype_code = input.OrderTypeCode,
                p_port_date_from = input.PortDateFrom,
                p_port_date_to = input.PortDateTo,
                p_receive_date_from = input.ReceiveDateFrom,
                p_receive_date_to = input.ReceiveDateTo,
                p_goodstype_code = input.GoodsTypeCode,
                p_radio = input.radio,
                p_bill_date_from = input.BillDateFrom,
                p_bill_date_to = input.BillDateTo,
                p_CkdPio = input.CkdPio,
                p_lotno = input.LotNo,
            });

            var exportToExcel = result.ToList();
            return _containerListExcelExporter.ExportToFile(exportToExcel);
        }

        public async Task<FileDto> GetShipmentInfoDetailsToExcel(InvCkdContainerListExportInput input)
        {

            string _sql = "Exec INV_CKD_CONTAINER_LIST_EXPORT_SHIPMENT_INFO @p_container_no, @p_renban, @p_supplier_no, @p_haisen_no, @p_bill_of_lading_no, @p_invoice_no, " +
                "@p_ordertype_code, @p_port_date_from, @p_port_date_to, @p_receive_date_from, @p_receive_date_to, @p_goodstype_code, @p_radio,@p_CkdPio";
            IEnumerable<ShipmentInfoDetailListDto> result = await _dapperRepo.QueryAsync<ShipmentInfoDetailListDto>(_sql,
              new
              {
                  p_container_no = input.ContainerNo,
                  p_renban = input.Renban,
                  p_supplier_no = input.SupplierNo,
                  p_haisen_no = input.HaisenNo,
                  p_bill_of_lading_no = input.BillOfLadingNo,
                  p_invoice_no = input.InvoiceNo,
                  p_ordertype_code = input.OrderTypeCode,
                  p_port_date_from = input.PortDateFrom,
                  p_port_date_to = input.PortDateTo,
                  p_receive_date_from = input.ReceiveDateFrom,
                  p_receive_date_to = input.ReceiveDateTo,
                  p_goodstype_code = input.GoodsTypeCode,
                  p_radio = input.radio,
                  p_CkdPio = input.CkdPio
              });
            var listPoHeaderResult = result.ToList();

            return _containerListExcelExporter.ShipmentInfoDetailExportToFile(listPoHeaderResult);
        }

        public async Task<FileDto> GetShipmentInfoDetailsPxpToExcel(InvCkdContainerListExportInput input)
        {
            string _sql = "Exec INV_CKD_CONTAINER_LIST_EXPORT_SUMMARY @p_container_no, @p_renban, @p_supplier_no, @p_haisen_no, @p_bill_of_lading_no, @p_invoice_no, " +
                "@p_ordertype_code, @p_port_date_from, @p_port_date_to, @p_receive_date_from, @p_receive_date_to, @p_goodstype_code, @p_radio,@p_CkdPio";
            IEnumerable<ShipmentInfoDetailListDto> result = await _dapperRepo.QueryAsync<ShipmentInfoDetailListDto>(_sql,
              new
              {
                  p_container_no = input.ContainerNo,
                  p_renban = input.Renban,
                  p_supplier_no = input.SupplierNo,
                  p_haisen_no = input.HaisenNo,
                  p_bill_of_lading_no = input.BillOfLadingNo,
                  p_invoice_no = input.InvoiceNo,
                  p_ordertype_code = input.OrderTypeCode,
                  p_port_date_from = input.PortDateFrom,
                  p_port_date_to = input.PortDateTo,
                  p_receive_date_from = input.ReceiveDateFrom,
                  p_receive_date_to = input.ReceiveDateTo,
                  p_goodstype_code = input.GoodsTypeCode,
                  p_radio = input.radio,
                  p_CkdPio = input.CkdPio
              });
            var listPoHeaderResult = result.ToList();

            return _containerListExcelExporter.ShipmentInfoDetailPXPExportToFile(listPoHeaderResult);
        }

        public async Task<FileDto> GetInvoiceDetailsListToExcel(InvCkdContainerListExportInput input)
        {
            Dapper.SqlMapper.Settings.CommandTimeout = 900;
            string _sql = "Exec INV_CKD_CONTAINER_LIST_EXPORT_PART_INFO @p_container_no, @p_renban, @p_supplier_no, @p_haisen_no, @p_bill_of_lading_no, @p_invoice_no, " +
              "@p_ordertype_code, @p_port_date_from, @p_port_date_to, @p_receive_date_from, @p_receive_date_to, @p_goodstype_code, @p_radio,@p_CkdPio";
            IEnumerable<GetInvCkdContainerListExportInvoiceList> result = await _dapperRepo.QueryAsync<GetInvCkdContainerListExportInvoiceList>(_sql,
              new
              {
                  p_container_no = input.ContainerNo,
                  p_renban = input.Renban,
                  p_supplier_no = input.SupplierNo,
                  p_haisen_no = input.HaisenNo,
                  p_bill_of_lading_no = input.BillOfLadingNo,
                  p_invoice_no = input.InvoiceNo,
                  p_ordertype_code = input.OrderTypeCode,
                  p_port_date_from = input.PortDateFrom,
                  p_port_date_to = input.PortDateTo,
                  p_receive_date_from = input.ReceiveDateFrom,
                  p_receive_date_to = input.ReceiveDateTo,
                  p_goodstype_code = input.GoodsTypeCode,
                  p_radio = input.radio,
                  p_CkdPio = input.CkdPio
              });
            var list = result.ToList();
            return _containerListExcelExporter.InvoiceInfoDetailExportToFile(list);
        }

        public async Task<FileDto> GetNoCustomsDeclareToExcel()
        {
            string _sql = "Exec INV_CKD_CONTAINER_LIST_EXPORT_NO_DECLARE_CUSTOMS ";
            IEnumerable<GetInvCkdContainerListExportNoDeclareCustomsList> result = await _dapperRepo.QueryAsync<GetInvCkdContainerListExportNoDeclareCustomsList>(_sql,
              new
              {

              });
            var list = result.ToList();
           
            return _containerListExcelExporter.ListNoCutomsDeclareExportToFile(list);
        }

        public async Task<PagedResultDto<InvCkdContainerInvoiceDto>> GetDataContainerInvoiceByCont(GetInvCkdContIntransitbyContInput input)
        {
            string _sql = "Exec INV_CKD_CONTAINER_INVOICE_SEARCH_BY_CONT @p_container_no, @p_renban";

            IEnumerable<InvCkdContainerInvoiceDto> result = await _dapperRepo.QueryAsync<InvCkdContainerInvoiceDto>(_sql, new
            {
                p_container_no = input.containerNo,
                p_renban = input.Renban
            });

            var listResult = result.ToList();

            var totalCount = result.ToList().Count();
            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            return new PagedResultDto<InvCkdContainerInvoiceDto>(
                totalCount,
                pagedAndFiltered);

        }

        public async Task<FileDto> GetDataContainerInvoiceByContToExcel(GetInvCkdContIntransitbyContInput input)
        {
            string _sql = "Exec INV_CKD_CONTAINER_INVOICE_SEARCH_BY_CONT @p_container_no, @p_renban";

            IEnumerable<InvCkdContainerInvoiceDto> result = await _dapperRepo.QueryAsync<InvCkdContainerInvoiceDto>(_sql, new
            {
                p_container_no = input.containerNo,
                p_renban = input.Renban
            });

            var exportToExcel = result.ToList();
            return _containerListExcelExporter.ExportContainerInvoicebyContToFile(exportToExcel);
        }


        public async Task<PagedResultDto<ImportDevanDto>> GetImportDevan(GetInvCkdContainerListInput input)
        {
            string _sql = "Exec INV_CKD_CONTAINERLIST_SEARCH_IMPORT_DEVAN @p_container_no,@p_renban,@p_supplier_no,@p_haisen_no,@p_bill_of_lading_no,@p_invoice_no,@p_ordertype_code,@p_port_date_from,@p_port_date_to,@p_receive_date_from, @p_receive_date_to,@p_goodstype_code,@p_radio,@p_CkdPio";

            IEnumerable<ImportDevanDto> result = await _dapperRepo.QueryAsync<ImportDevanDto>(_sql, new
            {
                p_container_no = input.ContainerNo,
                p_renban = input.Renban,
                p_supplier_no = input.SupplierNo,
                p_haisen_no = input.HaisenNo,
                p_bill_of_lading_no = input.BillOfLadingNo,
                p_invoice_no = input.InvoiceNo,
                p_ordertype_code = input.OrderTypeCode,
                p_port_date_from = input.PortDateFrom,
                p_port_date_to = input.PortDateTo,
                p_receive_date_from = input.ReceiveDateFrom,
                p_receive_date_to = input.ReceiveDateTo,
                p_goodstype_code = input.GoodsTypeCode,
                p_radio = input.radio,
                p_CkdPio = input.CkdPio
            });

            var listResult = result.ToList();

            var totalCount = result.ToList().Count();
            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            return new PagedResultDto<ImportDevanDto>(
                totalCount,
                pagedAndFiltered);


        }
        //
        public async Task<FileDto> GetImPortDeVanExcel(GetGetImPortDeVanList input)
        {
            string _sql = "Exec INV_CKD_CONTAINERLIST_SEARCH_IMPORT_DEVAN @p_container_no,@p_renban,@p_supplier_no,@p_haisen_no,@p_bill_of_lading_no,@p_invoice_no,@p_ordertype_code,@p_port_date_from,@p_port_date_to,@p_receive_date_from, @p_receive_date_to,@p_goodstype_code,@p_radio,@p_CkdPio";

            IEnumerable<ImportDevanDto> result = await _dapperRepo.QueryAsync<ImportDevanDto>(_sql, new
            {
                p_container_no = input.ContainerNo,
                p_renban = input.Renban,
                p_supplier_no = input.SupplierNo,
                p_haisen_no = input.HaisenNo,
                p_bill_of_lading_no = input.BillOfLadingNo,
                p_invoice_no = input.InvoiceNo,
                p_ordertype_code = input.OrderTypeCode,
                p_port_date_from = input.PortDateFrom,
                p_port_date_to = input.PortDateTo,
                p_receive_date_from = input.ReceiveDateFrom,
                p_receive_date_to = input.ReceiveDateTo,
                p_goodstype_code = input.GoodsTypeCode,
                p_radio = input.radio,
                p_CkdPio = input.CkdPio

            });
            var list = result.ToList();
            return _containerListExcelExporter.ListImPortDeVanExportToFile(list);

        }

        public async Task<FileDto> GetContainerInvoiceAllToExcel(InvCkdContainerListExportInput input)
        {
            string _sql = "Exec INV_CKD_CONTAINER_INVOICE_SEARCH_BY_CONT_ALL @p_container_no, @p_renban, @p_supplier_no, @p_haisen_no, @p_bill_of_lading_no, @p_invoice_no, " +
               "@p_ordertype_code, @p_port_date_from, @p_port_date_to, @p_receive_date_from, @p_receive_date_to, @p_goodstype_code, @p_radio, @p_bill_date_from, @p_bill_date_to,@p_CkdPio";

            IEnumerable<InvCkdContainerInvoiceDto> result = await _dapperRepo.QueryAsync<InvCkdContainerInvoiceDto>(_sql, new
            {
                p_container_no = input.ContainerNo,
                p_renban = input.Renban,
                p_supplier_no = input.SupplierNo,
                p_haisen_no = input.HaisenNo,
                p_bill_of_lading_no = input.BillOfLadingNo,
                p_invoice_no = input.InvoiceNo,
                p_ordertype_code = input.OrderTypeCode,
                p_port_date_from = input.PortDateFrom,
                p_port_date_to = input.PortDateTo,
                p_receive_date_from = input.ReceiveDateFrom,
                p_receive_date_to = input.ReceiveDateTo,
                p_goodstype_code = input.GoodsTypeCode,
                p_radio = input.radio,
                p_bill_date_from = input.BillDateFrom,
                p_bill_date_to = input.BillDateTo,
                p_CkdPio = input.CkdPio
            });

            var exportToExcel = result.ToList();
            return _containerListExcelExporter.ExportContainerInvoicebyContToFile(exportToExcel);
        }


        public async Task<FileDto> GetContainerListByPeriodToExcel(int? periodid)
        {
            //Dapper.SqlMapper.Settings.CommandTimeout = 900;
            string _sql = "Exec INV_CKD_CONTAINERLIST_BY_PERIOD @p_Period";

            IEnumerable<InvCkdContainerListDto> result = await _dapperRepo.QueryAsync<InvCkdContainerListDto>(_sql, new
            {
                p_Period = periodid
            });

            var exportToExcel = result.ToList();
            return _containerListExcelExporter.ExportToFileByPeriod(exportToExcel);
        }
    }

}
