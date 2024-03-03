using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Welding.Andon.Dto
{

	public class WldAdoWeldingProgressDto : EntityDto<long?>
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

	public class CreateOrEditWldAdoWeldingProgressDto : EntityDto<long?>
	{

		public virtual long? ScanningId { get; set; }

		[StringLength(WldAdoWeldingProgressConsts.MaxBodyNoLength)]
		public virtual string BodyNo { get; set; }

		[StringLength(WldAdoWeldingProgressConsts.MaxColorLength)]
		public virtual string Color { get; set; }

		[StringLength(WldAdoWeldingProgressConsts.MaxColorOrgLength)]
		public virtual string ColorOrg { get; set; }

		[StringLength(WldAdoWeldingProgressConsts.MaxScanTypeCdLength)]
		public virtual string ScanTypeCd { get; set; }

		[StringLength(WldAdoWeldingProgressConsts.MaxScanLocationLength)]
		public virtual string ScanLocation { get; set; }

		public virtual DateTime? ScanTime { get; set; }

		[StringLength(WldAdoWeldingProgressConsts.MaxScanValueLength)]
		public virtual string ScanValue { get; set; }

		[StringLength(WldAdoWeldingProgressConsts.MaxModeLength)]
		public virtual string Mode { get; set; }

		public virtual int? ProcessGroup { get; set; }

		public virtual int? ProcessSubgroup { get; set; }

		public virtual int? ProcessSeq { get; set; }

		public virtual int? ConveyerStatus { get; set; }

		public virtual DateTime? LastConveyerRun { get; set; }

		public virtual int? TcStatus { get; set; }

		[StringLength(WldAdoWeldingProgressConsts.MaxModelLength)]
		public virtual string Model { get; set; }

		[StringLength(WldAdoWeldingProgressConsts.MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		public virtual int? NoInLot { get; set; }

		[StringLength(WldAdoWeldingProgressConsts.MaxSequenceNoLength)]
		public virtual string SequenceNo { get; set; }

		[StringLength(WldAdoWeldingProgressConsts.MaxDefectDescLength)]
		public virtual string DefectDesc { get; set; }

		[StringLength(WldAdoWeldingProgressConsts.MaxTargetRepairLength)]
		public virtual string TargetRepair { get; set; }

		[StringLength(WldAdoWeldingProgressConsts.MaxLocationLength)]
		public virtual string Location { get; set; }

		[StringLength(WldAdoWeldingProgressConsts.MaxDuplicateLotLength)]
		public virtual string DuplicateLot { get; set; }

		[StringLength(WldAdoWeldingProgressConsts.MaxWeldTransferLength)]
		public virtual string WeldTransfer { get; set; }

		[StringLength(WldAdoWeldingProgressConsts.MaxRescanBodyNoLength)]
		public virtual string RescanBodyNo { get; set; }

		[StringLength(WldAdoWeldingProgressConsts.MaxRescanLotNoLength)]
		public virtual string RescanLotNo { get; set; }

		[StringLength(WldAdoWeldingProgressConsts.MaxRescanModeLength)]
		public virtual string RescanMode { get; set; }

		[StringLength(WldAdoWeldingProgressConsts.MaxErrorCdLength)]
		public virtual string ErrorCd { get; set; }

		[StringLength(WldAdoWeldingProgressConsts.MaxIsRescanLength)]
		public virtual string IsRescan { get; set; }

		[StringLength(WldAdoWeldingProgressConsts.MaxIsPaintOutLength)]
		public virtual string IsPaintOut { get; set; }

		[StringLength(WldAdoWeldingProgressConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetWldAdoWeldingProgressInput : PagedAndSortedResultRequestDto
	{
		public virtual string BodyNo { get; set; }

        public virtual DateTime? ScanTimeFrom { get; set; }

        public virtual DateTime? ScanTimeTo { get; set; }

        public virtual string Model { get; set; }
		public virtual string LotNo { get; set; }
        public virtual int? ProcessGroup { get; set; }
    }

    public class GetWldAdoWeldingProgressExportInput 
    {
        public virtual string BodyNo { get; set; }

        public virtual DateTime? ScanTimeFrom { get; set; }

        public virtual DateTime? ScanTimeTo { get; set; }
        public virtual string Model { get; set; }
        public virtual string LotNo { get; set; }
        public virtual int? ProcessGroup { get; set; }
    }

}


