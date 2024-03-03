using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{
    [Table("InvCkdPaymentRequest")]
    public class InvCkdPaymentRequest : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxCustomsPortLength = 12;

        public const int MaxReqPersonLength = 50;

        public const int MaxReqDepartmentLength = 50;

        public const int MaxOrdertypeCodeLength = 2;

        public const int MaxIsActiveLength = 1;



        public virtual TimeSpan? Time { get; set; }

        [StringLength(MaxCustomsPortLength)]
        public virtual string CustomsPort { get; set; }

        [Column(TypeName = "Date")]
        public virtual DateTime? ReqDate { get; set; }

        [StringLength(MaxReqPersonLength)]
        public virtual string ReqPerson { get; set; }

        [StringLength(MaxReqDepartmentLength)]
        public virtual string ReqDepartment { get; set; }

        [StringLength(MaxOrdertypeCodeLength)]
        public virtual string OrdertypeCode { get; set; }

        public virtual int? Status { get; set; }

        public virtual DateTime? CreateDate { get; set; }

        public virtual DateTime? UpdateDate { get; set; }

        public virtual int? IsFromEcus5 { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }
}
