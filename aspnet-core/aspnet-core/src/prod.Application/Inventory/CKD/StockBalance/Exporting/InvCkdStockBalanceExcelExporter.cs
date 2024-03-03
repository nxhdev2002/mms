using Abp.AspNetZeroCore.Net;
using GemBox.Spreadsheet;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.PaymentRequest.Exporting;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prod.Common;

namespace prod.Inventory.CKD.StockBalance.Exporting
{
    public class InvCkdStockBalanceExcelExporter : NpoiExcelExporterBase, IInvCkdStockBalanceExcelExporter
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        public InvCkdStockBalanceExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) {
            _tempFileCacheManager = tempFileCacheManager;
        }

        public FileDto ExportToFileStockBalance(List<InvCkdStockBalanceDto> invckdstockbalance)
        {

            var file = new FileDto("InventoryCKDStockBalance.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_InventoryCKDStockBalance";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];


            List<string> listHeader = new List<string>();
            listHeader.Add("PartNo");
            listHeader.Add("Cfc");
            listHeader.Add("Supplier No");
            listHeader.Add("Color Sfx");
            listHeader.Add("Begining");
            listHeader.Add("Receiving");
            listHeader.Add("Issuing");
            listHeader.Add("Closing");
            listHeader.Add("Concept");
            listHeader.Add("Diff");



            List<string> listExport = new List<string>();
            listExport.Add("PartNoNormalizedS4");
            listExport.Add("Cfc");
            listExport.Add("Source");
            listExport.Add("ColorSfx");
            listExport.Add("Begining");
            listExport.Add("Receiving");
            listExport.Add("Issuing");
            listExport.Add("Closing");
            listExport.Add("Concept");
            listExport.Add("Diff");



            string[] properties = listExport.ToArray();
            string[] p_header = listHeader.ToArray();
            Commons.FillExcel2(invckdstockbalance, workSheet, 1, 0, properties, p_header);

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
