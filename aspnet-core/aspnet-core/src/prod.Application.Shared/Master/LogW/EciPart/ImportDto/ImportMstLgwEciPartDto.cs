using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Master.LogW.EciPart.ImportDto
{
    public class ImportMstLgwEciPartDto
    {

		[StringLength(MstLgwEciPartConsts.MaxGuidLength)]
		public virtual string Guid { get; set; }

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

		public string Exception { get; set; }
		public bool CanBeImported()
		{
			return string.IsNullOrEmpty(Exception);
		}
	}
}
