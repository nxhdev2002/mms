using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.LogA
{

	[Table("MstLgaLdsTrip")]
	[Index(nameof(ProdLine), Name = "IX_MstLgaLdsTrip_ProdLine")]
	[Index(nameof(IsActive), Name = "IX_MstLgaLdsTrip_IsActive")]
	public class MstLgaLdsTrip : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxProdLineLength = 50;

		public const int MaxDeliveryNameLength = 50;

        public const int MaxModelLength = 1;

        public const int MaxDollyNameLength = 250;

		public const int MaxIsActiveLength = 1;

		


		[StringLength(MaxProdLineLength)]
		public virtual string ProdLine { get; set; }

		public virtual int? DeliveryNo { get; set; }

		[StringLength(MaxDeliveryNameLength)]
		public virtual string DeliveryName { get; set; }

        [StringLength(MaxModelLength)]
        public virtual string Model { get; set; }

        public virtual int? TripNumber { get; set; }

		[StringLength(MaxDollyNameLength)]
		public virtual string DollyName { get; set; }

		public virtual int? TaktTime { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}


