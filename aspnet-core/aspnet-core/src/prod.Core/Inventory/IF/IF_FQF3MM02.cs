using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.IF
{

    [Table("IF_FQF3MM02")]
    public class IF_FQF3MM02 : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxRecordIdLength = 7;

        public const int MaxCompanyCodeLength = 8;

        public const int MaxPlantCodeLength = 4;

        public const int MaxMaruCodeLength = 1;

        public const int MaxReceivingStockLineLength = 3;

        public const int MaxProductionDateLength = 8;

        public const int MaxPostingDateLength = 8;

        public const int MaxPartCodeLength = 14;

        public const int MaxMaterialCodeLength = 40;

        public const int MaxCostCenterLength = 10;

        public const int MaxNormalCancelFlagLength = 1;

        public const int MaxGrgiNoLength = 10;

        public const int MaxGrgiTypeLength = 2;

        public const int MaxMaterialDocTypeLength = 2;

        public const int MaxRelatedPartReceiveNoLength = 10;

        public const int MaxRelatedGrTypeLength = 2;

        public const int MaxRelatedGrTransactionTypeLength = 2;

        public const int MaxRelatedPartIssueNoLength = 10;

        public const int MaxRelatedGiTypeLength = 2;

        public const int MaxRelatedGiTransactionTypeLength = 2;

        public const int MaxProductionIdLength = 38;

        public const int MaxWbsLength = 24;

        public const int MaxEarmarkedFundLength = 10;

        public const int MaxEarmarkedFundItemLength = 3;

        public const int MaxPsmsCodeLength = 40;

        public const int MaxGiUomLength = 3;

        public const int MaxEndingOfRecordLength = 1;


        [StringLength(MaxRecordIdLength)]
        public virtual string RecordId { get; set; }

        [StringLength(MaxCompanyCodeLength)]
        public virtual string CompanyCode { get; set; }

        [StringLength(MaxPlantCodeLength)]
        public virtual string PlantCode { get; set; }

        [StringLength(MaxMaruCodeLength)]
        public virtual string MaruCode { get; set; }

        [StringLength(MaxReceivingStockLineLength)]
        public virtual string ReceivingStockLine { get; set; }

        [StringLength(MaxProductionDateLength)]
        public virtual string ProductionDate { get; set; }

        [StringLength(MaxPostingDateLength)]
        public virtual string PostingDate { get; set; }

        [StringLength(MaxPartCodeLength)]
        public virtual string PartCode { get; set; }

        public virtual int? Quantity { get; set; }

        public virtual int? SpoiledPartsQuantity { get; set; }

        public virtual int? SpoiledMaterialQuantity1 { get; set; }

        [StringLength(MaxMaterialCodeLength)]
        public virtual string MaterialCode { get; set; }

        public virtual int? FreeShotQuantity { get; set; }

        public virtual int? RecycledQuantity { get; set; }

        [StringLength(MaxCostCenterLength)]
        public virtual string CostCenter { get; set; }

        [StringLength(MaxNormalCancelFlagLength)]
        public virtual string NormalCancelFlag { get; set; }

        [StringLength(MaxGrgiNoLength)]
        public virtual string GrgiNo { get; set; }

        [StringLength(MaxGrgiTypeLength)]
        public virtual string GrgiType { get; set; }

        [StringLength(MaxMaterialDocTypeLength)]
        public virtual string MaterialDocType { get; set; }

        public virtual int? MaterialQuantity { get; set; }

        public virtual int? SpoiledMaterialQuantity2 { get; set; }

        [StringLength(MaxRelatedPartReceiveNoLength)]
        public virtual string RelatedPartReceiveNo { get; set; }

        [StringLength(MaxRelatedGrTypeLength)]
        public virtual string RelatedGrType { get; set; }

        [StringLength(MaxRelatedGrTransactionTypeLength)]
        public virtual string RelatedGrTransactionType { get; set; }

        public virtual int? InHousePartQuantityReceive { get; set; }

        [StringLength(MaxRelatedPartIssueNoLength)]
        public virtual string RelatedPartIssueNo { get; set; }

        [StringLength(MaxRelatedGiTypeLength)]
        public virtual string RelatedGiType { get; set; }

        [StringLength(MaxRelatedGiTransactionTypeLength)]
        public virtual string RelatedGiTransactionType { get; set; }

        public virtual int? RelatedInHousePartQuantityIssued { get; set; }

        public virtual int? RelatedSpoiledPartQuantityIssued { get; set; }

        public virtual int? Wip { get; set; }

        [StringLength(MaxProductionIdLength)]
        public virtual string ProductionId { get; set; }

        public virtual int? FinalPrice { get; set; }

        [StringLength(MaxWbsLength)]
        public virtual string Wbs { get; set; }

        [StringLength(MaxEarmarkedFundLength)]
        public virtual string EarmarkedFund { get; set; }

        [StringLength(MaxEarmarkedFundItemLength)]
        public virtual string EarmarkedFundItem { get; set; }

        [StringLength(MaxPsmsCodeLength)]
        public virtual string PsmsCode { get; set; }

        [StringLength(MaxGiUomLength)]
        public virtual string GiUom { get; set; }

        [StringLength(MaxEndingOfRecordLength)]
        public virtual string EndingOfRecord { get; set; }
    }

}


