using System.Collections.Generic;

using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Cmm.Dto;
using prod.Master.Cmm.Exporting;
using prod.Storage;
using Microsoft.Extensions.Logging;
using Abp.AspNetZeroCore.Net;
using GemBox.Spreadsheet;
using Newtonsoft.Json.Linq;
using prod.Common;
using prod.Master.Pio.Exporting;
using System.IO;
using System.Linq;
using System;

namespace prod.Master.Common.EngineMaster.Exporting
{
    public class MstCmmEngineMasterExcelExporter : NpoiExcelExporterBase, IMstCmmEngineMasterExcelExporter
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly ILogger _logger;
        public MstCmmEngineMasterExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { 
            _tempFileCacheManager = tempFileCacheManager;
        }
        public FileDto ExportToFile(List<MstCmmEngineMasterDto> enginemaster)
        {
            return CreateExcelPackage(
                "MasterCmmEngineMaster.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("EngineMaster");
                    AddHeader(
                                sheet,
                                "MaterialCode",
                                "ClassType",
                                "ClassName",
                                "TransmissionType",
                                "EngineModel",
                                "EngineType"

                               );
                    AddObjects(
                         sheet, enginemaster,
                                _ => _.MaterialCode,
                                _ => _.ClassType,
                                _ => _.ClassName,
                                _ => _.TransmissionType,
                                _ => _.EngineModel,
                                _ => _.EngineType

                                );

                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });

        }

        public FileDto ExportToHistoricalFile(List<string> data)
        {
            string fileName = "MstCmmEngineMasterHistorical.xlsx";
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
                _logger.LogError($"[EXCEPTION] While exporting {nameof(MstCmmEngineMasterExcelExporter)} with error: {ex}");
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