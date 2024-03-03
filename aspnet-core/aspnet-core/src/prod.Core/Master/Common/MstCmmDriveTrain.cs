using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Cmm
{
    [Table("MstCmmDriveTrain")]
    public class MstCmmDriveTrain : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxCodeLength = 10;

        public const int MaxNameLength = 50;

        [StringLength(MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }
    }

}