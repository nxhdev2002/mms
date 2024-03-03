using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Inventory
{
    [Table("MstInvInvoiceStatus")]
    [Index(nameof(Code), Name = "IX_MstInvInvoiceStatus_Code")]
    [Index(nameof(Description), Name = "IX_MstInvInvoiceStatus_Description")]
    [Index(nameof(IsActive), Name = "IX_MstInvInvoiceStatus_IsActive")]
    public class MstInvInvoiceStatus : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxCodeLength = 20;

        public const int MaxDescriptionLength = 50;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MaxDescriptionLength)]
        public virtual string Description { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }
}
