using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.IHP
{

    [Table("InvIhpPartList")]
    public class InvIhpPartList : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxSupplierTypeLength = 20;

        public const int MaxSupplierCdLength = 20;

        public const int MaxCfcLength = 4;

        public const int MaxPartNoLength = 20;

        public const int MaxPartNameLength = 200;

        public const int MaxPartSizeLength = 40;

        public const int MaxSourcingLength = 40;

        public const int MaxCuttingLength = 40;


        [StringLength(MaxSupplierTypeLength)]
        public virtual string SupplierType { get; set; }

        [StringLength(MaxSupplierCdLength)]
        public virtual string SupplierCd { get; set; }

        [StringLength(MaxCfcLength)]
        public virtual string Cfc { get; set; }

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string MaterialCode { get; set; }

        [StringLength(MaxSourcingLength)]
        public virtual string Sourcing { get; set; }

        [StringLength(MaxCuttingLength)]
        public virtual string Cutting { get; set; }

        public virtual int? Packing { get; set; }

        public virtual decimal? SheetWeight { get; set; }

        public virtual decimal? YiledRation { get; set; }
    }

}


