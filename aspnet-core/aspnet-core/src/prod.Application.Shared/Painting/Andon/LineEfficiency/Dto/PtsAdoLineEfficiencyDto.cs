using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Painting.Andon.Dto
{

	public class PtsAdoLineEfficiencyDto : EntityDto<long?>
	{

		public virtual string Line { get; set; }

		public virtual string Shift { get; set; }

		public virtual DateTime? WorkingDate { get; set; }
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }

        public virtual int? VolTarget { get; set; }

		public virtual int? VolActual { get; set; }

		public virtual int? VolBalance { get; set; }

		public virtual string StopTime { get; set; }

		public virtual string Efficiency { get; set; }

		public virtual string TaktTime { get; set; }

		public virtual string Overtime { get; set; }

		public virtual string NonProdAct { get; set; }

		public virtual int? OffLine1 { get; set; }

		public virtual int? OffLine2 { get; set; }

		public virtual int? OffLine3 { get; set; }

		public virtual int? ShiftVolPlan { get; set; }

		public virtual string IsActive { get; set; }

    

    }

	public class CreateOrEditPtsAdoLineEfficiencyDto : EntityDto<long?>
	{

		[Required]
		[StringLength(PtsAdoLineEfficiencyConsts.MaxLineLength)]
		public virtual string Line { get; set; }

		[StringLength(PtsAdoLineEfficiencyConsts.MaxShiftLength)]
		public virtual string Shift { get; set; }

		[Required]
		public virtual DateTime? WorkingDate { get; set; }

		public virtual int? VolTarget { get; set; }

		public virtual int? VolActual { get; set; }

		public virtual int? VolBalance { get; set; }

		[StringLength(PtsAdoLineEfficiencyConsts.MaxStopTimeLength)]
		public virtual string StopTime { get; set; }

		[StringLength(PtsAdoLineEfficiencyConsts.MaxEfficiencyLength)]
		public virtual string Efficiency { get; set; }

		[StringLength(PtsAdoLineEfficiencyConsts.MaxTaktTimeLength)]
		public virtual string TaktTime { get; set; }

		[StringLength(PtsAdoLineEfficiencyConsts.MaxOvertimeLength)]
		public virtual string Overtime { get; set; }

		[StringLength(PtsAdoLineEfficiencyConsts.MaxNonProdActLength)]
		public virtual string NonProdAct { get; set; }

		public virtual int? OffLine1 { get; set; }

		public virtual int? OffLine2 { get; set; }

		public virtual int? OffLine3 { get; set; }

		public virtual int? ShiftVolPlan { get; set; }

		[StringLength(PtsAdoLineEfficiencyConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetPtsAdoLineEfficiencyInput : PagedAndSortedResultRequestDto
	{

		public virtual string Line { get; set; }

		public virtual string Shift { get; set; }

		public virtual DateTime? WorkingDate { get; set; }

        public virtual DateTime? RequestDateFrom { get; set; }

        public virtual DateTime? RequestDateTo { get; set; }


    }

    public class GetPtsAdoLineEfficiencyExportInput 
    {

        public virtual string Line { get; set; }

        public virtual string Shift { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }
    }

}


