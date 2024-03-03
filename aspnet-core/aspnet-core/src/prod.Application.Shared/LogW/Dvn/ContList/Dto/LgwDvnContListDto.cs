using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.LogW.Dvn.Dto
{

	public class LgwDvnContListDto : EntityDto<long?>
	{

		public virtual string ContainerNo { get; set; }

		public virtual string Renban { get; set; }

		public virtual string SupplierNo { get; set; }

		public virtual string LotNo { get; set; }

		public virtual DateTime? WorkingDate { get; set; }

        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }

        public virtual string ShiftNo { get; set; }

		public virtual string DevanningDock { get; set; }

		public virtual DateTime? PlanDevanningDate { get; set; }

		public virtual DateTime? ActDevanningDate { get; set; }

		public virtual DateTime? ActDevanningDateFinished { get; set; }

		public virtual string DevanningType { get; set; }

		public virtual string Status { get; set; }

		public virtual int? DevLeadtime { get; set; }

		public virtual int? PlanDevanningLineOff { get; set; }

		public virtual string SortingStatus { get; set; }

		public virtual string IsActive { get; set; }

		public virtual int? AvgTaktDevLT { get; set; }

		public virtual int? TotalActual { get; set; }

		public virtual string IsEci { get; set; }

		public virtual string IsDelayed { get; set; }	

	}

	public class CreateOrEditLgwDvnContListDto : EntityDto<long?>
	{

		[StringLength(LgwDvnContListConsts.MaxContainerNoLength)]
		public virtual string ContainerNo { get; set; }

		[StringLength(LgwDvnContListConsts.MaxRenbanLength)]
		public virtual string Renban { get; set; }

		[StringLength(LgwDvnContListConsts.MaxSupplierNoLength)]
		public virtual string SupplierNo { get; set; }

		[StringLength(LgwDvnContListConsts.MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		public virtual DateTime? WorkingDate { get; set; }

		[StringLength(LgwDvnContListConsts.MaxShiftNoLength)]
		public virtual string ShiftNo { get; set; }

		[StringLength(LgwDvnContListConsts.MaxDevanningDockLength)]
		public virtual string DevanningDock { get; set; }

		public virtual DateTime? PlanDevanningDate { get; set; }

		public virtual DateTime? ActDevanningDate { get; set; }

		public virtual DateTime? ActDevanningDateFinished { get; set; }

		[StringLength(LgwDvnContListConsts.MaxDevanningTypeLength)]
		public virtual string DevanningType { get; set; }

		[StringLength(LgwDvnContListConsts.MaxStatusLength)]
		public virtual string Status { get; set; }

		public virtual int? DevLeadtime { get; set; }

		public virtual int? PlanDevanningLineOff { get; set; }

		[StringLength(LgwDvnContListConsts.MaxSortingStatusLength)]
		public virtual string SortingStatus { get; set; }

		[StringLength(LgwDvnContListConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetLgwDvnContListInput : PagedAndSortedResultRequestDto
	{
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }
        public virtual string ContainerNo { get; set; }
		public virtual string Renban { get; set; }
		public virtual string LotNo { get; set; }
		public virtual DateTime? WorkingDate { get; set; }
	}

    public class GetLgwDvnContListExportInput
    {
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }
        public virtual string ContainerNo { get; set; }
        public virtual string Renban { get; set; }
        public virtual string LotNo { get; set; }
        public virtual DateTime? WorkingDate { get; set; }
    }
}


