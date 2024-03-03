using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Common.Dto
{

	public class MstCmmProductGroupDto : EntityDto<long?>
	{

		public virtual string Code { get; set; }

		public virtual string Description { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditMstCmmGroupTypeDto : EntityDto<long?>
	{

		[StringLength(MstCmmProductTypeConsts.MaxCodeLength)]
		public virtual string Code { get; set; }

		[StringLength(MstCmmProductTypeConsts.MaxNameLength)]
		public virtual string Description { get; set; }

		[StringLength(MstCmmProductTypeConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetMstCmmProductGroupInput : PagedAndSortedResultRequestDto
	{

		public virtual string Code { get; set; }

		public virtual string Description { get; set; }


	}

}

