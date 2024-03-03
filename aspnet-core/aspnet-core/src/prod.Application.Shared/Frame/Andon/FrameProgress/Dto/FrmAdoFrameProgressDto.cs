using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Frame.Andon.Dto
{

	public class FrmAdoFrameProgressDto : EntityDto<long?>
	{

		public virtual long? ScanningId { get; set; }

		public virtual string BodyNo { get; set; }

		public virtual string Color { get; set; }

		public virtual string ScanTypeCd { get; set; }

		public virtual string ScanLocation { get; set; }

		public virtual DateTime? ScanTime { get; set; }

		public virtual string ScanValue { get; set; }

		public virtual string Mode { get; set; }

		public virtual int? ProcessGroup { get; set; }

		public virtual int? ProcessSubgroup { get; set; }

		public virtual string VinNo { get; set; }

		public virtual string FrameNo { get; set; }

		public virtual string Model { get; set; }

		public virtual string Grade { get; set; }

		public virtual string LotNo { get; set; }

		public virtual int? NoInLot { get; set; }

		public virtual string SequenceNo { get; set; }

		public virtual string Location { get; set; }

		public virtual string AndonTransfer { get; set; }

		public virtual string RescanBodyNo { get; set; }

		public virtual string RescanLotNo { get; set; }

		public virtual string RescanMode { get; set; }

		public virtual string ErrorCd { get; set; }

		public virtual string IsRescan { get; set; }

		public virtual string IsActive { get; set; }

	}


    public class GetFrmAdoFrameProgressInput : PagedAndSortedResultRequestDto
    {

        public virtual string BodyNo { get; set; }

        public virtual DateTime? ScanTimeFrom { get; set; }

        public virtual DateTime? ScanTimeTo { get; set; }

        public virtual string VinNo { get; set; }

        public virtual string FrameNo { get; set; }

        public virtual string Model { get; set; }

        public virtual string Grade { get; set; }

        public virtual string ScanLocation { get; set; }

        public virtual int? ProcessGroup { get; set; }


    }

    public class GetFrmAdoFrameProgressExportInput
    {

        public virtual string BodyNo { get; set; }

        public virtual DateTime? ScanTimeFrom { get; set; }

        public virtual DateTime? ScanTimeTo { get; set; }

        public virtual string VinNo { get; set; }

        public virtual string FrameNo { get; set; }

        public virtual string Model { get; set; }

        public virtual string Grade { get; set; }

        public virtual string ScanLocation { get; set; }

        public virtual int? ProcessGroup { get; set; }


    }

}

