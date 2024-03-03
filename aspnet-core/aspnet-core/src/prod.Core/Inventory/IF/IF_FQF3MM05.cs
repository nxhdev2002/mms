using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.IF
{

    [Table("IF_FQF3MM05")]
    public class IF_FQF3MM05 : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxDocumentDateLength = 10;

        public const int MaxPostingDateLength = 10;

        public const int MaxDocumentHeaderTextLength = 25;

        public const int MaxMovementTypeLength = 3;

        public const int MaxMaterialCodeFromLength = 40;

        public const int MaxPlantFromLength = 4;

        public const int MaxValuationTypeFromLength = 10;

        public const int MaxStorageLocationFromLength = 4;

        public const int MaxProductionVersionLength = 4;

        public const int MaxUnitOfEntryLength = 3;

        public const int MaxItemTextLength = 50;

        public const int MaxGlAccountLength = 10;

        public const int MaxCostCenterLength = 10;

        public const int MaxWbsLength = 24;

        public const int MaxMaterialCodeToLength = 40;

        public const int MaxPlantToLength = 4;

        public const int MaxValuationTypeToLength = 10;

        public const int MaxStorageLocationToLength = 4;

        public const int MaxBfPcLength = 1;

        public const int MaxCancelFlagLength = 1;

        public const int MaxReffMatDocNoLength = 25;

        public const int MaxVendorNoLength = 10;

        public const int MaxProfitCenterLength = 10;

        public const int MaxShipemntCatLength = 1;

        public const int MaxReferenceLength = 16;

        public const int MaxAssetNoLength = 12;

        public const int MaxSubAssetNoLength = 4;

        public const int MaxEndOfRecordLength = 1;

        public virtual int? RunningNo { get; set; }

        [StringLength(MaxDocumentDateLength)]
        public virtual string DocumentDate { get; set; }

        [StringLength(MaxPostingDateLength)]
        public virtual string PostingDate { get; set; }

        [StringLength(MaxDocumentHeaderTextLength)]
        public virtual string DocumentHeaderText { get; set; }

        [StringLength(MaxMovementTypeLength)]
        public virtual string MovementType { get; set; }

        [StringLength(MaxMaterialCodeFromLength)]
        public virtual string MaterialCodeFrom { get; set; }

        [StringLength(MaxPlantFromLength)]
        public virtual string PlantFrom { get; set; }

        [StringLength(MaxValuationTypeFromLength)]
        public virtual string ValuationTypeFrom { get; set; }

        [StringLength(MaxStorageLocationFromLength)]
        public virtual string StorageLocationFrom { get; set; }

        [StringLength(MaxProductionVersionLength)]
        public virtual string ProductionVersion { get; set; }

        public virtual int? Quantity { get; set; }

        [StringLength(MaxUnitOfEntryLength)]
        public virtual string UnitOfEntry { get; set; }

        [StringLength(MaxItemTextLength)]
        public virtual string ItemText { get; set; }

        [StringLength(MaxGlAccountLength)]
        public virtual string GlAccount { get; set; }

        [StringLength(MaxCostCenterLength)]
        public virtual string CostCenter { get; set; }

        [StringLength(MaxWbsLength)]
        public virtual string Wbs { get; set; }

        [StringLength(MaxMaterialCodeToLength)]
        public virtual string MaterialCodeTo { get; set; }

        [StringLength(MaxPlantToLength)]
        public virtual string PlantTo { get; set; }

        [StringLength(MaxValuationTypeToLength)]
        public virtual string ValuationTypeTo { get; set; }

        [StringLength(MaxStorageLocationToLength)]
        public virtual string StorageLocationTo { get; set; }

        [StringLength(MaxBfPcLength)]
        public virtual string BfPc { get; set; }

        [StringLength(MaxCancelFlagLength)]
        public virtual string CancelFlag { get; set; }

        [StringLength(MaxReffMatDocNoLength)]
        public virtual string ReffMatDocNo { get; set; }

        [StringLength(MaxVendorNoLength)]
        public virtual string VendorNo { get; set; }

        [StringLength(MaxProfitCenterLength)]
        public virtual string ProfitCenter { get; set; }

        [StringLength(MaxShipemntCatLength)]
        public virtual string ShipemntCat { get; set; }

        [StringLength(MaxReferenceLength)]
        public virtual string Reference { get; set; }

        [StringLength(MaxAssetNoLength)]
        public virtual string AssetNo { get; set; }

        [StringLength(MaxSubAssetNoLength)]
        public virtual string SubAssetNo { get; set; }

        [StringLength(MaxEndOfRecordLength)]
        public virtual string EndOfRecord { get; set; }
    }

}

