using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.LogW.Mwh.Dto
{

	public class LgwMwhPartDataDto : EntityDto<long?>
	{

		public virtual long? PxppartId { get; set; }

		public virtual string PartNo { get; set; }

		public virtual string LotNo { get; set; }

		public virtual string Fixlot { get; set; }

		public virtual string CaseNo { get; set; }

		public virtual string ModuleNo { get; set; }

		public virtual string ContainerNo { get; set; }

		public virtual string SupplierNo { get; set; }

		public virtual decimal? UsageQty { get; set; }

		public virtual string PartName { get; set; }

		public virtual string CarfamilyCode { get; set; }

		public virtual long? InvoiceParentId { get; set; }

		public virtual long? PxpcaseId { get; set; }

		public virtual string OrderType { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class GetLgwMwhPartDataInput : PagedAndSortedResultRequestDto
	{
		public virtual string PartNo { get; set; }
		public virtual string LotNo { get; set; }
		public virtual string CaseNo { get; set; }
		public virtual string ContainerNo { get; set; }
		public virtual string SupplierNo { get; set; }
		public virtual string IsActive { get; set; }
	}

    public class GetLgwMwhPartDataExportInput
    {
        public virtual string PartNo { get; set; }
        public virtual string LotNo { get; set; }
        public virtual string CaseNo { get; set; }
        public virtual string ContainerNo { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string IsActive { get; set; }
    }
}


