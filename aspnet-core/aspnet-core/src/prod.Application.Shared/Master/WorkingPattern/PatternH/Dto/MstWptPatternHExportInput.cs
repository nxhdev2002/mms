using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.WorkingPattern.Dto
{

	public class MstWptPatternHExportInput
	{

		public virtual int? Type { get; set; }

		public virtual DateTime? StartDate { get; set; }

		public virtual string IsActive { get; set; }

	}

}


