using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{

    [Table("InvCkdFrameEngine")]
    public class InvCkdFrameEngine : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxFrameNoLength = 30;

        public const int MaxEngineNoLength = 30;

        public const int MaxPartNoLength = 15;

        public const int MaxLotNoLength = 10;

        public const int MaxLotCodeLength = 10;

        public const int MaxContainerNoLength = 15;

        public const int MaxCaseNoLength = 6;

        public const int MaxModuleNoLength = 6;

        public const int MaxIsActiveLength = 1;

        public virtual long? InvoiceId { get; set; }

        [StringLength(MaxFrameNoLength)]
        public virtual string FrameNo { get; set; }

        [StringLength(MaxEngineNoLength)]
        public virtual string EngineNo { get; set; }

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxLotNoLength)]
        public virtual string LotNo { get; set; }

        [StringLength(MaxLotCodeLength)]
        public virtual string LotCode { get; set; }

        [StringLength(MaxContainerNoLength)]
        public virtual string ContainerNo { get; set; }

        [StringLength(MaxCaseNoLength)]
        public virtual string CaseNo { get; set; }

        [StringLength(MaxModuleNoLength)]
        public virtual string ModuleNo { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}


