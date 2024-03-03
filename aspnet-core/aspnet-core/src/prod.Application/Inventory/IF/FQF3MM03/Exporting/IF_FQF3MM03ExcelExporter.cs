using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Storage;
using prod.Inventory.IF.Exporting;
using prod.Inventory.IF.FQF3MM03.Dto;

namespace prod.IF.IF.Exporting
{
    public class IF_FQF3MM03ExcelExporter : NpoiExcelExporterBase, IIF_FQF3MM03ExcelExporter
    {
        public IF_FQF3MM03ExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<IF_FQF3MM03Dto> fqf3mm03)
        {
            return CreateExcelPackage(
                "IFIFFQF3MM03.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("FQF3MM03");
                    AddHeader(
                                sheet,
                                ("RecordId(M)"),
                                ("CompanyCode(M)"),
                                ("DocumentNo(O)"),
                                ("DocumentType(M)"),
                                ("DocumentDate(M)"),
                                ("CustomerCode(O)"),
                                ("CustomerPlantCode(M)"),
                                ("CustomerDockCode(M)"),
                                ("PartCategory(M)"),
                                ("WithholdingTaxFlag(O)"),
                                ("WithholdingTaxRate(O)"),
                                ("OrderType(M)"),
                                ("PdsNo(M)"),
                                ("PartReceivedDate(M)"),
                                ("SequenceDate(M)"),
                                ("SequenceNo(M)"),
                                ("PartNo(M)"),
                                ("ReferenceDocumentNo(O)"),
                                ("PostingDate(M)"),
                                ("SupplierCode(M)"),
                                ("SupplierPlantCode(O)"),
                                ("PartQuantity(M)"),
                                ("UnitBuyingPrice(M)"),
                                ("UnitBuyingAmount(O)"),
                                ("UnitSellingPrice(O)"),
                                ("UnitSellingAmount(O)"),
                                ("PriceStatus(M)"),
                                ("TotalAmount(O)"),
                                ("VatAmount(O)"),
                                ("VatCode(M)"),
                                ("PaymentTerm(O)"),
                                ("ReasonCode(O)"),
                                ("MarkCode(O)"),
                                ("SignCode(O)"),
                                ("CancelFlag(O)"),
                                ("SupplierInvoiceNo(O)"),
                                ("TopasSmrNo(O)"),
                                ("TopasSmrItemNo(O)"),
                                ("CustomerBranch(O)"),
                                ("CostCenter(O)"),
                                ("Wbs(O)"),
                                ("Asset(O)"),
                                ("OrderReasonCode(O)"),
                                ("RetroFlag(O)"),
                                ("ValuationType(M)"),
                                ("ConditionType(O)"),
                                ("ConditionTypeAmt(O)"),
                                ("PrepaidTaxAmt(O)"),
                                ("WithholdingTaxAmt(O)"),
                                ("StampFeeAmt(O)"),
                                ("GlAmount(O)"),
                                ("SptCode(M)"),
                                ("HigherLevelItem(O)"),
                                ("WithholdingTaxBaseAmt(O)"),
                                ("TypeOfSales(O)"),
                                ("ProfitCenter(O)"),
                                ("DueDate(O)"),
                                ("ItemText(O)"),
                                ("PaymentMethod(O)"),
                                ("EndingOfRecord(M)")

                               );
                    AddObjects(
                         sheet,  fqf3mm03,
                                _ => _.RecordId,
                                _ => _.CompanyCode,
                                _ => _.DocumentNo,
                                _ => _.DocumentType,
                                _ => _.DocumentDate,
                                _ => _.CustomerCode,
                                _ => _.CustomerPlantCode,
                                _ => _.CustomerDockCode,
                                _ => _.PartCategory,
                                _ => _.WithholdingTaxFlag,
                                _ => _.WithholdingTaxRate,
                                _ => _.OrderType,
                                _ => _.PdsNo,
                                _ => _.PartReceivedDate,
                                _ => _.SequenceDate,
                                _ => _.SequenceNo,
                                _ => _.PartNo,
                                _ => _.ReferenceDocumentNo,
                                _ => _.PostingDate,
                                _ => _.SupplierCode,
                                _ => _.SupplierPlantCode,
                                _ => _.PartQuantity,
                                _ => _.UnitBuyingPrice,
                                _ => _.UnitBuyingAmount,
                                _ => _.UnitSellingPrice,
                                _ => _.UnitSellingAmount,
                                _ => _.PriceStatus,
                                _ => _.TotalAmount,
                                _ => _.VatAmount,
                                _ => _.VatCode,
                                _ => _.PaymentTerm,
                                _ => _.ReasonCode,
                                _ => _.MarkCode,
                                _ => _.SignCode,
                                _ => _.CancelFlag,
                                _ => _.SupplierInvoiceNo,
                                _ => _.TopasSmrNo,
                                _ => _.TopasSmrItemNo,
                                _ => _.CustomerBranch,
                                _ => _.CostCenter,
                                _ => _.Wbs,
                                _ => _.Asset,
                                _ => _.OrderReasonCode,
                                _ => _.RetroFlag,
                                _ => _.ValuationType,
                                _ => _.ConditionType,
                                _ => _.ConditionTypeAmt,
                                _ => _.PrepaidTaxAmt,
                                _ => _.WithholdingTaxAmt,
                                _ => _.StampFeeAmt,
                                _ => _.GlAmount,
                                _ => _.SptCode,
                                _ => _.HigherLevelItem,
                                _ => _.WithholdingTaxBaseAmt,
                                _ => _.TypeOfSales,
                                _ => _.ProfitCenter,
                                _ => _.DueDate,
                                _ => _.ItemText,
                                _ => _.PaymentMethod,
                                _ => _.EndingOfRecord
                                );

                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });

        }



        public FileDto ExportValidateToFile(List<GetIF_FQF3MM03_VALIDATE> fqf3mm03)
        {
            return CreateExcelPackage(
                "VALIDATEFFQF3MM03.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("VALIDATE_FQF3MM03");
                    AddHeader(
                                sheet,
                                ("ErrorDescription"),
                                ("RecordId(M)"),
                                ("CompanyCode(M)"),
                                ("DocumentType(M)"),
                                ("DocumentDate(M)"),
                                ("CustomerPlantCode(M)"),
                                ("CustomerDockCode(M)"),
                                ("PartCategory(M)"),
                                ("OrderType(M)"),
                                ("PdsNo(M)"),
                                ("PartReceivedDate(M)"),
                                ("SequenceDate(M)"),
                                ("SequenceNo(M)"),
                                ("PartNo(M)"),
                                ("PostingDate(M)"),
                                ("SupplierCode(M)"),
                                ("PartQuantity(M)"),
                                ("UnitBuyingPrice(M)"),
                                ("PriceStatus(M)"),
                                ("VatCode(M)"),
                                ("ValuationType(M)"),
                                ("SptCode(M)"),
                                ("EndingOfRecord(M)"),
                                ("HeaderFwgId"),
                                ("HeaderId"),
                                ("TrailerId")
                         

                               );
                    AddObjects(
                         sheet, fqf3mm03,
                               _ => _.ErrorDescription,
                                _ => _.RecordId,
                                _ => _.CompanyCode,
                                _ => _.DocumentType,
                                _ => _.DocumentDate,
                                _ => _.CustomerPlantCode,
                                _ => _.CustomerDockCode,
                                _ => _.PartCategory,
                                _ => _.OrderType,
                                _ => _.PdsNo,
                                _ => _.PartReceivedDate,
                                _ => _.SequenceDate,
                                _ => _.SequenceNo,
                                _ => _.PartNo,
                                _ => _.PostingDate,
                                _ => _.SupplierCode,
                                _ => _.PartQuantity,
                                _ => _.UnitBuyingPrice,
                                _ => _.PriceStatus,
                                _ => _.VatCode,
                                _ => _.ValuationType,
                                _ => _.SptCode,
                                _ => _.EndingOfRecord,
                                _ => _.HeaderFwgId,
                                _ => _.HeaderId,
                                _ => _.TrailerId
                          
                                );

                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });

        }
    }
}
