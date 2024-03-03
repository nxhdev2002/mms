using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Painting.Andon.Dto
{

	public class PtsAdoPaintingDataDto : EntityDto<long?>
	{

		public virtual long? LifetimeNo { get; set; }

		public virtual string Model { get; set; }

		public virtual string Cfc { get; set; }

		public virtual string Grade { get; set; }

		public virtual string LotNo { get; set; }

		public virtual int? NoInLot { get; set; }

		public virtual string BodyNo { get; set; }

		public virtual string Color { get; set; }

		public virtual string ProdLine { get; set; }

		public virtual int? ProcessGroup { get; set; }

		public virtual int? SubGroup { get; set; }

		public virtual int? ProcessSeq { get; set; }

		public virtual int? Filler { get; set; }

		public virtual DateTime? WorkingDate { get; set; }
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }

        public virtual string Shift { get; set; }

		public virtual int? NoInDate { get; set; }

		public virtual string ProcessCode { get; set; }

		public virtual string InfoProcess { get; set; }

		public virtual int? InfoProcessNo { get; set; }

		public virtual string IsProject { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditPtsAdoPaintingDataDto : EntityDto<long?>
	{

		public virtual long? LifetimeNo { get; set; }

		[StringLength(PtsAdoPaintingDataConsts.MaxModelLength)]
		public virtual string Model { get; set; }

		[StringLength(PtsAdoPaintingDataConsts.MaxCfcLength)]
		public virtual string Cfc { get; set; }

		[StringLength(PtsAdoPaintingDataConsts.MaxGradeLength)]
		public virtual string Grade { get; set; }

		[StringLength(PtsAdoPaintingDataConsts.MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		public virtual int? NoInLot { get; set; }

		[StringLength(PtsAdoPaintingDataConsts.MaxBodyNoLength)]
		public virtual string BodyNo { get; set; }

		[StringLength(PtsAdoPaintingDataConsts.MaxColorLength)]
		public virtual string Color { get; set; }

		[StringLength(PtsAdoPaintingDataConsts.MaxProdLineLength)]
		public virtual string ProdLine { get; set; }

		public virtual int? ProcessGroup { get; set; }

		public virtual int? SubGroup { get; set; }

		public virtual int? ProcessSeq { get; set; }

		public virtual int? Filler { get; set; }

		public virtual DateTime? WorkingDate { get; set; }

		[StringLength(PtsAdoPaintingDataConsts.MaxShiftLength)]
		public virtual string Shift { get; set; }

		public virtual int? NoInDate { get; set; }

		[StringLength(PtsAdoPaintingDataConsts.MaxProcessCodeLength)]
		public virtual string ProcessCode { get; set; }

		[StringLength(PtsAdoPaintingDataConsts.MaxInfoProcessLength)]
		public virtual string InfoProcess { get; set; }

		public virtual int? InfoProcessNo { get; set; }

		[StringLength(PtsAdoPaintingDataConsts.MaxIsProjectLength)]
		public virtual string IsProject { get; set; }

		[StringLength(PtsAdoPaintingDataConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetPtsAdoPaintingDataInput : PagedAndSortedResultRequestDto
	{

		public virtual string Model { get; set; }

		public virtual string Cfc { get; set; }

		public virtual string Grade { get; set; }

		public virtual string ProdLine { get; set; }

        public virtual DateTime? WorkingDateFrom { get; set; }

        public virtual DateTime? WorkingDateTo { get; set; }
        public virtual string Shift { get; set; }

    }

    public class PtsAdoBumperGetDataBumperInDto
	{
		public virtual int? PlanCount { get; set; }

		public virtual int? ActualCount { get; set; }

		public virtual long? ProgressId { get; set; }

		public virtual string PartTypeName { get; set; }

		public virtual string ProdLine { get; set; }

		public virtual long? PartId { get; set; }

		public virtual string BackNo { get; set; }

		public virtual string Model { get; set; }

		public virtual string Cfc { get; set; }

		public virtual string Grade { get; set; }

		public virtual string IsPunch { get; set; }

		public virtual string IsBumper { get; set; }

		public virtual string LotNo { get; set; }

		public virtual int? NoInLot { get; set; }

		public virtual string SequenceNo { get; set; }

		public virtual string BodyNo { get; set; }

		public virtual string Color { get; set; }

		public virtual string DisplayInfor { get; set; }

		public virtual string SpecilColor { get; set; }

		public virtual string Process { get; set; }

		public virtual string PkProcess { get; set; }

		public virtual DateTime? WorkingDate { get; set; }

		public virtual string Shift { get; set; }

		public virtual int? NoInDate { get; set; }

		public virtual string ProcessCode { get; set; }

		public virtual string InfoProcess { get; set; }

		public virtual int? InfoProcessNo { get; set; }

		public virtual string IsProject { get; set; }

		public virtual string IsActive { get; set; }

        public virtual string Status { get; set; }

        public virtual int? RowNo { get; set; }

        public virtual int? ActualProgress { get; set; }

        public virtual int? PlanProgress { get; set; }
    }

	public class PtsBmpPartTypeDto
    {
        public virtual long? Id { get; set; }
        public virtual string PartType { get; set; }
        public virtual string PartTypeName { get; set; }
        public virtual string ProdLine { get; set; }
        public virtual int? Sorting { get; set; }
        public virtual string IsBumper { get; set; }
        public virtual string IsActive { get; set; }
    }
}