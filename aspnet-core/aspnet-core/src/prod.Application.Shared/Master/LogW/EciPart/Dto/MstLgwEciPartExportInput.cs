using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogW.Dto
{

	public class MstLgwEciPartExportInput
	{
		public virtual string ModuleNo { get; set; }

		public virtual string PartNo { get; set; }

		public virtual string SupplierNo { get; set; }

		public virtual string ModuleNoEci { get; set; }

		public virtual string PartNoEci { get; set; }
		

	}

}


