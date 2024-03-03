using Abp.AspNetZeroCore.Net;
using GemBox.Spreadsheet;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.ReceivingPhysicalStock.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using prod.Common;

namespace prod.Inventory.CKD.Exporting
{
    public class InvCkdStockReceivingExcelExporter : NpoiExcelExporterBase, IInvCkdStockReceivingExcelExporter
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        public InvCkdStockReceivingExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) {
            _tempFileCacheManager = tempFileCacheManager;
        }
        public FileDto ExportToFile(List<InvCkdStockReceivingDto> stockReceiving)
        {
            return CreateExcelPackage(
                "InventoryCKDStockReceiving.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("StockReceiving");
                    AddHeader(
                                sheet,
                                ("PartNo"),
                                ("ColorSfx"),
                                ("Cfc"),
                                ("SupplierNo"),
                                ("ContainerNo"),
                                ("InvoiceNo"),
                                ("Qty"),
                                ("ParName"),
                                ("TransactionDatetime"),
                                ("WorkingDate"),
                                ("MaterialId")
                                );
                    AddObjects(
                         sheet, stockReceiving,
                                _ => _.PartNoNormalizedS4,
                                _ => _.ColorSfx,
                                _ => _.Cfc,
                                _ => _.SupplierNo,
                                _ => _.ContainerNo,
                                _ => _.InvoiceNo,
                                _ => _.Qty,
                                _ => _.PartName,
                                _ => _.TransactionDatetime,
                                _ => _.WorkingDate,
                                _ => _.MaterialId
                                );

                });

        }

        public FileDto ExportByMaterialToFile(List<InvCkdStockPartByMaterialDto> invckdstockreceivingbymaterial)
        {
            return CreateExcelPackage(
                "InvCkdStockReceivingByMaterial.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("StockReceivingByMasterial");
                    AddHeader(
                                sheet,
                                ("PartNo"),
                                ("Description"),
                                ("ValuationType"),
                                ("Qty"),
                                ("WorkingDate"),
                                ("LastCalDatetime"),
                                ("MaterialId")
                               );
                    AddObjects(
                         sheet, invckdstockreceivingbymaterial,
                                _ => _.PartNo,
                                _ => _.Description,
                                _ => _.ValuationType,
                                _ => _.Qty,
                                _ => _.WorkingDate,
                                _ => _.LastCalDatetime,
                                _ => _.MaterialId
                                );


                });

        }

        public FileDto ExportToFileDetailsData(List<InvCkdReceivingPhysStockDetailsDataDto> detailsdata)
        {
            var file = new FileDto("tInvCKDPhysStockReceivingDetailsData.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_InvCKDPhysStockReceivingDetailsData";
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
       
        }

        public FileDto ExportValidateToFile(List<InvCkdStockReceivingValidateDto> stockReceivingValidate)
        {
            return CreateExcelPackage(
             "InventoryCKDStockReceivingValidate.xlsx",
             excelPackage =>
             {
                 var sheet = excelPackage.CreateSheet("StockReceivingValidate");
                 AddHeader(
                                sheet,
                                ("PartNo"),
                                ("SupplierNo"),
                                ("Cfc"),
                                ("ContainerNo"),
                                ("Renban"),
                                ("ReceiveDate"),
                                ("Status"),
                                ("CdDate"),
                                ("ErrDesc")

                               );
                 AddObjects(
                         sheet, stockReceivingValidate,
                                _ => _.PartNo,
                                _ => _.SupplierNo,
                                _ => _.Cfc,
                                _ => _.ContainerNo,
                                _ => _.Renban,
                                _ => _.ReceiveDate,
                                _ => _.Status,
                                _ => _.CdDate,
                                _ => _.ErrDesc
                                );

                
             });
        }
        public FileDto GetReceivingPhysicalStockToExcel(List<InvCkdReceivingPhysicalStockDto> receivingPhysicalStock)
        {
            var file = new FileDto("InventoryCKDPhysicalStockReceiving.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_InventoryCKDPhysicalStockReceiving";
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
            listHeader.Add("ContainerNo");
            listHeader.Add("Qty");
            listHeader.Add("InvoiceNo");
            listHeader.Add("PartName");
            listHeader.Add("TransactionDatetime");
            listHeader.Add("WorkingDate");
            listHeader.Add("MaterialId");
            listHeader.Add("LotNo");

            List<string> listExport = new List<string>();
            listExport.Add("PartNo");
            listExport.Add("ColorSfx");
            listExport.Add("Cfc");
            listExport.Add("SupplierNo");
            listExport.Add("ContainerNo");
            listExport.Add("Qty");
            listExport.Add("InvoiceNo");
            listExport.Add("PartName");
            listExport.Add("TransactionDatetime");
            listExport.Add("WorkingDate");
            listExport.Add("MaterialId");
            listExport.Add("LotNo");

            string[] properties = listExport.ToArray();
            string[] p_header = listHeader.ToArray();
            Commons.FillExcel2(receivingPhysicalStock, workSheet, 1, 0, properties, p_header);

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
