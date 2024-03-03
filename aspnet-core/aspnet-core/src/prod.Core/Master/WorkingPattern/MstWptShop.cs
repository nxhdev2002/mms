using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.WorkingPattern
{

	[Table("MstWptShop")]
	[Index(nameof(ShopName), Name = "IX_MstWptShop_ShopName")]
	public class MstWptShop : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxShopNameLength = 40;

		public const int MaxDescriptionLength = 250;

		public const int MaxIsActiveLength = 1;

		[Required]
		[StringLength(MaxShopNameLength)]
		public virtual string ShopName { get; set; }

		[StringLength(MaxDescriptionLength)]
		public virtual string Description { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}

