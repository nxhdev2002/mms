using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogW.Dto
{

	public class MstLgwContDevanningLTDto : EntityDto<long?>
	{

		public virtual string RenbanCode { get; set; }

		public virtual string Source { get; set; }

		public virtual int? DevLeadtime { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditMstLgwContDevanningLTDto : EntityDto<long?>
	{

		[StringLength(MstLgwContDevanningLTConsts.MaxRenbanCodeLength)]
		public virtual string RenbanCode { get; set; }

		[StringLength(MstLgwContDevanningLTConsts.MaxSourceLength)]
		public virtual string Source { get; set; }

		public virtual int? DevLeadtime { get; set; }

		[StringLength(MstLgwContDevanningLTConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetMstLgwContDevanningLTInput : PagedAndSortedResultRequestDto
	{

		public virtual string RenbanCode { get; set; }

		public virtual string Source { get; set; }

		public virtual int? DevLeadtime { get; set; }

		public virtual string IsActive { get; set; }

	}

}


