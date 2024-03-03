using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Painting.Andon.Dto
{

	public class PtsAdoLineEfficiencyDetailsExportInput
	{

		public virtual string Line { get; set; }

		public virtual int? VolActual { get; set; }

		public virtual int? LineStopTime { get; set; }

		public virtual decimal LineEfficiency { get; set; }

		public virtual DateTime? WorkingDate { get; set; }

		public virtual string Shift { get; set; }

		public virtual string Status { get; set; }

	}

}


