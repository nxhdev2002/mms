using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Cmm.Exporting;
using prod.Master.Cmm.Dto;
using prod.Storage;
using prod.Master.Cmm.Dto;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Cmm.Dto;
using prod.Master.Cmm.Exporting;
using prod.Storage;
using prod.Inventory.DRM.Dto;
using Abp.AspNetZeroCore.Net;
using GemBox.Spreadsheet;
using Newtonsoft.Json.Linq;
using prod.Common;
using prod.Master.Pio.Exporting;
using System.IO;
using System.Linq;
using System;
using Microsoft.Extensions.Logging;

namespace prod.Master.Cmm.Exporting
{
    public class MstCmmMMCheckingRuleExcelExporter : NpoiExcelExporterBase, IMstCmmMMCheckingRuleExcelExporter
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly ILogger _logger;
        public MstCmmMMCheckingRuleExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) {
            _tempFileCacheManager = tempFileCacheManager;
        }
        public FileDto ExportToFile(List<MstCmmMMCheckingRuleDto> cmmmmcheckingrule)
        {
            return CreateExcelPackage(
                "MasterCmmCmmMMCheckingRule.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("CmmMMCheckingRule");
                    AddHeader(
                                sheet,
                                ("RuleCode"),
                                ("RuleDescription"),
                                ("RuleItem"),
                                ("Field1Name"),
                                ("Field1Value"),
                                ("Field2Name"),
                                ("Field2Value"),
                                ("Field3Name"),
                                ("Field3Value"),
                                ("Field4Name"),
                                ("Field4Value"),
                                ("Field5Name"),
                                ("Field5Value"),
                                ("Option"),
                                ("Resultfield"),
                                ("Expectedresult"),
                                ("Checkoption"),
                                ("Errormessage"),
                                ("IsActive")

                               );
                    AddObjects(
                         sheet, cmmmmcheckingrule,
                                _ => _.RuleCode,
                                _ => _.RuleDescription,
                                _ => _.RuleItem,
                                _ => _.Field1Name,
                                _ => _.Field1Value,
                                _ => _.Field2Name,
                                _ => _.Field2Value,
                                _ => _.Field3Name,
                                _ => _.Field3Value,
                                _ => _.Field4Name,
                                _ => _.Field4Value,
                                _ => _.Field5Name,
                                _ => _.Field5Value,
                                _ => _.Option,
                                _ => _.Resultfield,
                                _ => _.Expectedresult,
                                _ => _.Checkoption,
                                _ => _.Errormessage,
                                _ => _.IsActive

                                );

                });
        }

            public FileDto ExportToFileErr(List<MstCmmMMCheckingRuleImportDto> mmcheckingrule_err)
            {
                return CreateExcelPackage(
                    "MstCmmMMCheckingRule List Error.xlsx",
                    excelPackage =>
                    {
                        var sheet = excelPackage.CreateSheet("MMCheckingRule");
                        AddHeader(
                                    sheet,
                                   ("RuleCode"),
                                ("RuleDescription"),
                                ("RuleItem"),
                                ("Field1Name"),
                                ("Field1Value"),
                                ("Field2Name"),
                                ("Field2Value"),
                                ("Field3Name"),
                                ("Field3Value"),
                                ("Field4Name"),
                                ("Field4Value"),
                                ("Field5Name"),
                                ("Field5Value"),
                                ("Option"),
                                ("Resultfield"),
                                ("Checkoption"),
                                ("Errormessage"),
                                ("IsActive"),
                                 ("ErrorDescription")                        
                                   );
                        AddObjects(
                             sheet, mmcheckingrule_err,
                                     _ => _.RuleCode,
                                _ => _.RuleDescription,
                                _ => _.RuleItem,
                                _ => _.Field1Name,
                                _ => _.Field1Value,
                                _ => _.Field2Name,
                                _ => _.Field2Value,
                                _ => _.Field3Name,
                                _ => _.Field3Value,
                                _ => _.Field4Name,
                                _ => _.Field4Value,
                                _ => _.Field5Name,
                                _ => _.Field5Value,
                                _ => _.Option,
                                _ => _.Resultfield,
                                _ => _.Checkoption,
                                _ => _.Errormessage,
                                _ => _.IsActive,
                                _ => _.ErrorDescription
                                    );
                    });

            }
        public FileDto ExportToHistoricalFile(List<string> data)
        {
            string fileName = "InventoryGpsMaterialHistorical.xlsx";
            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            var allHeaders = new List<string>();
            var rowDatas = new List<JObject>();
            var exceptCols = new List<string>()
            {
                "UpdateMask",
                "LastModificationTime",
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
                _logger.LogError($"[EXCEPTION] While exporting {nameof(MstCmmMMCheckingRuleExcelExporter)} with error: {ex}");
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