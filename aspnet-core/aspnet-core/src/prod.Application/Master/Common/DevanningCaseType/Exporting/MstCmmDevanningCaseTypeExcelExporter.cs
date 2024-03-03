using System;
using System.Collections.Generic;
using Abp.AspNetZeroCore.Net;
using GemBox.Spreadsheet;
using Newtonsoft.Json.Linq;
using prod.Common;
using System.IO;
using System.Linq;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Common.Dto;
using prod.Master.Common.Exporting;
using prod.Master.Inventory.Exporting;
using prod.Storage;
using Microsoft.Extensions.Logging;

namespace vovina.Master.Common.Exporting
{
    public class MstCmmDevanningCaseTypeExcelExporter : NpoiExcelExporterBase, IMstCmmDevanningCaseTypeExcelExporter
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly ILogger _logger;
        public MstCmmDevanningCaseTypeExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) 
        {
            _tempFileCacheManager = tempFileCacheManager;
        }
        public FileDto ExportToFile(List<MstCmmDevanningCaseTypeDto> devanningcasetype)
        {
            return CreateExcelPackage("MasterCommonDevanningCaseType.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("DevanningCaseType");
                    AddHeader(
                                sheet,
                                ("CaseNo"),
                                ("CarFamilyCode"),
                                ("ShoptypeCode"),
                                ("SupplierNo")
                                );
                    AddObjects(
                                sheet, devanningcasetype,
                            _ => _.CaseNo,
                            _ => _.CarFamilyCode,
                            _ => _.ShoptypeCode,
                            _ => _.SupplierNo
                            );
                });

        }

        public FileDto ExportToHistoricalFile(List<string> data)
        {
            string fileName = "MstCmmDevanningCaseTypeHistorical.xlsx";
            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            var allHeaders = new List<string>();
            var rowDatas = new List<JObject>();
            var exceptCols = new List<string>()
        {
            "UpdateMask",
            "LastModificationTime",
            "LastModifierUserId"
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


                /// Mapping Data
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
                _logger.LogError($"[EXCEPTION] While exporting {nameof(MstCmmDevanningCaseTypeExcelExporter)} with error: {ex}");
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
