using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
using prod.Master.LogW.EciPart;

namespace prod.Master.LogW.Dto
{

    public class MstLgwEciPartDto : EntityDto<long?>
	{

        public virtual string ModuleNo { get; set; }

		public virtual string PartNo { get; set; }

		public virtual string SupplierNo { get; set; }

		public virtual string ModuleNoEci { get; set; }

		public virtual string PartNoEci { get; set; }

		public virtual string SupplierNoEci { get; set; }

		public virtual string StartEciSeq { get; set; }

		public virtual string StartEciRenban { get; set; }

		public virtual string StartEciModule { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditMstLgwEciPartDto : EntityDto<long?>
	{

		[StringLength(MstLgwEciPartConsts.MaxModuleNoLength)]
		public virtual string ModuleNo { get; set; }

		[StringLength(MstLgwEciPartConsts.MaxPartNoLength)]
		public virtual string PartNo { get; set; }

		[StringLength(MstLgwEciPartConsts.MaxSupplierNoLength)]
		public virtual string SupplierNo { get; set; }

		[StringLength(MstLgwEciPartConsts.MaxModuleNoEciLength)]
		public virtual string ModuleNoEci { get; set; }

		[StringLength(MstLgwEciPartConsts.MaxPartNoEciLength)]
		public virtual string PartNoEci { get; set; }

		[StringLength(MstLgwEciPartConsts.MaxSupplierNoEciLength)]
		public virtual string SupplierNoEci { get; set; }

		[StringLength(MstLgwEciPartConsts.MaxStartEciSeqLength)]
		public virtual string StartEciSeq { get; set; }

		[StringLength(MstLgwEciPartConsts.MaxStartEciRenbanLength)]
		public virtual string StartEciRenban { get; set; }

		[StringLength(MstLgwEciPartConsts.MaxStartEciModuleLength)]
		public virtual string StartEciModule { get; set; }

		[StringLength(MstLgwEciPartConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetMstLgwEciPartInput : PagedAndSortedResultRequestDto
	{

		public virtual string ModuleNo { get; set; }

		public virtual string PartNo { get; set; }

		public virtual string SupplierNo { get; set; }

		public virtual string ModuleNoEci { get; set; }

		public virtual string PartNoEci { get; set; }

		public virtual string SupplierNoEci { get; set; }

		public virtual string StartEciSeq { get; set; }

		public virtual string StartEciRenban { get; set; }

		public virtual string StartEciModule { get; set; }

		public virtual string IsActive { get; set; }

	}

}


