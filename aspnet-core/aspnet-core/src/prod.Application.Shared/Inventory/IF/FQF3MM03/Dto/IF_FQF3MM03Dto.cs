using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Inventory.IF.FQF3MM03.Dto
{
    public class IF_FQF3MM03Dto : EntityDto<long?>
    {
        public virtual string RecordId { get; set; }

        public virtual string CompanyCode { get; set; }

        public virtual string DocumentNo { get; set; }

        public virtual string DocumentType { get; set; }

        public virtual string DocumentDate { get; set; }

        public virtual string CustomerCode { get; set; }

        public virtual string CustomerPlantCode { get; set; }

        public virtual string CustomerDockCode { get; set; }

        public virtual string PartCategory { get; set; }

        public virtual string WithholdingTaxFlag { get; set; }

        public virtual string WithholdingTaxRate { get; set; }

        public virtual string OrderType { get; set; }

        public virtual string PdsNo { get; set; }

        public virtual string PartReceivedDate { get; set; }

        public virtual string SequenceDate { get; set; }

        public virtual string SequenceNo { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string ReferenceDocumentNo { get; set; }

        public virtual string PostingDate { get; set; }

        public virtual string SupplierCode { get; set; }

        public virtual string SupplierPlantCode { get; set; }

        public virtual string PartQuantity { get; set; }

        public virtual string UnitBuyingPrice { get; set; }

        public virtual string UnitBuyingAmount { get; set; }

        public virtual string UnitSellingPrice { get; set; }

        public virtual string UnitSellingAmount { get; set; }

        public virtual string PriceStatus { get; set; }

        public virtual string TotalAmount { get; set; }

        public virtual string VatAmount { get; set; }

        public virtual string VatCode { get; set; }

        public virtual string PaymentTerm { get; set; }

        public virtual string ReasonCode { get; set; }

        public virtual string MarkCode { get; set; }

        public virtual string SignCode { get; set; }

        public virtual string CancelFlag { get; set; }

        public virtual string SupplierInvoiceNo { get; set; }

        public virtual string TopasSmrNo { get; set; }

        public virtual string TopasSmrItemNo { get; set; }

        public virtual string CustomerBranch { get; set; }

        public virtual string CostCenter { get; set; }

        public virtual string Wbs { get; set; }

        public virtual string Asset { get; set; }

        public virtual string OrderReasonCode { get; set; }

        public virtual string RetroFlag { get; set; }

        public virtual string ValuationType { get; set; }

        public virtual string ConditionType { get; set; }

        public virtual string ConditionTypeAmt { get; set; }

        public virtual string PrepaidTaxAmt { get; set; }

        public virtual string WithholdingTaxAmt { get; set; }

        public virtual string StampFeeAmt { get; set; }

        public virtual string GlAmount { get; set; }

        public virtual string SptCode { get; set; }

        public virtual string HigherLevelItem { get; set; }

        public virtual string WithholdingTaxBaseAmt { get; set; }

        public virtual string TypeOfSales { get; set; }

        public virtual string ProfitCenter { get; set; }

        public virtual string DueDate { get; set; }

        public virtual string ItemText { get; set; }

        public virtual string PaymentMethod { get; set; }

        public virtual string EndingOfRecord { get; set; }

    }

    public class GetIF_FQF3MM03Input : PagedAndSortedResultRequestDto
    {

        public virtual string PartNo { get; set; }

        public virtual string SupplierCode { get; set; }

        public virtual DateTime? PostingDateFrom { get; set; }

        public virtual DateTime? PostingDateTo { get; set; }
        public virtual string PdsNo { get; set; }

        public virtual DateTime? SequenceDateFrom { get; set; }

        public virtual DateTime? SequenceDateTo { get; set; }

    }

    public class BusinessDataInput : PagedAndSortedResultRequestDto
    {
        public virtual string PartNo { get; set; }
        public virtual string SupplierCode { get; set; }
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }

    }

        public class BusinessDataDto : EntityDto<long?>
    {
        public virtual DateTime? OrderMonth { get; set; }
        public virtual DateTime? OrderWkDate { get; set; }
        public virtual DateTime? ActArrivalWkDate { get; set; }
        public virtual DateTime? ActArrivalDateTime { get; set; }
        public virtual string SupplierCode { get; set; }
        public virtual string SptCode { get; set; }
        public virtual string SupplierAbbr { get; set; }
        public virtual string OrderNo { get; set; }
        public virtual string ContentNo { get; set; }
        public virtual string PartNo { get; set; }
        public virtual string PartName { get; set; }
        public virtual string Unit { get; set; }
        public virtual int? UsageOrderQty { get; set; }
        public virtual int? UsageActualQty { get; set; }
        public virtual int? ActualQty { get; set; }
        public virtual int? BoxSize { get; set; }
        public virtual string PackagingType { get; set; }
        public virtual string Shift { get; set; }
        public virtual long? UlScanUserId { get; set; }
        public virtual string IsActive { get; set; }
        public virtual long? RowNo { get; set; }
        public virtual long? ItemNo { get; set; }
    }

    public class GetIF_FQF3MM03_VALIDATE : EntityDto<long?>
    {
        public virtual string RecordId { get; set; }

        public virtual string CompanyCode { get; set; }

        public virtual string DocumentNo { get; set; }

        public virtual string DocumentType { get; set; }

        public virtual string DocumentDate { get; set; }

        public virtual string CustomerCode { get; set; }

        public virtual string CustomerPlantCode { get; set; }

        public virtual string CustomerDockCode { get; set; }

        public virtual string PartCategory { get; set; }

        public virtual string WithholdingTaxFlag { get; set; }

        public virtual string WithholdingTaxRate { get; set; }

        public virtual string OrderType { get; set; }

        public virtual string PdsNo { get; set; }

        public virtual string PartReceivedDate { get; set; }

        public virtual string SequenceDate { get; set; }

        public virtual string SequenceNo { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string ReferenceDocumentNo { get; set; }

        public virtual string PostingDate { get; set; }

        public virtual string SupplierCode { get; set; }

        public virtual string SupplierPlantCode { get; set; }

        public virtual string PartQuantity { get; set; }

        public virtual string UnitBuyingPrice { get; set; }

        public virtual string UnitBuyingAmount { get; set; }

        public virtual string UnitSellingPrice { get; set; }

        public virtual string UnitSellingAmount { get; set; }

        public virtual string PriceStatus { get; set; }

        public virtual string TotalAmount { get; set; }

        public virtual string VatAmount { get; set; }

        public virtual string VatCode { get; set; }

        public virtual string PaymentTerm { get; set; }

        public virtual string ReasonCode { get; set; }

        public virtual string MarkCode { get; set; }

        public virtual string SignCode { get; set; }

        public virtual string CancelFlag { get; set; }

        public virtual string SupplierInvoiceNo { get; set; }

        public virtual string TopasSmrNo { get; set; }

        public virtual string TopasSmrItemNo { get; set; }

        public virtual string CustomerBranch { get; set; }

        public virtual string CostCenter { get; set; }

        public virtual string Wbs { get; set; }

        public virtual string Asset { get; set; }

        public virtual string OrderReasonCode { get; set; }

        public virtual string RetroFlag { get; set; }

        public virtual string ValuationType { get; set; }

        public virtual string ConditionType { get; set; }

        public virtual string ConditionTypeAmt { get; set; }

        public virtual string PrepaidTaxAmt { get; set; }

        public virtual string WithholdingTaxAmt { get; set; }

        public virtual string StampFeeAmt { get; set; }

        public virtual string GlAmount { get; set; }

        public virtual string SptCode { get; set; }

        public virtual string HigherLevelItem { get; set; }

        public virtual string WithholdingTaxBaseAmt { get; set; }

        public virtual string TypeOfSales { get; set; }

        public virtual string ProfitCenter { get; set; }

        public virtual string DueDate { get; set; }

        public virtual string ItemText { get; set; }

        public virtual string PaymentMethod { get; set; }

        public virtual string EndingOfRecord { get; set; }
        public virtual long? HeaderFwgId { get; set; }
        public virtual long? HeaderId { get; set; }
        public virtual string TrailerId { get; set; }
        public virtual string ErrorDescription { get; set; }

    }


    public class GetIF_FQF3MM03_VALIDATEInput : PagedAndSortedResultRequestDto
    {
        public DateTime? PostingDateFrom { get; set; }
        public DateTime? PostingDateTo { get; set; }
    }


    public class GetIF_FQF3MM03_VALIDATE_Input
    { 
        public DateTime? PostingDateFrom { get; set; }
        public DateTime? PostingDateTo { get; set; }
    }

}
