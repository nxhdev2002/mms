using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.IF.FQF3MM07.Dto
{

    public class IF_FQF3MM07Dto : EntityDto<long?>
    {

        public virtual string RecordId { get; set; }

        public virtual string CountryCode { get; set; }

        public virtual string CompanyCode { get; set; }

        public virtual string CompanyBranch { get; set; }

        public virtual string PostingKey { get; set; }

        public virtual string CostCenter { get; set; }

        public virtual string DocumentNo { get; set; }

        public virtual string DocumentType { get; set; }

        public virtual string DocumentDate { get; set; }

        public virtual string PostingDate { get; set; }

        public virtual string Plant { get; set; }

        public virtual string ReferenceDocumentNo { get; set; }

        public virtual long? Amount { get; set; }

        public virtual string Currency { get; set; }

        public virtual string Order { get; set; }

        public virtual string GlAccount { get; set; }

        public virtual string NormalCancelFlag { get; set; }

        public virtual string Text { get; set; }

        public virtual string ProfitCenter { get; set; }

        public virtual string Wbs { get; set; }

        public virtual int? Quantity { get; set; }

        public virtual string BaseUnitOfMeasure { get; set; }

        public virtual string AmountInLocalCurrency { get; set; }

        public virtual string ExchangeRate { get; set; }

        public virtual string RefKey1 { get; set; }

        public virtual string RefKey2 { get; set; }

        public virtual string RefKey3 { get; set; }

        public virtual string EarmarkFund { get; set; }

        public virtual string EarmarkFundItem { get; set; }

        public virtual string MaterialNo { get; set; }

        public virtual string MainAssetNumber { get; set; }

        public virtual string AssetSubNumber { get; set; }

        public virtual string TransType { get; set; }

        public virtual string EndingOfRecord { get; set; }

    }


    public class GetIF_FQF3MM07Input : PagedAndSortedResultRequestDto
    {

        public virtual string DocumentNo { get; set; }

        public virtual DateTime? DocumentDateFrom { get; set; }

        public virtual DateTime? DocumentDateTo { get; set; }

        public virtual DateTime? PostingDateFrom { get; set; }

        public virtual DateTime? PostingDateTo { get; set; }

    }
    public class GetIF_FQF3MM07ExportInput 
    {

        public virtual string DocumentNo { get; set; }

        public virtual DateTime? DocumentDateFrom { get; set; }

        public virtual DateTime? DocumentDateTo { get; set; }

        public virtual DateTime? PostingDateFrom { get; set; }

        public virtual DateTime? PostingDateTo { get; set; }

    }

    public class GetIF_FundCommitmentItemDMExportDto  : EntityDto<long?>
    {
        public virtual string FundCommitmentHeaderType { get; set; }
        public virtual long? FundCommitmentHeaderId { get; set; }
        public virtual long? FundCommitmentItemId { get; set; }
        public virtual string DocumentNo { get; set; }
        public virtual string LineNo { get; set; }
        public virtual string Closed { get; set; }
        public virtual string ReferenceDocumentNo { get; set; }
        public virtual string ReferenceDocumentLineItemNo { get; set; }
        public virtual string ItemCode { get; set; }
        public virtual string ItemDescription { get; set; }
        public virtual string PartCategory { get; set; }
        public virtual string InventoryType { get; set; }
        public virtual string MaterialType { get; set; }
        public virtual string SupplierCode { get; set; }
        public virtual string Asset { get; set; }
        public virtual string WbsElement { get; set; }
        public virtual string CostCenterCharger { get; set; }
        public virtual decimal TotalAmount { get; set; }
        public virtual decimal Quantity { get; set; }
        public virtual string Uom { get; set; }
        public virtual string JournalSource { get; set; }
        public virtual string GlAccount { get; set; }
        public virtual DateTime? SubmitDate { get; set; }
        public virtual string Action { get; set; }
        public virtual bool MarkAsSapTransfer { get; set; }
        public virtual DateTime? LatestSapSuccessTransferDate { get; set; }
        public virtual long? LatestSapTransferUserId { get; set; }
        public virtual DateTime? LatestSapTransferDate { get; set; }
        public virtual string LatestSapTransferMessage { get; set; }
        public virtual bool MarkAsLegacyTransfer { get; set; }
        public virtual DateTime? LatestLegacySuccessTransferDate { get; set; }
        public virtual long? LatestLegacyTransferUserId { get; set; }
        public virtual DateTime? LatestLegacyTransferDate { get; set; }
        public virtual string LatestLegacyTransferMessage { get; set; }
        public virtual string EarmarkedFundsDocument { get; set; }
        public virtual string EarmarkedFundsDocumentItem { get; set; }
        public virtual string FundCommitmentHeaderNo { get; set; }
        public virtual string BudgetCodeOld { get; set; }
        public virtual string CostCenterOld { get; set; }
        public virtual string GlAccountOld { get; set; }
        public virtual string MessageType { get; set; }
        public virtual string MessageID { get; set; }
        public virtual string MessageNo { get; set; }
        public virtual string Message { get; set; }
        public virtual long? LoggingId { get; set; }


    }

    public class GetIF_FQF3MM07_OnlBudgetCheck_Input : PagedAndSortedResultRequestDto
    {
        public virtual string p_DocumentNo { get; set; }
    }
    public class GetIF_FQF3MM07_Fundcmm_Input : PagedAndSortedResultRequestDto
    {
        public virtual long? IdFund { get; set; }
    }

    public class LoggingResponseDetailsOnlBudgetCheckDto : EntityDto<long?>
    {
        public virtual long? LoggingId { get; set; }
        public virtual string AvailableBudgetWBSMasterData { get; set; }
        public virtual string AvailableBudgetFiscalYear { get; set; }
        public virtual decimal? AvailableBudgetAvailableAmount { get; set; }
        public virtual string AvailableBudgetMessageType { get; set; }
        public virtual string AvailableBudgetMessageID { get; set; }
        public virtual string AvailableBudgetMessageNo { get; set; }
        public virtual string AvailableBudgetMessage { get; set; }
        public virtual string DataValidationWBSMasterData { get; set; }
        public virtual string DataValidationCostCenter { get; set; }
        public virtual string DataValidationFixedAssetNo { get; set; }
        public virtual string DataValidationResult { get; set; }
        public virtual string DataValidationMessageType { get; set; }
        public virtual string DataValidationMessageID { get; set; }
        public virtual string DataValidationMessageNo { get; set; }
        public virtual string DocumentNo { get; set; }
    }

    public class GetRequestBudgetCheck
    {
        public virtual DateTime CreationTime { get; set; }

        public virtual string CreationTime_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy HH:mm:ss}", CreationTime);
                }
                catch
                {
                    return "";
                }
            }
            set { }
        }

        public virtual string Request { get; set; }
        public virtual string Response { get; set; }
    }



    public class GetIF_FQF3MM07_VALIDATE : EntityDto<long?>
    {
        public virtual string RecordId { get; set; }

        public virtual string CountryCode { get; set; }

        public virtual string CompanyCode { get; set; }

        public virtual string CompanyBranch { get; set; }

        public virtual string PostingKey { get; set; }

        public virtual string CostCenter { get; set; }

        public virtual string DocumentNo { get; set; }

        public virtual string DocumentType { get; set; }

        public virtual string DocumentDate { get; set; }

        public virtual string PostingDate { get; set; }

        public virtual string Plant { get; set; }

        public virtual string ReferenceDocumentNo { get; set; }

        public virtual long? Amount { get; set; }

        public virtual string Currency { get; set; }

        public virtual string Order { get; set; }

        public virtual string GlAccount { get; set; }

        public virtual string NormalCancelFlag { get; set; }

        public virtual string Text { get; set; }

        public virtual string ProfitCenter { get; set; }

        public virtual string Wbs { get; set; }

        public virtual int? Quantity { get; set; }

        public virtual string BaseUnitOfMeasure { get; set; }

        public virtual string AmountInLocalCurrency { get; set; }

        public virtual string ExchangeRate { get; set; }

        public virtual string RefKey1 { get; set; }

        public virtual string RefKey2 { get; set; }

        public virtual string RefKey3 { get; set; }

        public virtual string EarmarkFund { get; set; }

        public virtual string EarmarkFundItem { get; set; }

        public virtual string MaterialNo { get; set; }

        public virtual string MainAssetNumber { get; set; }

        public virtual string AssetSubNumber { get; set; }

        public virtual string TransType { get; set; }

        public virtual string EndingOfRecord { get; set; }
        public virtual long? HeaderFwgId { get; set; }
        public virtual long? HeaderId { get; set; }
        public virtual string TrailerId { get; set; }
        public virtual string ErrorDescription { get; set; }

    }


    public class GetIF_FQF3MM07_VALIDATEInput : PagedAndSortedResultRequestDto
    {
        public DateTime? PostingDateFrom { get; set; }
        public DateTime? PostingDateTo { get; set; }
    }

    public class GetIF_FQF3MM07_VALIDATEExcelInput
    {
        public virtual DateTime? PostingDateFrom { get; set; }
        public virtual DateTime? PostingDateTo { get; set; }
    }


    public class GetRequestBudgetCheckFQF3MM07
    {
        public virtual DateTime CreationTime { get; set; }

        public virtual string CreationTime_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy HH:mm:ss}", CreationTime);
                }
                catch
                {
                    return "";
                }
            }
            set { }
        }

        public virtual string Request { get; set; }
        public virtual string Response { get; set; }
    }
}


