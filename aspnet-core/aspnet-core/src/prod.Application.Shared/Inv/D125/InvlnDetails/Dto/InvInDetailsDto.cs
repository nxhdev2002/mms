using Abp.Application.Services.Dto;
using System;
namespace prod.Inv.D125.Dto
{
	public class InvInDetailsDto : EntityDto<long?>
	{
        public virtual decimal PeriodId { get; set; }
        public virtual string InvoiceNo { get; set; }
        public virtual string PartNo { get; set; }
        public virtual decimal? UsageQty { get; set; }
        public virtual DateTime? InvoiceDate { get; set; }
        public virtual DateTime? ReceiveDate { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string FixLot { get; set; }
        public virtual string CarfamilyCode { get; set; }
        public virtual string CustomsDeclareNo { get; set; }
        public virtual DateTime? DeclareDate { get; set; }
    }
	public class GetInvInDetailsInput : PagedAndSortedResultRequestDto
	{
		public virtual decimal? PeriodId { get; set; }
		public virtual string PartNo { get; set; }
		public virtual string SupplierNo { get; set; }
		public virtual string FixLot { get; set; }
		public virtual string CarfamilyCode { get; set; }

	}

    public class GetInvInDetailsExportInput 
    {
        public virtual decimal? PeriodId { get; set; }
        public virtual string PartNo { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string FixLot { get; set; }
        public virtual string CarfamilyCode { get; set; }

    }

}


