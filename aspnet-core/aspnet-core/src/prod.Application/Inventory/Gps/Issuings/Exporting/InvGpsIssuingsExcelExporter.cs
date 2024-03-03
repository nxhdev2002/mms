using Abp.AspNetZeroCore.Net;
using GemBox.Spreadsheet;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using prod.Common;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.Gps.Issuings.Dto;
using prod.Inventory.GPS.Dto;
using prod.Inventory.GPS.Exporting;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace vovina.Inventory.GPS.Exporting
{
    public class InvGpsIssuingsExcelExporter : NpoiExcelExporterBase, IInvGpsIssuingsExcelExporter
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly ILogger _logger;
        public InvGpsIssuingsExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) {
            _tempFileCacheManager = tempFileCacheManager;
        }
        public FileDto ExportToHistoricalFile(List<string> data)
        {
            string fileName = "InventoryGpsIssuingsHistorical.xlsx";
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
                _logger.LogError($"[EXCEPTION] While exporting {nameof(InvGpsIssuingsExcelExporter)} with error: {ex}");
            }
            finally
            {
                File.Delete(tempFile);
            }

            return file;
        }

    }
}
