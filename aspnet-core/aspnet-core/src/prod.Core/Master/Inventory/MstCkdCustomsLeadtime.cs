using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.CKD
{

    [Table("MstCkdCustomsLeadtime")]
    [Index(nameof(SupplierNo), Name = "IX_MstCkdCustomsLeadtime_SupplierNo")]
    public class MstCkdCustomsLeadtime : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxSupplierNoLength = 10;


        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        public virtual int Leadtime { get; set; }
    }

}

