using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.LogW.Mwh.Dto
{

	public class LgwMwhContListDto : EntityDto<long?>
	{

		public virtual string ContainerNo { get; set; }

		public virtual string Renban { get; set; }

		public virtual string SupplierNo { get; set; }

		public virtual DateTime? DevanningDate { get; set; }

		public virtual DateTime? StartDevanningDate { get; set; }

		public virtual DateTime? FinishDevanningDate { get; set; }

		public virtual string Status { get; set; }

		public virtual int? ContScheduleId { get; set; }

		public virtual string Shop { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditLgwMwhContListDto : EntityDto<long?>
	{

		[StringLength(LgwMwhContListConsts.MaxContainerNoLength)]
		public virtual string ContainerNo { get; set; }

		[StringLength(LgwMwhContListConsts.MaxRenbanLength)]
		public virtual string Renban { get; set; }

		[StringLength(LgwMwhContListConsts.MaxSupplierNoLength)]
		public virtual string SupplierNo { get; set; }

		public virtual DateTime? DevanningDate { get; set; }

		public virtual DateTime? StartDevanningDate { get; set; }

		public virtual DateTime? FinishDevanningDate { get; set; }

		[StringLength(LgwMwhContListConsts.MaxStatusLength)]
		public virtual string Status { get; set; }

		public virtual int? ContScheduleId { get; set; }

		[StringLength(LgwMwhContListConsts.MaxShopLength)]
		public virtual string Shop { get; set; }

		[StringLength(LgwMwhContListConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetLgwMwhContListInput : PagedAndSortedResultRequestDto
	{
		public virtual string ContainerNo { get; set; }
		public virtual string Renban { get; set; }
        public virtual DateTime? DevanningDateFrom { get; set; }
        public virtual DateTime? DevanningDateTo { get; set; }

    }

    public class GetLgwMwhContListExportInput
    {
        public virtual string ContainerNo { get; set; }
        public virtual string Renban { get; set; }
        public virtual DateTime? DevanningDateFrom { get; set; }
        public virtual DateTime? DevanningDateTo { get; set; }

    }
}


