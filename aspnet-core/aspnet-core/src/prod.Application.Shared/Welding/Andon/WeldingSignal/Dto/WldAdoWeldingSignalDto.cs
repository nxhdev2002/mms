using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Welding.Andon.Dto
{

	public class WldAdoWeldingSignalDto : EntityDto<long?>
	{

		public virtual string ProdLine { get; set; }

		public virtual string Process { get; set; }

		public virtual string SignalType { get; set; }

		public virtual string SignalBy { get; set; }

		public virtual DateTime? SignalDate { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditWldAdoWeldingSignalDto : EntityDto<long?>
	{

		[StringLength(WldAdoWeldingSignalConsts.MaxProdLineLength)]
		public virtual string ProdLine { get; set; }

		[StringLength(WldAdoWeldingSignalConsts.MaxProcessLength)]
		public virtual string Process { get; set; }

		[StringLength(WldAdoWeldingSignalConsts.MaxSignalTypeLength)]
		public virtual string SignalType { get; set; }

		[StringLength(WldAdoWeldingSignalConsts.MaxSignalByLength)]
		public virtual string SignalBy { get; set; }

		public virtual DateTime? SignalDate { get; set; }

		[StringLength(WldAdoWeldingSignalConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetWldAdoWeldingSignalInput : PagedAndSortedResultRequestDto
	{
		public virtual string ProdLine { get; set; }
		public virtual string Process { get; set; }
		public virtual string SignalType { get; set; }

        public virtual DateTime? SignalDateFrom { get; set; }

        public virtual DateTime? SignalDateTo { get; set; }

    }

    public class GetWldAdoWeldingSignalExportInput
    {
        public virtual string ProdLine { get; set; }
        public virtual string Process { get; set; }
        public virtual string SignalType { get; set; }

        public virtual DateTime? SignalDateFrom { get; set; }

        public virtual DateTime? SignalDateTo { get; set; }
    }
}


