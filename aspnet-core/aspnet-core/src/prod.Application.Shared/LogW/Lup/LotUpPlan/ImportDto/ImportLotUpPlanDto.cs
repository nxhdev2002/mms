using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.LogW.Lup.Dto
{

	public class ImportLotUpPlanDto : EntityDto<long?>
	{
		[StringLength(LgwLupLotUpPlanConsts.MaxGuidLength)]
		public virtual string Guid { get; set; }
		[StringLength(LgwLupLotUpPlanConsts.MaxProdLineLength)]
		public virtual string ProdLine { get; set; }

		public virtual DateTime? WorkingDate { get; set; }

		[StringLength(LgwLupLotUpPlanConsts.MaxShiftLength)]
		public virtual string Shift { get; set; }

		public virtual int? NoInShift { get; set; }

		public virtual int? NoInDay { get; set; }

		[StringLength(LgwLupLotUpPlanConsts.MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		public virtual int? LotPartialNo { get; set; }

		public virtual DateTime? UnpackingStartDatetime { get; set; }

		public virtual DateTime? UnpackingFinishDatetime { get; set; }

		[StringLength(LgwLupLotUpPlanConsts.MaxTpmLength)]
		public virtual string Tpm { get; set; }

		[StringLength(LgwLupLotUpPlanConsts.MaxRemarksLength)]
		public virtual string Remarks { get; set; }

		[StringLength(LgwLupLotUpPlanConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
		public string Exception { get; set; }
		public bool CanBeImported()
		{
			return string.IsNullOrEmpty(Exception);
		}
	}




}


