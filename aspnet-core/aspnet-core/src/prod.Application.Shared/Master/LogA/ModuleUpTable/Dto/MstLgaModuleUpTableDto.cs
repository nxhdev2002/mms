using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogA.Dto
{
	public class MstLgaModuleUpTableDto : EntityDto<long?>
	{
		public virtual string Line { get; set; }

		public virtual string UpTable { get; set; }

		public virtual string DisplayOrder { get; set; }

		public virtual string IsActive { get; set; }

	}

}
