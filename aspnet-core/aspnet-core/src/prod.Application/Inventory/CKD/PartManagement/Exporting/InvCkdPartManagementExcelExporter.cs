using System.Collections.Generic;
using Abp.AspNetZeroCore.Net;
using GemBox.Spreadsheet;
using NPOI.SS.Formula.Functions;
using System.IO;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Storage;
using prod.Common;
using System;
using Newtonsoft.Json.Linq;
using prod.Master.Pio.Exporting;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace prod.Inventory.CKD.Exporting
{
    public class InvCkdPartManagementExcelExporter : NpoiExcelExporterBase, IInvCkdPartManagementExcelExporter
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly ILogger _logger;
        public InvCkdPartManagementExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) {
            _tempFileCacheManager = tempFileCacheManager;
        }
        public FileDto ExportToFile(List<InvCkdPartManagementDto> shipment)
        {
            var file = new FileDto("InventoryCKDPartManagement.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_InventoryCKDPartManagement";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];
          

            List<string> listHeader = new List<string>();
            listHeader.Add("PartNo");
            listHeader.Add("Cfc");
            listHeader.Add("SupplierNo");
            listHeader.Add("PartName");
            listHeader.Add("LotNo");
            listHeader.Add("OrderNo");
            listHeader.Add("Qty");
            listHeader.Add("Fixlot");
            listHeader.Add("ModuleCaseNo");
            listHeader.Add("Firmpackingmonth");
            listHeader.Add("CarName");
            listHeader.Add("InvoiceNo");
            listHeader.Add("InvoiceDate");
            listHeader.Add("ContainerNo");
            listHeader.Add("Renban");
            listHeader.Add("BillNo");
            listHeader.Add("BillDate");
            listHeader.Add("ShipmentNo");
            listHeader.Add("SealNo");
            listHeader.Add("CdDate");
            listHeader.Add("CdStatus");
            listHeader.Add("ContainerSize");
            listHeader.Add("ETD");
            listHeader.Add("ETA");
            listHeader.Add("ReceiveDate");
            listHeader.Add("ATA");
            listHeader.Add("PortTransitDate");
            listHeader.Add("UnpackingDate");
            listHeader.Add("StorageLocationCode");
            listHeader.Add("Fob");
            listHeader.Add("Freight");
            listHeader.Add("Insurance");
            listHeader.Add("Cif");
            listHeader.Add("Tax");
            listHeader.Add("Amount");


            List<string> listExport = new List<string>();
            listExport.Add("PartNo");
            listExport.Add("CarfamilyCode");
            listExport.Add("SupplierNo");
            listExport.Add("PartName");
            listExport.Add("LotNo");
            listExport.Add("OrderNo");
            listExport.Add("UsageQty");
            listExport.Add("Fixlot");
            listExport.Add("ModuleCaseNo");
            listExport.Add("Firmpackingmonth");
            listExport.Add("CarName");
            listExport.Add("InvoiceNo");
            listExport.Add("InvoiceDate");
            listExport.Add("ContainerNo");
            listExport.Add("Renban");
            listExport.Add("BillofladingNo");
            listExport.Add("BillDate");
            listExport.Add("ShipmentNo");
            listExport.Add("SealNo");
            listExport.Add("CdDate");
            listExport.Add("CdStatus");
            listExport.Add("ContainerSize");
            listExport.Add("ShippingDate");
            listExport.Add("PortDate");
            listExport.Add("ReceiveDate");
            listExport.Add("PortDateActual");
            listExport.Add("PortTransitDate");
            listExport.Add("UnpackingDate");
            listExport.Add("StorageLocationCode");
            listExport.Add("Fob");
            listExport.Add("Freight");
            listExport.Add("Insurance");
            listExport.Add("Cif");
            listExport.Add("Tax");
            listExport.Add("Amount");
    


            string[] properties = listExport.ToArray();
            string[] p_header = listHeader.ToArray();
            Commons.FillExcel2(shipment, workSheet, 1, 0, properties, p_header);

            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);
            MemoryStream obj_stream = new MemoryStream();
            obj_stream = new MemoryStream(File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;
        }

        public FileDto ExportReportToFile(List<InvCkdPartManagementReportDto> reports)
        {
            return CreateExcelPackage(
                "InvCkdPartManagementReport.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Report");
                    AddHeader(
                                sheet,
                                ("SupplierNo"),
                                ("PartNo"),
                                ("PartName"),
                                ("FixLot"),
                                ("GoodstypeCode"),
                                ("ContainerNo"),
                                ("Total UsageQty"),
                                ("Fob"),
                                ("Total Fob"),
                                ("Cif"),
                                ("Total Cif"),
                                ("Tax"),
                                ("Total Tax"),
                                ("Vat"),
                                ("Total Vat"),
                                ("Invoice No"),
                                ("Invoice Date"),
                                ("Ordertype Code"),
                                ("Customs Declare No"),
                                ("Declare Date"),
                                ("FirmpackingMonth")
                               );
                    AddObjects(
                         sheet, reports,
                             _ => _.SupplierNo,
                             _ => _.PartNo,
                             _ => _.PartName,
                             _ => _.fixlot,
                             _ => _.GoodstypeCode,
                             _ => _.ContainerNo,
                             _ => _.sum_usageqty,
                             _ => _.fob,
                             _ => _.sum_fob,
                             _ => _.cif,
                             _ => _.sum_cif,
                             _ => _.tax,
                             _ => _.sum_tax,
                             _ => _.vat,
                             _ => _.sum_vat,
                             _ => _.InvoiceNo,
                             _ => _.InvoiceDate,
                             _ => _.OrdertypeCode,
                             _ => _.CustomsDeclareNo,
                             _ => _.DeclareDate,
                             _ => _.firmpackingmonth
                                );
                });
        }

        public FileDto ExportToHistoricalFile(List<string> data)
        {
            string fileName = "InventoryCKDPartManagementHistorical.xlsx";
            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            var allHeaders = new List<string>();
            var rowDatas = new List<JObject>();
            var exceptCols = new List<string>()
            {
                "UpdateMask",

            };

            try
            {
                SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                var xlWorkBook = new ExcelFile();
                var workSheet = xlWorkBook.Worksheets.Add("Sheet1");

                foreach (var item in data)
                {
                    var json = JObject.Parse(item);
                    rowDatas.Add(json);
                    foreach (var prop in json.Properties())
                    {
                        if (prop.Name == "Action")
                        {

                            switch (prop.Value.ToString())
                            {
                                case "1":
                                    prop.Value = "Xóa";
                                    break;
                                case "2":
                                    prop.Value = "Tạo mới";
                                    break;
                                case "3":
                                    prop.Value = "Trước Update";
                                    break;
                                case "4":
                                    prop.Value = "Sau Update";
                                    break;
                            }
                        }
                        if (!allHeaders.Contains(prop.Name) && !exceptCols.Contains(prop.Name))
                        {
                            allHeaders.Add(prop.Name);
                        }
                    }
                }

                var ins = new object();
                var properties = allHeaders.Where(x => !exceptCols.Contains(x))
                                            .ToArray();

                Commons.FillHistoriesExcel(rowDatas, workSheet, 1, 0, properties, properties);
                xlWorkBook.Save(tempFile);
                using (var obj_stream = new MemoryStream(File.ReadAllBytes(tempFile)))
                {
                    _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[EXCEPTION] While exporting {nameof(InvCkdPartManagementExcelExporter)} with error: {ex}");
            }
            finally
            {
                File.Delete(tempFile);
            }

            return file;
        }
    }
}
