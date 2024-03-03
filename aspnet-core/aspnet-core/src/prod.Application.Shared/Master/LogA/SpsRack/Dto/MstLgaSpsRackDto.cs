using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogA.Dto
{

	public class MstLgaSpsRackDto : EntityDto<long?>
	{

		public virtual string Code { get; set; }

		public virtual string Address { get; set; }

		public virtual int? Ordering { get; set; } 

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditMstLgaSpsRackDto : EntityDto<long?>
	{

		[StringLength(MstLgaSpsRackConsts.MaxCodeLength)]
		public virtual string Code { get; set; }

		[StringLength(MstLgaSpsRackConsts.MaxAddressLength)]
		public virtual string Address { get; set; }

		public virtual int? Ordering { get; set; }

		[StringLength(MstLgaSpsRackConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetMstLgaSpsRackInput : PagedAndSortedResultRequestDto
	{
		public virtual string Code { get; set; }

		public virtual string Address { get; set; }

	}

}

