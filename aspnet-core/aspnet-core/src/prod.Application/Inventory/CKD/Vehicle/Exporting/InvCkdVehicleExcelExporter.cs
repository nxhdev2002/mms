using Abp.AspNetZeroCore.Net;
using FastMember;
using GemBox.Spreadsheet;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.Formula.Functions;
using NPOI.Util.Collections;
using prod.Assy.Andon.Dto;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.Vehicle.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using prod.Common;

namespace prod.Assy.Andon.Exporting
{
    public class InvCkdVehicleExcelExporter : NpoiExcelExporterBase, IInvCkdVehicleExcelExporter
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        public InvCkdVehicleExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) {
            _tempFileCacheManager = tempFileCacheManager;
        }

        public FileDto DetailOutPartExportToFile(List<InvCkdVehicleDetailOutPartDto> invckdvehicledetailoutpart)
        {

            var file = new FileDto("InvCkdVehicleDetailOutPart.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_InvCkdVehicleDetailOutPart";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];
     

            List<string> listHeader = new List<string>();
            listHeader.Add("Vin No");
            listHeader.Add("Lot No");
            listHeader.Add("No In Lot");
            listHeader.Add("Sequence No");
            listHeader.Add("Part No");
            listHeader.Add("Cfc");
            listHeader.Add("Supplier No");
            listHeader.Add("Grade");
            listHeader.Add("Shop");
            listHeader.Add("Body Color");
            listHeader.Add("Usage Qty");

            List<string> listExport = new List<string>();
            listExport.Add("VinNo");
            listExport.Add("LotNo");
            listExport.Add("NoInLot");
            listExport.Add("SequenceNo");
            listExport.Add("PartNo");
            listExport.Add("Cfc");
            listExport.Add("SupplierNo");
            listExport.Add("Grade");
            listExport.Add("Shop");
            listExport.Add("BodyColor");
            listExport.Add("UsageQty");

            string[] properties = listExport.ToArray();
            string[] p_header = listHeader.ToArray();
            Commons.FillExcel2(invckdvehicledetailoutpart, workSheet, 1, 0, properties, p_header);

            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);
            MemoryStream obj_stream = new MemoryStream();
            obj_stream = new MemoryStream(File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;



            /*    return CreateExcelPackage(
                    "Vehicle_Lineoff_IF.xlsx",
                   excelPackage =>
                   {
                       var sheet = excelPackage.CreateSheet("VehicleDetailOutPart");
                       AddHeader(sheet,
                           ("Vin No"),
                           ("Lot No"),
                           ("No In Lot"),
                           ("Sequence No"),
                           ("Part No"),
                           ("Cfc"),
                           ("Supplier No"),
                           ("Grade"),
                           ("Shop"),
                           ("Body Color"),
                           ("Usage Qty")
                           );
                       AddObjects(
                           sheet, invckdvehicledetailoutpart,
                            _ => _.VinNo,
                            _ => _.LotNo,
                            _ => _.NoInLot,
                            _ => _.SequenceNo,
                            _ => _.PartNo,
                            _ => _.Cfc,
                            _ => _.SupplierNo,
                            _ => _.Grade,
                            _ => _.Shop,
                            _ => _.BodyColor,
                            _ => _.UsageQty
                           );

                   }
                    );*/
        }

        public FileDto ExportToFile(List<InvCkdVehicleDto> ivnckdvehicle)
        {
            var file = new FileDto("InvCkdVehicle.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_InvCkdVehicle";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];
          

            List<string> listHeader = new List<string>();
            listHeader.Add("Cfc");
            listHeader.Add("Grade");
            listHeader.Add("Body No");
            listHeader.Add("Lot No");
            listHeader.Add("No In Lot");
            listHeader.Add("Sequence No");
            listHeader.Add("Vin");
            listHeader.Add("Color");
            listHeader.Add("W In Actual Date");
            listHeader.Add("W Out Actual Date");
            listHeader.Add("T In Actual Date");
            listHeader.Add("T Out Actual Date");
            listHeader.Add("A In Actual Date");
            listHeader.Add("A Out Actual Date");
            listHeader.Add("Lineoff Date");
            listHeader.Add("Pdi Date");
            listHeader.Add("PIO Date");
            listHeader.Add("Sales Date");
            listHeader.Add("Engine Id");
            listHeader.Add("Trans Id");
            listHeader.Add("Sales Sfx");
            listHeader.Add("Color Type");
            listHeader.Add("Description");
            listHeader.Add("Indent Line");
            listHeader.Add("Ss No");
            listHeader.Add("Ed Number");
            listHeader.Add("Goshi Car");



            List<string> listExport = new List<string>();
            listExport.Add("Cfc");
            listExport.Add("Grade");
            listExport.Add("BodyNo");
            listExport.Add("LotNo");
            listExport.Add("NoInLot");
            listExport.Add("SequenceNo");
            listExport.Add("Vin");
            listExport.Add("Color");
            listExport.Add("WInActualDate");
            listExport.Add("WOutActualDate");
            listExport.Add("TInActualDate");
            listExport.Add("TOutActualDate");
            listExport.Add("AInActualDate");
            listExport.Add("AOutActualDate");
            listExport.Add("LineoffDate");
            listExport.Add("PdiDate");
            listExport.Add("PIOActualDate_DDMMYYYY");
            listExport.Add("SalesActualDate_DDMMYYYY");
            listExport.Add("EngineId");
            listExport.Add("TransId");
            listExport.Add("SalesSfx");
            listExport.Add("ColorType");
            listExport.Add("Name");
            listExport.Add("IndentLine");
            listExport.Add("SsNo");
            listExport.Add("EdNumber");
            listExport.Add("GoshiCar");


            string[] properties = listExport.ToArray();
            string[] p_header = listHeader.ToArray();
            Commons.FillExcel2(ivnckdvehicle, workSheet, 1, 0, properties, p_header);

            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);
            MemoryStream obj_stream = new MemoryStream();
            obj_stream = new MemoryStream(File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;
        
        }

        public FileDto ExportToFileGIVehicle(List<InvCkdVehicleGIDto> ivnckdvehiclegi)
        {
            var file = new FileDto("InvCkdGIVehicle.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_InvCkdGIVehicle";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];
          

            List<string> listHeader = new List<string>();
            listHeader.Add("Vin No");
            listHeader.Add("Lot No");
            listHeader.Add("No In Lot");
            listHeader.Add("Sequence No");
            listHeader.Add("Pdi Date");
            listHeader.Add("Body No");
            listHeader.Add("Cfc");
     


            List<string> listExport = new List<string>();
            listExport.Add("VinNo");
            listExport.Add("LotNo");
            listExport.Add("NoInLot");
            listExport.Add("SequenceNo");
            listExport.Add("PdiDate");
            listExport.Add("BodyNo");
            listExport.Add("Cfc");
       


            string[] properties = listExport.ToArray();
            string[] p_header = listHeader.ToArray();
            Commons.FillExcel2(ivnckdvehiclegi, workSheet, 1, 0, properties, p_header);

            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);
            MemoryStream obj_stream = new MemoryStream();
            obj_stream = new MemoryStream(File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;

 
        }

        public FileDto ExportToFileOutDetailsVehicle(List<InvCkdVehicleOutDetailsDto> ivnckdvehicleoutdetails)
        {

            return CreateExcelPackage(
                "InvCkdVehicleOutDetails.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("CKDVehicleOutDetails");
                    AddHeader(
                                sheet,
                                ("Lot No"),
                                ("No In Lot"),
                                ("Part No"),
                                ("Cfc"),
                                ("Supplier No"),
                                ("Grade"),
                                ("Shop"),
                                ("Body Color"),
                                ("Usage Qty")
                               );
                    AddObjects(
                         sheet, ivnckdvehicleoutdetails,
                                _ => _.LotNo,
                                _ => _.NoInLot,
                                _ => _.PartNo,
                                _ => _.Cfc,
                                _ => _.SupplierNo,
                                _ => _.Grade,
                                _ => _.Shop,
                                _ => _.BodyColor,
                                _ => _.UsageQty
                                );
                });
        }

        public FileDto OutPartByVehicleExportToFile(List<InvCkdOutPartByVehicleDto> invckdoutpartbyvehicle)
        {
            var file = new FileDto("InvCkdVehicleOutPartByVehicle.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_InvCkdVehicleOutPartByVehicle";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];


            List<string> listHeader = new List<string>();
            listHeader.Add("Vin No");
            listHeader.Add("Lot No");
            listHeader.Add("No In Lot");
            listHeader.Add("Sequence No");
            listHeader.Add("Part No");
            listHeader.Add("Cfc");
            listHeader.Add("Supplier No");
            listHeader.Add("Grade");
            listHeader.Add("Shop");
            listHeader.Add("Body Color");
            listHeader.Add("Usage Qty");


            List<string> listExport = new List<string>();
            listExport.Add("VinNo");
            listExport.Add("LotNo");
            listExport.Add("NoInLot");
            listExport.Add("SequenceNo");
            listExport.Add("PartNo");
            listExport.Add("Cfc");
            listExport.Add("SupplierNo");
            listExport.Add("Grade");
            listExport.Add("Shop");
            listExport.Add("BodyColor");
            listExport.Add("UsageQty");


            string[] properties = listExport.ToArray();
            string[] p_header = listHeader.ToArray();
            Commons.FillExcel2(invckdoutpartbyvehicle, workSheet, 1, 0, properties, p_header);

            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);
            MemoryStream obj_stream = new MemoryStream();
            obj_stream = new MemoryStream(File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;

        }

        public FileDto ListViewIFExportToFile(List<ViewIF> input)
        {
            var file = new FileDto("ListViewIFExportToFile.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_ListViewIFExportToFile";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];


            List<string> listHeader = new List<string>();
            listHeader.Add("RecordId");
            listHeader.Add("Vin");
            listHeader.Add("Urn");
            listHeader.Add("SpecSheetNo");
            listHeader.Add("IdLine");
            listHeader.Add("Katashiki");
            listHeader.Add("SaleKatashiki");
            listHeader.Add("SaleSuffix");
            listHeader.Add("Spec200Digits");
            listHeader.Add("ProductionSuffix");
            listHeader.Add("LotCode");
            listHeader.Add("EnginePrefix");
            listHeader.Add("EngineNo");
            listHeader.Add("PlantCode");
            listHeader.Add("CurrentStatus");
            listHeader.Add("LineOffDatetime");
            listHeader.Add("InteriorColor");
            listHeader.Add("ExteriorColor");
            listHeader.Add("DestinationCode");
            listHeader.Add("EdOdno");
            listHeader.Add("CancelFlag");
            listHeader.Add("SmsCarFamilyCode");
            listHeader.Add("OrderType");
            listHeader.Add("KatashikiCode");
            listHeader.Add("EndOfRecord");



            List<string> listExport = new List<string>();
            listExport.Add("RecordId");
            listExport.Add("Vin");
            listExport.Add("Urn");
            listExport.Add("SpecSheetNo");
            listExport.Add("IdLine");
            listExport.Add("Katashiki");
            listExport.Add("SaleKatashiki");
            listExport.Add("SaleSuffix");
            listExport.Add("Spec200Digits");
            listExport.Add("ProductionSuffix");
            listExport.Add("LotCode");
            listExport.Add("EnginePrefix");
            listExport.Add("EngineNo");
            listExport.Add("PlantCode");
            listExport.Add("CurrentStatus");
            listExport.Add("LineOffDatetime");
            listExport.Add("InteriorColor");
            listExport.Add("ExteriorColor");
            listExport.Add("DestinationCode");
            listExport.Add("EdOdno");
            listExport.Add("CancelFlag");
            listExport.Add("SmsCarFamilyCode");
            listExport.Add("OrderType");
            listExport.Add("KatashikiCode");
            listExport.Add("EndOfRecord");



            string[] properties = listExport.ToArray();
            string[] p_header = listHeader.ToArray();
            Commons.FillExcel2(input, workSheet, 1, 0, properties, p_header);

            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);
            MemoryStream obj_stream = new MemoryStream();
            obj_stream = new MemoryStream(File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;

        }


        public FileDto ExportToFileByPeriod(List<InvCkdVehicleDto> ivnckdvehicle)
        {
            var file = new FileDto("InvCkdVehicleByPeriod.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_InvCkdVehicleByPeriod";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];


            List<string> listHeader = new List<string>();
            listHeader.Add("Cfc");
            listHeader.Add("Grade");
            listHeader.Add("Prod Line");
            listHeader.Add("Body No");
            listHeader.Add("Lot No");
            listHeader.Add("No In Lot");
            listHeader.Add("Sequence No");
            listHeader.Add("Vin");
            listHeader.Add("Color");
            listHeader.Add("W In Actual Date");
            listHeader.Add("W Out Actual Date");
            listHeader.Add("T In Actual Date");
            listHeader.Add("T Out Actual Date");
            listHeader.Add("A In Actual Date");
            listHeader.Add("A Out Actual Date");
            listHeader.Add("Lineoff Date");
            listHeader.Add("Pdi Date");
            listHeader.Add("PIO Date");
            listHeader.Add("Sales Date");
            listHeader.Add("Engine Id");
            listHeader.Add("Trans Id");
            listHeader.Add("Sales Sfx");
            listHeader.Add("Color Type");
            listHeader.Add("Description");
            listHeader.Add("Indent Line");
            listHeader.Add("Ss No");
            listHeader.Add("Ed Number");
            listHeader.Add("Goshi Car");



            List<string> listExport = new List<string>();
            listExport.Add("Cfc");
            listExport.Add("Grade");
            listExport.Add("ProdLine");
            listExport.Add("BodyNo");
            listExport.Add("LotNo");
            listExport.Add("NoInLot");
            listExport.Add("SequenceNo");
            listExport.Add("Vin");
            listExport.Add("Color");
            listExport.Add("WInActualDate");
            listExport.Add("WOutActualDate");
            listExport.Add("TInActualDate");
            listExport.Add("TOutActualDate");
            listExport.Add("AInActualDate");
            listExport.Add("AOutActualDate");
            listExport.Add("LineoffDate");
            listExport.Add("PdiDate");
            listExport.Add("PIOActualDate_DDMMYYYY");
            listExport.Add("SalesActualDate_DDMMYYYY");
            listExport.Add("EngineId");
            listExport.Add("TransId");
            listExport.Add("SalesSfx");
            listExport.Add("ColorType");
            listExport.Add("Name");
            listExport.Add("IndentLine");
            listExport.Add("SsNo");
            listExport.Add("EdNumber");
            listExport.Add("GoshiCar");


            string[] properties = listExport.ToArray();
            string[] p_header = listHeader.ToArray();
            Commons.FillExcel2(ivnckdvehicle, workSheet, 1, 0, properties, p_header);

            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);
            MemoryStream obj_stream = new MemoryStream();
            obj_stream = new MemoryStream(File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;

        }
    }
}
