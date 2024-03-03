using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogW.Dto
{

	public class MstLgwContDevanningLTExportInput
	{

		public virtual string RenbanCode { get; set; }

		public virtual string Source { get; set; }

		public virtual int? DevLeadtime { get; set; }

		public virtual string IsActive { get; set; }

	}

}


