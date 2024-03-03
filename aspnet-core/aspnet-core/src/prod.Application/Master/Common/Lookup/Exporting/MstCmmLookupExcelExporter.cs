using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Common.Dto;
using prod.Storage;
using prod.Master.Common.Exporting;
using Abp.AspNetZeroCore.Net;
using GemBox.Spreadsheet;
using Newtonsoft.Json.Linq;
using prod.Common;
using prod.Master.Pio.Exporting;
using System.IO;
using System.Linq;
using System;
using Microsoft.Extensions.Logging;

namespace prod.Master.Common.Exporting
{
    public class MstCmmLookupExcelExporter : NpoiExcelExporterBase, IMstCmmLookupExcelExporter
    {
        public readonly ITempFileCacheManager _tempFileCacheManager;
        public readonly ILogger _logger;
        public MstCmmLookupExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) {
            _tempFileCacheManager = tempFileCacheManager;
        }
        public FileDto ExportToFile(List<MstCmmLookupDto> lookup)
        {
            return CreateExcelPackage(
                                 "MasterCommonLookup.xlsx",
                                 excelPackage =>
                                 {
                                     var sheet = excelPackage.CreateSheet("Lookup");
                                     AddHeader(
                                               sheet,
                                                      ("DomainCode"),
                                                        ("ItemCode"),
                                                        ("ItemValue"),
                                                        ("ItemOrder"),
                                                        ("Description"),
                                                        ("IsUse"),
                                                        ("IsRestrict")

                                               );
                                     AddObjects(
                                         sheet, lookup,

                                         _ => _.DomainCode,
                                        _ => _.ItemCode,
                                        _ => _.ItemValue,
                                        _ => _.ItemOrder,
                                        _ => _.Description,
                                        _ => _.IsUse,
                                        _ => _.IsRestrict
                                         );
                                 });

        }
        public FileDto ExportToHistoricalFile(List<string> data)
        {
            string fileName = "MstCmmLookupHistorical.xlsx";
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
                _logger.LogError($"[EXCEPTION] While exporting {nameof(MstCmmLookupExcelExporter)} with error: {ex}");
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
