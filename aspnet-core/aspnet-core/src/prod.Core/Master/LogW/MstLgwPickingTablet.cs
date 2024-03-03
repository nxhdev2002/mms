using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.LogW
{

	[Table("MstLgwPickingTablet")]
	[Index(nameof(PickingTabletId), Name = "IX_MstLgwPickingTablet_PickingTabletId")]
	[Index(nameof(IsActive), Name = "IX_MstLgwPickingTablet_IsActive")]
	public class MstLgwPickingTablet : FullAuditedEntity<long>, IEntity<long>
	{

		public const int MaxPickingTabletIdLength = 50;

		public const int MaxDeviceIpLength = 50;

		public const int MaxScanTypeLength = 50;

		public const int MaxScanNameLength = 50;

		public const int MaxCurrentActionLength = 50;

		public const int MaxLotNoLength = 50;

		public const int MaxUpTableLength = 20;

		public const int MaxIsActiveLength = 1;

		[StringLength(MaxPickingTabletIdLength)]
		public virtual string PickingTabletId { get; set; }

		[StringLength(MaxDeviceIpLength)]
		public virtual string DeviceIp { get; set; }

		[StringLength(MaxScanTypeLength)]
		public virtual string ScanType { get; set; }

		[StringLength(MaxScanNameLength)]
		public virtual string ScanName { get; set; }

		[StringLength(MaxCurrentActionLength)]
		public virtual string CurrentAction { get; set; }

		[StringLength(MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		[StringLength(MaxUpTableLength)]
		public virtual string UpTable { get; set; }

		[StringLength(MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

}

