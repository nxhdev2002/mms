using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.WorkingPattern.Dto
{

	public class MstWptSeasonMonthDto : EntityDto<long?>
	{

		public virtual DateTime? SeasonMonth { get; set; }

		public virtual string SeasonType { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditMstWptSeasonMonthDto : EntityDto<long?>
	{

		public virtual DateTime? SeasonMonth { get; set; }

		[StringLength(MstWptSeasonMonthConsts.MaxSeasonTypeLength)]
		public virtual string SeasonType { get; set; }

		[StringLength(MstWptSeasonMonthConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetMstWptSeasonMonthInput : PagedAndSortedResultRequestDto
	{

		public virtual DateTime? SeasonMonth { get; set; }

		public virtual string SeasonType { get; set; }

		public virtual string IsActive { get; set; }

	}

}


