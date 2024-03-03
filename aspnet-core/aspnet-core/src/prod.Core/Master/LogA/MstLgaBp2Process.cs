using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.LogA
{

	[Table("MstLgaBp2Process")]
	[Index(nameof(Code), Name = "IX_MstLgaBp2Process_Code")]
    [Index(nameof(ProdLine), Name = "IX_MstLgaBp2Process_ProdLine")]
	[Index(nameof(IsActive), Name = "IX_MstLgaBp2Process_IsActive")]
	public class MstLgaBp2Process : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxCodeLength = 50;

		public const int MaxProcessNameLength = 50;

		public const int MaxProdLineLength = 50;

        public const int MaxProcessTypeLength = 50;

        public const int MaxIsActiveLength = 1;


        [StringLength(MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MaxProcessNameLength)]
        public virtual string ProcessName { get; set; }

        [StringLength(MaxProdLineLength)]
        public virtual string ProdLine { get; set; }
        public virtual int? LeadTime { get; set; }
		public virtual int? Sorting { get; set; }

        [StringLength(MaxProcessTypeLength)]
        public virtual string ProcessType { get; set; }

        [StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}

