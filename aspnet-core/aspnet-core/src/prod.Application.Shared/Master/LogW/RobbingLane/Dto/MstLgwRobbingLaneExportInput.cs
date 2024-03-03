using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogW.Dto
{

	public class MstLgwRobbingLaneExportInput
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


