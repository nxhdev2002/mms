using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogW.Dto
{

	public class MstLgwModelGradeDto : EntityDto<long?>
	{

		public virtual string Model { get; set; }

		public virtual string Grade { get; set; }

		public virtual int? ModuleUpQty { get; set; }

		public virtual int? ModuleMkQty { get; set; }

		public virtual int? UpLeadtime { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditMstLgwModelGradeDto : EntityDto<long?>
	{

		[StringLength(MstLgwModelGradeConsts.MaxModelLength)]
		public virtual string Model { get; set; }

		[StringLength(MstLgwModelGradeConsts.MaxGradeLength)]
		public virtual string Grade { get; set; }

		public virtual int? ModuleUpQty { get; set; }

		public virtual int? ModuleMkQty { get; set; }

		public virtual int? UpLeadtime { get; set; }

		[StringLength(MstLgwModelGradeConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetMstLgwModelGradeInput : PagedAndSortedResultRequestDto
	{

		public virtual string Model { get; set; }

		public virtual string Grade { get; set; }

		public virtual int? ModuleUpQty { get; set; }

		public virtual int? ModuleMkQty { get; set; }

		public virtual int? UpLeadtime { get; set; }

		public virtual string IsActive { get; set; }

	}

}

