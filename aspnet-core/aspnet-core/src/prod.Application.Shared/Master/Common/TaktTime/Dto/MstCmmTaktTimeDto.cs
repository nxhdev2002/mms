using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Cmm.Dto
{

	public class MstCmmTaktTimeDto : EntityDto<long?>
	{

		public virtual string ProdLine { get; set; }

		public virtual string GroupCd { get; set; }

		public virtual int? TaktTimeSecond { get; set; }

		public virtual string TaktTime { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditMstCmmTaktTimeDto : EntityDto<long?>
	{

		[StringLength(MstCmmTaktTimeConsts.MaxProdLineLength)]
		public virtual string ProdLine { get; set; }

		[StringLength(MstCmmTaktTimeConsts.MaxGroupCdLength)]
		public virtual string GroupCd { get; set; }

		public virtual int? TaktTimeSecond { get; set; }

		[StringLength(MstCmmTaktTimeConsts.MaxTaktTimeLength)]
		public virtual string TaktTime { get; set; }

		[StringLength(MstCmmTaktTimeConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetMstCmmTaktTimeInput : PagedAndSortedResultRequestDto
	{

		public virtual string ProdLine { get; set; }

		public virtual string GroupCd { get; set; }

		public virtual int? TaktTimeSecond { get; set; }

		public virtual string TaktTime { get; set; }

		public virtual string IsActive { get; set; }

	}

}


