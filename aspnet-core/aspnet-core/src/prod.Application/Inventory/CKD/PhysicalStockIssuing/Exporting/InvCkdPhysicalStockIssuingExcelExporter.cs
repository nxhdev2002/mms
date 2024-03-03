using Abp.AspNetZeroCore.Net;
using GemBox.Spreadsheet;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.PhysicalStockIssuing.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using prod.Common;

namespace prod.Inventory.CKD.Exporting
{
    public class InvCkdPhysicalStockIssuingExcelExporter : NpoiExcelExporterBase, IInvCkdPhysicalStockIssuingExcelExporter
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        public InvCkdPhysicalStockIssuingExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager)
        {
            _tempFileCacheManager = tempFileCacheManager;
        }
        public FileDto ExportToFile(List<InvCkdPhysicalStockIssuingDto> physicalstockissuing)
        {
            var file = new FileDto("InventoryCKDPhysicalStockIssuing.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_InventoryCKDPhysicalStockIssuing";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];


            List<string> listHeader = new List<string>();
            listHeader.Add("PartNo");
            listHeader.Add("ColorSfx");
            listHeader.Add("Cfc");
            listHeader.Add("SupplierNo");
            listHeader.Add("UseLot");
            listHeader.Add("LotNo");
            listHeader.Add("NoInLot");
            listHeader.Add("Qty");
            listHeader.Add("VinNo");
            listHeader.Add("BodyNo");
            listHeader.Add("Color");
            listHeader.Add("PartName");
            listHeader.Add("TransactionDatetime");
            listHeader.Add("WorkingDate");
            listHeader.Add("MaterialId");


            List<string> listExport = new List<string>();
            listExport.Add("PartNo");
            listExport.Add("ColorSfx");
            listExport.Add("Cfc");
            listExport.Add("SupplierNo");
            listExport.Add("UseLot");
            listExport.Add("LotNo");
            listExport.Add("NoInLot");
            listExport.Add("Qty");
            listExport.Add("VinNo");
            listExport.Add("BodyNo");
            listExport.Add("Color");
            listExport.Add("PartName");
            listExport.Add("TransactionDatetime");
            listExport.Add("WorkingDate");
            listExport.Add("MaterialId");


            string[] properties = listExport.ToArray();
            string[] p_header = listHeader.ToArray();
            Commons.FillExcel2(physicalstockissuing, workSheet, 1, 0, properties, p_header);

            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);
            MemoryStream obj_stream = new MemoryStream();
            obj_stream = new MemoryStream(File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;

        }

        public FileDto ExportToFileDetailsData(List<InvCkdPhysicalStockIssuingDetailsDataDto> detailsdata)
        {
            var file = new FileDto("InvCKDPhysStockIssuingDetailsData.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_InvCKDPhysStockIssuingDetailsData";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];


            List<string> listHeader = new List<string>();
            listHeader.Add("Part No");
            listHeader.Add("Cfc");
            listHeader.Add("Supplier No");
            listHeader.Add("Lot No");
            listHeader.Add("Module No");
            listHeader.Add("Container No");
            listHeader.Add("Invoice No");
            listHeader.Add("Out Qty");
            listHeader.Add("Part Name");


            List<string> listExport = new List<string>();
            listExport.Add("PartNo");
            listExport.Add("Cfc");
            listExport.Add("SupplierNo");
            listExport.Add("LotNo");
            listExport.Add("ModuleNo");
            listExport.Add("ContainerNo");
            listExport.Add("InvoiceNo");
            listExport.Add("Out_Qty");
            listExport.Add("PartName");



            string[] properties = listExport.ToArray();
            string[] p_header = listHeader.ToArray();
            Commons.FillExcel2(detailsdata, workSheet, 1, 0, properties, p_header);

            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);
            MemoryStream obj_stream = new MemoryStream();
            obj_stream = new MemoryStream(File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;

         /*   return CreateExcelPackage(
              "InvCKDPhysStockIssuingDetailsData.xlsx",
              excelPackage =>
              {
                  var sheet = excelPackage.CreateSheet("PhysStockIssuingDetailsData");
                  AddHeader(
                              sheet,
                              ("Part No"),
                              ("Cfc"),
                              ("Supplier No"),
                              ("Lot No"),
                              ("Module No"),
                              ("Container No"),
                              ("Invoice No"),
                              ("Out Qty"),
                              ("Part Name")
                             );
                  AddObjects(
                       sheet, detailsdata,
                              _ => _.PartNo,
                              _ => _.Cfc,
                              _ => _.SupplierNo,
                              _ => _.LotNo,
                              _ => _.ModuleNo,
                              _ => _.ContainerNo,
                              _ => _.InvoiceNo,
                              _ => _.Out_Qty,
                              _ => _.PartName
                              );
              });*/
        }

        public FileDto ExportReportSummaryToFile(List<InvCkdPhysicalStockIssuingReportSummartDto> physicalstockissuing)
        {
            var file = new FileDto("InventoryCKDPhysicalStockIssuingReport.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_InventoryCKDPhysicalStockIssuingReport";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];


            List<string> listHeader = new List<string>();
            listHeader.Add("Part No");
            listHeader.Add("Part Name");
            listHeader.Add("Cfc");
            listHeader.Add("Supplier No");
            listHeader.Add("Qty");
            listHeader.Add("WorkingDate");


            List<string> listExport = new List<string>();
            listExport.Add("PartNo");
            listExport.Add("PartName");
            listExport.Add("Cfc");
            listExport.Add("SupplierNo");          
            listExport.Add("Qty");
            listExport.Add("WorkingDate");


            string[] properties = listExport.ToArray();
            string[] p_header = listHeader.ToArray();
            Commons.FillExcel2(physicalstockissuing, workSheet, 1, 0, properties, p_header);

            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);
            MemoryStream obj_stream = new MemoryStream();
            obj_stream = new MemoryStream(File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;

        }

    }
}
