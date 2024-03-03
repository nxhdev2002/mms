using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Painting.Andon.Dto
{

	public class GetDetailsOutput : EntityDto<long?>
	{
		public virtual string Line { get; set; }
		public virtual string Shift { get; set; }

		public virtual string WorkingDate { get; set; }

		public virtual int? VolTarget { get; set; }

		public virtual int? VolActual { get; set; }

		public virtual double? VolBalance { get; set; }

		public virtual int? StopTime { get; set; }

		public virtual decimal? Efficiency { get; set; }

		public virtual decimal? TaktTime { get; set; }

		public virtual int? Overtime { get; set; }

		public virtual int? NonProdAct { get; set; }

		public virtual int? ShiftVolPlan { get; set; }

		public virtual int? OffLine1 { get; set; }

		public virtual int? OffLine2 { get; set; }

		public virtual int? OffLine3 { get; set; }

		public virtual int? OffLine4 { get; set; }

		public virtual int? OffLine5 { get; set; }

		public virtual int? OffLine6 { get; set; }

		public virtual int? OffLine7 { get; set; }

		public virtual int? OffLine8 { get; set; }

	}


}


