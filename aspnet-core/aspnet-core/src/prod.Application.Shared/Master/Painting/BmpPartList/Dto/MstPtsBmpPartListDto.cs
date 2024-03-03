using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Painting.Dto
{

	public class MstPtsBmpPartListDto : EntityDto<long?>
	{

		public virtual string Model { get; set; }

		public virtual string Cfc { get; set; }

		public virtual string Grade { get; set; }

		public virtual string BackNo { get; set; }

		public virtual string ProdLine { get; set; }

		public virtual string PartTypeCode { get; set; }

		public virtual long? PartTypeId { get; set; }

		public virtual string Process { get; set; }

		public virtual string PkProcess { get; set; }

		public virtual string IsPunch { get; set; }

		public virtual string SpecialColor { get; set; }

		public virtual int? SignalId { get; set; }

		public virtual string SignalCode { get; set; }

		public virtual string Remark { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditMstPtsBmpPartListDto : EntityDto<long?>
	{

		[StringLength(MstPtsBmpPartListConsts.MaxModelLength)]
		public virtual string Model { get; set; }

		[StringLength(MstPtsBmpPartListConsts.MaxCfcLength)]
		public virtual string Cfc { get; set; }

		[StringLength(MstPtsBmpPartListConsts.MaxGradeLength)]
		public virtual string Grade { get; set; }

		[StringLength(MstPtsBmpPartListConsts.MaxBackNoLength)]
		public virtual string BackNo { get; set; }

		[StringLength(MstPtsBmpPartListConsts.MaxProdLineLength)]
		public virtual string ProdLine { get; set; }

		[StringLength(MstPtsBmpPartListConsts.MaxPartTypeCodeLength)]
		public virtual string PartTypeCode { get; set; }

		public virtual long? PartTypeId { get; set; }

		[StringLength(MstPtsBmpPartListConsts.MaxProcessLength)]
		public virtual string Process { get; set; }

		[StringLength(MstPtsBmpPartListConsts.MaxPkProcessLength)]
		public virtual string PkProcess { get; set; }

		[StringLength(MstPtsBmpPartListConsts.MaxIsPunchLength)]
		public virtual string IsPunch { get; set; }

		[StringLength(MstPtsBmpPartListConsts.MaxSpecialColorLength)]
		public virtual string SpecialColor { get; set; }

		public virtual int? SignalId { get; set; }

		[StringLength(MstPtsBmpPartListConsts.MaxSignalCodeLength)]
		public virtual string SignalCode { get; set; }

		[StringLength(MstPtsBmpPartListConsts.MaxRemarkLength)]
		public virtual string Remark { get; set; }

		[StringLength(MstPtsBmpPartListConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetMstPtsBmpPartListInput : PagedAndSortedResultRequestDto
	{

		public virtual string Model { get; set; }

		public virtual string Cfc { get; set; }

		public virtual string Grade { get; set; }

		public virtual string ProdLine { get; set; }

		public virtual string PartTypeCode { get; set; }

        public virtual string IsBumper { get; set; }

    }

	public class GetMstPtsBmpPartListExcelInput
	{
        public virtual string Model { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string Grade { get; set; }

        public virtual string ProdLine { get; set; }

        public virtual string PartTypeCode { get; set; }

        public virtual string IsBumper { get; set; }     

    }
}

