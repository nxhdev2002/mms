using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.WorkingPattern.Dto
{

	public class MstWptShopDto : EntityDto<long?>
	{

		public virtual string ShopName { get; set; }

		public virtual string Description { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditMstWptShopDto : EntityDto<long?>
	{

		[Required]
		[StringLength(MstWptShopConsts.MaxShopNameLength)]
		public virtual string ShopName { get; set; }

		[StringLength(MstWptShopConsts.MaxDescriptionLength)]
		public virtual string Description { get; set; }

		[StringLength(MstWptShopConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetMstWptShopInput : PagedAndSortedResultRequestDto
	{

		public virtual string ShopName { get; set; }

		public virtual string Description { get; set; }

		public virtual string IsActive { get; set; }

	}

}

