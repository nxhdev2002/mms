using Abp.AspNetZeroCore.Net;
using GemBox.Spreadsheet;
using prod.Assy.Andon.Dto;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using prod.Common;
using Newtonsoft.Json.Linq;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace prod.Assy.Andon.Exporting
{
    public class AsyAdoAInPlanExcelExporter : NpoiExcelExporterBase, IAsyAdoAInPlanExcelExporter
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly ILogger _logger;
        public AsyAdoAInPlanExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager)
        {
            _tempFileCacheManager = tempFileCacheManager;
        }
        public FileDto ExportToFile(List<AsyAdoAInPlanDto> ainplan)
        {
            /*  var file = new FileDto("AssyAndonAInPlan.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
              SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
              string fileName = "temp_AssyAndonAInPlan";
              string template = "wwwroot/Template";
              string path = "";
              path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
              var xlWorkBook = ExcelFile.Load(path + ".xlsx");
              var workSheet = xlWorkBook.Worksheets[0];


              List<string> listHeader = new List<string>();
              listHeader.Add("Model");
              listHeader.Add("Cfc");
              listHeader.Add("Lot No");
              listHeader.Add("No In Lot");
              listHeader.Add("Grade");
              listHeader.Add("Body No");
              listHeader.Add("Color");
              listHeader.Add("Vin No");
              listHeader.Add("A In Date Actual");
              listHeader.Add("A Out Date Actual");
              listHeader.Add("A In Plan Datetime");
              listHeader.Add("A Out Plan Datetime");
              listHeader.Add("Sequence No");
              listHeader.Add("Is Start");
              listHeader.Add("Vehicle Id");
              listHeader.Add("Lineoff Datetime");
              listHeader.Add("Lineoff Date");
              listHeader.Add("Lineoff Time");
              listHeader.Add("Pdi Datetime");
              listHeader.Add("Pdi Date");
              listHeader.Add("Pdi Time");
              listHeader.Add("PIO Actual Datetime");
              listHeader.Add("PIO Actual Date");
              listHeader.Add("PIO Actual Time");
              listHeader.Add("Sales Actual Datetime");
              listHeader.Add("Sales Actual Date");
              listHeader.Add("Sales Actual Time");
              listHeader.Add("AssemblyId");
              listHeader.Add("Engine Id");
              listHeader.Add("Trans Id");
              listHeader.Add("Sales Sfx");
              listHeader.Add("Color Type");
              listHeader.Add("Indent Line");
              listHeader.Add("Ss No");
              listHeader.Add("Ed Number");
              listHeader.Add("Goshi Car");



              List<string> listExport = new List<string>();
              listExport.Add("Model");
              listExport.Add("Cfc");
              listExport.Add("LotNo");
              listExport.Add("NoInLot");
              listExport.Add("Grade");
              listExport.Add("BodyNo");
              listExport.Add("Color");
              listExport.Add("VinNo");
              listExport.Add("AInDateActual");
              listExport.Add("AOutDateActual");
              listExport.Add("AInPlanDatetime");
              listExport.Add("AOutPlanDatetime");
              listExport.Add("SequenceNo");
              listExport.Add("IsStart");
              listExport.Add("VehicleId");
              listExport.Add("LineoffDatetime");
              listExport.Add("LineoffDate");
              listExport.Add("LineoffTime");
              listExport.Add("PdiDatetime");
              listExport.Add("PdiDate");
              listExport.Add("PdiTime");
              listExport.Add("PIOActualDatetime");
              listExport.Add("PIOActualDate");
              listExport.Add("PIOActualTime");
              listExport.Add("SalesActualDatetime");
              listExport.Add("SalesActualDate");
              listExport.Add("SalesActualTime");
              listExport.Add("AssemblyId");
              listExport.Add("EngineId");
              listExport.Add("TransId");
              listExport.Add("SalesSfx");
              listExport.Add("ColorType");
              listExport.Add("IndentLine");
              listExport.Add("SsNo");
              listExport.Add("EdNumber");
              listExport.Add("GoshiCar");


              string[] properties = listExport.ToArray();
              string[] p_header = listHeader.ToArray();
              Commons.FillExcel2(ainplan, workSheet, 1, 0, properties, p_header);

              var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
              xlWorkBook.Save(tempFile);
              MemoryStream obj_stream = new MemoryStream();
              obj_stream = new MemoryStream(File.ReadAllBytes(tempFile));
              _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
              File.Delete(tempFile);
              obj_stream.Position = 0;
              return file;*/

            return CreateExcelPackage(
                "AssyAndonAInPlan.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("AInPlan");
                    AddHeader(
                                sheet,
                                ("Model"),
                                ("Cfc"),
                                ("Lot No"),
                                ("No In Lot"),
                                ("Grade"),
                                ("Body No"),
                                ("Color"),
                                ("Vin No"),
                                ("A In Date Actual"),
                                ("A Out Date Actual"),
                                ("A In Plan Datetime"),
                                ("A Out Plan Datetime"),
                                ("Sequence No"),
                                ("Is Start"),
                                ("Vehicle Id"),
                                ("Lineoff Datetime"),
                                ("Lineoff Date"),
                                ("Lineoff Time"),
                                ("Pdi Datetime"),
                                ("Pdi Date"),
                                ("Pdi Time"),
                                ("PIO Actual Datetime"),
                                ("PIO Actual Date"),
                                ("PIO Actual Time"),
                                ("Sales Actual Datetime"),
                                ("Sales Actual Date"),
                                ("Sales Actual Time"),
                                ("AssemblyId"),
                                ("Engine Id"),
                                ("Trans Id"),
                                ("Sales Sfx"),
                                ("Color Type"),
                                ("Indent Line"),
                                ("Ss No"),
                                ("Ed Number"),
                                ("Goshi Car")
                               );
                    AddObjects(
                         sheet, ainplan,
                                _ => _.Model,
                                _ => _.Cfc,
                                _ => _.LotNo,
                                _ => _.NoInLot,
                                _ => _.Grade,
                                _ => _.BodyNo,
                                _ => _.Color,
                                _ => _.VinNo,
                                _ => _.FormatAInDateActual,
                                _ => _.FormatAOutDateActual,
                                _ => _.AInPlanDatetime,
                                _ => _.AOutPlanDatetime,
                                _ => _.SequenceNo,
                                _ => _.IsStart,
                                _ => _.VehicleId,
                                _ => _.LineoffDatetime,
                                _ => _.LineoffDate,
                                _ => _.LineoffTime,
                                _ => _.PdiDatetime,
                                _ => _.PdiDate,
                                _ => _.PdiTime,
                                _ => _.PIOActualDatetime,
                                _ => _.PIOActualDate,
                                _ => _.FormatPIOActualTime,
                                _ => _.SalesActualDatetime,
                                _ => _.SalesActualDate,
                                _ => _.FormatSalesActualTime,
                                _ => _.AssemblyId,
                                _ => _.EngineId,
                                _ => _.TransId,
                                _ => _.SalesSfx,
                                _ => _.ColorType,
                                _ => _.IndentLine,
                                _ => _.SsNo,
                                _ => _.EdNumber,
                                _ => _.GoshiCar
                                );
                });
        }

        public FileDto ExportToHistoricalFile(List<string> data)
        {
            string fileName = "AsyAsyAdoAInPlanExcelExporterHistorical.xlsx";
            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            var allHeaders = new List<string>();
            var rowDatas = new List<JObject>();
            var exceptCols = new List<string>()
            {
                "UpdateMask",
/*                "AInActualDate",
                "AInActualTime",
                "AOutActualDate",
                "AOutActualTime",*/
                "CreationTime",
                

            };
            var date = new List<string>() {
                "AInActualDate",
                "AOutActualDate",
                "LineoffDate",
                "PdiDate",
                "PIOActualDate",
                "SalesActualDate"

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
                        if (date.Contains(prop.Name))
                        {
                            prop.Value = ((DateTime)prop.Value).ToString("dd/MM/yyyy");
                            Console.WriteLine(prop.Value);
                            /*Console.WriteLine(((DateTime)prop.Value).ToString("dd/MM/yyyy"));*/
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
                _logger.LogError($"[EXCEPTION] While exporting {nameof(AsyAdoAInPlanExcelExporter)} with error: {ex}");
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
