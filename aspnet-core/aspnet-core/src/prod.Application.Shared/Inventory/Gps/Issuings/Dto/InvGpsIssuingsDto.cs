using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Inventory.Gps.Issuings.Dto
{
    public class InvGpsIssuingsHeaderDto : EntityDto<long?>
    {
        public string DocumentNo { get; set; }
        public string DocumentDate { get; set; }
        public string Shop { get; set; }
        public string UserId { get; set; }
        public string UserRequest { get; set; }
        public DateTime RequestDate { get; set; }
        public string Team { get; set; }
        public string CostCenter { get; set; }
        public string Status { get; set; }
        public string EmployeeCode { get; set; }
        
    }

    public class InvGpsIssuingsHeaderInput : PagedAndSortedResultRequestDto
    {
        public string DocumentNo { get; set; }
        public string DocumentDate { get; set; }
        public string Shop { get; set; }
        public string Team { get; set; }
        public string CostCenter { get; set; }
        public string PartNo { get; set; }
        public string LotNo { get; set; }
        public DateTime? IssueFromDate { get; set; }
        public DateTime? IssueToDate { get; set; }
        public DateTime? RequestFromDate { get; set; }
        public DateTime? RequestToDate { get; set; }
        public string Status { get; set; }
        public string Today { get; set; }
        public string PerMember { get; set; }
        public string PerShop { get; set; }
        public string PerGps { get; set; }

    }


    public class InvGpsIssuingsDetails : EntityDto<long?>
    {
        public long? DocumentHeaderId { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentDate { get; set; }
        public string ItemNo { get; set; }
        public string MaterialCode { get; set; }
        public string PartNo { get; set; }
        public string PartName { get; set; }
        public string Uom { get; set; }
        public int? BoxQty { get; set; }
        public int? Box { get; set; }
        public string LotNo { get; set; }
        public DateTime? ProdDate { get; set; }
        public DateTime? ExpDate { get; set; }
        public string CostCenter { get; set; }
        public int? QtyRequest { get; set; }
        public int? QtyIssue { get; set; }
        public string IsIssue { get; set; }
        public decimal? Price { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? IssueDate { get; set; }
        public string IsGentani { get; set; }
        public DateTime? RequestDate { get; set; }
        public string Supplier { get; set; }
        public string Status { get; set; }
        public string EarmarkFund { get; set; }
        public string EarmarkFundItem { get; set; }
        public string Wbs { get; set; }
        public string GlAccount { get; set; }
        public string AssetNo { get; set; }
        public string AssetItem { get; set; }
        public string TransType { get; set; }
        public int? QtyReject { get; set; }
        public string BudgetCheckMessage { get; set; }
        public string FundCommitmentMessage { get; set; }
        public int? QtyRemain { get; set; }
        public string HeaderStatus { get; set; }
        public string Category { get; set; }
        public string PartType { get; set; }
        public string Location { get; set; }
        public string FinStatus { get; set; }

        public string WbsMapping { get; set; }
        public string CostCenterMapping { get; set; }
        public string IsBudgetCheck { get; set; }
    }
    public class InvGpsIssuingsDetailsInput :  PagedAndSortedResultRequestDto
    {
      
        public string IssuingHeaderId { get; set; }
        public string PartNo { get; set; }
        public string LotNo { get; set; }
        public DateTime? IssueFromDate { get; set; }
        public DateTime? IssueToDate { get; set; }
        public DateTime? RequestFromDate { get; set; }
        public DateTime? RequestToDate { get; set; }
        public string PerMember { get; set; }
        public string PerShop { get; set; }
        public string PerGps { get; set; }
    }

    public class InvGpsIssuingItemInsert
    {
        public string PartNo { get; set; }
        public int? QtyRequest { get; set; }
        public string IssuingHeaderId { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentDate { get; set; }
        public string Shop { get; set; }
        public string Wbs { get; set; }
        public string WbsMapping { get; set; }
        public string CostCenter { get; set; }
        public string CostCenterMapping { get; set; }
        public string GlAccount { get; set; }
        public string IsBudgetCheck { get; set; }
    }


    public class GetInvGgsIssuingsImport
    {
        public virtual long? ROW_NO { get; set; }
        public virtual string Guid { get; set; }
        public string PartNo { get; set; }
        public string PartName { get; set; }
        public string Uom { get; set; }
        public int? QtyRequest { get; set; }
        public virtual string ErrorDescription { get; set; }
        public virtual long? CreatorUserId { get; set; }


    }


    public class LoggingResponseDetailsOnlBudgetCheckIssuingDto : EntityDto<long?>
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


    public class GetIF_FundCommitmentItemDMIssuingExportDto : EntityDto<long?>
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

    public class GetIF_OnlBudgetCheck_Input : PagedAndSortedResultRequestDto
    {
        public virtual string p_DocumentNo { get; set; }
    }
    public class GetInv_Fundcmm_Item_Issuing_Input : PagedAndSortedResultRequestDto
    {
        public virtual long? IdFund { get; set; }
    }

    public class GetRequestBudgetCheckIssuing
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

    public class GetInvCkdIssuingsHistoryInput : PagedAndSortedResultRequestDto
    {
        public virtual long Id { get; set; }

        public virtual string TableName { get; set; }
    }

    public class GetInvCkdIssuingsHistoryExcelInput
    {
        public virtual long Id { get; set; }

        public virtual string TableName { get; set; }
    }
    public class ChangedRecordIssuingsIdsDto
    {
        public virtual List<long?> IssuingHeader { get; set; }
        public virtual List<long?> IssuingDetail { get; set; }

    }
}
