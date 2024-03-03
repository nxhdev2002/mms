using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Painting.Andon.Dto
{

	public class PtsAdoPaintingProgressExportInput
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

}


