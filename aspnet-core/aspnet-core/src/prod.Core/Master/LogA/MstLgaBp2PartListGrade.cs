using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.LogA
{

	[Table("MstLgaBp2PartListGrade")]
	[Index(nameof(PartNo), Name = "IX_MstLgaBp2PartListGrade_PartName")]
    [Index(nameof(ProdLine), Name = "IX_MstLgaBp2PartListGrade_ProdLine")]
	[Index(nameof(Grade), Name = "IX_MstLgaBp2PartListGrade_PikProcess")]
    [Index(nameof(IsActive), Name = "IX_MstLgaBp2PartListGrade_IsActive")]
	public class MstLgaBp2PartListGrade : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxPartNoLength = 50;

		public const int MaxPartNameLength = 500;

		public const int MaxProdLineLength = 50;

        public const int MaxModelLength = 50;

        public const int MaxGradeLength = 50;   

        public const int MaxPikLocTypeLength = 50;

        public const int MaxPikAddressLength = 50;

        public const int MaxPikAddressDisplayLength = 50;

        public const int MaxDelLocTypeLength = 50;

        public const int MaxDelAddressLength = 50;

        public const int MaxDelAddressDisplayLength = 50;

        public const int MaxRemarkLength = 500;

        public const int MaxIsActiveLength = 1;


        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        public virtual int? PartListId { get; set; }

        [StringLength(MaxModelLength)]
        public virtual string ProdLine { get; set; }

        [StringLength(MaxGradeLength)]
        public virtual string Model { get; set; }

        [StringLength(MaxProdLineLength)]
        public virtual string Grade { get; set; }

        public virtual int? UsageQty { get; set; }

        [StringLength(MaxPikLocTypeLength)]
        public virtual string PikLocType { get; set; }

        [StringLength(MaxPikAddressLength)]
        public virtual string PikAddress { get; set; }

        [StringLength(MaxPikAddressDisplayLength)]
        public virtual string PikAddressDisplay { get; set; }

        [StringLength(MaxDelLocTypeLength)]
        public virtual string DelLocType { get; set; }

        [StringLength(MaxDelAddressLength)]
        public virtual string DelAddress { get; set; }

        [StringLength(MaxDelAddressDisplayLength)]
        public virtual string DelAddressDisplay { get; set; }
        public virtual int? Sorting { get; set; }

        [StringLength(MaxRemarkLength)]
        public virtual string Remark { get; set; }

        [StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}

