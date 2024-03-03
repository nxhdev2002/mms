using Abp.Application.Services.Dto;
using prod.Master.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Master.Inventory.Dto
{
	public class MstInvDevanningCaseTypeDto : EntityDto<long?>
	{
		public virtual string Source { get; set; }
		public virtual string CaseNo { get; set; }

		public virtual string ShoptypeCode { get; set; }

		public virtual string Type { get; set; }

		public virtual string CarFamilyCode { get; set; }

		public virtual string IsActive { get; set; }
	}

	public class CreateOrEditMstInvDevanningCaseTypeDto : EntityDto<long?>
	{

		[StringLength(MstInvDevanningCaseTypeConsts.MaxSourceLength)]
		public virtual string Source { get; set; }

		[StringLength(MstInvDevanningCaseTypeConsts.MaxCaseNoLength)]
		public virtual string CaseNo { get; set; }

		[StringLength(MstInvDevanningCaseTypeConsts.MaxShoptypeCodeLength)]
		public virtual string ShoptypeCode { get; set; }

		[StringLength(MstInvDevanningCaseTypeConsts.MaxTypeLength)]
		public virtual string Type { get; set; }

		[StringLength(MstInvDevanningCaseTypeConsts.MaxCarfamilyCodeLength)]
		public virtual string CarFamilyCode { get; set; }

		[StringLength(MstInvDevanningCaseTypeConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetMstInvDevanningCaseTypeInput : PagedAndSortedResultRequestDto
	{
		public virtual string Source { get; set; }
		public virtual string CaseNo { get; set; }
		public virtual string CarFamilyCode { get; set; }

		public virtual string ShoptypeCode { get; set; }

	}

    public class GetMstInvDevanningCaseTypeHistoryInput : PagedAndSortedResultRequestDto
    {
        public virtual long Id { get; set; }
        public virtual string TableName { get; set; }
    }

    public class GetMstInvDevanningCaseTypeHistoryExcelInput
    {
        public virtual long Id { get; set; }

        public virtual string TableName { get; set; }
    }
}
