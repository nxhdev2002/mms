using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Master.Inventory.Dto
{
	public class MstInvDevanningCaseTypeExportInput
	{
		public virtual string Source { get; set; }
		public virtual string CaseNo { get; set; }

		public virtual string ShoptypeCode { get; set; }

		public virtual string Type { get; set; }

		public virtual string CarFamilyCode { get; set; }

		public virtual string IsActive { get; set; }
	}
}
