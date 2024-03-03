using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.IF
{

    [Table("IF_HEADER_FWG")]
    public class IF_HEADER_FWG : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxBeginOfRecordLength = 5;

        public const int MaxRecordIdLength = 1;

        public const int MaxFromSystemLength = 5;

        public const int MaxToSystemLength = 30;

        [StringLength(MaxBeginOfRecordLength)]
        public virtual string BeginOfRecord { get; set; }

        [StringLength(MaxRecordIdLength)]
        public virtual string RecordId { get; set; }

        [StringLength(MaxFromSystemLength)]
        public virtual string FromSystem { get; set; }

        [StringLength(MaxToSystemLength)]
        public virtual string ToSystem { get; set; }
    }

}
