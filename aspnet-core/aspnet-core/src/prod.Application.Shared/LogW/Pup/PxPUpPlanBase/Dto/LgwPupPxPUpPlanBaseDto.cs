using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.LogW.Pup.Dto
{

	public class LgwPupPxPUpPlanBaseDto : EntityDto<long?>
	{

		public virtual DateTime? WorkingDate { get; set; }
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }

        public virtual string ProdLine { get; set; }

		public virtual int? Shift1 { get; set; }

		public virtual int? Shift2 { get; set; }

		public virtual int? Shift3 { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditLgwPupPxPUpPlanBaseDto : EntityDto<long?>
	{

		public virtual DateTime? WorkingDate { get; set; }

		[StringLength(LgwPupPxPUpPlanBaseConsts.MaxProdLineLength)]
		public virtual string ProdLine { get; set; }

		public virtual int? Shift1 { get; set; }

		public virtual int? Shift2 { get; set; }

		public virtual int? Shift3 { get; set; }

		[StringLength(LgwPupPxPUpPlanBaseConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetLgwPupPxPUpPlanBaseInput : PagedAndSortedResultRequestDto
	{
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }
        public virtual DateTime? WorkingDate { get; set; }
		public virtual string ProdLine { get; set; }
	}

    public class GetLgwPupPxPUpPlanBaseExportInput
    {
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }
        public virtual DateTime? WorkingDate { get; set; }
        public virtual string ProdLine { get; set; }
    }
}


