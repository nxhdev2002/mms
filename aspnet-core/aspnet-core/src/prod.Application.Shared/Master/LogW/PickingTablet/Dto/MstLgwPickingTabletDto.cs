using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogW.Dto
{

	public class MstLgwPickingTabletDto : EntityDto<long?>
	{

		public virtual string PickingTabletId { get; set; }

		public virtual string DeviceIp { get; set; }

		public virtual string ScanType { get; set; }

		public virtual string ScanName { get; set; }

		public virtual string CurrentAction { get; set; }

		public virtual string LotNo { get; set; }

		public virtual string UpTable { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditMstLgwPickingTabletDto : EntityDto<long?>
	{

		[StringLength(MstLgwPickingTabletConsts.MaxPickingTabletIdLength)]
		public virtual string PickingTabletId { get; set; }

		[StringLength(MstLgwPickingTabletConsts.MaxDeviceIpLength)]
		public virtual string DeviceIp { get; set; }

		[StringLength(MstLgwPickingTabletConsts.MaxScanTypeLength)]
		public virtual string ScanType { get; set; }

		[StringLength(MstLgwPickingTabletConsts.MaxScanNameLength)]
		public virtual string ScanName { get; set; }

		[StringLength(MstLgwPickingTabletConsts.MaxCurrentActionLength)]
		public virtual string CurrentAction { get; set; }

		[StringLength(MstLgwPickingTabletConsts.MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		[StringLength(MstLgwPickingTabletConsts.MaxUpTableLength)]
		public virtual string UpTable { get; set; }

		[StringLength(MstLgwPickingTabletConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetMstLgwPickingTabletInput : PagedAndSortedResultRequestDto
	{

		public virtual string PickingTabletId { get; set; }

		public virtual string DeviceIp { get; set; }

		public virtual string ScanType { get; set; }

		public virtual string ScanName { get; set; }

		public virtual string CurrentAction { get; set; }

		public virtual string LotNo { get; set; }

		public virtual string UpTable { get; set; }

		public virtual string IsActive { get; set; }

	}

}


