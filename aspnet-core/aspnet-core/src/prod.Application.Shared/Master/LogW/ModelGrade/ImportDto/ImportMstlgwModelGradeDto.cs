using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Master.LogW.ModelGrade.ImportDto
{
	public class ImportMstlgwModelGradeDto
   
    {

		[StringLength(MstLgwModelGradeConsts.MaxGuidLength)]
		public virtual string Guid { get; set; }

		[StringLength(MstLgwModelGradeConsts.MaxModelLength)]
		public virtual string Model { get; set; }

		[StringLength(MstLgwModelGradeConsts.MaxGradeLength)]
		public virtual string Grade { get; set; }

		public virtual int? ModuleUpQty { get; set; }

		public virtual int? ModuleMkQty { get; set; }

		public virtual int? UpLeadtime { get; set; }

		[StringLength(MstLgwModelGradeConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
		public string Exception { get; set; }
		public bool CanBeImported()
		{
			return string.IsNullOrEmpty(Exception);
		}
	}
}
