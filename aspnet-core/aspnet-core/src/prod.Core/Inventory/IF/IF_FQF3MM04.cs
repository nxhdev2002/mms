using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.IF
{

    [Table("IF_FQF3MM04")]
    public class IF_FQF3MM04 : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxInvoiceNoLength = 10;

        public const int MaxRenbanLength = 6;

        public const int MaxContainerNoLength = 15;

        public const int MaxModuleNoLength = 9;

        public const int MaxPlantLength = 4;

        public const int MaxCancelFlagLength = 1;

        public const int MaxEndOfRecordLength = 1;

        public virtual int? RecordId { get; set; }

        [StringLength(MaxInvoiceNoLength)]
        public virtual string InvoiceNo { get; set; }

        [StringLength(MaxRenbanLength)]
        public virtual string Renban { get; set; }

        [StringLength(MaxContainerNoLength)]
        public virtual string ContainerNo { get; set; }

        [StringLength(MaxModuleNoLength)]
        public virtual string ModuleNo { get; set; }

        [Column(TypeName = "Date")]
        public virtual DateTime? DevaningDate { get; set; }

        [StringLength(MaxPlantLength)]
        public virtual string Plant { get; set; }

        [StringLength(MaxCancelFlagLength)]
        public virtual string CancelFlag { get; set; }

        [StringLength(MaxEndOfRecordLength)]
        public virtual string EndOfRecord { get; set; }
    }

}

