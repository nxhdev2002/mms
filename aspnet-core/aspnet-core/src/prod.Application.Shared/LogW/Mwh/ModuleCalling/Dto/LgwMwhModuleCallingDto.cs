using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.LogW.Mwh.Dto
{

	public class LgwMwhModuleCallingDto : EntityDto<long?>
	{

		public virtual string Renban { get; set; }

		public virtual string CaseNo { get; set; }

		public virtual string SupplierNo { get; set; }

		public virtual DateTime? CallTime { get; set; }

		public virtual string CalledModuleNo { get; set; }

		public virtual string CaseType { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditLgwMwhModuleCallingDto : EntityDto<long?>
	{

		[StringLength(LgwMwhModuleCallingConsts.MaxRenbanLength)]
		public virtual string Renban { get; set; }

		[StringLength(LgwMwhModuleCallingConsts.MaxCaseNoLength)]
		public virtual string CaseNo { get; set; }

		[StringLength(LgwMwhModuleCallingConsts.MaxSupplierNoLength)]
		public virtual string SupplierNo { get; set; }

		public virtual DateTime? CallTime { get; set; }

		[StringLength(LgwMwhModuleCallingConsts.MaxCalledModuleNoLength)]
		public virtual string CalledModuleNo { get; set; }

		[StringLength(LgwMwhModuleCallingConsts.MaxCaseTypeLength)]
		public virtual string CaseType { get; set; }

		[StringLength(LgwMwhModuleCallingConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetLgwMwhModuleCallingInput : PagedAndSortedResultRequestDto
	{

		public virtual string Renban { get; set; }

		public virtual string CaseNo { get; set; }

		public virtual string SupplierNo { get; set; }

		public virtual DateTime? CallTime { get; set; }

		public virtual string CalledModuleNo { get; set; }

		public virtual string CaseType { get; set; }

		public virtual string IsActive { get; set; }

	}

}


