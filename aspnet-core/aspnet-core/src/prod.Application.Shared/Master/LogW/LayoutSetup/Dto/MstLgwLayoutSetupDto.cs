using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogW.Dto
{

	public class MstLgwLayoutSetupDto : EntityDto<long?>
	{

		public virtual string Zone { get; set; }

		public virtual int? SubScreenNo { get; set; }

		public virtual string ScreenArea { get; set; }

		public virtual string CellName { get; set; }

		public virtual string CellType { get; set; }

		public virtual int? NumRow { get; set; }

		public virtual string ColumnName { get; set; }

		public virtual string IsDisabled { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditMstLgwLayoutSetupDto : EntityDto<long?>
	{

		[StringLength(MstLgwLayoutSetupConsts.MaxZoneLength)]
		public virtual string Zone { get; set; }

		public virtual int? SubScreenNo { get; set; }

		[StringLength(MstLgwLayoutSetupConsts.MaxScreenAreaLength)]
		public virtual string ScreenArea { get; set; }

		[StringLength(MstLgwLayoutSetupConsts.MaxCellNameLength)]
		public virtual string CellName { get; set; }

		[StringLength(MstLgwLayoutSetupConsts.MaxCellTypeLength)]
		public virtual string CellType { get; set; }

		public virtual int? NumRow { get; set; }

		[StringLength(MstLgwLayoutSetupConsts.MaxColumnNameLength)]
		public virtual string ColumnName { get; set; }

		[StringLength(MstLgwLayoutSetupConsts.MaxIsDisabledLength)]
		public virtual string IsDisabled { get; set; }

		[StringLength(MstLgwLayoutSetupConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetMstLgwLayoutSetupInput : PagedAndSortedResultRequestDto
	{

		public virtual string Zone { get; set; }

		public virtual int? SubScreenNo { get; set; }

		public virtual string ScreenArea { get; set; }

		public virtual string CellName { get; set; }

		public virtual string CellType { get; set; }

		public virtual int? NumRow { get; set; }

		public virtual string ColumnName { get; set; }

		public virtual string IsDisabled { get; set; }

		public virtual string IsActive { get; set; }

	}

}


