using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.Tmss.Exporting;
using prod.Inventory.Tmss.Dto;
using prod.Storage;
using Abp.AspNetZeroCore.Net;
using GemBox.Spreadsheet;
using Newtonsoft.Json.Linq;
using prod.Common;
using prod.Master.Pio.Exporting;
using System.IO;
using System.Linq;
using System;
using Microsoft.Extensions.Logging;

namespace prod.Inventory.Tmss.Exporting
{
    public class InvTmssDispatchPlanExcelExporter : NpoiExcelExporterBase, IInvTmssDispatchPlanExcelExporter
    {
        public readonly ITempFileCacheManager _tempFileCacheManager;
        public readonly ILogger _logger;
        public InvTmssDispatchPlanExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) {
            _tempFileCacheManager = tempFileCacheManager;
        }
        public FileDto ExportToFile(List<InvTmssDispatchPlanDto> tmssdispatchplan)
        {
            return CreateExcelPackage(
                "InventoryTmssTmssDispatchPlan.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("TmssDispatchPlan");
                    AddHeader(
                                sheet,
                                ("VehicleType"),
                                ("Model"),
                                ("MarketingCode"),
                                ("ProductionCode"),    
                                ("Vin"),
                                ("Dealer"),
                                ("ExtColor"),
                                ("IntColor"),
                                ("Katashiki"),
                                ("DlrDispatchPlan"),
                                ("DlrDispatchDate"),
                                ("PInstallDate"),
                                ("InstallDate"),
                                ("Route")



                               );
                    AddObjects(
                         sheet,  tmssdispatchplan,
                                _ => _.VehicleType,
                                _ => _.Model,
                                _ => _.MarketingCode,
                                _ => _.ProductionCode,                                
                                _ => _.Vin,
                                _ => _.Dealer,
                                _ => _.ExtColor,
                                _ => _.IntColor,
                                _ => _.Katashiki,
                                _ => _.DlrDispatchPlan,
                                _ => _.DlrDispatchDate,
                                _ => _.PInstallDate,
                                _ => _.InstallDate,
                                _ => _.Route

                                );

                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });

        }
        public FileDto ExportToHistoricalFile(List<string> data)
        {
            string fileName = "InvTmssDispatchPlanHistorical.xlsx";
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
                        if (!allHeaders.Contains(prop.Name) && !exceptCols.Contains(prop.Name))
                        {
                            allHeaders.Add(prop.Name);
                        }
                    }
                }

                var properties = allHeaders.Where(x => !exceptCols.Contains(x)).ToArray();

                //Mapping data
                MappingData(ref rowDatas, ref properties);


                Commons.FillHistoriesExcel(rowDatas, workSheet, 1, 0, properties, properties);
                xlWorkBook.Save(tempFile);
                using (var obj_stream = new MemoryStream(File.ReadAllBytes(tempFile)))
                {
                    _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[EXCEPTION] While exporting {nameof(InvTmssDispatchPlanExcelExporter)} with error: {ex}");
            }
            finally
            {
                File.Delete(tempFile);
            }

            return file;
        }
        private void MappingData(ref List<JObject> rowDatas, ref string[] properties)
        {
            /// Mapping row data
            /// add new here
            var dataMapping = new Dictionary<string, Dictionary<string, string>>()
            {
                {
                    "Action", new Dictionary<string, string>() {
                        {"1", "Xoá"},
                        {"2", "Tạo mới"},
                        {"3", "Trước update"},
                        {"4", "Sau update"},
                    }
                    //"Header1", new Dictionary<string, string>()
                    //{
                    //    {"1", "Xoá"},
                    //    {"2", "Tạo mới"},
                    //    {"3", "Trước update"},
                    //    {"4", "Sau update"},
                    //},
                },
            };

            foreach (var header in dataMapping)
            {
                rowDatas.ConvertAll(x =>
                    x[header.Key] = dataMapping[header.Key][x[header.Key].ToString()]
                );
            }
        }
    }
}
