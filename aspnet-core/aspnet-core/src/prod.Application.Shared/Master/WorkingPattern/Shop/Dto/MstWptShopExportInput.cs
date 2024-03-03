using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.WorkingPattern.Dto
{
	public class MstWptShopExportInput
	{
		public virtual string ShopName { get; set; }

		public virtual string Description { get; set; }

		public virtual string IsActive { get; set; }

	}
}

