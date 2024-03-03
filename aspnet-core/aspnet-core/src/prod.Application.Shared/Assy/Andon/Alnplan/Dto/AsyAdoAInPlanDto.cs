using Abp.Application.Services.Dto;
using Castle.MicroKernel.SubSystems.Conversion;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Assy.Andon.Dto
{

	public class AsyAdoAInPlanDto : EntityDto<long?>
	{

		public virtual string Model { get; set; }

		public virtual string LotNo { get; set; }

		public virtual int? NoInLot { get; set; }

		public virtual string Grade { get; set; }

		public virtual string BodyNo { get; set; }

		public virtual string Color { get; set; }

		public virtual DateTime? AInDateActual { get; set; }

        public virtual string FormatAInDateActual
        {
            get
            {
                return AInDateActual == null ? "" : string.Format("{0:dd/MM/yyyy}", AInDateActual);
            }
            set { }
        }

        public virtual DateTime? AOutDateActual { get; set; }

        public virtual string FormatAOutDateActual
        {
            get
            {
                return AOutDateActual == null ? "" : string.Format("{0:dd/MM/yyyy}", AOutDateActual);
            }
            set { }
        }

        public virtual DateTime? AInPlanDatetime { get; set; }

		public virtual DateTime? AOutPlanDatetime { get; set; }

		public virtual string SequenceNo { get; set; }

		public virtual int? IsStart { get; set; }

        public virtual long? VehicleId { get; set; }
       
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

        public virtual long? AssemblyId { get; set; }

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
	public class GetAsyAdoAInPlanInput : PagedAndSortedResultRequestDto
	{
		public virtual string Model { get; set; }

		public virtual string LotNo { get; set; }

		public virtual string Grade { get; set; }

		public virtual string BodyNo { get; set; }

        public virtual string SequenceNo { get; set; }

        public virtual string VinNo { get; set; }
    }


    public class GetAsyAdoAInPlanExportInput
    {
        public virtual string Model { get; set; }

        public virtual string LotNo { get; set; }

        public virtual string Grade { get; set; }

        public virtual string BodyNo { get; set; }
        public virtual string SequenceNo { get; set; }

        public virtual string VinNo { get; set; }

    }

}


