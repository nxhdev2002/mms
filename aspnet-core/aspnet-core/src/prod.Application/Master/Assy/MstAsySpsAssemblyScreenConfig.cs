using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Assy
{

	[Table("MstAsySpsAssemblyScreenConfig")]
	[Index(nameof(ScreenCode), Name = "IX_MstAsySpsAssemblyScreenConfig_ScreenCode")]
	[Index(nameof(ScreenName), Name = "IX_MstAsySpsAssemblyScreenConfig_ScreenName")]

    public class MstAsySpsAssemblyScreenConfig : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxScreenCodeLength = 50;

		public const int MaxScreenNameLength = 50;

		public const int MaxProdLineLength = 50;

		public const int MaxProcessGroupLength = 50;

		public const int MaxProcessListLength = 200;

        public const int MaxHeaderListLength = 200;

        public const int MaxHeaderColor1Length = 50;

        public const int MaxHeaderColor2Length = 50;

        public const int MaxIsShowHeaderLength = 1;

        public const int MaxLeftTitleLength = 200;

        public const int MaxIsShowLeftTitleLength = 1;

        public const int MaxIsShowModelLength = 1;

        public const int MaxIsShowGradeLength = 1;

        public const int MaxIsShowSequenceLength = 1;

        public const int MaxIsShowBodyLength = 1;

        public const int MaxIsShowLotNoLength = 1;

        public const int MaxIsShowColorLength = 1;

        public const int MaxIsActiveLength = 1;


		[StringLength(MaxScreenCodeLength)]
		public virtual string ScreenCode { get; set; }

		[StringLength(MaxScreenNameLength)]
		public virtual string ScreenName { get; set; }

        [StringLength(MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

		[StringLength(MaxProcessGroupLength)]
		public virtual string ProcessGroup { get; set; }

        [StringLength(MaxProcessListLength)]
		public virtual string ProcessList { get; set; }

		[StringLength(MaxHeaderListLength)]
		public virtual string HeaderList { get; set; }

        [StringLength(MaxHeaderColor1Length)]
        public virtual string HeaderColor1 { get; set; }

        [StringLength(MaxHeaderColor2Length)]
        public virtual string HeaderColor2 { get; set; }

        [StringLength(MaxIsShowHeaderLength)]
        public virtual string IsShowHeader { get; set; }

        [StringLength(MaxLeftTitleLength)]
        public virtual string LeftTitle { get; set; }

        [StringLength(MaxIsShowLeftTitleLength)]
        public virtual string IsShowLeftTitle { get; set; }

        [StringLength(MaxIsShowModelLength)]
        public virtual string IsShowModel { get; set; }

        [StringLength(MaxIsShowGradeLength)]
        public virtual string IsShowGrade { get; set; }

        [StringLength(MaxIsShowSequenceLength)]
        public virtual string IsShowSequence { get; set; }

        [StringLength(MaxIsShowBodyLength)]
        public virtual string IsShowBody { get; set; }

        [StringLength(MaxIsShowLotNoLength)]
        public virtual string IsShowLotNo { get; set; }

        [StringLength(MaxIsShowColorLength)]
        public virtual string IsShowColor { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}
