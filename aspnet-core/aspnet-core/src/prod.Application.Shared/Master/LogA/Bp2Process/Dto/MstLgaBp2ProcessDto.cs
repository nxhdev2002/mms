using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogA.Dto
{
	public class MstLgaBp2ProcessDto : EntityDto<long?>
	{

		public virtual string Code { get; set; }

		public virtual string ProcessName { get; set; }

		public virtual string ProdLine { get; set; }

		public virtual int? LeadTime { get; set; }

		public virtual int? Sorting { get; set; }

		public virtual string ProcessType { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditMstLgaBp2ProcessDto : EntityDto<long?>
	{

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
	}

	public class GetMstLgaBp2ProcessInput : PagedAndSortedResultRequestDto
	{
		public virtual string Code { get; set; }

		public virtual string ProcessName { get; set; }

		public virtual string ProdLine { get; set; }

	}
    public class GetMstLgaBp2ProcessExcelInput
    {
        public virtual string Code { get; set; }

        public virtual string ProcessName { get; set; }

        public virtual string ProdLine { get; set; }

    }

}


