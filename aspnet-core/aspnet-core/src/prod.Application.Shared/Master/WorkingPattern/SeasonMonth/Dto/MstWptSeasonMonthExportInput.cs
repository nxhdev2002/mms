using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.WorkingPattern.Dto
{

	public class MstWptSeasonMonthExportInput
	{

		public virtual DateTime? SeasonMonth { get; set; }

		public virtual string SeasonType { get; set; }

		public virtual string IsActive { get; set; }

	}

}


