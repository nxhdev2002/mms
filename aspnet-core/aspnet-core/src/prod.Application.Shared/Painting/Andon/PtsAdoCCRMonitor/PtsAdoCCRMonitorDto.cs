using Abp.Application.Services.Dto;
using System; 
namespace prod.Painting.Andon.Dto
{

	

	public class GetWeldingDataOutput : EntityDto<long?>
	{
		public virtual string Seq { get; set; } 
		public virtual string ProcessCd { get; set; } 
		public virtual string BodyNo { get; set; }
		public virtual string Model { get; set; }
		public virtual string W_Line { get; set; }
		public virtual string LotNo { get; set; } 
		public virtual string Color { get; set; }
		 
	}
	public class GetPaintingDataOutput : EntityDto<long?>
	{
		public virtual string Seq { get; set; }
		public virtual string ProcessSeq { get; set; }
		public virtual string BodyNo { get; set; }
		public virtual string Filler { get; set; }
		public virtual int? ProcessGroup { get; set; }
		public virtual string PossibleMissScan { get; set; }
		public virtual string DelayFlag { get; set; }
		public virtual string BumperFlag { get; set; }
		public virtual int? SubGroup { get; set; }
		public virtual int? TcStatus { get; set; }
		public virtual string Mode { get; set; }
		public virtual string ErrorCd { get; set; }
		public virtual string IsActive { get; set; }
		public virtual string ModeL { get; set; }
		public virtual string LotNo { get; set; }
		public virtual string NoInLot { get; set; }
		public virtual string DefectDesc { get; set; }
		public virtual string ModeScreen { get; set; }
		public virtual string Location { get; set; }
		public virtual string CarSequence { get; set; }
		public virtual string Color { get; set; }
		public virtual string ScanTime { get; set; }
		public virtual string IsRescan { get; set; }

	}

	public class GetAssemblyDataOutput : EntityDto<long?>
	{

		public virtual string ProdLine { get; set; }
		public virtual string ProcessCd { get; set; }
		public virtual string Model { get; set; }
		public virtual string BodyNo { get; set; }
		public virtual string SequenceNo { get; set; }
		public virtual string Grade { get; set; }
		public virtual string LotNo { get; set; }
		public virtual string NoInLot { get; set; }
		public virtual string Color { get; set; }
		public virtual string IsProject { get; set; }
		public virtual DateTime? CreateDate { get; set; }
		public virtual string DelayFlag { get; set; }

	}
	
	public class GetInspectionDataOutput : EntityDto<long?>
	{

		public virtual string ProdLine { get; set; }
		public virtual string ProcessCd { get; set; }
		public virtual string Model { get; set; }
		public virtual string BodyNo { get; set; }
		public virtual string SequenceNo { get; set; }
		public virtual string Color { get; set; }
		public virtual string LotNo { get; set; }
		public virtual string DelayFlag { get; set; } 

	}

	public class GetAllBufferDataOutput : EntityDto<long?>
	{

		public virtual string Loc { get; set; }
		public virtual int? MinVal { get; set; }
		public virtual int? MaxVal { get; set; }
		public virtual int? BuffAct { get; set; }
		public virtual string ColorCd { get; set; } 

	}
	 
	public class GetVehicleDetailsOutput : EntityDto<long?>
	{
		public virtual int? No { get; set; }
		public virtual string Model { get; set; }
		public virtual string Color { get; set; }
		public virtual string BodyNo { get; set; }
		public virtual string LotNo { get; set; }
		public virtual string NoInLot { get; set; }
		public virtual string SequenceNo { get; set; }
		public virtual string KeyNo { get; set; }
		public virtual string Vin { get; set; }
		public virtual string Eng { get; set; }
		public virtual string Trs { get; set; }
		public virtual string Ecu { get; set; }
		public virtual DateTime? WInDateActual { get; set; }
		public virtual DateTime? TInPlanDatetime { get; set; }
		public virtual DateTime? PaintInTime { get; set; }
		public virtual DateTime? AInDateActual { get; set; }
		public virtual DateTime? InsOutDateActual { get; set; }
		public virtual DateTime? InsLineOutVp4DateActual { get; set; }
		public virtual string DriverAirBag { get; set; }
		public virtual string PassengerAirBag { get; set; }
		public virtual string SideAirBagLh { get; set; }
		public virtual string SideAirBagRh { get; set; }
		public virtual string KneeAirBagLh { get; set; }
		public virtual string CurtainSideAirBagLh { get; set; }
		public virtual string CurtainSideAirBagRh { get; set; }
		public virtual string TotalDelay { get; set; }
		public virtual DateTime? ShippingTime { get; set; }
		public virtual string VehicleId { get; set; }  
		public virtual string TestNo { get; set; }
		public virtual DateTime? UpdatedDate { get; set; } 
		public virtual string IsProject { get; set; }
		 

	}

	public class GetFrameOutput : EntityDto<long?>
	{


		public virtual int? Seq { get; set; }
		public virtual int? Filler { get; set; }
		public virtual int? ProcessGroup { get; set; }
		public virtual string BodyNo { get; set; }
		public virtual string VinNo { get; set; }
		public virtual string FrameNo { get; set; }
		public virtual string Mode { get; set; }
		public virtual string ErrorCd { get; set; }
		public virtual string IsActive { get; set; }
		public virtual string IsRescan { get; set; }
		public virtual string ModeL { get; set; }
		public virtual string LotNo { get; set; }
		public virtual int? NoInLot { get; set; }
		public virtual string ModeScreen { get; set; }
		public virtual string Location { get; set; }
		public virtual string CarSequence { get; set; }



	}


    public class GetInventoryStdOutput : EntityDto<long?>
    {
        public virtual string Model { get; set; }
        public virtual string STD { get; set; }
        public virtual string Act { get; set; }
        public virtual string WarningFlag { get; set; }
      
    }


}

