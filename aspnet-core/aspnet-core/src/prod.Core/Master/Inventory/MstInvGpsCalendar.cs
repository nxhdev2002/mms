using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Inv
{

    [Table("MstInvGpsCalendar")]
    public class MstInvGpsCalendar : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxSupplierCodeLength = 10;

        public const int MaxWorkingTypeLength = 2;

        public const int MaxWorkingStatusLength = 2;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxSupplierCodeLength)]
        public virtual string SupplierCode { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        [StringLength(MaxWorkingTypeLength)]
        public virtual string WorkingType { get; set; }

        [StringLength(MaxWorkingStatusLength)]
        public virtual string WorkingStatus { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}