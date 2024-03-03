using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Painting.Andon.Dto
{

	public class PtsAdoLineEfficiencyExportInput
	{

		public virtual string Line { get; set; }

		public virtual string Shift { get; set; }

		public virtual DateTime? WorkingDate { get; set; }

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

}


