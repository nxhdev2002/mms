using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.GPS
{

    [Table("InvGpsMaterial_T")]
    public class InvGpsMaterial_T : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxPartNoLength = 20;

        public const int MaxPartNoNormalizedLength = 20;

        public const int MaxPartNameLength = 300;

        public const int MaxPartNameVnLength = 300;

        public const int MaxColorSfxLength = 10;

        public const int MaxPartTypeLength = 50;

        public const int MaxPurposeOfUseLength = 50;

        public const int MaxSpecLength = 1000;

        public const int MaxHasExpiryDateLength = 1;

        public const int MaxPartGroupLength = 50;

        public const int MaxCurrencyLength = 50;

        public const int MaxUOMLength = 50;

        public const int MaxSupplierNameLength = 200;

        public const int MaxSupplierNoLength = 50;

        public const int MaxLocalImportLength = 50;

        public const int MaxRemarkLength = 4000;

        public const int MaxIsActiveLength = 1;

        public const int MaxGuidLength = 128;

        public const int MaxErrorDescriptionLength = 5000;


        [StringLength(MaxGuidLength)]
        public virtual string Guid { get; set; }

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxPartNoNormalizedLength)]
        public virtual string PartNoNormalized { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(MaxPartNameVnLength)]
        public virtual string PartNameVn { get; set; }

        [StringLength(MaxColorSfxLength)]
        public virtual string ColorSfx { get; set; }

        [StringLength(MaxPartTypeLength)]
        public virtual string PartType { get; set; }

        [StringLength(MaxPurposeOfUseLength)]
        public virtual string PurposeOfUse { get; set; }

        [StringLength(MaxSpecLength)]
        public virtual string Spec { get; set; }

        [StringLength(MaxHasExpiryDateLength)]
        public virtual string HasExpiryDate { get; set; }

        [StringLength(MaxPartGroupLength)]
        public virtual string PartGroup { get; set; }

        public virtual decimal? Price { get; set; }

        [StringLength(MaxCurrencyLength)]
        public virtual string Currency { get; set; }

        public virtual int? ConvertPrice { get; set; }

        [StringLength(MaxUOMLength)]
        public virtual string UOM { get; set; }

        [StringLength(MaxSupplierNameLength)]
        public virtual string SupplierName { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(MaxLocalImportLength)]
        public virtual string LocalImport { get; set; }

        public virtual int? LeadTime { get; set; }

        public virtual int? LeadTimeForecast { get; set; }

        public virtual decimal? MinLot { get; set; }

        public virtual decimal? BoxQty { get; set; }

        [StringLength(MaxRemarkLength)]
        public virtual string Remark { get; set; }

        public virtual int? PalletL { get; set; }

        public virtual int? PalletR { get; set; }

        public virtual int? PalletH { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }

        [StringLength(MaxErrorDescriptionLength)]
        public virtual string ErrorDescription { get; set; }
    }

}

