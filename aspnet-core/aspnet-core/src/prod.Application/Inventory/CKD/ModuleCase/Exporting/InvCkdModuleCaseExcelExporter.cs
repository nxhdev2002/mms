using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Exporting;
using prod.Inventory.CKD.Dto;
using prod.Storage;
using Abp.AspNetZeroCore.Net;
using GemBox.Spreadsheet;
using NPOI.SS.Formula.Functions;
using System.IO;
using prod.Common;
using System;

namespace prod.Inventory.CKD.Exporting
{
    public class InvCkdModuleCaseExcelExporter : NpoiExcelExporterBase, IInvCkdModuleCaseExcelExporter
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        public InvCkdModuleCaseExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) {
            _tempFileCacheManager = tempFileCacheManager;
        }
        public FileDto ExportToFile(List<InvCkdModuleCaseDto> modulecase)
        {

            var file = new FileDto("InventoryCKDModuleCase.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_InventoryCKDModuleCase";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];
           // workSheet.Cells.GetSubrange($"A2:BB{count + 1}").Style.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromName(ColorName.Black), LineStyle.Thin);

            List<string> listHeader = new List<string>();
            listHeader.Add("Module Case No");
            listHeader.Add("Container No");
            listHeader.Add("Renban");
            listHeader.Add("Supplier No");
            listHeader.Add("Lot No");
            listHeader.Add("Invoice No");
            listHeader.Add("Cd Date");
            listHeader.Add("Devanning Date");
            listHeader.Add("Devanning Time");
            listHeader.Add("Unpacking Date");
            listHeader.Add("Unpacking Time");
            listHeader.Add("Unpacking Datetime");
            listHeader.Add("Storage Location Code");
            listHeader.Add("Location Id");
            listHeader.Add("Location Date");
            listHeader.Add("Location By");
            listHeader.Add("Bill Date");
            listHeader.Add("Net Weight");
            listHeader.Add("Gross Weight");
            listHeader.Add("Measuarement M3");
            listHeader.Add("Length Of Casemodule");
            listHeader.Add("Width Of Casemodule");
            listHeader.Add("Height Of Casemodule");
            listHeader.Add("Dummy");
            listHeader.Add("Planned Packingdate");
            listHeader.Add("Dgflag");
            listHeader.Add("Packing Type");
            listHeader.Add("Rr Type");
            listHeader.Add("Freight Per Invoice");
            listHeader.Add("Insurance Per Invoice");
            listHeader.Add("Inner Material Type 1");
            listHeader.Add("Inner Material Type 2");
            listHeader.Add("Inner Material Type 3");
            listHeader.Add("Inner Material Type 4");
            listHeader.Add("Inner Material Type 5");
            listHeader.Add("Inner Material Type 6");
            listHeader.Add("Inner Material Type 7");
            listHeader.Add("Inner Material Type 8");
            listHeader.Add("Inner Material Type 9");
            listHeader.Add("Inner Material Type 10");
            listHeader.Add("Inner Material Type 11");
            listHeader.Add("Inner Material Type 12");
            listHeader.Add("Inner Material Quantity 1");
            listHeader.Add("Inner Material Quantity 2");
            listHeader.Add("Inner Material Quantity 3");
            listHeader.Add("Inner Material Quantity 4");
            listHeader.Add("Inner Material Quantity 5");
            listHeader.Add("Inner Material Quantity 6");
            listHeader.Add("Inner Material Quantity 7");
            listHeader.Add("Inner Material Quantity 8");
            listHeader.Add("Inner Material Quantity 9");
            listHeader.Add("Inner Material Quantity 10");
            listHeader.Add("Inner Material Quantity 11");
            listHeader.Add("Inner Material Quantity 12");


            List<string> listExport = new List<string>();

            listExport.Add("ModuleCaseNo");
            listExport.Add("ContainerNo");
            listExport.Add("Renban");
            listExport.Add("SupplierNo");
            listExport.Add("LotNo");
            listExport.Add("InvoiceNo");
            listExport.Add("CdDate");
            listExport.Add("DevanningDate");
            listExport.Add("DevanningTime");
            listExport.Add("UnpackingDate");
            listExport.Add("UnpackingTime");
            listExport.Add("UnpackingDatetime");
            listExport.Add("StorageLocationCode");
            listExport.Add("LocationId");
            listExport.Add("LocationDate");
            listExport.Add("LocationBy");
            listExport.Add("BillDate");
            listExport.Add("NetWeight");
            listExport.Add("GrossWeight");
            listExport.Add("MeasuarementM3");
            listExport.Add("Lengthofcasemodule");
            listExport.Add("Widthofcasemodule");
            listExport.Add("Heightofcasemodule");
            listExport.Add("Dummy");
            listExport.Add("Plannedpackingdate");
            listExport.Add("Dgflag");
            listExport.Add("PackingType");
            listExport.Add("RrType");
            listExport.Add("FreightPerInvoice");
            listExport.Add("InsurancePerInvoice");
            listExport.Add("InnerMaterialType1");
            listExport.Add("InnerMaterialType2");
            listExport.Add("InnerMaterialType3");
            listExport.Add("InnerMaterialType4");
            listExport.Add("InnerMaterialType5");
            listExport.Add("InnerMaterialType6");
            listExport.Add("InnerMaterialType7");
            listExport.Add("InnerMaterialType8");
            listExport.Add("InnerMaterialType9");
            listExport.Add("InnerMaterialType10");
            listExport.Add("InnerMaterialType11");
            listExport.Add("InnerMaterialType12");
            listExport.Add("InnerMaterialQuantity1");
            listExport.Add("InnerMaterialQuantity2");
            listExport.Add("InnerMaterialQuantity3");
            listExport.Add("InnerMaterialQuantity4");
            listExport.Add("InnerMaterialQuantity5");
            listExport.Add("InnerMaterialQuantity6");
            listExport.Add("InnerMaterialQuantity7");
            listExport.Add("InnerMaterialQuantity8");
            listExport.Add("InnerMaterialQuantity9");
            listExport.Add("InnerMaterialQuantity10");
            listExport.Add("InnerMaterialQuantity11");
            listExport.Add("InnerMaterialQuantity12");



            string[] properties = listExport.ToArray();
            string[] p_header = listHeader.ToArray();
            Commons.FillExcel2(modulecase, workSheet, 1, 0, properties, p_header);

            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);
            MemoryStream obj_stream = new MemoryStream();
            obj_stream = new MemoryStream(File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;
          /*  return CreateExcelPackage(
                "InventoryCKDModuleCase.xlsx",
                excelPackage =>
                {
                var sheet = excelPackage.CreateSheet("ModuleCase");
                AddHeader(
                            sheet,
                                ("Module Case No"),
                                ("Container No"),
								("Renban"),
                                ("Supplier No"),
								("Lot No"),
								("Invoice No"),
								("Cd Date"),
                                ("Devanning Date"),
                                ("Devanning Time"),
                                ("Unpacking Date"),
                                ("Unpacking Time"),
                                ("Unpacking Datetime"),
                                ("Storage Location Code"),
								("Location Id"),
								("Location Date"),
								("Location By"),
                                ("Bill Date"),
                                ("Net Weight"),
								("Gross Weight"),
								("Measuarement M3"),
								("Length Of Casemodule"),
								("Width Of Casemodule"),
								("Height Of Casemodule"),
								("Dummy"),
								("Planned Packingdate"),
								("Dgflag"),
								("Packing Type"),
								("Rr Type"),
								("Freight Per Invoice"),
								("Insurance Per Invoice"),
								("Inner Material Type 1"),
								("Inner Material Type 2"),
								("Inner Material Type 3"),
								("Inner Material Type 4"),
								("Inner Material Type 5"),
								("Inner Material Type 6"),
								("Inner Material Type 7"),
								("Inner Material Type 8"),
								("Inner Material Type 9"),
								("Inner Material Type 10"),
								("Inner Material Type 11"),
								("Inner Material Type 12"),
								("Inner Material Quantity 1"),
								("Inner Material Quantity 2"),
								("Inner Material Quantity 3"),
								("Inner Material Quantity 4"),
								("Inner Material Quantity 5"),
								("Inner Material Quantity 6"),
								("Inner Material Quantity 7"),
								("Inner Material Quantity 8"),
								("Inner Material Quantity 9"),
								("Inner Material Quantity 10"),
								("Inner Material Quantity 11"),
								("Inner Material Quantity 12")
							   );
            AddObjects(
                 sheet, modulecase,
                        _ => _.ModuleCaseNo,
                        _ => _.ContainerNo,
                        _ => _.Renban,
                        _ => _.SupplierNo,
                        _ => _.LotNo,
                        _ => _.InvoiceNo,
                        _ => _.CdDate,
                        _ => _.DevanningDate,
                        _ => _.DevanningTime,
                        _ => _.UnpackingDate,
                        _ => _.UnpackingTime,
                        _ => _.UnpackingDatetime,
                        _ => _.StorageLocationCode,
                        _ => _.LocationId,
                        _ => _.LocationDate,
                        _ => _.LocationBy,
                        _ => _.BillDate,
                        _ => _.NetWeight,
                        _ => _.GrossWeight,
                        _ => _.MeasuarementM3,
                        _ => _.Lengthofcasemodule,
                        _ => _.Widthofcasemodule,
                        _ => _.Heightofcasemodule,
                        _ => _.Dummy,
                        _ => _.Plannedpackingdate,
                        _ => _.Dgflag,
                        _ => _.PackingType,
                        _ => _.RrType,
                        _ => _.FreightPerInvoice,
                        _ => _.InsurancePerInvoice,
                        _ => _.InnerMaterialType1,
                        _ => _.InnerMaterialType2,
                        _ => _.InnerMaterialType3,
                        _ => _.InnerMaterialType4,
                        _ => _.InnerMaterialType5,
                        _ => _.InnerMaterialType6,
                        _ => _.InnerMaterialType7,
                        _ => _.InnerMaterialType8,
                        _ => _.InnerMaterialType9,
                        _ => _.InnerMaterialType10,
                        _ => _.InnerMaterialType11,
                        _ => _.InnerMaterialType12,
                        _ => _.InnerMaterialQuantity1,
                        _ => _.InnerMaterialQuantity2,
                        _ => _.InnerMaterialQuantity3,
                        _ => _.InnerMaterialQuantity4,
                        _ => _.InnerMaterialQuantity5,
                        _ => _.InnerMaterialQuantity6,
                        _ => _.InnerMaterialQuantity7,
                        _ => _.InnerMaterialQuantity8,
                        _ => _.InnerMaterialQuantity9,
                        _ => _.InnerMaterialQuantity10,
                        _ => _.InnerMaterialQuantity11,
                        _ => _.InnerMaterialQuantity12

                        );

        });*/

        }

    }
}