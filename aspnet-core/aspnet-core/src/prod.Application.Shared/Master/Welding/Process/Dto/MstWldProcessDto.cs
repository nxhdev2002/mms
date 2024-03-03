using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Welding.Dto
{

	public class MstWldProcessDto : EntityDto<long?>
	{

		public virtual int? ProcessSeq { get; set; }

		public virtual string ProcessCode { get; set; }

		public virtual string ProcessName { get; set; }

		public virtual string ProcessDesc { get; set; }

		public virtual int? ProcessGroup { get; set; }

		public virtual string GroupName { get; set; }

		public virtual int? ProcessSubgroup { get; set; }

		public virtual string SubgroupName { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditMstWldProcessDto : EntityDto<long?>
	{

		[Required]
		public virtual int? ProcessSeq { get; set; }

		[Required]
		[StringLength(MstWldProcessConsts.MaxProcessCodeLength)]
		public virtual string ProcessCode { get; set; }

		[StringLength(MstWldProcessConsts.MaxProcessNameLength)]
		public virtual string ProcessName { get; set; }

		[StringLength(MstWldProcessConsts.MaxProcessDescLength)]
		public virtual string ProcessDesc { get; set; }

		[Required]
		public virtual int? ProcessGroup { get; set; }

		[StringLength(MstWldProcessConsts.MaxGroupNameLength)]
		public virtual string GroupName { get; set; }

		public virtual int? ProcessSubgroup { get; set; }

		[StringLength(MstWldProcessConsts.MaxSubgroupNameLength)]
		public virtual string SubgroupName { get; set; }

		[StringLength(MstWldProcessConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetMstWldProcessInput : PagedAndSortedResultRequestDto
	{

		public virtual int? ProcessSeq { get; set; }

		public virtual string ProcessCode { get; set; }

		public virtual string ProcessName { get; set; }

		public virtual string ProcessDesc { get; set; }

		public virtual int? ProcessGroup { get; set; }

		public virtual string GroupName { get; set; }

		public virtual int? ProcessSubgroup { get; set; }

		public virtual string SubgroupName { get; set; }

		public virtual string IsActive { get; set; }

	}

}


