using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{

    [Table("InvCkdPhysicalStockPeriod")]
    [Index(nameof(Description), Name = "IX_InvCkdPhysicalStockPeriod_Description")]
    public class InvCkdPhysicalStockPeriod : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxDescriptionLength = 100;


        [StringLength(MaxDescriptionLength)]
        public virtual string Description { get; set; }

        [Column(TypeName = "DATE")]
        public virtual DateTime? FromDate { get; set; }

        public virtual string FromTime { get; set; }

        [Column(TypeName = "DATE")]
        public virtual DateTime? ToDate { get; set; }

        public virtual string ToTime { get; set; }

        public virtual int? Status { get; set; }
    }

}