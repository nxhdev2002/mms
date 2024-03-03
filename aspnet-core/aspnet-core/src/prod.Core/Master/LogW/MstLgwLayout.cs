using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.LogW
{

	[Table("MstLgwLayout")]
	[Index(nameof(ZoneCd), Name = "IX_MstLgwLayout_ZoneCd")]
	[Index(nameof(AreaCd), Name = "IX_MstLgwLayout_AreaCd")]
	[Index(nameof(LocationCd), Name = "IX_MstLgwLayout_LocationCd")]
	[Index(nameof(IsDisabled), Name = "IX_MstLgwLayout_IsDisabled")]
	[Index(nameof(IsActive), Name = "IX_MstLgwLayout_IsActive")]
	public class MstLgwLayout : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxZoneCdLength = 50;

		public const int MaxAreaCdLength = 50;

		public const int MaxRowNameLength = 50;

		public const int MaxColumnNameLength = 50;

		public const int MaxLocationCdLength = 50;

		public const int MaxLocationNameLength = 50;

		public const int MaxLocationTitleLength = 50;

		public const int MaxIsDisabledLength = 1;

		public const int MaxIsActiveLength = 1;

		[StringLength(MaxZoneCdLength)]
		public virtual string ZoneCd { get; set; }

		[StringLength(MaxAreaCdLength)]
		public virtual string AreaCd { get; set; }

		public virtual int? RowId { get; set; }

		public virtual int? ColumnId { get; set; }

		[StringLength(MaxRowNameLength)]
		public virtual string RowName { get; set; }

		[StringLength(MaxColumnNameLength)]
		public virtual string ColumnName { get; set; }

		[StringLength(MaxLocationCdLength)]
		public virtual string LocationCd { get; set; }

		[StringLength(MaxLocationNameLength)]
		public virtual string LocationName { get; set; }

		[StringLength(MaxLocationTitleLength)]
		public virtual string LocationTitle { get; set; }

		[StringLength(MaxIsDisabledLength)]
		public virtual string IsDisabled { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}

