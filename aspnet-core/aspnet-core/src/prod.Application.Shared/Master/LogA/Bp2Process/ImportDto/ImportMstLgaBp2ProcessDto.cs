using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Master.LogA.Bp2Process.ImportDto
{
    public class ImportMstLgaBp2ProcessDto
    {

		[StringLength(MstLgaBp2ProcessConsts.MaxGuidLength)]
		public virtual string Guid { get; set; }
		[StringLength(MstLgaBp2ProcessConsts.MaxCodeLength)]
		public virtual string Code { get; set; }

		[StringLength(MstLgaBp2ProcessConsts.MaxProcessNameLength)]
		public virtual string ProcessName { get; set; }

		[StringLength(MstLgaBp2ProcessConsts.MaxProdLineLength)]
		public virtual string ProdLine { get; set; }

		public virtual int? LeadTime { get; set; }

		public virtual int? Sorting { get; set; }

		[StringLength(MstLgaBp2ProcessConsts.MaxProcessTypeLength)]
		public virtual string ProcessType { get; set; }

		[StringLength(MstLgaBp2ProcessConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
		public string Exception { get; set; }
		public bool CanBeImported()
		{
			return string.IsNullOrEmpty(Exception);
		}
	}
}
