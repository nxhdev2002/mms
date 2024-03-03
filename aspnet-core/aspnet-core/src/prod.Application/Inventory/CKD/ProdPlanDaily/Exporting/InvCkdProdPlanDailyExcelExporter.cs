using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Exporting;
using prod.Inventory.CKD.Dto;
using prod.Storage;
using prod.Inventory.CKD.Dto;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.Exporting;
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
using NPOI.Util.Collections;
namespace prod.Inventory.CKD.Exporting
{
    public class InvCkdProdPlanDailyExcelExporter : NpoiExcelExporterBase, IInvCkdProdPlanDailyExcelExporter
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly ILogger _logger;

        public InvCkdProdPlanDailyExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) {
            _tempFileCacheManager = tempFileCacheManager;
        }
        public FileDto ExportToFile(List<InvCkdProdPlanDailyDto> ProdPlanDaily)
        {
            return CreateExcelPackage(
                "InventoryCKDProdPlanDaily.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ProdPlanDaily");
                    AddHeader(
                                sheet,
                                ("No"),
                                ("Prodline"),
                                ("Cfc"),
                                ("Model"),
                                ("Grade"),
                                ("Color"),
                                ("Bodyno"),
                                ("Lotno"),
                                ("Noinlot"),
                                ("Vinno"),
                                ("Winplandate"),
                               // ("Winplantime"),
                                ("Winplandatetime"),
                                ("Woutplandate"),
                               // ("Woutplantime"),
                                ("Woutplandatetime"),
                                ("Tinplandate"),
                               // ("Tinplantime"),
                                ("Tinplandatetime"),
                                ("Toutplandate"),
                               // ("Toutplantime"),
                                ("Toutplandatetime"),
                                ("Ainplandate"),
                              //  ("Ainplantime"),
                                ("Ainplandatetime"),
                                ("Aoutplandate"),
                              //  ("Aoutplantime"),
                                ("Aoutplandatetime"),
                                ("Lineoffdate"),
                              //  ("Lineofftime"),
                                ("Lineoffdatetime"),
                                ("Pdidate"),
                             //   ("Pditime"),
                                ("Pdidatetime"),
                                ("Isproject"),
                                ("Vehicleid"),
                                ("Indentline"),
                                ("Colortype"),
                                ("Engineid"),
                                ("Goshicar"),
                                ("Salessfx"),
                                ("EdNumber"),
                                ("Ssno"),
                                ("Transid")

                               );
                    AddObjects(
                         sheet, ProdPlanDaily,
                                _ => _.No,
                                _ => _.Prodline,
                                _ => _.Cfc,
                                _ => _.Model,
                                _ => _.Grade,
                                _ => _.Color,
                                _ => _.BodyNo,
                                _ => _.LotNo,
                                _ => _.NoInLot,
                                _ => _.VinNo,
                                _ => _.WInPlanDate,
                              //  _ => _.Winplantime,
                                _ => _.WInPlanDateTime,
                                _ => _.WOutPlanDate,
                              //  _ => _.Woutplantime,
                                _ => _.WOutPlanDateTime,
                                _ => _.TInPlanDate,
                              //  _ => _.Tinplantime,
                                _ => _.TInPlanDatetime,
                                _ => _.TOutPlanDate,
                               // _ => _.Toutplantime,
                                _ => _.TOutPlanDatetime,
                                _ => _.AInPlanDate,
                              //  _ => _.Ainplantime,
                                _ => _.AInPlanDatetime,
                                _ => _.AOutPlanDate,
                               // _ => _.Aoutplantime,
                                _ => _.AOutPlanDateTime,
                                _ => _.LineoffDate,
                               // _ => _.Lineofftime,
                                _ => _.LineoffDateTime,
                                _ => _.PdiDate,
                             //  _ => _.Pditime,
                                _ => _.PdiDateTime,
                                _ => _.Isproject,
                                _ => _.Vehicleid,
                                _ => _.Indentline,
                                _ => _.Colortype,
                                _ => _.Engineid,
                                _ => _.Goshicar,
                                _ => _.Salessfx,
                                _ => _.EdNumber,
                                _ => _.Ssno,
                                _ => _.Transid

                                );

                  
                });

        }

        public FileDto ExportToHistoricalFile(List<string> data)
        {
            string fileName = "InventoryCkdProdPlanDailyHistorical.xlsx";
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
                _logger.LogError($"[EXCEPTION] While exporting {nameof(InvCkdProdPlanDailyExcelExporter)} with error: {ex}");
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
