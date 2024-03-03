using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using Twilio.TwiML.Fax;

namespace prod.Inventory.GPS
{
    public class InvGpsReceiving_T : FullAuditedEntity<long>, IEntity<long>
    {
		public const int MaxPoNoLength = 12;

		public const int MaxPartNoLength = 12;

        public const int MaxPartNameLength = 300;

        public const int MaxUomLength = 50;

        public const int MaxLotNoLength = 50;

        public const int MaxSupplierLength = 50;

        public const int MaxGuidLength = 128;

        public const int MaxErrorDescriptionLength = 5000;


        [StringLength(MaxGuidLength)]
        public virtual string Guid { get; set; }

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


        [StringLength(MaxErrorDescriptionLength)]
        public virtual string ErrorDescription { get; set; }


    }
}
