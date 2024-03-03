using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogW.Dto
{

	public class MstLgwPlcSignalDto : EntityDto<long?>
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

	//public class CreateOrEditMstLgwPlcSignalDto : EntityDto<long?>
	//{

	//	public virtual int? SignalIndex { get; set; }

	//	[StringLength(MstLgwPlcSignalConsts.MaxSignalPatternLength)]
	//	public virtual string SignalPattern { get; set; }

	//	[StringLength(MstLgwPlcSignalConsts.MaxProdLineLength)]
	//	public virtual string ProdLine { get; set; }

	//	[StringLength(MstLgwPlcSignalConsts.MaxProcessLength)]
	//	public virtual string Process { get; set; }

	//	[StringLength(MstLgwPlcSignalConsts.MaxSubProcessLength)]
	//	public virtual string SubProcess { get; set; }

	//	[StringLength(MstLgwPlcSignalConsts.MaxSignalCodeLength)]
	//	public virtual string SignalCode { get; set; }

	//	[StringLength(MstLgwPlcSignalConsts.MaxSignalDescriptionLength)]
	//	public virtual string SignalDescription { get; set; }

	//	[StringLength(MstLgwPlcSignalConsts.MaxIsActiveLength)]
	//	public virtual string IsActive { get; set; }
	//}

	public class GetMstLgwPlcSignalInput : PagedAndSortedResultRequestDto
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

}


