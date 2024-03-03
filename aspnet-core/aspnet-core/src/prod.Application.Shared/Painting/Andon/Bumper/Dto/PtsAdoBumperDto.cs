using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Painting.Andon.Dto
{

	public class PtsAdoBumperDto : EntityDto<long?>
	{

		public virtual long? WipId { get; set; }

		public virtual string Model { get; set; }

		public virtual string Grade { get; set; }

		public virtual string BodyNo { get; set; }

		public virtual string LotNo { get; set; }

		public virtual int? NoInLot { get; set; }

		public virtual string Color { get; set; }

		public virtual int? BumperStatus { get; set; }

		public virtual DateTime? InitialDate { get; set; }

		public virtual DateTime? I1Date { get; set; }

		public virtual DateTime? I2Date { get; set; }

		public virtual DateTime? InlInDate { get; set; }

		public virtual DateTime? InlOutDate { get; set; }

		public virtual DateTime? PreparationDate { get; set; }

		public virtual DateTime? UnpackingDate { get; set; }

		public virtual DateTime? JigSettingDate { get; set; }

		public virtual DateTime? BoothDate { get; set; }

		public virtual DateTime? FinishedDate { get; set; }

		public virtual int? ExtSeq { get; set; }

		public virtual int? UnpSeq { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditPtsAdoBumperDto : EntityDto<long?>
	{

		public virtual long? WipId { get; set; }

		[StringLength(PtsAdoBumperConsts.MaxModelLength)]
		public virtual string Model { get; set; }

		[StringLength(PtsAdoBumperConsts.MaxGradeLength)]
		public virtual string Grade { get; set; }

		[StringLength(PtsAdoBumperConsts.MaxBodyNoLength)]
		public virtual string BodyNo { get; set; }

		[StringLength(PtsAdoBumperConsts.MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		public virtual int? NoInLot { get; set; }

		[StringLength(PtsAdoBumperConsts.MaxColorLength)]
		public virtual string Color { get; set; }

		public virtual int? BumperStatus { get; set; }

		public virtual DateTime? InitialDate { get; set; }

		public virtual DateTime? I1Date { get; set; }

		public virtual DateTime? I2Date { get; set; }

		public virtual DateTime? InlInDate { get; set; }

		public virtual DateTime? InlOutDate { get; set; }

		public virtual DateTime? PreparationDate { get; set; }

		public virtual DateTime? UnpackingDate { get; set; }

		public virtual DateTime? JigSettingDate { get; set; }

		public virtual DateTime? BoothDate { get; set; }

		public virtual DateTime? FinishedDate { get; set; }

		public virtual int? ExtSeq { get; set; }

		public virtual int? UnpSeq { get; set; }

		[StringLength(PtsAdoBumperConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetPtsAdoBumperInput : PagedAndSortedResultRequestDto
	{

		public virtual string Model { get; set; }

		public virtual string Grade { get; set; }

		public virtual string BodyNo { get; set; }

		public virtual string LotNo { get; set; }

		
	}
    public class GetPtsAdoBumperExportInput 
    {

        public virtual string Model { get; set; }

        public virtual string Grade { get; set; }

        public virtual string BodyNo { get; set; }

        public virtual string LotNo { get; set; }


    }
    public class PtsAdoBumperGetDataSmallSubassyDto : EntityDto<long?>
    {
		public virtual string Process { get; set; }

		public virtual string Grade { get; set; }

		public virtual string Color { get; set; }

		public virtual string BodyNo { get; set; }

		public virtual int? CarSequence { get; set; }

		public virtual string LotNo { get; set; }

		public virtual DateTime? OrderDate { get; set; }

		public virtual string Line { get; set; }

		public virtual string Model { get; set; }


	}
	//bumper clr
	public class PtsAdoBumperGetdataClrIndicatorDto : EntityDto<long?>
	{
		public virtual string BodyNo { get; set; }
		public virtual string LotNo { get; set; }
		public virtual string Model { get; set; }
		public virtual string Grade { get; set; }
		public virtual string Color { get; set; }
		public virtual DateTime? OrderDate { get; set; }
		public virtual string Line { get; set; }
		public virtual string Process { get; set; }

	}

	

}


