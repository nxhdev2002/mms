﻿using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.LogA
{

	[Table("MstLgaBp2Ecar_T")]
	[Index(nameof(Code), Name = "IX_MstLgaBp2Ecar_T_Code")]
    [Index(nameof(EcarName), Name = "IX_MstLgaBp2Ecar_T_EcarName")]
    [Index(nameof(ProdLine), Name = "IX_MstLgaBp2Ecar_T_ProdLine")]
	[Index(nameof(IsActive), Name = "IX_MstLgaBp2Ecar_T_IsActive")]
	public class MstLgaBp2Ecar_T : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxCodeLength = 50;

		public const int MaxEcarNameLength = 50;

		public const int MaxProdLineLength = 50;

		public const int MaxEcarTypeLength = 50;

		public const int MaxIsActiveLength = 1;

        public const int MaxGuidLength = 128;


        [StringLength(MaxGuidLength)]
        public virtual string Guid { get; set; }

        [StringLength(MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MaxEcarNameLength)]
        public virtual string EcarName { get; set; }

        [StringLength(MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        [StringLength(MaxEcarTypeLength)]
        public virtual string EcarType { get; set; }

		public virtual int? Sorting { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}
