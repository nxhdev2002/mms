using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Master.LogW.UnPackingPart.ImportDto
{
    public class ImportMstLgwUnpackingPartDto
    {
		[StringLength(MstLgwUnpackingPartConsts.MaxGuidLength)]
		public virtual string Guid { get; set; }
		[StringLength(MstLgwUnpackingPartConsts.MaxCfcLength)]
		public virtual string Cfc { get; set; }

		[StringLength(MstLgwUnpackingPartConsts.MaxModelLength)]
		public virtual string Model { get; set; }

		[StringLength(MstLgwUnpackingPartConsts.MaxBackNoLength)]
		public virtual string BackNo { get; set; }

		[StringLength(MstLgwUnpackingPartConsts.MaxPartNoLength)]
		public virtual string PartNo { get; set; }

		[StringLength(MstLgwUnpackingPartConsts.MaxPartNameLength)]
		public virtual string PartName { get; set; }

		[StringLength(MstLgwUnpackingPartConsts.MaxModuleCodeLength)]
		public virtual string ModuleCode { get; set; }

		[StringLength(MstLgwUnpackingPartConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }

		public string Exception { get; set; }
		public bool CanBeImported()
		{
			return string.IsNullOrEmpty(Exception);
		}
	}
}
