using Abp.AspNetZeroCore.Net;
using GemBox.Spreadsheet;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using prod.Common;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inventory.Dto;
using prod.Master.Pio.Exporting;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Inventory.Exporting
{
	public class MstGpsWbsCCMappingExcelExporter : NpoiExcelExporterBase, IMstGpsWbsCCMappingExcelExporter
	{
		private readonly ITempFileCacheManager _tempFileCacheManager;
		private readonly ILogger _logger;

		public MstGpsWbsCCMappingExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) {
			_tempFileCacheManager = tempFileCacheManager;
		}
		public FileDto ExportToFile(List<MstGpsWbsCCMappingDto> mstgpswbsccmapping)
		{
			return CreateExcelPackage(
						   "MstGpsWbsCCMapping.xlsx",
						   excelPackage =>
						   {
							   var sheet = excelPackage.CreateSheet("GpsWbsCCMapping");
							   AddHeader(
										   sheet,
										   ("Cost Center From"),
										   ("Wbs From"),
										   ("Cost Center To"),
										   ("Wbs To"),
										   ("Effective From Date"),
										   ("Effective From To"),
										   ("Is Active")


											  );
							   AddObjects(
									sheet, mstgpswbsccmapping,
										   _ => _.CostCenterFrom,
										   _ => _.WbsFrom,
										   _ => _.CostCenterTo,
											_ => _.WbsTo,
											_ => _.EffectiveFromDate,
											_ => _.EffectiveFromTo,
											_ => _.IsActive

								);
						   });
		}

		public FileDto ExportToHistoricalFile(List<string> data)
		{
			string fileName = "MstGpsWbsCCMappingHistorical.xlsx";
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
				_logger.LogError($"[EXCEPTION] While exporting {nameof(MstGpsWbsCCMappingExcelExporter)} with error: {ex}");
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
