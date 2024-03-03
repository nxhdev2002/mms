using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Painting.Andon.Dto
{

	public class PtsAdoLineEfficiencyDetailsDto : EntityDto<long?>
	{

		public virtual string Line { get; set; }

		public virtual int? VolActual { get; set; }

		public virtual int? LineStopTime { get; set; }

		public virtual decimal? LineEfficiency { get; set; }

		public virtual DateTime? WorkingDate { get; set; }

		public virtual string Shift { get; set; }

		public virtual string Status { get; set; }

	}

	public class CreateOrEditPtsAdoLineEfficiencyDetailsDto : EntityDto<long?>
	{

		[StringLength(PtsAdoLineEfficiencyDetailsConsts.MaxLineLength)]
		public virtual string Line { get; set; }

		public virtual int? VolActual { get; set; }

		public virtual int? LineStopTime { get; set; }

		public virtual decimal LineEfficiency { get; set; }

		public virtual DateTime? WorkingDate { get; set; }

		[StringLength(PtsAdoLineEfficiencyDetailsConsts.MaxShiftLength)]
		public virtual string Shift { get; set; }

		[StringLength(PtsAdoLineEfficiencyDetailsConsts.MaxStatusLength)]
		public virtual string Status { get; set; }
	}

	public class GetPtsAdoLineEfficiencyDetailsInput : PagedAndSortedResultRequestDto
	{

		public virtual string Line { get; set; }

		public virtual DateTime? WorkingDate { get; set; }
        public virtual DateTime? RequestDateFrom { get; set; }
        public virtual DateTime? RequestDateTo { get; set; }

        public virtual string Shift { get; set; }


	}

    public class GetPtsAdoLineEfficiencyDetailsExportInput
    {

        public virtual string Line { get; set; }

        public virtual DateTime? WorkingDate { get; set; }
        public virtual DateTime? RequestDateFrom { get; set; }
        public virtual DateTime? RequestDateTo { get; set; }
        public virtual string Shift { get; set; }


    }

}


