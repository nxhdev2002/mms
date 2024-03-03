using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
using prod.LogW.Lup.ContModule;

namespace prod.LogW.Lup.Dto
{

    public class LgwLupContModuleDto : EntityDto<long?>
	{

		public virtual string InvoiceNo { get; set; }

		public virtual string SupplierNo { get; set; }

		public virtual string ContainerNo { get; set; }

		public virtual string Renban { get; set; }

		public virtual string LotNo { get; set; }

		public virtual string ModuleNo { get; set; }

		public virtual string Status { get; set; }

		public virtual string SortingType { get; set; }

		public virtual string SortingStatus { get; set; }

		public virtual DateTime? UpdatedSortingStatus { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditLgwLupContModuleDto : EntityDto<long?>
	{

		[StringLength(LgwLupContModuleConsts.MaxInvoiceNoLength)]
		public virtual string InvoiceNo { get; set; }

		[StringLength(LgwLupContModuleConsts.MaxSupplierNoLength)]
		public virtual string SupplierNo { get; set; }

		[StringLength(LgwLupContModuleConsts.MaxContainerNoLength)]
		public virtual string ContainerNo { get; set; }

		[StringLength(LgwLupContModuleConsts.MaxRenbanLength)]
		public virtual string Renban { get; set; }

		[StringLength(LgwLupContModuleConsts.MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		[StringLength(LgwLupContModuleConsts.MaxModuleNoLength)]
		public virtual string ModuleNo { get; set; }

		[StringLength(LgwLupContModuleConsts.MaxStatusLength)]
		public virtual string Status { get; set; }

		[StringLength(LgwLupContModuleConsts.MaxSortingTypeLength)]
		public virtual string SortingType { get; set; }

		[StringLength(LgwLupContModuleConsts.MaxSortingStatusLength)]
		public virtual string SortingStatus { get; set; }

		public virtual DateTime? UpdatedSortingStatus { get; set; }

		[StringLength(LgwLupContModuleConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetLgwLupContModuleInput : PagedAndSortedResultRequestDto
	{
        public virtual string ContainerNo { get; set; }
        public virtual string LotNo { get; set; }
        public virtual string ModuleNo { get; set; }
    }

    public class GetLgwLupContModuleExportInput
    {  
        public virtual string ContainerNo { get; set; }
        public virtual string LotNo { get; set; }
        public virtual string ModuleNo { get; set; }
   }
}

