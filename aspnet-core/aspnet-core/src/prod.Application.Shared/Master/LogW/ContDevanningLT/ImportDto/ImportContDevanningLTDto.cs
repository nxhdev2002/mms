using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogW
{

	public class ImportContDevanningLTDto : EntityDto<long?>
	{
		[StringLength(MstLgwContDevanningLTConsts.MaxGuidLength)]
		public virtual string Guid { get; set; }

		[StringLength(MstLgwContDevanningLTConsts.MaxRenbanCodeLength)]
		public virtual string RenbanCode { get; set; }
		
		[StringLength(MstLgwContDevanningLTConsts.MaxSourceLength)]
		public virtual string Source { get; set; }

		public virtual int? DevLeadtime { get; set; }

		[StringLength(MstLgwContDevanningLTConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
		public string Exception { get; set; }
		public bool CanBeImported()
		{
			return string.IsNullOrEmpty(Exception);
		}
	}




}


