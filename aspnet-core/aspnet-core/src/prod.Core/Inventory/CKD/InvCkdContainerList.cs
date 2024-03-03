using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{
    [Table("InvCkdContainerList")]
    [Index(nameof(TransitPortReqDate), Name = "IX_InvCkdContainerList_TransitPortReqDate")]

    public class InvCkdContainerList : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxContainerNoLength = 15;

        public const int MaxRenbanLength = 6;

        public const int MaxSupplierNoLength = 10;

        public const int MaxHaisenNoLength = 10;

        public const int MaxBillOfLadingNoLength = 100;

        public const int MaxSealNoLength = 20;

        public const int MaxCdStatusLength = 20;

        public const int MaxRequestLotNoLength = 10;

        public const int MaxInvoiceNoLength = 500;

        public const int MaxListLotNoLength = 1000;

        public const int MaxListCaseNoLength = 1000;

        public const int MaxTransportLength = 10;

        public const int MaxShopLength = 10;

        public const int MaxDockLength = 10;

        public const int MaxRemarkLength = 1000;

        public const int MaxWhLocationLength = 10;

        public const int MaxTransitPortRemarkLength = 1000;

        public const int MaxStatusLength = 20;

        public const int MaxLocationCodeLength = 20;

        public const int MaxIsActiveLength = 1;

        public const int MaxDevanningTimeLength = 255;

        public const int MaxTransitPortReqTimeLength = 255;

        public const int MaxGateinTimeLength = 255;

        public const int MaxOrderTypeCodeLength = 4;

        public const int MaxGoodsTypeCodeLength = 4;


        [StringLength(MaxContainerNoLength)]
        public virtual string ContainerNo { get; set; }

        [StringLength(MaxRenbanLength)]
        public virtual string Renban { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(MaxHaisenNoLength)]
        public virtual string HaisenNo { get; set; }

        [StringLength(MaxBillOfLadingNoLength)]
        public virtual string BillOfLadingNo { get; set; }

        [StringLength(MaxSealNoLength)]
        public virtual string SealNo { get; set; }

        public virtual DateTime? CdDate { get; set; }

        [StringLength(MaxCdStatusLength)]
        public virtual string CdStatus { get; set; }

        public virtual int? ContainerSize { get; set; }

        public virtual long? ShipmentId { get; set; }

        public virtual DateTime? ShippingDate { get; set; }

        public virtual DateTime? PortDate { get; set; }

        public virtual DateTime? PortDateActual { get; set; }

        public virtual DateTime? PortTransitDate { get; set; }

        public virtual DateTime? ReceiveDate { get; set; }

        public virtual long? RequestId { get; set; }

        [StringLength(MaxRequestLotNoLength)]
        public virtual string RequestLotNo { get; set; }

        [StringLength(MaxInvoiceNoLength)]
        public virtual string InvoiceNo { get; set; }

        [StringLength(MaxListLotNoLength)]
        public virtual string ListLotNo { get; set; }

        [StringLength(MaxListCaseNoLength)]
        public virtual string ListCaseNo { get; set; }

        [StringLength(MaxTransportLength)]
        public virtual string Transport { get; set; }

        [StringLength(MaxShopLength)]
        public virtual string Shop { get; set; }

        [StringLength(MaxDockLength)]
        public virtual string Dock { get; set; }

        public virtual DateTime? DevanningDate { get; set; }

        [StringLength(MaxDevanningTimeLength)]
        public virtual string DevanningTime { get; set; }

        [StringLength(MaxRemarkLength)]
        public virtual string Remark { get; set; }

        [StringLength(MaxWhLocationLength)]
        public virtual string WhLocation { get; set; }

        public virtual DateTime? GateinDate { get; set; }

        [StringLength(MaxGateinTimeLength)]
        public virtual string GateinTime { get; set; }

        public virtual long? TransitPortReqId { get; set; }

        public virtual DateTime? TransitPortReqDate { get; set; }

        [StringLength(MaxTransitPortReqTimeLength)]
        public virtual string TransitPortReqTime { get; set; }

        [StringLength(MaxTransitPortRemarkLength)]
        public virtual string TransitPortRemark { get; set; }

        public virtual decimal? Fob { get; set; }

        public virtual decimal? Freight { get; set; }

        public virtual decimal? Insurance { get; set; }

        public virtual decimal? Cif { get; set; }

        public virtual decimal? Tax { get; set; }

        public virtual decimal? Amount { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(MaxLocationCodeLength)]
        public virtual string LocationCode { get; set; }

        public virtual DateTime? LocationDate { get; set; }

        public virtual long? ReceivingPeriodId { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }

        [StringLength(MaxOrderTypeCodeLength)]
        public virtual string OrdertypeCode { get; set; }

        [StringLength(MaxGoodsTypeCodeLength)]
        public virtual string GoodstypeCode { get; set; }
    }

}

