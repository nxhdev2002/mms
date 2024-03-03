using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Storage;
using prod.Inv.Dmr.Dto;
using Abp.Extensions;
using GemBox.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Util;
using System.Globalization;
using System;

namespace prod.Inv.Dmr.Exporting
{
    public class InvImportByContExcelExporter : NpoiExcelExporterBase, IInvImportByContExcelExporter
    {
        public InvImportByContExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvImportByContDto> importbycont)
        {
            #region New Export

            IRow row;
            IRow rowHeader;
            ICell cellHeader;
            ICell cell;
            int rowIndex1 = 0;
            int rowIndex = 1;

            return CreateExcelPackage(
                "InvImportByCont.xlsx",
                xlsxObject =>
                {
                    var sheet = xlsxObject.CreateSheet("ImportByCont");

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
                    int IT1 = CellReference.ConvertColStringToIndex("T");
                    int IU1 = CellReference.ConvertColStringToIndex("U");
                    int IV1 = CellReference.ConvertColStringToIndex("V");
                    int IW1 = CellReference.ConvertColStringToIndex("W");
                    int IX1 = CellReference.ConvertColStringToIndex("X");

                    rowHeader = sheet.GetRow(0);
                   


               
                    rowHeader = sheet.GetRow(rowIndex1);
                    if (rowHeader == null)
                    {
                            rowHeader = sheet.CreateRow(rowIndex1);
                    }
                    cellHeader = rowHeader.CreateCell(IA1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("CONTAINER_NO");
                   cellHeader = rowHeader.CreateCell(IB1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("INVOICE_NO");
                   cellHeader = rowHeader.CreateCell(IC1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("CASE_NO");
                   cellHeader = rowHeader.CreateCell(ID1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("LOT_NO");
                   cellHeader = rowHeader.CreateCell(IE1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("PART_NO");
                   cellHeader = rowHeader.CreateCell(IF1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("DATE_IN");
                   cellHeader = rowHeader.CreateCell(IG1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("FOB");
                   cellHeader = rowHeader.CreateCell(IH1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("CIF");
                   cellHeader = rowHeader.CreateCell(II1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("IMPORT_TAX");
                   cellHeader = rowHeader.CreateCell(IJ1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("INLAND_CHARGE");
                   cellHeader = rowHeader.CreateCell(IK1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("AMOUNT");
                   cellHeader = rowHeader.CreateCell(IL1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("QTY");
                   cellHeader = rowHeader.CreateCell(IM1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("PRICE");
                   cellHeader = rowHeader.CreateCell(IN1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("FOB_VN");
                   cellHeader = rowHeader.CreateCell(IO1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("CIF_VN");
                   cellHeader = rowHeader.CreateCell(IP1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("IMPORT_TAX_VN");
                   cellHeader = rowHeader.CreateCell(IQ1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("INLAND_CHARGE_VN");
                   cellHeader = rowHeader.CreateCell(IR1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("AMOUNT_VN");
                   cellHeader = rowHeader.CreateCell(IS1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("PRICE_VN");
                   cellHeader = rowHeader.CreateCell(IT1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("INVOICE_DATE");
                   cellHeader = rowHeader.CreateCell(IU1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("RECEIVE_DATE");
                   cellHeader = rowHeader.CreateCell(IV1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("CONT_SIZE");
                   cellHeader = rowHeader.CreateCell(IW1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("ETA");
                   cellHeader = rowHeader.CreateCell(IX1, CellType.String); cellHeader.CellStyle = istyle3; cellHeader.SetCellValue("SUPPLIER_NO");






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
                    int IT = CellReference.ConvertColStringToIndex("T");
                    int IU = CellReference.ConvertColStringToIndex("U");
                    int IV = CellReference.ConvertColStringToIndex("V");
                    int IW = CellReference.ConvertColStringToIndex("W");
                    int IX = CellReference.ConvertColStringToIndex("X");

                    row = sheet.GetRow(1);
                    string v_DateIn = "";
                    string v_InvoiceDate = "";
                    string v_ReceiveDate = "";
                    string v_Eta = "";

                    foreach (InvImportByContDto lp in importbycont)
                    {
                        row = sheet.GetRow(rowIndex);
                        if (row == null)
                        {
                            row = sheet.CreateRow(rowIndex);
                        }
                        cell = row.CreateCell(IA, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.ContainerNo);
                        cell = row.CreateCell(IB, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.InvoiceNo);
                        cell = row.CreateCell(IC, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.CaseNo);
                        cell = row.CreateCell(ID, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.LotNo);
                        cell = row.CreateCell(IE, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.PartNo);
                        cell = row.CreateCell(IF, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.DateIn != null ? ((DateTime)lp.DateIn).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) :  null);
                        cell = row.CreateCell(IG, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.Fob.ToString());
                        cell = row.CreateCell(IH, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.Cif.ToString());
                        cell = row.CreateCell(II, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.ImportTax.ToString());
                        cell = row.CreateCell(IJ, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.InlandCharge.ToString());
                        cell = row.CreateCell(IK, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.Amount.ToString());
                        cell = row.CreateCell(IL, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.Qty.ToString());
                        cell = row.CreateCell(IM, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.Price.ToString());
                        cell = row.CreateCell(IN, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.FobVn.ToString());
                        cell = row.CreateCell(IO, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.CifVn.ToString());
                        cell = row.CreateCell(IP, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.ImportTaxVn.ToString());
                        cell = row.CreateCell(IQ, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.InlandChargeVn.ToString());
                        cell = row.CreateCell(IR, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.AmountVn.ToString());
                        cell = row.CreateCell(IS, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue(lp.PriceVn.ToString());
                        cell = row.CreateCell(IT, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.InvoiceDate != null ? ((DateTime)lp.InvoiceDate).ToString("dd/M/yyyy", CultureInfo.InvariantCulture) : null);
                        cell = row.CreateCell(IU, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.ReceiveDate != null ? ((DateTime)lp.ReceiveDate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null);
                        cell = row.CreateCell(IV, CellType.String); cell.CellStyle = istyle2; cell.SetCellValue((lp.ContSize ?? 0).ToString());
                        cell = row.CreateCell(IW, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.Eta != null ? ((DateTime)lp.Eta).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null);
                        cell = row.CreateCell(IX, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.SupplierNo);

                        rowIndex++;
                    }

                    for (var i = 0; i < 24; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
            #endregion

            #region Old Export
            //return CreateExcelPackage(
            //    "InvImportByCont.xlsx",
            //    excelPackage =>
            //    {
            //        var sheet = excelPackage.CreateSheet("ImportByCont");
            //        AddHeader(
            //                    sheet,
            //                    ("CONTAINER_NO"),
            //                    ("INVOICE_NO"),
            //                    ("CASE_NO"),
            //                    ("LOT_NO"),
            //                    ("PART_NO"),
            //                    ("DATE_IN"),
            //                    ("FOB"),
            //                    ("CIF"),
            //                    ("IMPORT_TAX"),
            //                    ("INLAND_CHARGE"),
            //                    ("AMOUNT"),
            //                    ("QTY"),
            //                    ("PRICE"),
            //                    ("FOB_VN"),
            //                    ("CIF_VN"),
            //                    ("IMPORT_TAX_VN"),
            //                    ("INLAND_CHARGE_VN"),
            //                    ("AMOUNT_VN"),
            //                    ("PRICE_VN"),
            //                    ("INVOICE_DATE"),
            //                    ("RECEIVE_DATE"),
            //                    ("CONT_SIZE"),
            //                    ("ETA"),
            //                    ("SUPPLIER_NO")
            //                   );
            //        AddObjects(
            //             sheet, importbycont,
            //                    _ => _.ContainerNo,
            //                    _ => _.InvoiceNo,
            //                    _ => _.CaseNo,
            //                    _ => _.LotNo,
            //                    _ => _.PartNo,
            //                    _ => _.DateIn,
            //                    _ => _.Fob,
            //                    _ => _.Cif,
            //                    _ => _.ImportTax,
            //                    _ => _.InlandCharge,
            //                    _ => _.Amount,
            //                    _ => _.Qty,
            //                    _ => _.Price,
            //                    _ => _.FobVn,
            //                    _ => _.CifVn,
            //                    _ => _.ImportTaxVn,
            //                    _ => _.InlandChargeVn,
            //                    _ => _.AmountVn,
            //                    _ => _.PriceVn,
            //                    _ => _.InvoiceDate,
            //                    _ => _.ReceiveDate,
            //                    _ => _.ContSize,
            //                    _ => _.Eta,
            //                    _ => _.SupplierNo
            //                    );

            //        for (var i = 0; i < 24; i++)
            //        {
            //            sheet.AutoSizeColumn(i);
            //        }
            //    });
            #endregion
        }
    }
}
