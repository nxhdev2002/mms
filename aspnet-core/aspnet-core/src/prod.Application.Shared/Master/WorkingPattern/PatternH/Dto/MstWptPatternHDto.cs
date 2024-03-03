using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.WorkingPattern.Dto
{

	public class MstWptPatternHDto : EntityDto<long?>
	{

		public virtual int? Type { get; set; }

		public virtual DateTime? StartDate { get; set; }

		public virtual DateTime? EndDate { get; set; }

		public virtual string Description { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditMstWptPatternHDto : EntityDto<long?>
	{

		[Required]
		public virtual int? Type { get; set; }

		public virtual DateTime? StartDate { get; set; }

		public virtual DateTime? EndDate { get; set; }

		[StringLength(MstWptPatternHConsts.MaxDescriptionLength)]
		public virtual string Description { get; set; }

		[StringLength(MstWptPatternHConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetMstWptPatternHInput : PagedAndSortedResultRequestDto
	{

		public virtual int? Type { get; set; }

		public virtual DateTime? StartDate { get; set; }

		public virtual DateTime? EndDate { get; set; }

		public virtual string Description { get; set; }

		public virtual string IsActive { get; set; }

	}

}


