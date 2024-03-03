using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.IF
{

    [Table("IF_FQF3MM03")]
    public class IF_FQF3MM03 : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxRecordIdLength = 7;

        public const int MaxCompanyCodeLength = 4;

        public const int MaxDocumentNoLength = 20;

        public const int MaxDocumentTypeLength = 2;

        public const int MaxDocumentDateLength = 8;

        public const int MaxCustomerCodeLength = 4;

        public const int MaxCustomerPlantCodeLength = 4;

        public const int MaxCustomerDockCodeLength = 4;

        public const int MaxPartCategoryLength = 1;

        public const int MaxWithholdingTaxFlagLength = 1;

        public const int MaxWithholdingTaxRateLength = 5;

        public const int MaxOrderTypeLength = 1;

        public const int MaxPdsNoLength = 25;

        public const int MaxPartReceivedDateLength = 8;

        public const int MaxSequenceDateLength = 8;

        public const int MaxSequenceNoLength = 10;

        public const int MaxPartNoLength = 40;

        public const int MaxReferenceDocumentNoLength = 20;

        public const int MaxPostingDateLength = 8;

        public const int MaxSupplierCodeLength = 10;

        public const int MaxSupplierPlantCodeLength = 1;

        public const int MaxPartQuantityLength = 14;

        public const int MaxUnitBuyingPriceLength = 11;

        public const int MaxUnitBuyingAmountLength = 13;

        public const int MaxUnitSellingPriceLength = 11;

        public const int MaxUnitSellingAmountLength = 13;

        public const int MaxPriceStatusLength = 1;

        public const int MaxTotalAmountLength = 13;

        public const int MaxVatAmountLength = 13;

        public const int MaxVatCodeLength = 5;

        public const int MaxPaymentTermLength = 4;

        public const int MaxReasonCodeLength = 1;

        public const int MaxMarkCodeLength = 1;

        public const int MaxSignCodeLength = 1;

        public const int MaxCancelFlagLength = 1;

        public const int MaxSupplierInvoiceNoLength = 16;

        public const int MaxTopasSmrNoLength = 9;

        public const int MaxTopasSmrItemNoLength = 4;

        public const int MaxCustomerBranchLength = 40;

        public const int MaxCostCenterLength = 10;

        public const int MaxWbsLength = 24;

        public const int MaxAssetLength = 17;

        public const int MaxOrderReasonCodeLength = 1;

        public const int MaxRetroFlagLength = 1;

        public const int MaxValuationTypeLength = 10;

        public const int MaxConditionTypeLength = 4;

        public const int MaxConditionTypeAmtLength = 23;

        public const int MaxPrepaidTaxAmtLength = 23;

        public const int MaxWithholdingTaxAmtLength = 23;

        public const int MaxStampFeeAmtLength = 23;

        public const int MaxGlAmountLength = 23;

        public const int MaxSptCodeLength = 5;

        public const int MaxHigherLevelItemLength = 7;

        public const int MaxWithholdingTaxBaseAmtLength = 23;

        public const int MaxTypeOfSalesLength = 3;

        public const int MaxProfitCenterLength = 10;

        public const int MaxDueDateLength = 8;

        public const int MaxItemTextLength = 50;

        public const int MaxPaymentMethodLength = 1;

        public const int MaxEndingOfRecordLength = 1;

        [StringLength(MaxRecordIdLength)]
        public virtual string RecordId { get; set; }

        [StringLength(MaxCompanyCodeLength)]
        public virtual string CompanyCode { get; set; }

        [StringLength(MaxDocumentNoLength)]
        public virtual string DocumentNo { get; set; }

        [StringLength(MaxDocumentTypeLength)]
        public virtual string DocumentType { get; set; }

        [StringLength(MaxDocumentDateLength)]
        public virtual string DocumentDate { get; set; }

        [StringLength(MaxCustomerCodeLength)]
        public virtual string CustomerCode { get; set; }

        [StringLength(MaxCustomerPlantCodeLength)]
        public virtual string CustomerPlantCode { get; set; }

        [StringLength(MaxCustomerDockCodeLength)]
        public virtual string CustomerDockCode { get; set; }

        [StringLength(MaxPartCategoryLength)]
        public virtual string PartCategory { get; set; }

        [StringLength(MaxWithholdingTaxFlagLength)]
        public virtual string WithholdingTaxFlag { get; set; }

        [StringLength(MaxWithholdingTaxRateLength)]
        public virtual string WithholdingTaxRate { get; set; }

        [StringLength(MaxOrderTypeLength)]
        public virtual string OrderType { get; set; }

        [StringLength(MaxPdsNoLength)]
        public virtual string PdsNo { get; set; }

        [StringLength(MaxPartReceivedDateLength)]
        public virtual string PartReceivedDate { get; set; }

        [StringLength(MaxSequenceDateLength)]
        public virtual string SequenceDate { get; set; }

        [StringLength(MaxSequenceNoLength)]
        public virtual string SequenceNo { get; set; }

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxReferenceDocumentNoLength)]
        public virtual string ReferenceDocumentNo { get; set; }

        [StringLength(MaxPostingDateLength)]
        public virtual string PostingDate { get; set; }

        [StringLength(MaxSupplierCodeLength)]
        public virtual string SupplierCode { get; set; }

        [StringLength(MaxSupplierPlantCodeLength)]
        public virtual string SupplierPlantCode { get; set; }

        [StringLength(MaxPartQuantityLength)]
        public virtual string PartQuantity { get; set; }

        [StringLength(MaxUnitBuyingPriceLength)]
        public virtual string UnitBuyingPrice { get; set; }

        [StringLength(MaxUnitBuyingAmountLength)]
        public virtual string UnitBuyingAmount { get; set; }

        [StringLength(MaxUnitSellingPriceLength)]
        public virtual string UnitSellingPrice { get; set; }

        [StringLength(MaxUnitSellingAmountLength)]
        public virtual string UnitSellingAmount { get; set; }

        [StringLength(MaxPriceStatusLength)]
        public virtual string PriceStatus { get; set; }

        [StringLength(MaxTotalAmountLength)]
        public virtual string TotalAmount { get; set; }

        [StringLength(MaxVatAmountLength)]
        public virtual string VatAmount { get; set; }

        [StringLength(MaxVatCodeLength)]
        public virtual string VatCode { get; set; }

        [StringLength(MaxPaymentTermLength)]
        public virtual string PaymentTerm { get; set; }

        [StringLength(MaxReasonCodeLength)]
        public virtual string ReasonCode { get; set; }

        [StringLength(MaxMarkCodeLength)]
        public virtual string MarkCode { get; set; }

        [StringLength(MaxSignCodeLength)]
        public virtual string SignCode { get; set; }

        [StringLength(MaxCancelFlagLength)]
        public virtual string CancelFlag { get; set; }

        [StringLength(MaxSupplierInvoiceNoLength)]
        public virtual string SupplierInvoiceNo { get; set; }

        [StringLength(MaxTopasSmrNoLength)]
        public virtual string TopasSmrNo { get; set; }

        [StringLength(MaxTopasSmrItemNoLength)]
        public virtual string TopasSmrItemNo { get; set; }

        [StringLength(MaxCustomerBranchLength)]
        public virtual string CustomerBranch { get; set; }

        [StringLength(MaxCostCenterLength)]
        public virtual string CostCenter { get; set; }

        [StringLength(MaxWbsLength)]
        public virtual string Wbs { get; set; }

        [StringLength(MaxAssetLength)]
        public virtual string Asset { get; set; }

        [StringLength(MaxOrderReasonCodeLength)]
        public virtual string OrderReasonCode { get; set; }

        [StringLength(MaxRetroFlagLength)]
        public virtual string RetroFlag { get; set; }

        [StringLength(MaxValuationTypeLength)]
        public virtual string ValuationType { get; set; }

        [StringLength(MaxConditionTypeLength)]
        public virtual string ConditionType { get; set; }

        [StringLength(MaxConditionTypeAmtLength)]
        public virtual string ConditionTypeAmt { get; set; }

        [StringLength(MaxPrepaidTaxAmtLength)]
        public virtual string PrepaidTaxAmt { get; set; }

        [StringLength(MaxWithholdingTaxAmtLength)]
        public virtual string WithholdingTaxAmt { get; set; }

        [StringLength(MaxStampFeeAmtLength)]
        public virtual string StampFeeAmt { get; set; }

        [StringLength(MaxGlAmountLength)]
        public virtual string GlAmount { get; set; }

        [StringLength(MaxSptCodeLength)]
        public virtual string SptCode { get; set; }

        [StringLength(MaxHigherLevelItemLength)]
        public virtual string HigherLevelItem { get; set; }

        [StringLength(MaxWithholdingTaxBaseAmtLength)]
        public virtual string WithholdingTaxBaseAmt { get; set; }

        [StringLength(MaxTypeOfSalesLength)]
        public virtual string TypeOfSales { get; set; }

        [StringLength(MaxProfitCenterLength)]
        public virtual string ProfitCenter { get; set; }

        [StringLength(MaxDueDateLength)]
        public virtual string DueDate { get; set; }

        [StringLength(MaxItemTextLength)]
        public virtual string ItemText { get; set; }

        [StringLength(MaxPaymentMethodLength)]
        public virtual string PaymentMethod { get; set; }

        [StringLength(MaxEndingOfRecordLength)]
        public virtual string EndingOfRecord { get; set; }
    }

}

