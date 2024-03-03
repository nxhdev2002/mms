using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{

    [Table("InvCkdSmqdOrder")]
    public class InvCkdSmqdOrder : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxShopLength = 1;

        public const int MaxRunNoLength = 50;

        public const int MaxCfcLength = 4;

        public const int MaxSupplierNoLength = 50;

        public const int MaxPartNameLength = 200;

        public const int MaxOrderNoLength = 50;

        public const int MaxTransportLength = 50;

        public const int MaxReasonCodeLength = 50;

        public const int MaxActualEtaPortLength = 50;

        public const int MaxInvoiceNoLength = 50;

        public const int MaxRemarkLength = 500;


        [StringLength(MaxShopLength)]
        public virtual string Shop { get; set; }

        public virtual DateTime? SmqdDate { get; set; }

        [StringLength(MaxRunNoLength)]
        public virtual string RunNo { get; set; }

        [StringLength(MaxCfcLength)]
        public virtual string Cfc { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        public virtual string PartNo { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(MaxOrderNoLength)]
        public virtual string OrderNo { get; set; }

        public virtual int? OrderQty { get; set; }

        public virtual DateTime? OrderDate { get; set; }

        [StringLength(MaxTransportLength)]
        public virtual string Transport { get; set; }

        [StringLength(MaxReasonCodeLength)]
        public virtual string ReasonCode { get; set; }

        public virtual DateTime? EtaRequest { get; set; }

        [StringLength(MaxActualEtaPortLength)]
        public virtual string ActualEtaPort { get; set; }

        public virtual DateTime? EtaExpReply { get; set; }

        [StringLength(MaxInvoiceNoLength)]
        public virtual string InvoiceNo { get; set; }

        public virtual DateTime? ReceiveDate { get; set; }

        public virtual int? ReceiveQty { get; set; }

        [StringLength(MaxRemarkLength)]
        public virtual string Remark { get; set; }

        public virtual int? OrderType { get; set; }
    }

}

