using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{

    [Table("InvCkdContainerTransitPortPlan")]
    [Index(nameof(ContainerNo), Name = "IX_InvCkdContainerTransitPortPlan_ContainerNo")]
    [Index(nameof(Renban), Name = "IX_InvCkdContainerTransitPortPlan_Renban")]
    [Index(nameof(RequestDate), Name = "IX_InvCkdContainerTransitPortPlan_RequestDate")]
    [Index(nameof(InvoiceNo), Name = "IX_InvCkdContainerTransitPortPlan_InvoiceNo")]
    [Index(nameof(IsActive), Name = "IX_InvCkdContainerTransitPortPlan_IsActive")]
    public class InvCkdContainerTransitPortPlan : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxContainerNoLength = 15;

        public const int MaxRenbanLength = 20;

        public const int MaxInvoiceNoLength = 200;

        public const int MaxBillOfLadingNoLength = 200;

        public const int MaxSupplierNoLength = 50;

        public const int MaxSealNoLength = 20;

        public const int MaxListCaseNoLength = 1000;

        public const int MaxListLotNoLength = 1000;

        public const int MaxTransportLength = 50;

        public const int MaxStatusLength = 10;

        public const int MaxPortCodeLength = 50;

        public const int MaxPortNameLength = 100;

        public const int MaxRemarksLength = 200;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxContainerNoLength)]
        public virtual string ContainerNo { get; set; }

        [StringLength(MaxRenbanLength)]
        public virtual string Renban { get; set; }

        public virtual DateTime? RequestDate { get; set; }

        public virtual TimeSpan? RequestTime { get; set; }

        [StringLength(MaxInvoiceNoLength)]
        public virtual string InvoiceNo { get; set; }

        [StringLength(MaxBillOfLadingNoLength)]
        public virtual string BillOfLadingNo { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(MaxSealNoLength)]
        public virtual string SealNo { get; set; }

        [StringLength(MaxListCaseNoLength)]
        public virtual string ListCaseNo { get; set; }

        [StringLength(MaxListLotNoLength)]
        public virtual string ListLotNo { get; set; }

        public virtual DateTime? CdDate { get; set; }

        [StringLength(MaxTransportLength)]
        public virtual string Transport { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(MaxPortCodeLength)]
        public virtual string PortCode { get; set; }

        [StringLength(MaxPortNameLength)]
        public virtual string PortName { get; set; }

        [StringLength(MaxRemarksLength)]
        public virtual string Remarks { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}

