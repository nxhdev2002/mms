using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogW.Dto
{

	public class MstLgwRenbanModuleExportInput
	{

		public virtual string Renban { get; set; }

		public virtual string CaseNo { get; set; }

		public virtual string SupplierNo { get; set; }
		

	}

}

