using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;

namespace prod.SapIF
{
    public class SapIFFundCommitmentItem : FullAuditedEntity<long>, IEntity<long>
    {
        public string FundCommitmentHeaderType { get; set; }
        public long FundCommitmentHeaderId { get; set; }
        public long FundCommitmentItemId { get; set; }
        public string DocumentNo { get; set; }
        public string LineNo { get; set; }
        public string Closed { get; set; }
        public string ReferenceDocumentNo { get; set; }
        public string ReferenceDocumentLineItemNo { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string PartCategory { get; set; }
        public string InventoryType { get; set; }
        public string MaterialType { get; set; }
        public string SupplierCode { get; set; }
        public string Asset { get; set; }
        public string WbsElement { get; set; }
        public string CostCenterCharger { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal? Quantity { get; set; }
        public string Uom { get; set; }
        public string JournalSource { get; set; }
        public string GlAccount { get; set; }
        public string Action { get; set; }
        public DateTime? SubmitDate { get; set; }
        //
        public bool? MarkAsSapTransfer { get; set; }
        public DateTime? LatestSapSuccessTransferDate { get; set; }
        public long? LatestSapTransferUserId { get; set; }
        public DateTime? LatestSapTransferDate { get; set; }        
        public string LatestSapTransferMessage { get; set; }
        //
        public bool? MarkAsLegacyTransfer { get; set; }
        public DateTime? LatestLegacySuccessTransferDate { get; set; }
        public long? LatestLegacyTransferUserId { get; set; }
        public DateTime? LatestLegacyTransferDate { get; set; }        
        public string LatestLegacyTransferMessage { get; set; }
        //
        public string EarmarkedFundsDocument { get; set; }
        public string EarmarkedFundsDocumentItem { get; set; }
    }
}
