using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace prod.LogW.Ado.Dto
{
	public class LgwAdoCallingLightStatusDto : EntityDto<long?>
	{

		public virtual string Code { get; set; }

		public virtual string LightName { get; set; }

		public virtual string ProdLine { get; set; }

		public virtual string Process { get; set; }

		public virtual string BlockCode { get; set; }

		public virtual string BlockDescription { get; set; }

		public virtual string Sorting { get; set; }

		public virtual int? SignalId { get; set; }

		public virtual string SignalCode { get; set; }

		public virtual DateTime? StartDate { get; set; }

		public virtual DateTime? FinshDate { get; set; }

		public virtual string Status { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        public virtual string Shift { get; set; }

        public virtual int? NoInDate { get; set; }

        public virtual int? NoInShift { get; set; }
		public virtual int? BlockPerLine { get; set; }

		public virtual string TimeAction { get; set; }
	}

	public class CreateOrEditLgwAdoCallingLightStatusDto : EntityDto<long?>
	{

		[StringLength(LgwAdoCallingLightStatusConsts.MaxCodeLength)]
		public virtual string Code { get; set; }

		[StringLength(LgwAdoCallingLightStatusConsts.MaxLightNameLength)]
		public virtual string LightName { get; set; }

		[StringLength(LgwAdoCallingLightStatusConsts.MaxProdLineLength)]
		public virtual string ProdLine { get; set; }

		[StringLength(LgwAdoCallingLightStatusConsts.MaxProcessLength)]
		public virtual string Process { get; set; }

		[StringLength(LgwAdoCallingLightStatusConsts.MaxBlockCodeLength)]
		public virtual string BlockCode { get; set; }

		[StringLength(LgwAdoCallingLightStatusConsts.MaxBlockDescriptionLength)]
		public virtual string BlockDescription { get; set; }

		[StringLength(LgwAdoCallingLightStatusConsts.MaxSortingLength)]
		public virtual string Sorting { get; set; }

		public virtual int? SignalId { get; set; }

		[StringLength(LgwAdoCallingLightStatusConsts.MaxSignalCodeLength)]
		public virtual string SignalCode { get; set; }

		public virtual DateTime? StartDate { get; set; }

		public virtual DateTime? FinshDate { get; set; }

		[StringLength(LgwAdoCallingLightStatusConsts.MaxStatusLength)]
		public virtual string Status { get; set; }


        public virtual DateTime? WorkingDate { get; set; }

        [StringLength(LgwAdoCallingLightStatusConsts.MaxShiftLength)]
        public virtual string Shift { get; set; }

        public virtual int? NoInDate { get; set; }

        public virtual int? NoInShift { get; set; }
    }

	public class GetLgwAdoCallingLightStatusInput : PagedAndSortedResultRequestDto
	{
		public virtual string Code { get; set; }
		public virtual string ProdLine { get; set; }
        public virtual string Shift { get; set; }
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }

    }

}
