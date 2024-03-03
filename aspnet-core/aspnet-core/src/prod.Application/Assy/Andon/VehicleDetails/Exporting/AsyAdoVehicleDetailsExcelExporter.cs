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
using prod.Inventory.CKD.Exporting;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace prod.Assy.Andon.Exporting
{
    public class AsyAdoVehicleDetailsExcelExporter : NpoiExcelExporterBase, IAsyAdoVehicleDetailsExcelExporter
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly ILogger _logger;
        public AsyAdoVehicleDetailsExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager)
        {
            _tempFileCacheManager = tempFileCacheManager;
        }
        public FileDto ExportToFile(List<AsyAdoVehicleDetailsExcelDto> vehicledetails)
        {
            var file = new FileDto("AssyAndonVehicleDetails.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_AssyAndonVehicleDetails";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];
            // workSheet.Cells.GetSubrange($"A2:BB{count + 1}").Style.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromName(ColorName.Black), LineStyle.Thin);

            List<string> listHeader = new List<string>();
            listHeader.Add("No");
            listHeader.Add("Cfc");
            listHeader.Add("BodyNo");
            listHeader.Add("LotNo");
            listHeader.Add("NoInLot");
            listHeader.Add("SequenceNo");
            listHeader.Add("KeyNo");
            listHeader.Add("Vin");
            listHeader.Add("Color");
            listHeader.Add("Eng");
            listHeader.Add("Trs");
            listHeader.Add("Ecu");
            listHeader.Add("WInDateActual");
            listHeader.Add("TInPlanDatetime");
            listHeader.Add("PaintInTime");
            listHeader.Add("AInDateActual");
            listHeader.Add("InsOutDateActual");
            listHeader.Add("InsLineOutVp4DateActual");
            listHeader.Add("DriverAirBag");
            listHeader.Add("PassengerAirBag");
            listHeader.Add("SideAirBagLh");
            listHeader.Add("SideAirBagRh");
            listHeader.Add("KneeAirBagLh");
            listHeader.Add("CurtainSideAirBagLh");
            listHeader.Add("CurtainSideAirBagRh");
            listHeader.Add("TotalDelay");
            listHeader.Add("ShippingTime");
            listHeader.Add("VehicleId");
            listHeader.Add("TestNo");
            listHeader.Add("IsPrintedQrcode");
            listHeader.Add("PrintedQrcodeDate");
            listHeader.Add("UpdatedDate");
            listHeader.Add("IsProject");
            listHeader.Add("LineoffDatetime");
            listHeader.Add("LineoffDate");
            listHeader.Add("LineoffTime");
            listHeader.Add("PdiDatetime");
            listHeader.Add("PdiDate");
            listHeader.Add("PdiTime");
            listHeader.Add("PioActualDatetime");
            listHeader.Add("PioActualDate");
            listHeader.Add("PioActualTime");
            listHeader.Add("SalesActualDatetime");
            listHeader.Add("SalesActualDate");
            listHeader.Add("SalesActualTime");
            listHeader.Add("EngineId");
            listHeader.Add("TransId");
            listHeader.Add("SalesSfx");
            listHeader.Add("ColorType");
            listHeader.Add("IndentLine");
            listHeader.Add("SsNo");
            listHeader.Add("EdNumber");
            listHeader.Add("GoshiCar");


            List<string> listExport = new List<string>();

            listExport.Add("No");
            listExport.Add("Cfc");
            listExport.Add("BodyNo");
            listExport.Add("LotNo");
            listExport.Add("NoInLot");
            listExport.Add("SequenceNo");
            listExport.Add("KeyNo");
            listExport.Add("Vin");
            listExport.Add("Color");
            listExport.Add("Eng");
            listExport.Add("Trs");
            listExport.Add("Ecu");
            listExport.Add("WInDateActual");
            listExport.Add("TInPlanDatetime");
            listExport.Add("PaintInTime");
            listExport.Add("AInDateActual");
            listExport.Add("InsOutDateActual");
            listExport.Add("InsLineOutVp4DateActual");
            listExport.Add("DriverAirBag");
            listExport.Add("PassengerAirBag");
            listExport.Add("SideAirBagLh");
            listExport.Add("SideAirBagRh");
            listExport.Add("KneeAirBagLh");
            listExport.Add("CurtainSideAirBagLh");
            listExport.Add("CurtainSideAirBagRh");
            listExport.Add("TotalDelay");
            listExport.Add("ShippingTime");
            listExport.Add("VehicleId");
            listExport.Add("TestNo");
            listExport.Add("IsPrintedQrcode");
            listExport.Add("PrintedQrcodeDate");
            listExport.Add("UpdatedDate");
            listExport.Add("IsProject");
            listExport.Add("LineoffDatetime");
            listExport.Add("LineoffDate");
            listExport.Add("FormatLineoffTime");
            listExport.Add("PdiDatetime");
            listExport.Add("PdiDate");
            listExport.Add("FormatPdiTime");
            listExport.Add("PIOActualDatetime");
            listExport.Add("PIOActualDate");
            listExport.Add("FormatPIOActualTime");
            listExport.Add("SalesActualDatetime");
            listExport.Add("SalesActualDate");
            listExport.Add("FormatSalesActualTime");
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
            Commons.FillExcel2(vehicledetails, workSheet, 1, 0, properties, p_header);

            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);
            MemoryStream obj_stream = new MemoryStream();
            obj_stream = new MemoryStream(File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;

            /*
                        return CreateExcelPackage(
                            "AssyAndonVehicleDetails.xlsx",
                            excelPackage =>
                            {
                                var sheet = excelPackage.CreateSheet("VehicleDetails");
                                AddHeader(
                                            sheet,
                                            ("No"),
                                            ("BodyNo"),
                                            ("LotNo"),
                                            ("NoInLot"),
                                            ("SequenceNo"),
                                            ("KeyNo"),
                                            ("Vin"),
                                            ("Color"),
                                            ("Eng"),
                                            ("Trs"),
                                            ("Ecu"),
                                            ("WInDateActual"),
                                            ("TInPlanDatetime"),
                                            ("PaintInTime"),
                                            ("AInDateActual"),
                                            ("InsOutDateActual"),
                                            ("InsLineOutVp4DateActual"),
                                            ("DriverAirBag"),
                                            ("PassengerAirBag"),
                                            ("SideAirBagLh"),
                                            ("SideAirBagRh"),
                                            ("KneeAirBagLh"),
                                            ("CurtainSideAirBagLh"),
                                            ("CurtainSideAirBagRh"),
                                            ("TotalDelay"),
                                            ("ShippingTime"),
                                            ("VehicleId"),
                                            ("TestNo"),
                                            ("IsPrintedQrcode"),
                                            ("PrintedQrcodeDate"),
                                            ("UpdatedDate"),
                                            ("IsProject"),
                                            ("Cfc"),
                                            ("LineoffDatetime"),
                                            ("LineoffDate"),
                                            ("LineoffTime"),
                                            ("PdiDatetime"),
                                            ("PdiDate"),
                                            ("PdiTime"),
                                            ("PioActualDatetime"),
                                            ("PioActualDate"),
                                            ("PioActualTime"),
                                            ("SalesActualDatetime"),
                                            ("SalesActualDate"),
                                            ("SalesActualTime"),
                                            ("EngineId"),
                                            ("TransId"),
                                            ("SalesSfx"),
                                            ("ColorType"),
                                            ("IndentLine"),
                                            ("SsNo"),
                                            ("EdNumber"),
                                            ("GoshiCar")
                                           );
                                AddObjects(
                                     sheet, vehicledetails,
                                            _ => _.No,
                                            _ => _.BodyNo,
                                            _ => _.LotNo,
                                            _ => _.NoInLot,
                                            _ => _.SequenceNo,
                                            _ => _.KeyNo,
                                            _ => _.Vin,
                                            _ => _.Color,
                                            _ => _.Eng,
                                            _ => _.Trs,
                                            _ => _.Ecu,
                                            _ => _.WInDateActual,
                                            _ => _.TInPlanDatetime,
                                            _ => _.PaintInTime,
                                            _ => _.AInDateActual,
                                            _ => _.InsOutDateActual,
                                            _ => _.InsLineOutVp4DateActual,
                                            _ => _.DriverAirBag,
                                            _ => _.PassengerAirBag,
                                            _ => _.SideAirBagLh,
                                            _ => _.SideAirBagRh,
                                            _ => _.KneeAirBagLh,
                                            _ => _.CurtainSideAirBagLh,
                                            _ => _.CurtainSideAirBagRh,
                                            _ => _.TotalDelay,
                                            _ => _.ShippingTime,
                                            _ => _.VehicleId,
                                            _ => _.TestNo,
                                            _ => _.IsPrintedQrcode,
                                            _ => _.PrintedQrcodeDate,
                                            _ => _.UpdatedDate,
                                            _ => _.IsProject,
                                            _ => _.Cfc,
                                            _ => _.LineoffDatetime,
                                            _ => _.LineoffDate,
                                            _ => _.LineoffTime,
                                            _ => _.PdiDatetime,
                                            _ => _.PdiDate,
                                            _ => _.PdiTime,
                                            _ => _.PIOActualDatetime,
                                            _ => _.PIOActualDate,
                                            _ => _.PIOActualTime,
                                            _ => _.SalesActualDatetime,
                                            _ => _.SalesActualDate,
                                            _ => _.SalesActualTime,
                                            _ => _.EngineId,
                                            _ => _.TransId,
                                            _ => _.SalesSfx,
                                            _ => _.ColorType,
                                            _ => _.IndentLine,
                                            _ => _.SsNo,
                                            _ => _.EdNumber,
                                            _ => _.GoshiCar
                                            );


                            });*/

        }

        public FileDto ExportToHistoricalFile(List<string> data)
        {
            string fileName = "AsyAdoVehicleDetailsExcelExporterHistorical.xlsx";
            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            var allHeaders = new List<string>();
            var rowDatas = new List<JObject>();
            var exceptCols = new List<string>()
            {
                "UpdateMask",

            };

            var date = new List<string>() {
                "WInActualDate",
                "WOutActualDate",
                "TInActualDate",
                "TOutActualDate",
                "AInActualDate",
                "PdiDate",
                "PIOActualDate",
                "SalesActualDate",
                "AOutActualDate"

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
                _logger.LogError($"[EXCEPTION] While exporting {nameof(AsyAdoVehicleDetailsExcelExporter)} with error: {ex}");
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
