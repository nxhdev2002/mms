using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Inventory
{
    [Table("MstInvInvoiceDetailsStatus")]
    [Index(nameof(Code), Name = "IX_MstInvInvoiceDetailsStatus_Code")]
    [Index(nameof(IsActive), Name = "IX_MstInvInvoiceDetailsStatus_IsActive")]
    public class MstInvInvoiceDetailsStatus : FullAuditedEntity<long>, IEntity<long>
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
