
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{
    [Table("InvCkdPartPackingDetails")]
    public class InvCkdPartPackingDetails : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxPartNoLength = 50;

        public const int MaxBackNoLength = 10;

        public const int MaxModuleNoLength = 10;

        public const int MaxRenbanLength = 10;

        public const int MaxReExportCdLength = 10;

        public const int MaxIsActiveLength = 1;

        public const int MaxTypeLength = 50;

        public const int MaxCommonLength = 50;

        public const int MaxIcoFlagLength = 50;



        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        public virtual long? PartId { get; set; }

        [StringLength(MaxBackNoLength)]
        public virtual string BackNo { get; set; }

        [StringLength(MaxModuleNoLength)]
        public virtual string ModuleNo { get; set; }

        [StringLength(MaxRenbanLength)]
        public virtual string Renban { get; set; }

        public virtual int? BoxSize { get; set; }

        [StringLength(MaxTypeLength)]
        public virtual string Type { get; set; }

        [StringLength(MaxCommonLength)]
        public virtual string Common { get; set; }

        [StringLength(MaxIcoFlagLength)]
        public virtual string IcoFlag { get; set; }

        [StringLength(MaxReExportCdLength)]
        public virtual string ReExportCd { get; set; }

        [Column(TypeName = "Date")]
        public virtual DateTime? StartPackingMonth { get; set; }

        [Column(TypeName = "Date")]
        public virtual DateTime? EndPackingMonth { get; set; }

        [Column(TypeName = "Date")]
        public virtual DateTime? StartProductionMonth { get; set; }

        [Column(TypeName = "Date")]
        public virtual DateTime? EndProductionMonth { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }
}

