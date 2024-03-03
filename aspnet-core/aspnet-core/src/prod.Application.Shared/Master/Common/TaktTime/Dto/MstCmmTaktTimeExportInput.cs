using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Cmm.Dto
{

	public class MstCmmTaktTimeExportInput
	{

		public virtual string ProdLine { get; set; }

		public virtual string GroupCd { get; set; }

		public virtual int? TaktTimeSecond { get; set; }

		public virtual string TaktTime { get; set; }

		public virtual string IsActive { get; set; }

	}

}


