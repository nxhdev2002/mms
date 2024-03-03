using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.IF
{

    [Table("IF_FQF3MM07")]
    public class IF_FQF3MM07 : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxRecordIdLength = 7;

        public const int MaxCountryCodeLength = 3;

        public const int MaxCompanyCodeLength = 4;

        public const int MaxCompanyBranchLength = 5;

        public const int MaxPostingKeyLength = 2;

        public const int MaxCostCenterLength = 10;

        public const int MaxDocumentNoLength = 25;

        public const int MaxDocumentTypeLength = 2;

        public const int MaxDocumentDateLength = 8;

        public const int MaxPostingDateLength = 8;

        public const int MaxPlantLength = 4;

        public const int MaxReferenceDocumentNoLength = 16;

        public const int MaxCurrencyLength = 3;

        public const int MaxOrderLength = 12;

        public const int MaxGlAccountLength = 10;

        public const int MaxNormalCancelFlagLength = 1;

        public const int MaxTextLength = 50;

        public const int MaxProfitCenterLength = 10;

        public const int MaxWbsLength = 24;

        public const int MaxBaseUnitOfMeasureLength = 3;

        public const int MaxAmountInLocalCurrencyLength = 23;

        public const int MaxExchangeRateLength = 10;

        public const int MaxRefKey1Length = 12;

        public const int MaxRefKey2Length = 12;

        public const int MaxRefKey3Length = 20;

        public const int MaxEarmarkFundLength = 10;

        public const int MaxEarmarkFundItemLength = 3;

        public const int MaxMaterialNoLength = 40;

        public const int MaxMainAssetNumberLength = 12;

        public const int MaxAssetSubNumberLength = 4;

        public const int MaxTransTypeLength = 3;

        public const int MaxEndingOfRecordLength = 24;

        [StringLength(MaxRecordIdLength)]
        public virtual string RecordId { get; set; }

        [StringLength(MaxCountryCodeLength)]
        public virtual string CountryCode { get; set; }

        [StringLength(MaxCompanyCodeLength)]
        public virtual string CompanyCode { get; set; }

        [StringLength(MaxCompanyBranchLength)]
        public virtual string CompanyBranch { get; set; }

        [StringLength(MaxPostingKeyLength)]
        public virtual string PostingKey { get; set; }

        [StringLength(MaxCostCenterLength)]
        public virtual string CostCenter { get; set; }

        [StringLength(MaxDocumentNoLength)]
        public virtual string DocumentNo { get; set; }

        [StringLength(MaxDocumentTypeLength)]
        public virtual string DocumentType { get; set; }

        [StringLength(MaxDocumentDateLength)]
        public virtual string DocumentDate { get; set; }

        [StringLength(MaxPostingDateLength)]
        public virtual string PostingDate { get; set; }

        [StringLength(MaxPlantLength)]
        public virtual string Plant { get; set; }

        [StringLength(MaxReferenceDocumentNoLength)]
        public virtual string ReferenceDocumentNo { get; set; }

        public virtual int? Amount { get; set; }

        [StringLength(MaxCurrencyLength)]
        public virtual string Currency { get; set; }

        [StringLength(MaxOrderLength)]
        public virtual string Order { get; set; }

        [StringLength(MaxGlAccountLength)]
        public virtual string GlAccount { get; set; }

        [StringLength(MaxNormalCancelFlagLength)]
        public virtual string NormalCancelFlag { get; set; }

        [StringLength(MaxTextLength)]
        public virtual string Text { get; set; }

        [StringLength(MaxProfitCenterLength)]
        public virtual string ProfitCenter { get; set; }

        [StringLength(MaxWbsLength)]
        public virtual string Wbs { get; set; }

        public virtual int? Quantity { get; set; }

        [StringLength(MaxBaseUnitOfMeasureLength)]
        public virtual string BaseUnitOfMeasure { get; set; }

        [StringLength(MaxAmountInLocalCurrencyLength)]
        public virtual string AmountInLocalCurrency { get; set; }

        [StringLength(MaxExchangeRateLength)]
        public virtual string ExchangeRate { get; set; }

        [StringLength(MaxRefKey1Length)]
        public virtual string RefKey1 { get; set; }

        [StringLength(MaxRefKey2Length)]
        public virtual string RefKey2 { get; set; }

        [StringLength(MaxRefKey3Length)]
        public virtual string RefKey3 { get; set; }

        [StringLength(MaxEarmarkFundLength)]
        public virtual string EarmarkFund { get; set; }

        [StringLength(MaxEarmarkFundItemLength)]
        public virtual string EarmarkFundItem { get; set; }

        [StringLength(MaxMaterialNoLength)]
        public virtual string MaterialNo { get; set; }

        [StringLength(MaxMainAssetNumberLength)]
        public virtual string MainAssetNumber { get; set; }

        [StringLength(MaxAssetSubNumberLength)]
        public virtual string AssetSubNumber { get; set; }

        [StringLength(MaxTransTypeLength)]
        public virtual string TransType { get; set; }

        [StringLength(MaxEndingOfRecordLength)]
        public virtual string EndingOfRecord { get; set; }
    }

}

