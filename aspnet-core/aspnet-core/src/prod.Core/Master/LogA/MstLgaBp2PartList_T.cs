using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.LogA
{

	[Table("MstLgaBp2PartList_T")]
	[Index(nameof(PartName), Name = "IX_MstLgaBp2PartList_T_PartName")]
    [Index(nameof(ShortName), Name = "IX_MstLgaBp2PartList_T_ShortName")]
    [Index(nameof(ProdLine), Name = "IX_MstLgaBp2PartList_T_ProdLine")]
    [Index(nameof(IsActive), Name = "IX_MstLgaBp2PartList_T_IsActive")]
    [Index(nameof(Remark), Name = "IX_MstLgaBp2PartList_Remark")]
    public class MstLgaBp2PartList_T : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxPartNameLength = 500;

		public const int MaxShortNameLength = 200;

		public const int MaxProdLineLength = 50;

		public const int MaxPikProcessLength = 50;

        public const int MaxDelProcessLength = 50;

        public const int MaxIsActiveLength = 1;

        public const int MaxGuidLength = 128;

        public const int MaxRemarkLength = 500;


        [StringLength(MaxGuidLength)]
        public virtual string Guid { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(MaxShortNameLength)]
        public virtual string ShortName { get; set; }

        [StringLength(MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        [StringLength(MaxPikProcessLength)]
        public virtual string PikProcess { get; set; }

		public virtual int? PikSorting { get; set; }

        [StringLength(MaxDelProcessLength)]
        public virtual string DelProcess { get; set; }

        public virtual int? DelSorting { get; set; }

        [StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }

        [StringLength(MaxRemarkLength)]
        public virtual string Remark { get; set; }
    }

}

