using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{

    [Table("InvCkdPhysicalStockLot_T")]
    public class InvCkdPhysicalStockLot_T : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxGuidLength = 128;

        public const int MaxLotNoLength = 12;

        public const int MaxShopLength = 12;

        public const int MaxErrorDescriptionLength = 5000;


        [StringLength(MaxGuidLength)]
        public virtual string Guid { get; set; }

        [StringLength(MaxLotNoLength)]
        public virtual string LotNo { get; set; }

        [StringLength(MaxShopLength)]
        public virtual string Shop { get; set; }
        public virtual decimal? Qty { get; set; }

        [StringLength(MaxErrorDescriptionLength)]
        public string ErrorDescription { get; set; }

    }

}

