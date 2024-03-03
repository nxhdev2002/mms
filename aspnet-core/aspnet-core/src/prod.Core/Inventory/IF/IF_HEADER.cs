using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.IF
{

    [Table("IF_HEADER")]
    public class IF_HEADER : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxBeginOfRecordLength = 5;

        public const int MaxRecordIdLength = 1;

        public const int MaxFromSystemLength = 5;

        public const int MaxToSystemLength = 30;

        public const int MaxFileNameLength = 26;

        public const int MaxInterfaceNameLength = 8;

        public const int MaxEndOfRecordLength = 1;

        [StringLength(MaxBeginOfRecordLength)]
        public virtual string BeginOfRecord { get; set; }

        [StringLength(MaxRecordIdLength)]
        public virtual string RecordId { get; set; }

        [StringLength(MaxFromSystemLength)]
        public virtual string FromSystem { get; set; }

        [StringLength(MaxToSystemLength)]
        public virtual string ToSystem { get; set; }

        [StringLength(MaxFileNameLength)]
        public virtual string FileName { get; set; }

        [StringLength(MaxInterfaceNameLength)]
        public virtual string InterfaceName { get; set; }

        public virtual int? RecordLength { get; set; }

        [StringLength(MaxEndOfRecordLength)]
        public virtual string EndOfRecord { get; set; }
    }

}

