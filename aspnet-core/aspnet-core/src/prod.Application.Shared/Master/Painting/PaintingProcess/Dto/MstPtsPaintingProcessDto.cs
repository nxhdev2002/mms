using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Painting.Dto
{

	public class MstPtsPaintingProcessDto : EntityDto<long?>
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

	public class CreateOrEditMstPtsPaintingProcessDto : EntityDto<long?>
	{

		[Required]
		public virtual int? ProcessSeq { get; set; }

		[Required]
		[StringLength(MstPtsPaintingProcessConsts.MaxProcessCodeLength)]
		public virtual string ProcessCode { get; set; }

		[StringLength(MstPtsPaintingProcessConsts.MaxProcessNameLength)]
		public virtual string ProcessName { get; set; }

		[StringLength(MstPtsPaintingProcessConsts.MaxProcessDescLength)]
		public virtual string ProcessDesc { get; set; }

		[Required]
		public virtual int? ProcessGroup { get; set; }

		[StringLength(MstPtsPaintingProcessConsts.MaxGroupNameLength)]
		public virtual string GroupName { get; set; }

		[Required]
		public virtual int? ProcessSubgroup { get; set; }

		[StringLength(MstPtsPaintingProcessConsts.MaxSubgroupNameLength)]
		public virtual string SubgroupName { get; set; }

		[StringLength(MstPtsPaintingProcessConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetMstPtsPaintingProcessInput : PagedAndSortedResultRequestDto
	{

		public virtual string ProcessCode { get; set; }

		public virtual string ProcessName { get; set; }

		public virtual string ProcessDesc { get; set; }

		public virtual string GroupName { get; set; }

		public virtual string SubgroupName { get; set; }

		public virtual string IsActive { get; set; }

	}

}

