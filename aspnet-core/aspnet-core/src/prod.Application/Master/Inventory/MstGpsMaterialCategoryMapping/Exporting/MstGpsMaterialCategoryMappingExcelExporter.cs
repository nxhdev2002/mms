using Newtonsoft.Json.Linq;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inventory.Dto;
using prod.Master.Inventory.GpsMaterialCategory.Dto;
using prod.Storage;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;
using Abp.AspNetZeroCore.Net;
using GemBox.Spreadsheet;
using prod.Common;
using Microsoft.Extensions.Logging;

namespace prod.Master.Inventory.Exporting
{
	public class MstGpsMaterialCategoryMappingExcelExporter : NpoiExcelExporterBase, IMstGpsMaterialCategoryMappingExcelExporter
	{
        public readonly ITempFileCacheManager _tempFileCacheManager;
        public readonly ILogger _logger;
        public MstGpsMaterialCategoryMappingExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) {
            _tempFileCacheManager = tempFileCacheManager;
        }
		public FileDto ExportToFile(List<MstGpsMaterialCategoryMappingDto> mstinvgpsmaterialcategorymapping)
		{
			return CreateExcelPackage(
						   "MstGpsMaterialCategoryMapping.xlsx",
						   excelPackage =>
						   {
							   var sheet = excelPackage.CreateSheet("GpsMaterialCategoryMapping");
							   AddHeader(
										   sheet,
										   ("YV Category"),
										   ("GL Expense Account"),
										   ("GL Level 5 In WBS"),
										   ("GL Account Description"),
										   ("Definition"),
										   ("Fixed/ VariableCost"),
										   ("Example"),
										   ("Account Type"),
										   ("Posting Key"),
										   ("Part Type"),
										   ("Document Type"),
										   ("Is Asset"),
										   ("Revert Cancel"),
                                           ("Is Active")


                                              );
							   AddObjects(
									sheet, mstinvgpsmaterialcategorymapping,
										   _ => _.YVCategory,
										   _ => _.GLExpenseAccount,
										   _ => _.GLLevel5InWBS,
											_ => _.GLAccountDescription,
											_ => _.Definition,
											_ => _.FixedVariableCost,
											_ => _.Example,
											_ => _.AccountType,
											_ => _.PostingKey,
											_ => _.PartType,
											_ => _.DocumentType,
											_ => _.IsAsset,
											_ => _.RevertCancel,
                                            _ => _.IsActive

                                );
						   });
		}
        public FileDto ExportToHistoricalFile(List<string> data)
        {
            string fileName = "MstGpsMaterialCategoryMappingHistorical.xlsx";
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
                _logger.LogError($"[EXCEPTION] While exporting {nameof(MstGpsMaterialCategoryMappingExcelExporter)} with error: {ex}");
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
