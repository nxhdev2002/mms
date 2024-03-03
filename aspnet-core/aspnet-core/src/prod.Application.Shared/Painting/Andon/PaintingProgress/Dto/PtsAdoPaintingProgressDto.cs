using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Painting.Andon.Dto
{

	public class PtsAdoPaintingProgressDto : EntityDto<long?>
	{

		public virtual long? ScanningId { get; set; }

		public virtual string BodyNo { get; set; }

		public virtual string Color { get; set; }

		public virtual string ColorOrg { get; set; }

		public virtual string ScanTypeCd { get; set; }

		public virtual string ScanLocation { get; set; }

		public virtual DateTime? ScanTime { get; set; }

		public virtual string ScanValue { get; set; }

		public virtual string Mode { get; set; }

		public virtual int? ProcessGroup { get; set; }

		public virtual int? ProcessSubgroup { get; set; }

		public virtual int? ProcessSeq { get; set; }

		public virtual int? ConveyerStatus { get; set; }

		public virtual DateTime? LastConveyerRun { get; set; }

		public virtual int? TcStatus { get; set; }

		public virtual string Model { get; set; }

		public virtual string LotNo { get; set; }

		public virtual int? NoInLot { get; set; }

		public virtual string SequenceNo { get; set; }

		public virtual string DefectDesc { get; set; }

		public virtual string TargetRepair { get; set; }

		public virtual string Location { get; set; }

		public virtual string DuplicateLot { get; set; }

		public virtual string WeldTransfer { get; set; }

		public virtual string RescanBodyNo { get; set; }

		public virtual string RescanLotNo { get; set; }

		public virtual string RescanMode { get; set; }

		public virtual string ErrorCd { get; set; }

		public virtual string IsRescan { get; set; }

		public virtual string IsPaintOut { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditPtsAdoPaintingProgressDto : EntityDto<long?>
	{

		public virtual long? ScanningId { get; set; }

		[StringLength(PtsAdoPaintingProgressConsts.MaxBodyNoLength)]
		public virtual string BodyNo { get; set; }

		[StringLength(PtsAdoPaintingProgressConsts.MaxColorLength)]
		public virtual string Color { get; set; }

		[StringLength(PtsAdoPaintingProgressConsts.MaxColorOrgLength)]
		public virtual string ColorOrg { get; set; }

		[StringLength(PtsAdoPaintingProgressConsts.MaxScanTypeCdLength)]
		public virtual string ScanTypeCd { get; set; }

		[StringLength(PtsAdoPaintingProgressConsts.MaxScanLocationLength)]
		public virtual string ScanLocation { get; set; }

		public virtual DateTime? ScanTime { get; set; }

		[StringLength(PtsAdoPaintingProgressConsts.MaxScanValueLength)]
		public virtual string ScanValue { get; set; }

		[StringLength(PtsAdoPaintingProgressConsts.MaxModeLength)]
		public virtual string Mode { get; set; }

		public virtual int? ProcessGroup { get; set; }

		public virtual int? ProcessSubgroup { get; set; }

		public virtual int? ProcessSeq { get; set; }

		public virtual int? ConveyerStatus { get; set; }

		public virtual DateTime? LastConveyerRun { get; set; }

		public virtual int? TcStatus { get; set; }

		[StringLength(PtsAdoPaintingProgressConsts.MaxModelLength)]
		public virtual string Model { get; set; }

		[StringLength(PtsAdoPaintingProgressConsts.MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		public virtual int? NoInLot { get; set; }

		[StringLength(PtsAdoPaintingProgressConsts.MaxSequenceNoLength)]
		public virtual string SequenceNo { get; set; }

		[StringLength(PtsAdoPaintingProgressConsts.MaxDefectDescLength)]
		public virtual string DefectDesc { get; set; }

		[StringLength(PtsAdoPaintingProgressConsts.MaxTargetRepairLength)]
		public virtual string TargetRepair { get; set; }

		[StringLength(PtsAdoPaintingProgressConsts.MaxLocationLength)]
		public virtual string Location { get; set; }

		[StringLength(PtsAdoPaintingProgressConsts.MaxDuplicateLotLength)]
		public virtual string DuplicateLot { get; set; }

		[StringLength(PtsAdoPaintingProgressConsts.MaxWeldTransferLength)]
		public virtual string WeldTransfer { get; set; }

		[StringLength(PtsAdoPaintingProgressConsts.MaxRescanBodyNoLength)]
		public virtual string RescanBodyNo { get; set; }

		[StringLength(PtsAdoPaintingProgressConsts.MaxRescanLotNoLength)]
		public virtual string RescanLotNo { get; set; }

		[StringLength(PtsAdoPaintingProgressConsts.MaxRescanModeLength)]
		public virtual string RescanMode { get; set; }

		[StringLength(PtsAdoPaintingProgressConsts.MaxErrorCdLength)]
		public virtual string ErrorCd { get; set; }

		[StringLength(PtsAdoPaintingProgressConsts.MaxIsRescanLength)]
		public virtual string IsRescan { get; set; }

		[StringLength(PtsAdoPaintingProgressConsts.MaxIsPaintOutLength)]
		public virtual string IsPaintOut { get; set; }

		[StringLength(PtsAdoPaintingProgressConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetPtsAdoPaintingProgressInput : PagedAndSortedResultRequestDto
	{

		public virtual string BodyNo { get; set; }		


		public virtual string Model { get; set; }

		public virtual string LotNo { get; set; }

        public virtual string ScanLocation { get; set; }

        public virtual int? ProcessGroup { get; set; }


    }

    public class GetPtsAdoPaintingProgressExportInput
    {

        public virtual string BodyNo { get; set; }


        public virtual string Model { get; set; }

        public virtual string LotNo { get; set; }

        public virtual string ScanLocation { get; set; }

        public virtual int? ProcessGroup { get; set; }


    }

}


