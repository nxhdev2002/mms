using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.WorkingPattern.Dto
{

	public class MstWptWorkingTypeDto : EntityDto<long?>
	{

		public virtual int? WorkingType { get; set; }

		public virtual string Description { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditMstWptWorkingTypeDto : EntityDto<long?>
	{

		public virtual int? WorkingType { get; set; }

		[StringLength(MstWptWorkingTypeConsts.MaxDescriptionLength)]
		public virtual string Description { get; set; }

		[StringLength(MstWptWorkingTypeConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetMstWptWorkingTypeInput : PagedAndSortedResultRequestDto
	{

		public virtual string Description { get; set; }

	}

}


