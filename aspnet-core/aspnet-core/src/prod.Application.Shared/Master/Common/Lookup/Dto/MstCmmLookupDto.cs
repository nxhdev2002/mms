using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Common.Dto
{

	public class MstCmmLookupDto : EntityDto<long?>
	{

		public virtual string DomainCode { get; set; }

		public virtual string ItemCode { get; set; }

		public virtual string ItemValue { get; set; }

		public virtual int? ItemOrder { get; set; }

		public virtual string Description { get; set; }

		public virtual string IsUse { get; set; }

		public virtual string IsRestrict { get; set; }

	}

	public class CreateOrEditMstCmmLookupDto : EntityDto<long?>
	{

		[StringLength(MstCmmLookupConsts.MaxDomainCodeLength)]
		public virtual string DomainCode { get; set; }

		[StringLength(MstCmmLookupConsts.MaxItemCodeLength)]
		public virtual string ItemCode { get; set; }

		[StringLength(MstCmmLookupConsts.MaxItemValueLength)]
		public virtual string ItemValue { get; set; }

		[Required]
		public virtual int? ItemOrder { get; set; }

		[StringLength(MstCmmLookupConsts.MaxDescriptionLength)]
		public virtual string Description { get; set; }

		[StringLength(MstCmmLookupConsts.MaxIsUseLength)]
		public virtual string IsUse { get; set; }

		[StringLength(MstCmmLookupConsts.MaxIsRestrictLength)]
		public virtual string IsRestrict { get; set; }
	}

	public class GetMstCmmLookupInput : PagedAndSortedResultRequestDto
	{

		public virtual string DomainCode { get; set; }

		public virtual string ItemCode { get; set; }

		public virtual string ItemValue { get; set; }

		public virtual int? ItemOrder { get; set; }

		public virtual string Description { get; set; }

		public virtual string IsUse { get; set; }

		public virtual string IsRestrict { get; set; }

	}

	public class GetMstCmmLookupByDomainCodeDto : EntityDto<long?>
	{
		public virtual int? RowNo { get; set; }
		public virtual string DomainCode { get; set; }

		public virtual string ItemCode { get; set; }

		public virtual string ItemValue { get; set; }
	}

	public class GetMstCmmLookupByDomainCodeInput : PagedAndSortedResultRequestDto
	{
		public virtual string DomainCode { get; set; }

		public virtual string ItemCode { get; set; }

		public virtual string ItemValue { get; set; }
	}
    public class GetMstCmmLookupHistoryInput : PagedAndSortedResultRequestDto
    {
        public virtual long Id { get; set; }
        public virtual string TableName { get; set; }
    }
    public class GetMstCmmLookupHistoryExcelInput
    {
        public virtual long Id { get; set; }
        public virtual string TableName { get; set; }
    }
}

