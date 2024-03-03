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
	public class InvOutLineOffExcelExporter : NpoiExcelExporterBase, IInvOutLineOffExcelExporter
	{
		public InvOutLineOffExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<InvOutLineOffDto> invoutlineoff)
		{
            #region New Export

            IRow row;
            IRow rowHeader;
            ICell cellHeader;
            ICell cell;
            int rowIndex1 = 0;
            int rowIndex = 1;

            return CreateExcelPackage(
                "InvInvOutLineOff.xlsx",
                xlsxObject =>
                {
                    var sheet = xlsxObject.CreateSheet("InvInvOutLineOff");

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
                    int IK1 = CellReference.ConvertColStringToIndex("K");
                    int IL1 = CellReference.ConvertColStringToIndex("L");
                    int IM1 = CellReference.ConvertColStringToIndex("M");
                    int IN1 = CellReference.ConvertColStringToIndex("N");
                    int IO1 = CellReference.ConvertColStringToIndex("O");


                    rowHeader = sheet.GetRow(0);




                    rowHeader = sheet.GetRow(rowIndex1);
                    if (rowHeader == null)
                    {
                        rowHeader = sheet.CreateRow(rowIndex1);
                    }
                    cellHeader = rowHeader.CreateCell(IA1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("PART_NO");
                    cellHeader = rowHeader.CreateCell(IB1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("CARFAMILY_CODE");
                    cellHeader = rowHeader.CreateCell(IC1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("USAGE_QTY");
                    cellHeader = rowHeader.CreateCell(ID1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("SUM_CIF");
                    cellHeader = rowHeader.CreateCell(IE1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("SUM_TAX");
                    cellHeader = rowHeader.CreateCell(IF1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("SUM_INLAND");
                    cellHeader = rowHeader.CreateCell(IG1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("AMOUNT");
                    cellHeader = rowHeader.CreateCell(IH1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("SUM_CIF_VN");
                    cellHeader = rowHeader.CreateCell(II1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("SUM_TAX_VN");
                    cellHeader = rowHeader.CreateCell(IJ1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("SUM_INLAND_VN");
                    cellHeader = rowHeader.CreateCell(IK1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("AMOUNT_VN");
                    cellHeader = rowHeader.CreateCell(IL1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("CUSTOMS_DECLARE_NO");
                    cellHeader = rowHeader.CreateCell(IM1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("DECLARE_DATE");
                    cellHeader = rowHeader.CreateCell(IN1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("DC_TYPE");
                    cellHeader = rowHeader.CreateCell(IO1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("IN_STOCK_BY_LOT");




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
                    int IK = CellReference.ConvertColStringToIndex("K");
                    int IL = CellReference.ConvertColStringToIndex("L");
                    int IM = CellReference.ConvertColStringToIndex("M");
                    int IN = CellReference.ConvertColStringToIndex("N");
                    int IO = CellReference.ConvertColStringToIndex("O");
                   

                    row = sheet.GetRow(1);
                    foreach (InvOutLineOffDto lp in invoutlineoff)
                    {
                        row = sheet.GetRow(rowIndex);
                        if (row == null)
                        {
                            row = sheet.CreateRow(rowIndex);
                        }
                        cell = row.CreateCell(IA, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.PartNo);
                        cell = row.CreateCell(IB, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.CarFamilyCode);
                        cell = row.CreateCell(IC, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.UsageQty.ToString());
                        cell = row.CreateCell(ID, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.SumCif.ToString());
                        cell = row.CreateCell(IE, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.SumTax.ToString());
                        cell = row.CreateCell(IF, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.SumInland.ToString());
                        cell = row.CreateCell(IG, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.Amount.ToString());
                        cell = row.CreateCell(IH, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.SumCifVn.ToString());
                        cell = row.CreateCell(II, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.SumTaxVn.ToString());
                        cell = row.CreateCell(IJ, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.SumInlandVn.ToString());
                        cell = row.CreateCell(IK, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.AmountVn.ToString());
                        cell = row.CreateCell(IL, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.CustomsDeclareNo);
                        cell = row.CreateCell(IM, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.DeclareDate.ToString());
                        cell = row.CreateCell(IN, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.DcType);
                        cell = row.CreateCell(IO, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.InStockByLot);


                        rowIndex++;
                    }

                    for (var i = 0; i < 24; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
            #endregion


            #region Old Export
    //        return CreateExcelPackage(
				//"InvInvOutLineOff.xlsx",
				//excelPackage =>
				//{
				//	var sheet = excelPackage.CreateSheet("InvOutLineOff");
				//	AddHeader(
				//				sheet,
				//				("PART_NO"),
				//				("CARFAMILY_CODE"),
				//				("USAGE_QTY"),
				//				("SUM_CIF"),
				//				("SUM_TAX"),
				//				("SUM_INLAND"),
				//				("AMOUNT"),
				//				("SUM_CIF_VN"),
				//				("SUM_TAX_VN"),
				//				("SUM_INLAND_VN"),
				//				("AMOUNT_VN"),
				//				("CUSTOMS_DECLARE_NO"),
				//				("DECLARE_DATE"),
				//				("DC_TYPE"),
				//				("IN_STOCK_BY_LOT")
				//			   );
				//	AddObjects(
				//		 sheet, invoutlineoff,
				//				_ => _.PartNo,
				//				_ => _.CarFamilyCode,
				//				_ => _.UsageQty,
				//				_ => _.SumCif,
				//				_ => _.SumTax,
				//				_ => _.SumInland,
				//				_ => _.Amount,
				//				_ => _.SumCifVn,
				//				_ => _.SumTaxVn,
				//				_ => _.SumInlandVn,
				//				_ => _.AmountVn,
				//				_ => _.CustomsDeclareNo,
				//				_ => _.DeclareDate,
				//				_ => _.DcType,
				//				_ => _.InStockByLot
				//				);

				//	for (var i = 0; i < 15; i++)
				//	{
				//		sheet.AutoSizeColumn(i);
				//	}
				//});
            #endregion
        }
    }
}
