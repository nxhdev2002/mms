using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace prod.Inventory.GPS
{
    public class InvGpsReceiving : FullAuditedEntity<long>, IEntity<long>
    {
		public const int MaxPoNoLength = 50;

		public const int MaxPartNoLength = 14;

        public const int MaxPartNameLength = 300;

        public const int MaxUomLength = 50;

        public const int MaxLotNoLength = 50;

        public const int MaxSupplierLength = 50;

        public const int MaxSockLength = 50;

        public const int MaxUserReceivesLength = 50;

        public const int MaxShopLength = 10;

        [StringLength(MaxPoNoLength)]
		public virtual string PoNo { get; set; }

		[StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(MaxUomLength)]
        public virtual string Uom { get; set; }

        public virtual int? BoxQty { get; set; }

        public virtual int? Box { get; set; }

        public virtual int? Qty { get; set; }

        [StringLength(MaxLotNoLength)]
        public virtual string LotNo { get; set; }

        public virtual DateTime? ProdDate { get; set; }

        public virtual DateTime? ExpDate { get; set; }

        public virtual DateTime? ReceivedDate { get; set; }

        [StringLength(MaxSupplierLength)]
        public virtual string Supplier { get; set; }

        [StringLength(MaxSockLength)]
        public virtual string Dock { get; set; }

        [StringLength(MaxUserReceivesLength)]
        public virtual string UserReceives { get; set; }

        [StringLength(MaxShopLength)]
        public virtual string Shop { get; set; }

        public virtual decimal? PoPrice { get; set; }
    }
}
