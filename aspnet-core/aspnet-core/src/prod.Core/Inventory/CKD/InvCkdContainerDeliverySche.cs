using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{

    [Table("InvCkdContainerDeliverySche")]
    public class InvCkdContainerDeliverySche : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxContainerNoLength = 15;

        public const int MaxRenbanLength = 20;

        public const int MaxInvoiceNoLength = 200;

        public const int MaxBillofladingNoLength = 200;

        public const int MaxListcaseNoLength = 1000;

        public const int MaxSourceLength = 50;

        public const int MaxTransportLength = 50;

        public const int MaxListLotNoLength = 1000;

        public const int MaxLotNoLength = 10;

        public const int MaxStatusLength = 10;

        public const int MaxReturnTypeLength = 10;

        public const int MaxDockLength = 10;

        public const int MaxDevanningUserLength = 100;

        public const int MaxRemarkLength = 1000;

        public const int MaxShopLength = 10;

        public const int MaxDevanningSpentTimeLength = 6;

        public const int MaxContDeliveryTypeLength = 10;

        public const int MaxTempPartnoLength = 100;

        public const int MaxLotFollowLength = 30;

        public const int MaxLotNoOrgLength = 10;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxContainerNoLength)]
        public virtual string ContainerNo { get; set; }

        [StringLength(MaxRenbanLength)]
        public virtual string Renban { get; set; }

        [StringLength(MaxInvoiceNoLength)]
        public virtual string InvoiceNo { get; set; }

        public virtual long? CkdReqId { get; set; }

        [StringLength(MaxBillofladingNoLength)]
        public virtual string BillofladingNo { get; set; }

        [StringLength(MaxListcaseNoLength)]
        public virtual string ListcaseNo { get; set; }

        [StringLength(MaxSourceLength)]
        public virtual string Source { get; set; }

        [StringLength(MaxTransportLength)]
        public virtual string Transport { get; set; }

        public virtual DateTime? CdDate { get; set; }

        public virtual int? TimeRequest { get; set; }

        [StringLength(MaxListLotNoLength)]
        public virtual string ListLotNo { get; set; }

        [StringLength(MaxLotNoLength)]
        public virtual string LotNo { get; set; }

        public virtual DateTime? DevanningDate { get; set; }

        public virtual DateTime? ActualDevanningDate { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(MaxReturnTypeLength)]
        public virtual string ReturnType { get; set; }

        [StringLength(MaxDockLength)]
        public virtual string Dock { get; set; }

        public virtual int? DevanningTime { get; set; }

        public virtual int? ActualDevanningTime { get; set; }

        [StringLength(MaxDevanningUserLength)]
        public virtual string DevanningUser { get; set; }

        [StringLength(MaxRemarkLength)]
        public virtual string Remark { get; set; }

        public virtual int? CkdTimeRequest { get; set; }

        [StringLength(MaxShopLength)]
        public virtual string Shop { get; set; }

        [StringLength(MaxDevanningSpentTimeLength)]
        public virtual string DevanningSpentTime { get; set; }

        [StringLength(MaxContDeliveryTypeLength)]
        public virtual string ContDeliveryType { get; set; }

        [StringLength(MaxTempPartnoLength)]
        public virtual string TempPartno { get; set; }

        public virtual int? ContainerOrder { get; set; }

        public virtual int? PcdTimeRequest { get; set; }

        public virtual int? Urgent { get; set; }

        [StringLength(MaxLotFollowLength)]
        public virtual string LotFollow { get; set; }

        [StringLength(MaxLotNoOrgLength)]
        public virtual string LotNoOrg { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}

