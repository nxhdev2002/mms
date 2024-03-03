using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.IF.IF.Exporting;
using prod.Storage;
using prod.Inventory.IF.FQF3MM07.Dto;

namespace prod.Inventory.IF.FQF3MM07.Exporting

{
    public class IF_FQF3MM07ExcelExporter : NpoiExcelExporterBase, IIF_FQF3MM07ExcelExporter
    {
        public IF_FQF3MM07ExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<IF_FQF3MM07Dto> fqf3mm07)
        {
            return CreateExcelPackage(
                "IFIFFQF3MM07.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("FQF3MM07");
                    AddHeader(
                                sheet,
                                ("RecordId (M)"),
                                ("CountryCode (O)"),
                                ("CompanyCode (M)"),
                                ("CompanyBranch (O)"),
                                ("PostingKey (M)"),
                                ("CostCenter (O)"),
                                ("DocumentNo (M)"),
                                ("DocumentType (M)"),
                                ("DocumentDate (M)"),
                                ("PostingDate (M)"),
                                ("Plant (O)"),
                                ("ReferenceDocumentNo (O)"),
                                ("Amount (M)"),
                                ("Currency (M)"),
                                ("Order (O)"),
                                ("GlAccount (O)"),
                                ("NormalCancelFlag (M)"),
                                ("Text (O)"),
                                ("ProfitCenter (O)"),
                                ("Wbs (O)"),
                                ("Quantity (O)"),
                                ("BaseUnitOfMeasure (O)"),
                                ("AmountInLocalCurrency (O)"),
                                ("ExchangeRate (O)"),
                                ("RefKey1 (O)"),
                                ("RefKey2 (O)"),
                                ("RefKey3 (O)"),
                                ("EarmarkFund (O)"),
                                ("EarmarkFundItem (O)"),
                                ("MaterialNo (O)"),
                                ("MainAssetNumber (O)"),
                                ("AssetSubNumber (O)"),
                                ("TransType (O)"),
                                ("EndingOfRecord (M)")

                               );
                    AddObjects(
                         sheet, fqf3mm07,
                                _ => _.RecordId,
                                _ => _.CountryCode,
                                _ => _.CompanyCode,
                                _ => _.CompanyBranch,
                                _ => _.PostingKey,
                                _ => _.CostCenter,
                                _ => _.DocumentNo,
                                _ => _.DocumentType,
                                _ => _.DocumentDate,
                                _ => _.PostingDate,
                                _ => _.Plant,
                                _ => _.ReferenceDocumentNo,
                                _ => _.Amount,
                                _ => _.Currency,
                                _ => _.Order,
                                _ => _.GlAccount,
                                _ => _.NormalCancelFlag,
                                _ => _.Text,
                                _ => _.ProfitCenter,
                                _ => _.Wbs,
                                _ => _.Quantity,
                                _ => _.BaseUnitOfMeasure,
                                _ => _.AmountInLocalCurrency,
                                _ => _.ExchangeRate,
                                _ => _.RefKey1,
                                _ => _.RefKey2,
                                _ => _.RefKey3,
                                _ => _.EarmarkFund,
                                _ => _.EarmarkFundItem,
                                _ => _.MaterialNo,
                                _ => _.MainAssetNumber,
                                _ => _.AssetSubNumber,
                                _ => _.TransType,
                                _ => _.EndingOfRecord

                                );

                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });

        }

        public FileDto ExportItemDMToFile(List<GetIF_FundCommitmentItemDMExportDto> if_FundCommitmentItemDM)
        {
            return CreateExcelPackage(
                "FUND_COMMITMENT_ITEM_DM.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("FUND_COMMITMENT_ITEM_DM");
                    AddHeader(
                                sheet,
                              
                                ("FundCommitmentHeaderType"),
                                ("FundCommitmentHeaderId"),
                                ("FundCommitmentItemId"),
                                ("DocumentNo"),
                                ("LineNo"),
                                ("Closed"),
                                ("ReferenceDocumentNo"),
                                ("ReferenceDocumentLineItemNo"),
                                ("ItemCode"),
                                ("ItemDescription"),
                                ("PartCategory"),
                                ("InventoryType"),
                                ("MaterialType"),
                                ("SupplierCode"),
                                ("Asset"),
                                ("WbsElement"),
                                ("CostCenterCharger"),
                                ("TotalAmount"),
                                ("Quantity"),
                                ("Uom"),
                                ("JournalSource"),
                                ("GlAccount"),
                                ("SubmitDate"),
                                ("Action"),
                                ("MarkAsSapTransfer"),
                                ("LatestSapSuccessTransferDate"),
                                ("LatestSapTransferUserId"),
                                ("LatestSapTransferDate"),
                                ("LatestSapTransferMessage"),
                                ("MarkAsLegacyTransfer"),
                                ("LatestLegacySuccessTransferDate"),
                                ("LatestLegacyTransferUserId"),
                                ("LatestLegacyTransferDate"),
                                ("LatestLegacyTransferMessage"),
                                ("EarmarkedFundsDocument"),
                                ("EarmarkedFundsDocumentItem"),
                                ("FundCommitmentHeaderNo"),
                                ("BudgetCodeOld"),
                                ("CostCenterOld"),
                                ("GlAccountOld")

                               );
                    AddObjects(
                         sheet, if_FundCommitmentItemDM,
                               
                                    _ => _.FundCommitmentHeaderType,
                                     _ => _.FundCommitmentHeaderId,
                                     _ => _.FundCommitmentItemId,
                                     _ => _.DocumentNo,
                                     _ => _.LineNo,
                                     _ => _.Closed,
                                     _ => _.ReferenceDocumentNo,
                                     _ => _.ReferenceDocumentLineItemNo,
                                     _ => _.ItemCode,
                                     _ => _.ItemDescription,
                                     _ => _.PartCategory,
                                     _ => _.InventoryType,
                                     _ => _.MaterialType,
                                     _ => _.SupplierCode,
                                     _ => _.Asset,
                                     _ => _.WbsElement,
                                     _ => _.CostCenterCharger,
                                     _ => _.TotalAmount,
                                     _ => _.Quantity,
                                     _ => _.Uom,
                                     _ => _.JournalSource,
                                     _ => _.GlAccount,
                                     _ => _.SubmitDate,
                                     _ => _.Action,
                                     _ => _.MarkAsSapTransfer,
                                     _ => _.LatestSapSuccessTransferDate,
                                     _ => _.LatestSapTransferUserId,
                                     _ => _.LatestSapTransferDate,
                                     _ => _.LatestSapTransferMessage,
                                     _ => _.MarkAsLegacyTransfer,
                                     _ => _.LatestLegacySuccessTransferDate,
                                     _ => _.LatestLegacyTransferUserId,
                                     _ => _.LatestLegacyTransferDate,
                                     _ => _.LatestLegacyTransferMessage,
                                     _ => _.EarmarkedFundsDocument,
                                     _ => _.EarmarkedFundsDocumentItem,
                                     _ => _.FundCommitmentHeaderNo,
                                     _ => _.BudgetCodeOld,
                                     _ => _.CostCenterOld,
                                     _ => _.GlAccountOld
                                );

                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });

        }

        public FileDto ExportValidateToFile(List<GetIF_FQF3MM07_VALIDATE> fqf3mm07)
        {
            return CreateExcelPackage(
                "IFIFFQF3MM07_VALIDATE.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("FQF3MM07_VALIDATE");
                    AddHeader(
                                sheet,
                                ("ErrorDescription"),
                                ("RecordId (M)"),
                                ("CompanyCode (M)"),
                                ("PostingKey (M)"),
                                ("DocumentNo (M)"),
                                ("DocumentType (M)"),
                                ("DocumentDate (M)"),
                                ("PostingDate (M)"),
                                ("Amount (M)"),
                                ("Currency (M)"),
                                ("NormalCancelFlag (M)"),
                                ("EndingOfRecord (M)"),
                                ("HeaderFwgId"),
                                ("HeaderId"),
                                ("TrailerId")


                               );
                    AddObjects(
                         sheet, fqf3mm07,
                                _ => _.ErrorDescription,
                                _ => _.RecordId,
                                _ => _.CompanyCode,
                                _ => _.PostingKey,
                                _ => _.DocumentNo,
                                _ => _.DocumentType,
                                _ => _.DocumentDate,
                                _ => _.PostingDate,
                                _ => _.Amount,
                                _ => _.Currency,
                                _ => _.NormalCancelFlag,
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
