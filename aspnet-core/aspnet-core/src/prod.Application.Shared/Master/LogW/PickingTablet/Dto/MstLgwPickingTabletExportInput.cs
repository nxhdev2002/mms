using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogW.Dto
{

	public class MstLgwPickingTabletExportInput
	{

		public virtual string PickingTabletId { get; set; }

		public virtual string DeviceIp { get; set; }

		public virtual string ScanType { get; set; }
		

	}

}


