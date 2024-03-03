using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Inventory
{

    [Table("MstInvForwarder")]
    [Index(nameof(Code), Name = "IX_MstInvForwarder_Code")]
    [Index(nameof(Name), Name = "IX_MstInvForwarder_Name")]
    [Index(nameof(EfDateto), Name = "IX_MstInvForwarder_EfDateto")]
    [Index(nameof(IsActive), Name = "IX_MstInvForwarder_IsActive")]
    public class MstInvForwarder : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxCodeLength = 10;

        public const int MaxNameLength = 100;

        public const int MaxSupplierNoLength = 10;

        public const int MaxShippingNoLength = 10;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(MaxShippingNoLength)]
        public virtual string ShippingNo { get; set; }

        public virtual DateTime? EfDatefrom { get; set; }

        public virtual DateTime? EfDateto { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}

