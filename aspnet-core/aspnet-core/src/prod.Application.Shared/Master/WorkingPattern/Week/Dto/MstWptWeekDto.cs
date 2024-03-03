using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.WorkingPattern.Dto
{

	public class MstWptWeekDto : EntityDto<long?>
	{

		public virtual int? WorkingYear { get; set; }

		public virtual int? WeekNumber { get; set; }

		public virtual int? WorkingDays { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditMstWptWeekDto : EntityDto<long?>
	{

		public virtual int? WorkingYear { get; set; }

		public virtual int? WeekNumber { get; set; }

		public virtual int? WorkingDays { get; set; }

		[StringLength(MstWptWeekConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetMstWptWeekInput : PagedAndSortedResultRequestDto
	{
		public virtual int? WorkingYear { get; set; }

	}

}


