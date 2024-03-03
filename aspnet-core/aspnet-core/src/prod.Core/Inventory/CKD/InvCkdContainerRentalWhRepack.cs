using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace prod.Inventory.CKD
{
    [Table("InvCkdContainerRentalWhRepack")]
    public class InvCkdContainerRentalWhRepack : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxExpLength = 15;

        public const int MaxModuleLength = 6;

        public const int MaxPartNoLength = 200;
        public const int MaxLotNoLength = 20;
        public const int MaxRepackModuleNoLength = 6;
        public const int MaxWHCurrentLength = 50;

        [StringLength(MaxExpLength)]
        public virtual string Exp { get; set; }

        [StringLength(MaxLotNoLength)]
        public virtual string LotNo { get; set; }

        [StringLength(MaxModuleLength)]
        public virtual string Module { get; set; }

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }
        public virtual int? Qty { get; set; }
        public virtual int? RemainQty { get; set; }
        public virtual DateTime? PackingDate { get; set; }

        [StringLength(MaxRepackModuleNoLength)]
        public virtual string RepackModuleNo { get; set; }

        [StringLength(MaxWHCurrentLength)]
        public virtual string WHCurrent { get; set; }

        [StringLength(MaxWHCurrentLength)]
        public virtual string WHNew { get; set; }

        public virtual int? Shift { get; set; }
        public virtual DateTime? ReceiveDateTime { get; set; }

        [StringLength(MaxWHCurrentLength)]
        public virtual string Container { get; set; }

        public virtual bool? Status { get; set; }
    }
}
