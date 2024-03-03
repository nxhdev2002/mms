using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
using prod.Master.LogA;

namespace prod.Master.LogA.Dto
{

	public class MstLgaBp2PartListDto : EntityDto<long?>
	{

		public virtual string PartName { get; set; }

		public virtual string ShortName { get; set; }

		public virtual string ProdLine { get; set; }

		public virtual string PikProcess { get; set; }

		public virtual int? PikSorting { get; set; }

		public virtual string DelProcess { get; set; }

		public virtual int? DelSorting { get; set; }

		public virtual string IsActive { get; set; }

		public virtual string Remark { get; set; }



	}

	public class CreateOrEditMstLgaBp2PartListDto : EntityDto<long?>
	{

		[StringLength(MstLgaBp2PartListConsts.MaxPartNameLength)]
		public virtual string PartName { get; set; }

		[StringLength(MstLgaBp2PartListConsts.MaxShortNameLength)]
		public virtual string ShortName { get; set; }

		[StringLength(MstLgaBp2PartListConsts.MaxProdLineLength)]
		public virtual string ProdLine { get; set; }

		[StringLength(MstLgaBp2PartListConsts.MaxPikProcessLength)]
		public virtual string PikProcess { get; set; }

		public virtual int? PikSorting { get; set; }

		[StringLength(MstLgaBp2PartListConsts.MaxDelProcessLength)]
		public virtual string DelProcess { get; set; }

		public virtual int? DelSorting { get; set; }

		[StringLength(MstLgaBp2PartListConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }

		[StringLength(MstLgaBp2PartListConsts.MaxRemarkLength)]
		public virtual string Remark { get; set; }
	}

	public class GetMstLgaBp2PartListInput : PagedAndSortedResultRequestDto
	{
		public virtual string PartName { get; set; }

		public virtual string ShortName { get; set; }

		public virtual string ProdLine { get; set; }

	}

    public class GetMstLgaBp2PartListExcelInput
    {
        public virtual string PartName { get; set; }

        public virtual string ShortName { get; set; }

        public virtual string ProdLine { get; set; }

    }

}


