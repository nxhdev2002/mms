using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogA.Dto
{

	public class MstLgaSpsRackExportInput
	{

		public virtual string Code { get; set; }

		public virtual string Address { get; set; }

		public virtual int? Sortingg { get; set; }

		public virtual string IsActive { get; set; }

	}

}


