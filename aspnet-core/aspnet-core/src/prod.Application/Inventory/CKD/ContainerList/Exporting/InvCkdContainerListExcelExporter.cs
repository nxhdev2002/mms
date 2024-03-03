using Abp.AspNetZeroCore.Net;
using GemBox.Spreadsheet;
using NPOI.SS.Formula.Functions;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using prod.Common;
using Newtonsoft.Json.Linq;
using prod.Master.Pio.Exporting;
using System.Linq;
using Microsoft.Extensions.Logging;


namespace prod.Inventory.CKD.Exporting
{
    public class InvCkdContainerListExcelExporter : NpoiExcelExporterBase, IInvCkdContainerListExcelExporter
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly ILogger _logger;
        public InvCkdContainerListExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager)
        {
            _tempFileCacheManager = tempFileCacheManager;

        }
        public FileDto ExportToFile(List<InvCkdContainerListDto> containerlist)
        {
            var file = new FileDto("InventoryCKDContainerList.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_InventoryCKDContainerList";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];


            List<string> listHeader = new List<string>();
            listHeader.Add("RequestStatus");
            listHeader.Add("ContainerNo");
            listHeader.Add("Renban");
            listHeader.Add("SupplierNo");
            listHeader.Add("Carrier");
            listHeader.Add("Status");
            listHeader.Add("HaisenNo");
            listHeader.Add("BillOfLadingNo");
            listHeader.Add("BillDate");
            listHeader.Add("SealNo");
            listHeader.Add("CdDate");
            listHeader.Add("CdStatus");
            listHeader.Add("ContainerSize");
            listHeader.Add("ETD");
            listHeader.Add("ETA");
            listHeader.Add("RecieveDate");
            listHeader.Add("ATA");
            listHeader.Add("PortTransitDate");
            listHeader.Add("Shop");
            listHeader.Add("Dock");
            listHeader.Add("RequestLotNo");
            listHeader.Add("InvoiceNo");
            listHeader.Add("ListLotNo");
            listHeader.Add("ListCaseNo");
            listHeader.Add("Transport");
            listHeader.Add("DevanningDate");
            listHeader.Add("DevanningTime");
            listHeader.Add("DevaningDateTime");
            listHeader.Add("Remark");
            listHeader.Add("WhLocation");
            listHeader.Add("GateinDate");
            listHeader.Add("GateinTime");
            listHeader.Add("TransitPortReqDate");
            listHeader.Add("TransitPortReqTime");
            listHeader.Add("TransitPortRemark");
            listHeader.Add("Fob");
            listHeader.Add("Freight");
            listHeader.Add("Insurance");
            listHeader.Add("Cif");
            listHeader.Add("Tax");
            listHeader.Add("Amount");
            listHeader.Add("LocationCode");
            listHeader.Add("LocationDate");
            listHeader.Add("OrdertypeCode");
            listHeader.Add("GoodstypeCode");
            listHeader.Add("OverFeeDEM");
            listHeader.Add("OverFeeDET");
            listHeader.Add("OverFeeDEMDET");
            listHeader.Add("OverDEM");
            listHeader.Add("OverDET");
            listHeader.Add("OverDEMDET");
            listHeader.Add("EstOverDEM");
            listHeader.Add("EstOverDET");
            listHeader.Add("EstOverCombine");

            List<string> listExport = new List<string>();
            listExport.Add("RequestStatus");
            listExport.Add("ContainerNo");
            listExport.Add("Renban");
            listExport.Add("SupplierNo");
            listExport.Add("Carrier");
            listExport.Add("Description");
            listExport.Add("HaisenNo");
            listExport.Add("BillOfLadingNo");
            listExport.Add("BillDate");
            listExport.Add("SealNo");
            listExport.Add("CdDate");
            listExport.Add("CdStatus");
            listExport.Add("ContainerSize");
            listExport.Add("ShippingDate");
            listExport.Add("PortDate");
            listExport.Add("ReceiveDate");
            listExport.Add("PortDateActual");
            listExport.Add("PortTransitDate");
            listExport.Add("Shop");
            listExport.Add("Dock");
            listExport.Add("RequestLotNo");
            listExport.Add("InvoiceNo");
            listExport.Add("ListLotNo");
            listExport.Add("ListCaseNo");
            listExport.Add("Transport");
            listExport.Add("DevanningDate");
            listExport.Add("DevanningTime");
            listExport.Add("DevaningDateTime");
            listExport.Add("Remark");
            listExport.Add("WhLocation");
            listExport.Add("GateinDate");
            listExport.Add("GateinTime");
            listExport.Add("TransitPortReqDate");
            listExport.Add("TransitPortReqTime");
            listExport.Add("TransitPortRemark");
            listExport.Add("Fob");
            listExport.Add("Freight");
            listExport.Add("Insurance");
            listExport.Add("Cif");
            listExport.Add("Tax");
            listExport.Add("Amount");
            listExport.Add("LocationCode");
            listExport.Add("LocationDate");
            listExport.Add("OrdertypeCode");
            listExport.Add("GoodstypeCode");
            listExport.Add("OverFeeDEM");
            listExport.Add("OverFeeDET");
            listExport.Add("OverFeeDEMDET");
            listExport.Add("OverDEM");
            listExport.Add("OverDET");
            listExport.Add("OverDEMDET");
            listExport.Add("EstOverDEM");
            listExport.Add("EstOverDET");
            listExport.Add("EstOverCombine");

            string[] properties = listExport.ToArray();
            string[] p_header = listHeader.ToArray();
            Commons.FillExcel2(containerlist, workSheet, 1, 0, properties, p_header);

            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);
            MemoryStream obj_stream = new MemoryStream();
            obj_stream = new MemoryStream(File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;



        }
        public FileDto ShipmentInfoDetailExportToFile(List<ShipmentInfoDetailListDto> shipment)
        {
            var file = new FileDto("ShipmentInfomationDetail.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_ShipmentInfomationDetail";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];


            List<string> listHeader = new List<string>();
            listHeader.Add("To Port");
            listHeader.Add("Exp");
            listHeader.Add("ETA");
            listHeader.Add("ATA");
            listHeader.Add("ShippingComp");
            listHeader.Add("BillNo");
            listHeader.Add("Invoice No");
            listHeader.Add("Container No");
            listHeader.Add("Renban");
            listHeader.Add("Lot No");
            listHeader.Add("Case/Mdl No");
            listHeader.Add("Seal No");
            listHeader.Add("Size");
            listHeader.Add("Exp & Renban");
            listHeader.Add("Pwd");


            List<string> listExport = new List<string>();
            listExport.Add("ToPort");
            listExport.Add("SupplierNo");
            listExport.Add("Eta");
            listExport.Add("Ata");
            listExport.Add("ShippingcompanyCode");
            listExport.Add("BillOfLadingNo");
            listExport.Add("InvoiceNo");
            listExport.Add("ContainerNo");
            listExport.Add("Renban");
            listExport.Add("LotNo");
            listExport.Add("ModuleCase");
            listExport.Add("SealNo");
            listExport.Add("ContainerSize");
            listExport.Add("ExpRenban");




            string[] properties = listExport.ToArray();
            string[] p_header = listHeader.ToArray();
            Commons.FillExcel2(shipment, workSheet, 1, 0, properties, p_header);

            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);
            MemoryStream obj_stream = new MemoryStream();
            obj_stream = new MemoryStream(File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;

            /* return CreateExcelPackage(
                 "ShipmentInfomationDetail.xlsx",
                 excelPackage =>
                 {
                     var sheet = excelPackage.CreateSheet("Shipment");
                     AddHeader(
                                 sheet,
                                 ("To Port"),
                                 ("Exp"),
                                 ("ETA"),

                                 ("ATA"),
                                 ("ShippingComp"),
                                 ("BillNo"),

                                 ("Invoice No"),
                                 ("Container No"),
                                 ("Renban"),

                                 ("Lot No"),
                                 ("Case/Mdl No"),
                                 ("Seal No"),

                                 ("Size"),
                                 ("Exp & Renban"),
                                 ("Pwd")                          
                                 );
                     AddObjects(
                          sheet, shipment,
                                 _ => _.ToPort,
                                 _ => _.SupplierNo,
                                 _ => _.Eta,

                                 _ => _.Ata,
                                 _ => _.ShippingcompanyCode,
                                 _ => _.BillOfLadingNo,

                                 _ => _.InvoiceNo,
                                 _ => _.ContainerNo,
                                 _ => _.Renban,

                                 _ => _.LotNo,
                                 _ => _.ModuleCase,
                                 _ => _.SealNo,

                                 _ => _.ContainerSize,
                                 _ => _.ExpRenban

                                 );


                 });*/

        }

        public FileDto ShipmentInfoDetailPXPExportToFile(List<ShipmentInfoDetailListDto> shipment)
        {
            var file = new FileDto("ShipmentInfomationDetailPXP.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_ShipmentInfomationDetailPxP";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];


            List<string> listHeader = new List<string>();
            listHeader.Add("To Port");
            listHeader.Add("Exp");
            listHeader.Add("ETA");
            listHeader.Add("ATA");
            listHeader.Add("BillNo");
            listHeader.Add("Invoice No");
            listHeader.Add("Container No");
            listHeader.Add("Renban");
            listHeader.Add("Lot No");
            listHeader.Add("Case/Mdl No");
            listHeader.Add("Part No");
            listHeader.Add("Part Name");
            listHeader.Add("Usage Qty");
            listHeader.Add("Firmpackingmonth");
            listHeader.Add("Reexport Code");
            listHeader.Add("ShippingComp");
            listHeader.Add("Seal No");
            listHeader.Add("Size");
            listHeader.Add("Exp & Renban");


            List<string> listExport = new List<string>();
            listExport.Add("ToPort");
            listExport.Add("SupplierNo");
            listExport.Add("Eta");
            listExport.Add("Ata");
            listExport.Add("BillOfLadingNo");
            listExport.Add("InvoiceNo");
            listExport.Add("ContainerNo");
            listExport.Add("Renban");
            listExport.Add("LotNo");
            listExport.Add("ModuleCase");
            listExport.Add("PartNo");
            listExport.Add("PartName");
            listExport.Add("UsageQty");
            listExport.Add("Firmpackingmonth");
            listExport.Add("ReexportCode");
            listExport.Add("ShippingcompanyCode");
            listExport.Add("SealNo");
            listExport.Add("ContainerSize");
            listExport.Add("ExpRenban");




            string[] properties = listExport.ToArray();
            string[] p_header = listHeader.ToArray();
            Commons.FillExcel2(shipment, workSheet, 1, 0, properties, p_header);

            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);
            MemoryStream obj_stream = new MemoryStream();
            obj_stream = new MemoryStream(File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;

            /* return CreateExcelPackage(
                 "ShipmentInfomationDetail.xlsx",
                 excelPackage =>
                 {
                     var sheet = excelPackage.CreateSheet("Shipment");
                     AddHeader(
                                 sheet,
                                 ("To Port"),
                                 ("Exp"),
                                 ("ETA"),

                                 ("ATA"),
                                 ("ShippingComp"),
                                 ("BillNo"),

                                 ("Invoice No"),
                                 ("Container No"),
                                 ("Renban"),

                                 ("Lot No"),
                                 ("Case/Mdl No"),
                                 ("Seal No"),

                                 ("Size"),
                                 ("Exp & Renban"),
                                 ("Pwd")                          
                                 );
                     AddObjects(
                          sheet, shipment,
                                 _ => _.ToPort,
                                 _ => _.SupplierNo,
                                 _ => _.Eta,

                                 _ => _.Ata,
                                 _ => _.ShippingcompanyCode,
                                 _ => _.BillOfLadingNo,

                                 _ => _.InvoiceNo,
                                 _ => _.ContainerNo,
                                 _ => _.Renban,

                                 _ => _.LotNo,
                                 _ => _.ModuleCase,
                                 _ => _.SealNo,

                                 _ => _.ContainerSize,
                                 _ => _.ExpRenban

                                 );


                 });*/

        }

        public FileDto InvoiceInfoDetailExportToFile(List<GetInvCkdContainerListExportInvoiceList> input)
        {

            var file = new FileDto("InvoiceDetails.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_InvoiceDetails";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];
            //workSheet.Cells.GetSubrange($"A2:O{count + 1}").Style.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromName(ColorName.Black), LineStyle.Thin);

            List<string> listHeader = new List<string>();
            listHeader.Add("Container No");
            listHeader.Add("Renban");
            listHeader.Add("Invoice No");
            listHeader.Add("Supplier No");
            listHeader.Add("Part No");
            listHeader.Add("Part Name");
            listHeader.Add("Fixlot");
            listHeader.Add("Lot No");
            listHeader.Add("Case No");
            listHeader.Add("Module No");
            listHeader.Add("Fob");
            listHeader.Add("Freight");
            listHeader.Add("Insurance");
            listHeader.Add("Cif");
            listHeader.Add("Tax");
            listHeader.Add("Tax Rate");
            listHeader.Add("Vat");
            listHeader.Add("Vat Rate");
            listHeader.Add("Cept Type");
            listHeader.Add("Car Family Code");
            listHeader.Add("Part Net Weight");
            listHeader.Add("Order No");
            listHeader.Add("Firm Packing Month");
            listHeader.Add("Vat Vn");
            listHeader.Add("Declare Type");



            List<string> listExport = new List<string>();

            listExport.Add("ContainerNo");
            listExport.Add("Renban");
            listExport.Add("InvoiceNo");
            listExport.Add("SupplierNo");
            listExport.Add("PartNo");
            listExport.Add("PartName");
            listExport.Add("Fixlot");
            listExport.Add("LotNo");
            listExport.Add("CaseNo");
            listExport.Add("ModuleNo");
            listExport.Add("Fob");
            listExport.Add("Freight");
            listExport.Add("Insurance");
            listExport.Add("Cif");
            listExport.Add("Tax");
            listExport.Add("TaxRate");
            listExport.Add("Vat");
            listExport.Add("VatRate");
            listExport.Add("CeptType");
            listExport.Add("CarfamilyCode");
            listExport.Add("PartNetWeight");
            listExport.Add("OrderNo");
            listExport.Add("Firmpackingmonth");
            listExport.Add("VatVn");
            listExport.Add("DeclareType");


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
            /*  return CreateExcelPackage(
                  "InvoiceDetails.xlsx",
                  excelPackage =>
                  {
                      var sheet = excelPackage.CreateSheet("InvoiceDetails");
                      AddHeader(
                                  sheet,
                                  ("Container No"),
                                  ("Renban"),
                                  ("Invoice No"),
                                  ("Supplier No"),
                                  ("Part No"),
                                  ("Part Name"),
                                  ("Fixlot"),
                                  ("Lot No"),
                                  ("Case No"),
                                  ("Module No"),
                                  ("Fob"),
                                  ("Freight"),
                                  ("Insurance"),
                                  ("Cif"),
                                  ("Tax"),
                                  ("Tax Rate"),
                                  ("Vat"),
                                  ("Vat Rate"),
                                  ("Cept Type"),
                                  ("Car Family Code"),
                                  ("Part Net Weight"),
                                  ("Order No"),
                                  ("Firm Packing Month"),
                                  ("Vat Vn"),
                                  ("Declare Type")
                                  );
                      AddObjects(
                           sheet, input,
                                  _ => _.ContainerNo,
                                  _ => _.Renban,
                                  _ => _.InvoiceNo,
                                  _ => _.SupplierNo,
                                  _ => _.PartNo,
                                  _ => _.PartName,
                                  _ => _.LotNo,
                                  _ => _.CaseNo,
                                  _ => _.ModuleNo,
                                  _ => _.Fob,
                                  _ => _.Freight,
                                  _ => _.Insurance,
                                  _ => _.Cif,
                                  _ => _.Tax,
                                  _ => _.TaxRate,
                                  _ => _.Vat,
                                  _ => _.VatRate,
                                  _ => _.CeptType,
                                  _ => _.CarfamilyCode,
                                  _ => _.PartNetWeight,
                                  _ => _.OrderNo,
                                  _ => _.Firmpackingmonth,
                                  _ => _.VatVn,
                                  _ => _.DeclareType
                                  );


                  });*/

        }

        public FileDto ListNoCutomsDeclareExportToFile(List<GetInvCkdContainerListExportNoDeclareCustomsList> input)
        {

            var file = new FileDto("NoCustomsDeclare.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_NoCustomsDeclare";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];


            List<string> listHeader = new List<string>();
            listHeader.Add("Container No");
            listHeader.Add("Renban");
            listHeader.Add("Invoice No"); ;
            listHeader.Add("Invoice Date");




            List<string> listExport = new List<string>();

            listExport.Add("ContainerNo");
            listExport.Add("Renban");
            listExport.Add("InvoiceNo"); ;
            listExport.Add("InvoiceDate");


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


        public FileDto ExportContainerInvoicebyContToFile(List<InvCkdContainerInvoiceDto> containerinvoice)
        {
            return CreateExcelPackage(
                "InventoryCKDContainerInvoicebyCont.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ContainerInvoice");
                    AddHeader(
                                sheet,
                                ("ContainerNo"),
                                ("Renban"),
                                ("InvoiceNo"),
                                ("SupplierNo"),
                                ("SealNo"),
                                ("BillOfLadingNo"),
                                ("BillDate"),
                                ("ContainerSize"),
                                ("PlandedvanningDate"),
                                ("ActualvanningDate"),
                                ("CdDate"),
                                ("Status"),
                                ("DateStatus"),
                                ("Fob"),
                                ("Freight"),
                                ("Insurance"),
                                ("Tax"),
                                ("Amount"),


                                ("TaxVn"),
                                ("VatVn")
                               );
                    AddObjects(
                         sheet, containerinvoice,
                                _ => _.ContainerNo,
                                _ => _.Renban,
                                _ => _.InvoiceNo,
                                _ => _.SupplierNo,
                                _ => _.SealNo,
                                _ => _.BillOfLadingNo,
                                _ => _.BillDate,
                                _ => _.ContainerSize,
                                _ => _.PlandedvanningDate,
                                _ => _.ActualvanningDate,
                                _ => _.CdDate,
                                _ => _.Status,
                                _ => _.DateStatus,
                                _ => _.Fob,
                                _ => _.Freight,
                                _ => _.Insurance,
                                _ => _.Tax,
                                _ => _.Amount,


                                _ => _.TaxVnd,
                                _ => _.VatVnd
                                );
                });



        }
        public FileDto ListImPortDeVanExportToFile(List<ImportDevanDto> importDevanDto)
        {
            var file = new FileDto("ListImPortDeVanExport.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_ListImPortDeVanExport";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];


            List<string> listHeader = new List<string>();
            listHeader.Add("Record Id");
            listHeader.Add("Invoice No");
            listHeader.Add("Renban");
            listHeader.Add("Container No");
            listHeader.Add("Module No");
            listHeader.Add("Devan Date");
            listHeader.Add("Plant");
            listHeader.Add("Cancel Flag");

            List<string> listExport = new List<string>();
            listExport.Add("NoNumber");
            listExport.Add("InvoiceNo");
            listExport.Add("Renban");
            listExport.Add("ContainerNo");
            listExport.Add("ModuleCaseNo");
            listExport.Add("DevanningDateS4");
            listExport.Add("Plant");
            listExport.Add("CancelFlag");


            string[] properties = listExport.ToArray();
            string[] p_header = listHeader.ToArray();
            Commons.FillExcel2(importDevanDto, workSheet, 1, 0, properties, p_header);

            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);
            MemoryStream obj_stream = new MemoryStream();
            obj_stream = new MemoryStream(File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;




            //    return CreateExcelPackage(
            //       "ImportDevan.xlsx",
            //       excelPackage =>
            //       {
            //           var sheet = excelPackage.CreateSheet("ImportDevan");
            //           AddHeader(
            //                       sheet,
            //                       ("Invoice No"),
            //                       ("Container Renban No"),
            //                       ("Container No"),
            //                       ("Module No"),
            //                       ("Devan Date"),
            //                       ("Plant"),
            //                       ("Cancel Flag")
            //                       );
            //           AddObjects(
            //                sheet, input,
            //                       _ => _.InvoiceNo,
            //                       _ => _.Renban,
            //                       _ => _.ContainerNo,
            //                       _ => _.ModuleCaseNo,
            //                       _ => _.DevanningDateS4,
            //                       _ => _.Plant,
            //                       _ => _.CancelFlag
            //                       );

            //       });
        }

        //public FileDto ExportToHistoricalFile(List<string> data)
        //{
        //    var allHeaders = new List<string>();
        //    var rowDatas = new List<object>();
        //    var exceptCols = new List<string>()
        //    {
        //        "UpdateMask",
        //        "ActionId",
        //    };


        //    // get all headers
        //    foreach (var item in data)
        //    {
        //        var json = JObject.Parse(item);
        //        rowDatas.Add(json);
        //        foreach (var prop in json.Properties())
        //        {
        //            if (!allHeaders.Contains(prop.Name) && !exceptCols.Contains(prop.Name))
        //            {
        //                allHeaders.Add(prop.Name);
        //            }
        //        }
        //    }

        //    // generate function (lấy value từ object thành value cell)
        //    var funcList = new Func<object, object>[allHeaders.Count];

        //    for (int i = 0; i < allHeaders.Count; i++)
        //    {
        //        var header = allHeaders[i];
        //        funcList[i] = _ => ((JObject)_)[header] ?? "";
        //    }

        //    return CreateExcelPackage(
        //        "InventoryCKDContainerListHistorical.xlsx",
        //        excelPackage =>
        //        {
        //            var sheet = excelPackage.CreateSheet("ContainerList");
        //            AddHeader(
        //                      sheet,
        //                      allHeaders.ToArray()
        //            );
        //            AddObjects(
        //                      sheet, rowDatas,
        //                      funcList
        //            );
        //        });
        //}



        public FileDto ExportToHistoricalFile(List<string> data)
        {
            string fileName = "InventoryCKDContainerListHistorical.xlsx";
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
                _logger.LogError($"[EXCEPTION] While exporting {nameof(InvCkdContainerListExcelExporter)} with error: {ex}");
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


        public FileDto ExportToFileByPeriod(List<InvCkdContainerListDto> containerlist)
        {
            var file = new FileDto("InvCKDContainerListByPeriod.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_InventoryCKDContainerListByPeriod";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];


            List<string> listHeader = new List<string>();
            listHeader.Add("RequestStatus");
            listHeader.Add("ContainerNo");
            listHeader.Add("Renban");
            listHeader.Add("SupplierNo");
            listHeader.Add("Carrier");
            listHeader.Add("Status");
            listHeader.Add("HaisenNo");
            listHeader.Add("BillOfLadingNo");
            listHeader.Add("BillDate");
            listHeader.Add("SealNo");
            listHeader.Add("CdDate");
            listHeader.Add("CdStatus");
            listHeader.Add("ContainerSize");
            listHeader.Add("ETD");
            listHeader.Add("ETA");
            listHeader.Add("RecieveDate");
            listHeader.Add("ATA");
            listHeader.Add("PortTransitDate");
            listHeader.Add("Shop");
            listHeader.Add("Dock");
            listHeader.Add("RequestLotNo");
            listHeader.Add("InvoiceNo");
            listHeader.Add("ListLotNo");
            listHeader.Add("ListCaseNo");
            listHeader.Add("Transport");
            listHeader.Add("DevanningDate");
            listHeader.Add("DevanningTime");
            listHeader.Add("DevaningDateTime");
            listHeader.Add("Remark");
            listHeader.Add("WhLocation");
            listHeader.Add("GateinDate");
            listHeader.Add("GateinTime");
            listHeader.Add("TransitPortReqDate");
            listHeader.Add("TransitPortReqTime");
            listHeader.Add("TransitPortRemark");
            listHeader.Add("Fob");
            listHeader.Add("Freight");
            listHeader.Add("Insurance");
            listHeader.Add("Cif");
            listHeader.Add("Tax");
            listHeader.Add("Amount");
            listHeader.Add("LocationCode");
            listHeader.Add("LocationDate");
            listHeader.Add("OrdertypeCode");
            listHeader.Add("GoodstypeCode");
            listHeader.Add("OverFeeDEM");
            listHeader.Add("OverFeeDET");
            listHeader.Add("OverFeeDEMDET");
            listHeader.Add("OverDEM");
            listHeader.Add("OverDET");
            listHeader.Add("OverDEMDET");
            listHeader.Add("EstOverDEM");
            listHeader.Add("EstOverDET");
            listHeader.Add("EstOverCombine");

            List<string> listExport = new List<string>();
            listExport.Add("RequestStatus");
            listExport.Add("ContainerNo");
            listExport.Add("Renban");
            listExport.Add("SupplierNo");
            listExport.Add("Carrier");
            listExport.Add("Description");
            listExport.Add("HaisenNo");
            listExport.Add("BillOfLadingNo");
            listExport.Add("BillDate");
            listExport.Add("SealNo");
            listExport.Add("CdDate");
            listExport.Add("CdStatus");
            listExport.Add("ContainerSize");
            listExport.Add("ShippingDate");
            listExport.Add("PortDate");
            listExport.Add("ReceiveDate");
            listExport.Add("PortDateActual");
            listExport.Add("PortTransitDate");
            listExport.Add("Shop");
            listExport.Add("Dock");
            listExport.Add("RequestLotNo");
            listExport.Add("InvoiceNo");
            listExport.Add("ListLotNo");
            listExport.Add("ListCaseNo");
            listExport.Add("Transport");
            listExport.Add("DevanningDate");
            listExport.Add("DevanningTime");
            listExport.Add("DevaningDateTime");
            listExport.Add("Remark");
            listExport.Add("WhLocation");
            listExport.Add("GateinDate");
            listExport.Add("GateinTime");
            listExport.Add("TransitPortReqDate");
            listExport.Add("TransitPortReqTime");
            listExport.Add("TransitPortRemark");
            listExport.Add("Fob");
            listExport.Add("Freight");
            listExport.Add("Insurance");
            listExport.Add("Cif");
            listExport.Add("Tax");
            listExport.Add("Amount");
            listExport.Add("LocationCode");
            listExport.Add("LocationDate");
            listExport.Add("OrdertypeCode");
            listExport.Add("GoodstypeCode");
            listExport.Add("OverFeeDEM");
            listExport.Add("OverFeeDET");
            listExport.Add("OverFeeDEMDET");
            listExport.Add("OverDEM");
            listExport.Add("OverDET");
            listExport.Add("OverDEMDET");
            listExport.Add("EstOverDEM");
            listExport.Add("EstOverDET");
            listExport.Add("EstOverCombine");

            string[] properties = listExport.ToArray();
            string[] p_header = listHeader.ToArray();
            Commons.FillExcel2(containerlist, workSheet, 1, 0, properties, p_header);

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
