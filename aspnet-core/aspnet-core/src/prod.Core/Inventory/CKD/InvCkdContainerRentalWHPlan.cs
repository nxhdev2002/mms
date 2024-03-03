using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{
    [Table("InvCkdContainerRentalWHPlan")]
    public class InvCkdContainerRentalWHPlan : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxContainerNoLength = 15;

        public const int MaxRenbanLength = 20;

        public const int MaxInvoiceNoLength = 200;

        public const int MaxBillofladingNoLength = 200;

        public const int MaxSupplierNoLength = 50;

        public const int MaxSealNoLength = 20;

        public const int MaxListcaseNoLength = 1000;

        public const int MaxListLotNoLength = 1000;

        public const int MaxTransportLength = 50;

        public const int MaxPlateIdLength = 50;

        public const int MaxStatusLength = 10;

        public const int MaxIsActiveLength = 1;

		public const int MaxWHCodeLength = 3;


		[StringLength(MaxContainerNoLength)]
        public virtual string ContainerNo { get; set; }

        [StringLength(MaxRenbanLength)]
        public virtual string Renban { get; set; }

        [Column(TypeName = "date")]
        public virtual DateTime? RequestDate { get; set; }

        [Column(TypeName = "time(7)")]
        public virtual TimeSpan? RequestTime { get; set; }

        [StringLength(MaxInvoiceNoLength)]
        public virtual string InvoiceNo { get; set; }

        [StringLength(MaxBillofladingNoLength)]
        public virtual string BillofladingNo { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(MaxSealNoLength)]
        public virtual string SealNo { get; set; }

        [StringLength(MaxListcaseNoLength)]
        public virtual string ListcaseNo { get; set; }

        [StringLength(MaxListLotNoLength)]
        public virtual string ListLotNo { get; set; }

        [Column(TypeName = "date")]
        public virtual DateTime? CdDate { get; set; }

        public virtual DateTime? DevanningDate { get; set; }

        [Column(TypeName = "time(7)")]
        public virtual TimeSpan? DevanningTime { get; set; }

        public virtual DateTime? ActualDevanningDate { get; set; }

        public virtual DateTime? GateInPlanTime { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? GateInActualDateTime { get; set; }

        [StringLength(MaxTransportLength)]
        public virtual string Transport { get; set; }

        [StringLength(MaxPlateIdLength)]
        public virtual string PlateId { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }

		[StringLength(MaxWHCodeLength)]
		public virtual string WHCode { get; set; }
	}
}

