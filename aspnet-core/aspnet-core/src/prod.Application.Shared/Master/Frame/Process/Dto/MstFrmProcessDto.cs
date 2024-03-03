using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Frame.Dto
{

	public class MstFrmProcessDto : EntityDto<long?>
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

	public class CreateOrEditMstFrmProcessDto : EntityDto<long?>
	{

		[Required]
		public virtual int? ProcessSeq { get; set; }

		[Required]
		[StringLength(MstFrmProcessConsts.MaxProcessCodeLength)]
		public virtual string ProcessCode { get; set; }

		[StringLength(MstFrmProcessConsts.MaxProcessNameLength)]
		public virtual string ProcessName { get; set; }

		[StringLength(MstFrmProcessConsts.MaxProcessDescLength)]
		public virtual string ProcessDesc { get; set; }

		[Required]
		public virtual int? ProcessGroup { get; set; }

		[StringLength(MstFrmProcessConsts.MaxGroupNameLength)]
		public virtual string GroupName { get; set; }

		public virtual int? ProcessSubgroup { get; set; }

		[StringLength(MstFrmProcessConsts.MaxSubgroupNameLength)]
		public virtual string SubgroupName { get; set; }

		[StringLength(MstFrmProcessConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetMstFrmProcessInput : PagedAndSortedResultRequestDto
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


