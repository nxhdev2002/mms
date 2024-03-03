using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.LogW.Pup.ImportDto

{
	public class ImportPxPUpPlanBaseDto
	{
		[StringLength(LgwPupPxPUpPlanBaseConsts.MaxGuidLength)]
		public virtual string Guid { get; set; }
		public virtual DateTime? WorkingDate { get; set; }

		[StringLength(LgwPupPxPUpPlanBaseConsts.MaxProdLineLength)]
		public virtual string ProdLine { get; set; }

		public virtual int? Shift1 { get; set; }

		public virtual int? Shift2 { get; set; }

		public virtual int? Shift3 { get; set; }

		[StringLength(LgwPupPxPUpPlanBaseConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	
		public string Exception { get; set; }
		public bool CanBeImported()
		{
			return string.IsNullOrEmpty(Exception);
		}
	}


}

