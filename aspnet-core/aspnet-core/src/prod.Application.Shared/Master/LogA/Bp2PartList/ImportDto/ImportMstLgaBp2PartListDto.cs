using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Master.LogA.Bp2PartList.ImportDto
{
	public class ImportMstLgaBp2PartListDto
	{

		[StringLength(MstLgaBp2PartListConsts.MaxGuidLength)]
		public virtual string Guid { get; set; }

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

		public string Exception { get; set; }
		public bool CanBeImported()
		{
			return string.IsNullOrEmpty(Exception);
		}
	}
}
