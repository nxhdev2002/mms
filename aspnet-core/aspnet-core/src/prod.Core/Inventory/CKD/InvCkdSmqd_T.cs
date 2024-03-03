
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{

    [Table("InvCkdSmqd_T")]
    public class InvCkdSmqd_T : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxGuidLength = 128;

        public const int MaxRunNoLength = 50;

        public const int MaxCfcLength = 50;

        public const int MaxLotNoLength = 10;

        public const int MaxCheckModelLength = 200;

        public const int MaxSupplierNoLength = 20;

        public const int MaxPartNoLength = 50;

        public const int MaxPartNameLength = 200;

        public const int MaxReasonCodeLength = 500;

        public const int MaxOrderStatusLength = 50;

        public const int MaxRemarkLength = 5000;

        public const int MaxErrorDescriptionLength = 5000;



        [StringLength(MaxGuidLength)]
        public virtual string Guid { get; set; }

        [StringLength(MaxRunNoLength)]
        public virtual string RunNo { get; set; }

        [StringLength(MaxCfcLength)]
        public virtual string Cfc { get; set; }

        [StringLength(MaxLotNoLength)]
        public virtual string LotNo { get; set; }

        [StringLength(MaxCheckModelLength)]
        public virtual string CheckModel { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        public virtual int? Qty { get; set; }

        public virtual int? EffectQty { get; set; }

        [StringLength(MaxReasonCodeLength)]
        public virtual string ReasonCode { get; set; }

        [StringLength(MaxOrderStatusLength)]
        public virtual string OrderStatus { get; set; }

        public virtual int? ReturnQty { get; set; }

        public virtual DateTime? ReturnDate { get; set; }

        [StringLength(MaxRemarkLength)]
        public virtual string Remark { get; set; }

        public virtual DateTime? SmqdDate { get; set; }

        [StringLength(MaxErrorDescriptionLength)]
        public string ErrorDescription { get; set; }

    }

}

