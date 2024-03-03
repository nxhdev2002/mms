using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Cmm
{
    [Table("MstCmmColor")]
    public class MstCmmColor : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxColorLength = 4;

        public const int MaxNameEnLength = 50;

        public const int MaxNameVnLength = 50;

        public const int MaxColorTypeLength = 50;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxColorLength)]
        public virtual string Color { get; set; }

        [StringLength(MaxNameEnLength)]
        public virtual string NameEn { get; set; }

        [StringLength(MaxNameVnLength)]
        public virtual string NameVn { get; set; }

        [StringLength(MaxColorTypeLength)]
        public virtual string ColorType { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}