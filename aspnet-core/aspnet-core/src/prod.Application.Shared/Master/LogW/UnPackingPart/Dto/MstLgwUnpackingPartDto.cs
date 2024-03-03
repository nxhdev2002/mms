using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogW.Dto
{

	public class MstLgwUnpackingPartDto : EntityDto<long?>
	{

		public virtual string Cfc { get; set; }

		public virtual string Model { get; set; }

		public virtual string BackNo { get; set; }

		public virtual string PartNo { get; set; }

		public virtual string PartName { get; set; }

		public virtual string ModuleCode { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditMstLgwUnpackingPartDto : EntityDto<long?>
	{

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
	}

	public class GetMstLgwUnpackingPartInput : PagedAndSortedResultRequestDto
	{

		public virtual string Cfc { get; set; }

		public virtual string Model { get; set; }

		public virtual string BackNo { get; set; }

		public virtual string PartNo { get; set; }

		public virtual string PartName { get; set; }

		public virtual string ModuleCode { get; set; }

		public virtual string IsActive { get; set; }

	}

}


