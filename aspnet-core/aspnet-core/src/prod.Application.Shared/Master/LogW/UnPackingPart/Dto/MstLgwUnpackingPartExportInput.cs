using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogW.Dto
{

	public class MstLgwUnpackingPartExportInput
	{

		public virtual string Cfc { get; set; }

		public virtual string Model { get; set; }

		public virtual string BackNo { get; set; }

		public virtual string PartNo { get; set; }

		public virtual string PartName { get; set; }

		public virtual string ModuleCode { get; set; }

		public virtual string IsActive { get; set; }

	}

}


