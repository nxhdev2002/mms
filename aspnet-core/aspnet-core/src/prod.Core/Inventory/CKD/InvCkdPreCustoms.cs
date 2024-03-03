using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{
    [Table("InvCkdPreCustoms")]
    public class InvCkdPreCustoms : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxBillofladingNoLength = 20;

        public const int MaxStatusLength = 4;

        public const int MaxOrdertypeCodeLength = 2;

        public const int MaxDeclareTypeLength = 10;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxBillofladingNoLength)]
        public virtual string BillofladingNo { get; set; }

        public virtual decimal? TotaltaxCept { get; set; }

        public virtual decimal? TotaltaxNoncept { get; set; }

        public virtual decimal? TotalvatCept { get; set; }

        public virtual decimal? TotalvatNoncept { get; set; }

        public virtual decimal? TotalcifCept { get; set; }

        public virtual decimal? TotalcifNoncept { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(MaxOrdertypeCodeLength)]
        public virtual string OrdertypeCode { get; set; }

        public virtual DateTime? UpdatedDate { get; set; }

        public virtual long? GroupId { get; set; }

        public virtual int? Isvnaccs { get; set; }

        public virtual long? GroupParentId { get; set; }

        [StringLength(MaxDeclareTypeLength)]
        public virtual string DeclareType { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }
}

