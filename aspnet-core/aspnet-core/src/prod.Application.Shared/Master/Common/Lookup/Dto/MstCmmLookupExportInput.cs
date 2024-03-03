using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Common.Dto
{
	public class MstCmmLookupExportInput
	{
		public virtual string DomainCode { get; set; }

		public virtual string ItemCode { get; set; }

		public virtual string ItemValue { get; set; }

		public virtual int? ItemOrder { get; set; }

		public virtual string Description { get; set; }

		public virtual string IsUse { get; set; }

		public virtual string IsRestrict { get; set; }
	}
}

