using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogA.Dto
{

	public class MstLgaPlcSignalDto : EntityDto<long?>
	{

		public virtual int? SignalIndex { get; set; }

		public virtual string SignalPattern { get; set; }

		public virtual string ProdLine { get; set; }

		public virtual string Process { get; set; }

		public virtual string SubProcess { get; set; }

		public virtual string SignalCode { get; set; }

		public virtual string SignalDescription { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditMstLgaPlcSignalDto : EntityDto<long?>
	{

		public virtual int? SignalIndex { get; set; }

		[StringLength(MstLgaPlcSignalConsts.MaxSignalPatternLength)]
		public virtual string SignalPattern { get; set; }

		[StringLength(MstLgaPlcSignalConsts.MaxProdLineLength)]
		public virtual string ProdLine { get; set; }

		[StringLength(MstLgaPlcSignalConsts.MaxProcessLength)]
		public virtual string Process { get; set; }

		[StringLength(MstLgaPlcSignalConsts.MaxSubProcessLength)]
		public virtual string SubProcess { get; set; }

		[StringLength(MstLgaPlcSignalConsts.MaxSignalCodeLength)]
		public virtual string SignalCode { get; set; }

		[StringLength(MstLgaPlcSignalConsts.MaxSignalDescriptionLength)]
		public virtual string SignalDescription { get; set; }

		[StringLength(MstLgaPlcSignalConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetMstLgaPlcSignalInput : PagedAndSortedResultRequestDto
	{


		public virtual string SignalPattern { get; set; }

		public virtual string ProdLine { get; set; }

		public virtual string Process { get; set; }
	
	}

    public class GetMstLgaPlcSignalExcelInput
	{

        public virtual string SignalPattern { get; set; }

        public virtual string ProdLine { get; set; }

        public virtual string Process { get; set; }

    }

}


