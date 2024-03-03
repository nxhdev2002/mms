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
	public class InvStockExcelExporter : NpoiExcelExporterBase, IInvStockExcelExporter
	{
		public InvStockExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<InvStockDto> stock)
		{
            #region New Export

            IRow row;
            IRow rowHeader;
            ICell cellHeader;
            ICell cell;
            int rowIndex1 = 0;
            int rowIndex = 1;

            return CreateExcelPackage(
                "InvStock.xlsx",
                xlsxObject =>
                {
                    var sheet = xlsxObject.CreateSheet("InvStock");

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
                    int IP1 = CellReference.ConvertColStringToIndex("P");
                    int IQ1 = CellReference.ConvertColStringToIndex("Q");
                    int IR1 = CellReference.ConvertColStringToIndex("R");
                    int IS1 = CellReference.ConvertColStringToIndex("S");
   

                    rowHeader = sheet.GetRow(0);

                    rowHeader = sheet.GetRow(rowIndex1);
                    if (rowHeader == null)
                    {
                        rowHeader = sheet.CreateRow(rowIndex1);
                    }
                    cellHeader = rowHeader.CreateCell(IA1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("PART_NO");
                    cellHeader = rowHeader.CreateCell(IB1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("SOURCE");
                    cellHeader = rowHeader.CreateCell(IC1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("CARFAMILY_CODE");
                    cellHeader = rowHeader.CreateCell(ID1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("LOT_NO");
                    cellHeader = rowHeader.CreateCell(IE1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("QUANTITY");
                    cellHeader = rowHeader.CreateCell(IF1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("CUSTOMS_DECLARE_NO");
                    cellHeader = rowHeader.CreateCell(IG1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("DECLARE_DATE");
                    cellHeader = rowHeader.CreateCell(IH1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("DC_TYPE");
                    cellHeader = rowHeader.CreateCell(II1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("IN_STOCK_BY_LOT");
                    cellHeader = rowHeader.CreateCell(IJ1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("CIF");
                    cellHeader = rowHeader.CreateCell(IK1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("TAX");
                    cellHeader = rowHeader.CreateCell(IL1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("INLAND");
                    cellHeader = rowHeader.CreateCell(IM1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("COST");
                    cellHeader = rowHeader.CreateCell(IN1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("AMOUNT");
                    cellHeader = rowHeader.CreateCell(IO1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("CIF_VN");
                    cellHeader = rowHeader.CreateCell(IP1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("TAX_VN");
                    cellHeader = rowHeader.CreateCell(IQ1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("INLAND_VN");
                    cellHeader = rowHeader.CreateCell(IR1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("COST_VN");
                    cellHeader = rowHeader.CreateCell(IS1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("AMOUNT_VN");




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
                    int IP = CellReference.ConvertColStringToIndex("P");
                    int IQ = CellReference.ConvertColStringToIndex("Q");
                    int IR = CellReference.ConvertColStringToIndex("R");
                    int IS = CellReference.ConvertColStringToIndex("S");
                 

                    row = sheet.GetRow(1);
                    foreach (InvStockDto lp in stock)
                    {
                        row = sheet.GetRow(rowIndex);
                        if (row == null)
                        {
                            row = sheet.CreateRow(rowIndex);
                        }
                        cell = row.CreateCell(IA, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.PartNo);
                        cell = row.CreateCell(IB, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.Source);
                        cell = row.CreateCell(IC, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.CarFamilyCode);
                        cell = row.CreateCell(ID, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.LotNo);
                        cell = row.CreateCell(IE, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.Quantity.ToString());
                        cell = row.CreateCell(IF, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.CustomsDeclareNo);
                        cell = row.CreateCell(IG, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.DeclareDate.ToString());
                        cell = row.CreateCell(IH, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.DcType);
                        cell = row.CreateCell(II, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.InStockByLot);
                        cell = row.CreateCell(IJ, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.Cif.ToString());
                        cell = row.CreateCell(IK, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.Tax.ToString());
                        cell = row.CreateCell(IL, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.Inland.ToString());
                        cell = row.CreateCell(IM, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.Cost.ToString());
                        cell = row.CreateCell(IN, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.Amount.ToString());
                        cell = row.CreateCell(IO, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.CifVn.ToString());
                        cell = row.CreateCell(IP, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.TaxVn.ToString());
                        cell = row.CreateCell(IQ, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.InlandVn.ToString());
                        cell = row.CreateCell(IR, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.CostVn.ToString());
                        cell = row.CreateCell(IS, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.AmountVn.ToString());

                        rowIndex++;
                    }

                    for (var i = 0; i < 19; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
            #endregion


            #region
    //        return CreateExcelPackage(
				//"InvStock.xlsx",
				//excelPackage =>
				//{
				//	var sheet = excelPackage.CreateSheet("Stock");
				//	AddHeader(
				//				sheet,					
				//				("PART_NO"),
				//				("SOURCE"),
				//				("CARFAMILY_CODE"),
				//				("LOT_NO"),
				//				("QUANTITY"),
				//				("CUSTOMS_DECLARE_NO"),
				//				("DECLARE_DATE"),
				//				("DC_TYPE"),
				//				("IN_STOCK_BY_LOT"),
				//				("CIF"),
				//				("TAX"),
				//				("INLAND"),
				//				("COST"),
				//				("AMOUNT"),
				//				("CIF_VN"),
				//				("TAX_VN"),
				//				("INLAND_VN"),
				//				("COST_VN"),
				//				("AMOUNT_VN")
				//			   );
				//	AddObjects(
				//		 sheet, stock,
				//				_ => _.PartNo,
				//				_ => _.Source,
				//				_ => _.CarFamilyCode,
				//				_ => _.LotNo,
				//				_ => _.Quantity,
				//				_ => _.CustomsDeclareNo,
				//				_ => _.DeclareDate,
				//				_ => _.DcType,
				//				_ => _.InStockByLot,
				//				_ => _.Cif,
				//				_ => _.Tax,
				//				_ => _.Inland,
				//				_ => _.Cost,
				//				_ => _.Amount,
				//				_ => _.CifVn,
				//				_ => _.TaxVn,
				//				_ => _.InlandVn,
				//				_ => _.CostVn,
				//				_ => _.AmountVn
				//				);

				//	for (var i = 0; i < 20; i++)
				//	{
				//		sheet.AutoSizeColumn(i);
				//	}
				//});
            #endregion

        }
    }
}
