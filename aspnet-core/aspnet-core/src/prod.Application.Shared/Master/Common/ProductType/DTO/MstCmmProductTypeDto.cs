using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Common.Dto
{

	public class MstCmmProductTypeDto : EntityDto<long?>
	{

		public virtual string Code { get; set; }

		public virtual string Name { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditMstCmmProductTypeDto : EntityDto<long?>
	{

		[StringLength(MstCmmProductTypeConsts.MaxCodeLength)]
		public virtual string Code { get; set; }

		[StringLength(MstCmmProductTypeConsts.MaxNameLength)]
		public virtual string Name { get; set; }

		[StringLength(MstCmmProductTypeConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetMstCmmProductTypeInput : PagedAndSortedResultRequestDto
	{

		public virtual string Code { get; set; }

		public virtual string Name { get; set; }

		public virtual string IsActive { get; set; }

	}

}

