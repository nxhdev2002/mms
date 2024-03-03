using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Welding.Andon.Exporting;
using prod.Welding.Andon.Dto;
using prod.Storage;
using prod.Welding.Andon.Dto;
using Abp.AspNetZeroCore.Net;
using GemBox.Spreadsheet;
using Newtonsoft.Json.Linq;
using prod.Common;
using prod.Welding.Andon.Importing;
using System.IO;
using System.Linq;
using System;
using Microsoft.Extensions.Logging;

namespace prod.Welding.Andon.Exporting
{
	public class WldAdoWeldingPlanExcelExporter : NpoiExcelExporterBase, IWldAdoWeldingPlanExcelExporter
	{
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private ILogger _logger;
        public WldAdoWeldingPlanExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) {
            _tempFileCacheManager = tempFileCacheManager;
        }
		public FileDto ExportToFile(List<WldAdoWeldingPlanDto> weldingplan)
		{
			return CreateExcelPackage(
				"WeldingAndonWeldingPlan.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("WeldingPlan");
					AddHeader(
								sheet,
								("Model"),
								("LotNo"),
								("NoInLot"),
                                ("Grade"),
                                ("ProdLine"),
                                ("BodyNo"),
                                ("VinNo"),
                                ("Color"),
								("PlanTime"),
								("RequestDate"),
								("Shift"),
								("WInDate"),
								("WOutDate"),
								("EdIn"),
                                ("TInPlanDatetime"),
                                ("VehicleId"),
                                ("Cfc"),
                                ("WeldingId"),
                                ("AssemblyId")
                               );
					AddObjects(
						 sheet, weldingplan,
								_ => _.Model,
								_ => _.LotNo,
								_ => _.NoInLot,
                                _ => _.Grade,
                                _ => _.ProdLine,
                                _ => _.BodyNo,
                                _ => _.VinNo,
                                _ => _.Color,
								_ => _.PlanTime,
								_ => _.RequestDate,
								_ => _.Shift,
								_ => _.FormatWInDate,
								_ => _.FormatWOutDate,
								_ => _.FormatEdIn,
                                _ => _.TInPlanDatetime,
                                _ => _.VehicleId,
                                _ => _.Cfc,
                                _ => _.WeldingId,
                                _ => _.AssemblyId
                                );
				});

		}

        public FileDto ExportToFileErr(List<WldAdoWeldingPlanDto> weldingplanerr)
        {
            return CreateExcelPackage(
                "WeldingAndonWeldingPlan_ListErrImport.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("weldingplanerr");
                    AddHeader(
                                sheet,
                                ("ProdLine"),
                                ("Model"),
                                ("LotNo"),
                                ("NoInLot"),
                                ("Grade"),
                                ("ProdLine"),
                                ("BodyNo"),
                                ("VinNo"),
                                ("Color"),
                                ("PlanTime"),
                                ("RequestDate"),
                                ("Shift"),
                                ("WInDate"),
                                ("WOutDate"),
                                ("EdIn"),
                                 ("MessagesError")
                               );
                    AddObjects(
                         sheet, weldingplanerr,
                                _ => _.ProdLine,
                                _ => _.Model,
                                _ => _.LotNo,
                                _ => _.NoInLot,
                                _ => _.Grade,
                                _ => _.ProdLine,
                                _ => _.BodyNo,
                                _ => _.VinNo,
                                _ => _.Color,
                                _ => _.PlanTime,
                                _ => _.RequestDate,
                                _ => _.Shift,
                                _ => _.WInDate,
                                _ => _.WOutDate,
                                _ => _.EdIn,
                                _ => _.MessagesError
                                );
                });

        }

        public FileDto ExportToHistoricalFile(List<string> data)
        {
            string fileName = "WldAdoWeldingPlanExcelExporterHistorical.xlsx";
            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            var allHeaders = new List<string>();
            var rowDatas = new List<JObject>();
            var exceptCols = new List<string>()
            {
                "UpdateMask",
                "WInActualDate",
                "WInActualTime",
                "WOutActualDate",
                "WOutActualTime",
                "EDInActualDate",
                "EDInActualTime",
                "LastModificationTime",
                "CreationTime",

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
                _logger.LogError($"[EXCEPTION] While exporting {nameof(WldAdoWeldingPlanExcelExporter)} with error: {ex}");
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
