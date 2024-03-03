using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.DRM
{

    [Table("InvDrmPartList")]
    public class InvDrmPartList : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxSupplierTypeLength = 20;

        public const int MaxSupplierCdLength = 20;

        public const int MaxCfcLength = 4;

        public const int MaxMaterialCodeLength = 40;

        public const int MaxMaterialSpecLength = 200;

        public const int MaxPartCodeLength = 60;

        public const int MaxSourcingLength = 40;

        public const int MaxCuttingLength = 40;

        public const int MaxPartSpecLength = 40;

        public const int MaxPartSizeLength = 40;

        //new
        public const int MaxLineItemLength = 4000;
        public const int MaxSubAssetNumberLength = 4;
        public const int MaxWBSLength = 24;
        public const int MaxCostCenterLength = 10;
        public const int MaxResponsibleCostCenterLength = 10;
        public const int MaxCostOfAssetLength = 24;


        [StringLength(MaxSupplierTypeLength)]
        public virtual string SupplierType { get; set; }

        [StringLength(MaxSupplierCdLength)]
        public virtual string SupplierCd { get; set; }

        [StringLength(MaxCfcLength)]
        public virtual string Cfc { get; set; }

        [StringLength(MaxMaterialCodeLength)]
        public virtual string MaterialCode { get; set; }

        [StringLength(MaxMaterialSpecLength)]
        public virtual string MaterialSpec { get; set; }

        [StringLength(MaxPartSpecLength)]
        public virtual string PartSpec { get; set; }

        [StringLength(MaxPartSizeLength)]
        public virtual string PartSize { get; set; }

        public virtual int? BoxQty { get; set; }

        [StringLength(MaxPartCodeLength)]
        public virtual string PartCode { get; set; }
        public virtual DateTime? FirstDayProduct { get; set; }
        public virtual DateTime? LastDayProduct { get; set; }

        [StringLength(MaxSourcingLength)]
        public virtual string Sourcing { get; set; }

        [StringLength(MaxCuttingLength)]
        public virtual string Cutting { get; set; }

        public virtual int? Packing { get; set; }

        public virtual decimal? SheetWeight { get; set; }

        public virtual decimal? YiledRation { get; set; }


        //new
        public virtual long? AssetId { get; set; }

        [StringLength(MaxLineItemLength)]
        public virtual string MainAssetNumber { get; set; }

        [StringLength(MaxWBSLength)]
        public virtual string WBS { get; set; }

        [StringLength(MaxCostCenterLength)]
        public virtual string CostCenter { get; set; }

        [StringLength(MaxResponsibleCostCenterLength)]
        public virtual string ResponsibleCostCenter { get; set; }

        [StringLength(MaxCostOfAssetLength)]
        public virtual string CostOfAsset { get; set; }

        [StringLength(MaxSubAssetNumberLength)]
        public virtual string AssetSubNumber { get; set; }
    }

}


