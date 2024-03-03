using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.IF
{

    [Table("IF_TRAILER")]
    public class IF_TRAILER : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxBeginOfRecordLength = 1;

        public const int MaxRecordIdLength = 1;

        public const int MaxRecordCountLength = 7;

        public const int MaxEndOfRecordLength = 1;

        [StringLength(MaxBeginOfRecordLength)]
        public virtual string BeginOfRecord { get; set; }

        [StringLength(MaxRecordIdLength)]
        public virtual string RecordId { get; set; }

        [StringLength(MaxRecordCountLength)]
        public virtual string RecordCount { get; set; }

        [StringLength(MaxEndOfRecordLength)]
        public virtual string EndOfRecord { get; set; }
    }

}

