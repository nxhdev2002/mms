using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.PIO
{
    [Table("InvPioPartList")]
    public class InvPioPartList : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxFullModelLength = 40;

        public const int MaxProdSfxLength = 40;

        public const int MaxMktCodeLength = 40;

        public const int MaxPartNoLength = 40;

        public const int MaxPartTypeLength = 10;

        public const int MaxPartNameLength = 40;

        public const int MaxPioTypeLength = 10;

        public const int MaxSupplierLength = 50;

        public const int MaxRemarkLength = 5000;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxFullModelLength)]
        public virtual string FullModel { get; set; }

        [StringLength(MaxProdSfxLength)]
        public virtual string ProdSfx { get; set; }

        [StringLength(MaxMktCodeLength)]
        public virtual string MktCode { get; set; }

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(MaxPartTypeLength)]
        public virtual string PartType { get; set; }

        [StringLength(MaxPioTypeLength)]
        public virtual string PioType { get; set; }

        public virtual int? BoxSize { get; set; }

        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }

        [StringLength(MaxSupplierLength)]
        public virtual string Supplier { get; set; }

        [StringLength(MaxRemarkLength)]
        public virtual string Remark { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }
}

