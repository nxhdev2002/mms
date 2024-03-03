using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inv.D125.Dto;
using prod.Inv.Dmr.Dto;
using prod.Storage;

namespace prod.Inv.D125.Exporting
{
	public class InvInDetailsExcelExporter : NpoiExcelExporterBase, IInvInDetailsExcelExporter
	{
		public InvInDetailsExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<InvInDetailsDto> indetails)
		{
            #region New Export
            IRow row;
            IRow rowHeader;
            ICell cellHeader;
            ICell cell;
            int rowIndex1 = 0;
            int rowIndex = 1;

            return CreateExcelPackage(
                "InvInDetails.xlsx",
                xlsxObject =>
                {
                    var sheet = xlsxObject.CreateSheet("InvInDetails");

                    ICellStyle istyle = xlsxObject.CreateCellStyle();
                    istyle.Alignment = HorizontalAlignment.Left;
                    istyle.FillPattern = FillPattern.SolidForeground;
                    istyle.FillForegroundColor = IndexedColors.White.Index;
                    istyle.BorderBottom = BorderStyle.Thin;
                    istyle.BorderTop = BorderStyle.Thin;
                    istyle.BorderLeft = BorderStyle.Thin;
                    istyle.BorderRight = BorderStyle.Thin;

                    ICellStyle istyle2 = xlsxObject.CreateCellStyle();
                    istyle2.Alignment = HorizontalAlignment.Right;
                    istyle2.FillPattern = FillPattern.SolidForeground;
                    istyle2.FillForegroundColor = IndexedColors.White.Index;
                    istyle2.BorderBottom = BorderStyle.Thin;
                    istyle2.BorderTop = BorderStyle.Thin;
                    istyle2.BorderLeft = BorderStyle.Thin;
                    istyle2.BorderRight = BorderStyle.Thin;

                    var font = xlsxObject.CreateFont();
                    font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                    font.FontHeight = 250;
                    ICellStyle istyle3 = xlsxObject.CreateCellStyle();
                    istyle3.FillPattern = FillPattern.SolidForeground;
                    istyle3.FillForegroundColor = IndexedColors.White.Index;
                    istyle3.BorderBottom = BorderStyle.Thin;
                    istyle3.BorderTop = BorderStyle.Thin;
                    istyle3.BorderLeft = BorderStyle.Thin;
                    istyle3.BorderRight = BorderStyle.Thin;
                    istyle3.SetFont(font);




                    //Header
                    int IA1 = CellReference.ConvertColStringToIndex("A");
                    int IB1 = CellReference.ConvertColStringToIndex("B");
                    int IC1 = CellReference.ConvertColStringToIndex("C");
                    int ID1 = CellReference.ConvertColStringToIndex("D");
                    int IE1 = CellReference.ConvertColStringToIndex("E");
                    int IF1 = CellReference.ConvertColStringToIndex("F");
                    int IG1 = CellReference.ConvertColStringToIndex("G");
                    int IH1 = CellReference.ConvertColStringToIndex("H");
                    int II1 = CellReference.ConvertColStringToIndex("I");
                    int IJ1 = CellReference.ConvertColStringToIndex("J");
                  
                    rowHeader = sheet.GetRow(0);

                    rowHeader = sheet.GetRow(rowIndex1);
                    if (rowHeader == null)
                    {
                        rowHeader = sheet.CreateRow(rowIndex1);
                    }
                    cellHeader = rowHeader.CreateCell(IA1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("INVOICE_NO");
                    cellHeader = rowHeader.CreateCell(IB1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("PART_NO");
                    cellHeader = rowHeader.CreateCell(IC1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("USAGE_QTY");
                    cellHeader = rowHeader.CreateCell(ID1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("INVOICE_DATE");
                    cellHeader = rowHeader.CreateCell(IE1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("RECEIVE_DATE");
                    cellHeader = rowHeader.CreateCell(IF1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("SUPPLIER_NO");
                    cellHeader = rowHeader.CreateCell(IG1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("FIXLOT");
                    cellHeader = rowHeader.CreateCell(IH1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("CARFAMILY_CODE");
                    cellHeader = rowHeader.CreateCell(II1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("CUSTOMS_DECLARE_NO");
                    cellHeader = rowHeader.CreateCell(IJ1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("DECLARE_DATE");
                  


                    //Data

                    int IA = CellReference.ConvertColStringToIndex("A");
                    int IB = CellReference.ConvertColStringToIndex("B");
                    int IC = CellReference.ConvertColStringToIndex("C");
                    int ID = CellReference.ConvertColStringToIndex("D");
                    int IE = CellReference.ConvertColStringToIndex("E");
                    int IF = CellReference.ConvertColStringToIndex("F");
                    int IG = CellReference.ConvertColStringToIndex("G");
                    int IH = CellReference.ConvertColStringToIndex("H");
                    int II = CellReference.ConvertColStringToIndex("I");
                    int IJ = CellReference.ConvertColStringToIndex("J");
               

                    row = sheet.GetRow(1);
                    foreach (InvInDetailsDto lp in indetails)
                    {
                        row = sheet.GetRow(rowIndex);
                        if (row == null)
                        {
                            row = sheet.CreateRow(rowIndex);
                        }
                        cell = row.CreateCell(IA, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.InvoiceNo);
                        cell = row.CreateCell(IB, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.PartNo);
                        cell = row.CreateCell(IC, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.UsageQty.ToString());
                        cell = row.CreateCell(ID, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.InvoiceDate.ToString());
                        cell = row.CreateCell(IE, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.ReceiveDate.ToString());
                        cell = row.CreateCell(IF, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.SupplierNo);
                        cell = row.CreateCell(IG, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.FixLot);
                        cell = row.CreateCell(IH, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.CarfamilyCode);
                        cell = row.CreateCell(II, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.CustomsDeclareNo);
                        cell = row.CreateCell(IJ, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.DeclareDate.ToString());

                        rowIndex++;
                    }

                  
                });
            #endregion

            #region Old Export
            //return CreateExcelPackage(
            //    "InvInDetails.xlsx",
            //    excelPackage =>
            //    {
            //        var sheet = excelPackage.CreateSheet("InDetails");
            //        AddHeader(
            //                    sheet,
            //                    ("INVOICE_NO"),
            //                    ("PART_NO"),
            //                    ("USAGE_QTY"),
            //                    ("INVOICE_DATE"),
            //                    ("RECEIVE_DATE"),
            //                    ("SUPPLIER_NO"),
            //                    ("FIXLOT"),
            //                    ("CARFAMILY_CODE"),
            //                    ("CUSTOMS_DECLARE_NO"),
            //                    ("DECLARE_DATE")
            //                   );
            //        AddObjects(
            //             sheet, indetails,
            //                    _ => _.InvoiceNo,
            //                    _ => _.PartNo,
            //                    _ => _.UsageQty,
            //                    _ => _.InvoiceDate,
            //                    _ => _.ReceiveDate,
            //                    _ => _.SupplierNo,
            //                    _ => _.FixLot,
            //                    _ => _.CarfamilyCode,
            //                    _ => _.CustomsDeclareNo,
            //                    _ => _.DeclareDate
            //                    );

            //        for (var i = 0; i < 10; i++)
            //        {
            //            sheet.AutoSizeColumn(i);
            //        }
            //    });
            #endregion
        }
    }
}

