using Abp.AspNetZeroCore.Net;
using GemBox.Spreadsheet;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.PIO.StockReceiving.Dto;
using prod.Inventory.PIO.StockTransaction.Dto;
using prod.Inventory.PIO.StockTransaction.Exporting;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prod.Common;

namespace prod.Inventory.PIO.StockReceiving.Exporting
{
    public class InvPIOStockReceivingExcelExporter : NpoiExcelExporterBase, IInvPIOStockReceivingExcelExporter
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        public InvPIOStockReceivingExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) {
            _tempFileCacheManager = tempFileCacheManager;
        }
        public FileDto ExportToFile(List<InvPIOStockReceivingDto> stockreceiving)
        {
            var file = new FileDto("InventoryCKDStockReceiving.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_InventoryCKDStockReceiving";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];


            List<string> listHeader = new List<string>();
            listHeader.Add("Part No");
            listHeader.Add("Part Name");
            listHeader.Add("Mkt Code");
            listHeader.Add("Working Date");
            listHeader.Add("Qty");
            listHeader.Add("Trans Datetime");
            listHeader.Add("Vin No");
            listHeader.Add("Part Type");
            listHeader.Add("Shop");
            listHeader.Add("Car Type");
            listHeader.Add("Interior Color");
            listHeader.Add("Ext Color");
            listHeader.Add("Route");
            listHeader.Add("InvoiceNo");
            listHeader.Add("Source");



            List<string> listExport = new List<string>();
            listExport.Add("PartNo");
            listExport.Add("PartName");
            listExport.Add("MktCode");
            listExport.Add("WorkingDate");
            listExport.Add("Qty");
            listExport.Add("TransDatetime");
            listExport.Add("VinNo");
            listExport.Add("PartType");
            listExport.Add("Shop");
            listExport.Add("CarType");
            listExport.Add("InteriorColor");
            listExport.Add("ExtColor");
            listExport.Add("Route");
            listExport.Add("InvoiceNo");
            listExport.Add("Source");


            string[] properties = listExport.ToArray();
            string[] p_header = listHeader.ToArray();
            Commons.FillExcel2(stockreceiving, workSheet, 1, 0, properties, p_header);

            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);
            MemoryStream obj_stream = new MemoryStream();
            obj_stream = new MemoryStream(File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;
           /* return CreateExcelPackage(
                "InventoryPIOStockReceiving.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("stockReceiving");
                    AddHeader(
                                sheet,
                                ("Part No"),
                                ("Part Name"),
                                ("Mkt Code"),
                                ("Working Date"),
                                ("Qty"),
                                ("Trans Datetime"),
                                ("Vin No"),
                                ("Part Type"),
                                ("Shop"),
                                ("Car Type"),
                                ("Interior Color"),
                                ("Ext Color"),
                                ("Route"),
                                ("InvoiceNo"),
                                ("Source")
                               );
                    AddObjects(
                         sheet, stockreceiving,
                                _ => _.PartNo,
                                _ => _.PartName,
                                _ => _.MktCode,
                                _ => _.WorkingDate,
                                _ => _.Qty,
                                _ => _.TransDatetime,
                                _ => _.VinNo,
                                _ => _.PartType,
                                _ => _.Shop,
                                _ => _.CarType,
                                _ => _.InteriorColor,
                                _ => _.ExtColor,
                                _ => _.Route,
                                _ => _.InvoiceNo,
                                _ => _.Source
                                );
                });*/
        }
    }
}

