using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogW.Dto
{

	public class MstLgwRobbingLaneDto : EntityDto<long?>
	{

		public virtual string LaneNo { get; set; }

		public virtual string LaneName { get; set; }

		public virtual string ContNo { get; set; }

		public virtual string Renban { get; set; }

		public virtual string SupplierNo { get; set; }

		public virtual string ShowOnly { get; set; }

		public virtual string IsDisabled { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditMstLgwRobbingLaneDto : EntityDto<long?>
	{

		[StringLength(MstLgwRobbingLaneConsts.MaxLaneNoLength)]
		public virtual string LaneNo { get; set; }

		[StringLength(MstLgwRobbingLaneConsts.MaxLaneNameLength)]
		public virtual string LaneName { get; set; }

		[StringLength(MstLgwRobbingLaneConsts.MaxContNoLength)]
		public virtual string ContNo { get; set; }

		[StringLength(MstLgwRobbingLaneConsts.MaxRenbanLength)]
		public virtual string Renban { get; set; }

		[StringLength(MstLgwRobbingLaneConsts.MaxSupplierNoLength)]
		public virtual string SupplierNo { get; set; }

		[StringLength(MstLgwRobbingLaneConsts.MaxShowOnlyLength)]
		public virtual string ShowOnly { get; set; }

		[StringLength(MstLgwRobbingLaneConsts.MaxIsDisabledLength)]
		public virtual string IsDisabled { get; set; }

		[StringLength(MstLgwRobbingLaneConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetMstLgwRobbingLaneInput : PagedAndSortedResultRequestDto
	{

		public virtual string LaneNo { get; set; }

		public virtual string LaneName { get; set; }

		public virtual string ContNo { get; set; }

		public virtual string Renban { get; set; }

		public virtual string SupplierNo { get; set; }

		public virtual string ShowOnly { get; set; }

		public virtual string IsDisabled { get; set; }

		public virtual string IsActive { get; set; }

	}

}


