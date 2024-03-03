using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Welding.Dto
{

	public class MstWldPunchIndicatorDto : EntityDto<long?>
	{

		public virtual string Grade { get; set; }

		public virtual string Indicator { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditMstWldPunchIndicatorDto : EntityDto<long?>
	{

		[StringLength(MstWldPunchIndicatorConsts.MaxGradeLength)]
		public virtual string Grade { get; set; }

		[StringLength(MstWldPunchIndicatorConsts.MaxIndicatorLength)]
		public virtual string Indicator { get; set; }

		[StringLength(MstWldPunchIndicatorConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetMstWldPunchIndicatorInput : PagedAndSortedResultRequestDto
	{

		public virtual string Grade { get; set; }

		public virtual string Indicator { get; set; }

		public virtual string IsActive { get; set; }

	}

}


