using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.DRM
{
    [Table("InvDrmStockRundownTransaction")]
    public class InvDrmStockRundownTransaction : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxMaterialCodeLength = 40;

        public const int MaxMaterialSpecLength = 200;

        public const int MaxPartNoLength = 20;

        public const int MaxPartNameLength = 200;


        [StringLength(MaxMaterialCodeLength)]
        public virtual string MaterialCode { get; set; }

        [StringLength(MaxMaterialSpecLength)]
        public virtual string MaterialSpec { get; set; }

        public virtual long? DrmMaterialId { get; set; }

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        public virtual long? PartId { get; set; }

        public virtual long? MaterialId { get; set; }

        public virtual int? Qty { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        public virtual DateTime? TransactionDate { get; set; }

        public virtual long? TransactionId { get; set; }
    }

}

