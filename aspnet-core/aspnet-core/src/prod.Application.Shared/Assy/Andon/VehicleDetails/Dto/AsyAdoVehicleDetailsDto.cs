using Abp.Application.Services.Dto;
using Castle.MicroKernel.SubSystems.Conversion;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Assy.Andon.Dto
{

	public class AsyAdoVehicleDetailsDto : EntityDto<long?>
	{

		public virtual long? No { get; set; }

		public virtual string BodyNo { get; set; }

		public virtual string LotNo { get; set; }

		public virtual int? NoInLot { get; set; }

		public virtual string SequenceNo { get; set; }

		public virtual string KeyNo { get; set; }

		public virtual string Vin { get; set; }
        public virtual string Color { get; set; }

        public virtual string Eng { get; set; }

		public virtual string Trs { get; set; }

		public virtual string Ecu { get; set; }

		public virtual DateTime? WInDateActual { get; set; }

		public virtual DateTime? TInPlanDatetime { get; set; }

		public virtual string PaintInTime { get; set; }

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

		public virtual string IsPrintedQrcode { get; set; }

		public virtual DateTime? PrintedQrcodeDate { get; set; }

		public virtual DateTime? UpdatedDate { get; set; }

		public virtual string IsProject { get; set; }

        //new
        public virtual string Cfc { get; set; }

        public virtual string VinNo { get; set; }

        public virtual string EngineId { get; set; }

        public virtual string TransId { get; set; }

        public virtual string SalesSfx { get; set; }

        public virtual string ColorType { get; set; }

        public virtual string IndentLine { get; set; }

        public virtual string SsNo { get; set; }

        public virtual string EdNumber { get; set; }

        public virtual string GoshiCar { get; set; }

        public virtual DateTime? LineoffDatetime { get; set; }

        public virtual DateTime? LineoffDate { get; set; }

        public virtual TimeSpan? LineoffTime { get; set; }

        public virtual DateTime? PdiDatetime { get; set; }

        public virtual DateTime? PdiDate { get; set; }

        public virtual TimeSpan? PdiTime { get; set; }

        public virtual DateTime? PIOActualDatetime { get; set; }

        public virtual DateTime? PIOActualDate { get; set; }

        public virtual TimeSpan? PIOActualTime { get; set; }

        public virtual string FormatPIOActualTime
        {
            get
            {
                return PIOActualTime == null ? "" : PIOActualTime.ToString().Substring(0, 8);
            }
            set { }
        }

        public virtual DateTime? SalesActualDatetime { get; set; }

        public virtual DateTime? SalesActualDate { get; set; }

		public virtual TimeSpan? SalesActualTime { get; set; }

        public virtual string FormatSalesActualTime
        {
            get
            {
                return SalesActualTime == null ? "" : SalesActualTime.ToString().Substring(0, 8);
            }
            set { }
        }
    }


    public class AsyAdoVehicleDetailsExcelDto : EntityDto<long?>
    {

        public virtual long? No { get; set; }

        public virtual string Cfc { get; set; }
        public virtual string BodyNo { get; set; }

        public virtual string LotNo { get; set; }

        public virtual int? NoInLot { get; set; }

        public virtual string SequenceNo { get; set; }

        public virtual string KeyNo { get; set; }

        public virtual string Vin { get; set; }
        public virtual string Color { get; set; }

        public virtual string Eng { get; set; }

        public virtual string Trs { get; set; }

        public virtual string Ecu { get; set; }

        public virtual DateTime? WInDateActual { get; set; }

        public virtual DateTime? TInPlanDatetime { get; set; }

        public virtual string PaintInTime { get; set; }

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

        public virtual string IsPrintedQrcode { get; set; }

        public virtual DateTime? PrintedQrcodeDate { get; set; }

        public virtual DateTime? UpdatedDate { get; set; }

        public virtual string IsProject { get; set; }

        //new

        public virtual string VinNo { get; set; }

        public virtual string EngineId { get; set; }

        public virtual string TransId { get; set; }

        public virtual string SalesSfx { get; set; }

        public virtual string ColorType { get; set; }

        public virtual string IndentLine { get; set; }

        public virtual string SsNo { get; set; }

        public virtual string EdNumber { get; set; }

        public virtual string GoshiCar { get; set; }

        public virtual DateTime? LineoffDatetime { get; set; }

        public virtual DateTime? LineoffDate { get; set; }

        public virtual TimeSpan? LineoffTime { get; set; }

        public virtual string FormatLineoffTime
        {
            get
            {
                return LineoffTime == null ? "" : LineoffTime.ToString().Substring(0, 8);
            }
            set { }
        }


        public virtual DateTime? PdiDatetime { get; set; }

        public virtual DateTime? PdiDate { get; set; }

        public virtual TimeSpan? PdiTime { get; set; }

        public virtual string FormatPdiTime {
            get
            {
                return PdiTime == null ? "" : PdiTime.ToString().Substring(0, 8);
            }
            set { }
        }

        public virtual DateTime? PIOActualDatetime { get; set; }

        public virtual DateTime? PIOActualDate { get; set; }

        public virtual TimeSpan? PIOActualTime { get; set; }

        public virtual string FormatPIOActualTime
        {
            get
            {
                return PIOActualTime == null ? "" : PIOActualTime.ToString().Substring(0, 8);
            }
            set { }
        }

        public virtual DateTime? SalesActualDatetime { get; set; }

        public virtual DateTime? SalesActualDate { get; set; }

        public virtual TimeSpan? SalesActualTime { get; set; }

        public virtual string FormatSalesActualTime
        {
            get
            {
                return SalesActualTime == null ? "" : SalesActualTime.ToString().Substring(0, 8);
            }
            set { }
        }
    }





    public class GetAsyAdoVehicleDetailsInput : PagedAndSortedResultRequestDto
	{
        public virtual string Cfc { get; set; }
        public virtual string BodyNo { get; set; }

		public virtual string LotNo { get; set; }
	
		public virtual string Vin { get; set; }

        public virtual string SequenceNo { get; set; }
    }	

    public class GetAsyAdoVehicleDetailsExportInput 
    {
        public virtual string Cfc { get; set; }
        public virtual string BodyNo { get; set; }
        public virtual string LotNo { get; set; }

        public virtual string Vin { get; set; }

        public virtual string SequenceNo { get; set; }
    }

    public class GetAsyAdoVehicleDetailsHistoryInput : PagedAndSortedResultRequestDto
    {
        public virtual long Id { get; set; }

        public virtual string TableName { get; set; }
    }

    public class GetAsyAdoVehicleDetailsHistoryExcelInput
    {
        public virtual long Id { get; set; }

        public virtual string TableName { get; set; }
    }
}


