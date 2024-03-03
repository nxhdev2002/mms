using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogW.Dto
{

	public class MstLgwLayoutSetupExportInput
	{

		public virtual string Zone { get; set; }

		public virtual int? SubScreenNo { get; set; }

		public virtual string CellType { get; set; }
	

	}

}


