using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Assy.Andon
{

	[Table("AsyAdoVehicleDetails")]
	[Index(nameof(No), Name = "IX_AsyAdoVehicleDetails_No")]
	[Index(nameof(BodyNo), Name = "IX_AsyAdoVehicleDetails_BodyNo")]
	[Index(nameof(LotNo), Name = "IX_AsyAdoVehicleDetails_LotNo")]
	[Index(nameof(SequenceNo), Name = "IX_AsyAdoVehicleDetails_SequenceNo")]
	[Index(nameof(Vin), Name = "IX_AsyAdoVehicleDetails_Vin")]
	[Index(nameof(VehicleId), Name = "IX_AsyAdoVehicleDetails_VehicleId")]
	public class AsyAdoVehicleDetails : FullAuditedEntity<long>, IEntity<long>
	{

        public const int MaxBodyNoLength = 10;

        public const int MaxLotNoLength = 20;

        public const int MaxSequenceNoLength = 25;

        public const int MaxKeyNoLength = 255;

        public const int MaxVinLength = 25;

        public const int MaxEngLength = 255;

        public const int MaxTrsLength = 255;

        public const int MaxEcuLength = 20;

        public const int MaxPaintInTimeLength = 10;

        public const int MaxDriverAirBagLength = 20;

        public const int MaxPassengerAirBagLength = 20;

        public const int MaxSideAirBagLhLength = 20;

        public const int MaxSideAirBagRhLength = 20;

        public const int MaxKneeAirBagLhLength = 20;

        public const int MaxCurtainSideAirBagLhLength = 255;

        public const int MaxCurtainSideAirBagRhLength = 255;

        public const int MaxTotalDelayLength = 10;

        public const int MaxVehicleIdLength = 10;

        public const int MaxTestNoLength = 10;

        public const int MaxIsPrintedQrcodeLength = 1;

        public const int MaxIsProjectLength = 1;

        public const int MaxCfcLength = 4;

    //    public const int MaxVinNoLength = 18;

        public const int MaxEngineIdLength = 12;

        public const int MaxTransIdLength = 18;

        public const int MaxSalesSfxLength = 2;

        public const int MaxColorTypeLength = 4;

        public const int MaxIndentLineLength = 3;

        public const int MaxSsNoLength = 2;

        public const int MaxEdNumberLength = 5;

        public const int MaxGoshiCarLength = 1;




        public virtual long? No { get; set; }

		[Required]
		[StringLength(MaxBodyNoLength)]
		public virtual string BodyNo { get; set; }

		[Required]
		[StringLength(MaxLotNoLength)]
		public virtual string LotNo { get; set; }

		[Required]
		public virtual int? NoInLot { get; set; }

		[StringLength(MaxSequenceNoLength)]
		public virtual string SequenceNo { get; set; }

		[StringLength(MaxKeyNoLength)]
		public virtual string KeyNo { get; set; }

		[StringLength(MaxVinLength)]
		public virtual string Vin { get; set; }

		[StringLength(MaxEngLength)]
		public virtual string Eng { get; set; }

		[StringLength(MaxTrsLength)]
		public virtual string Trs { get; set; }

		[StringLength(MaxEcuLength)]
		public virtual string Ecu { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? WInDateActual { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? TInPlanDatetime { get; set; }

		[StringLength(MaxPaintInTimeLength)]
		public virtual string PaintInTime { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? AInDateActual { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? InsOutDateActual { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? InsLineOutVp4DateActual { get; set; }

		[StringLength(MaxDriverAirBagLength)]
		public virtual string DriverAirBag { get; set; }

		[StringLength(MaxPassengerAirBagLength)]
		public virtual string PassengerAirBag { get; set; }

        [StringLength(MaxSideAirBagLhLength)]
        public virtual string SideAirBagLh { get; set; }

        [StringLength(MaxSideAirBagRhLength)]
        public virtual string SideAirBagRh { get; set; }

        [StringLength(MaxKneeAirBagLhLength)]
        public virtual string KneeAirBagLh { get; set; }

        [StringLength(MaxCurtainSideAirBagLhLength)]
        public virtual string CurtainSideAirBagLh { get; set; }

        [StringLength(MaxCurtainSideAirBagRhLength)]
        public virtual string CurtainSideAirBagRh { get; set; }

        [StringLength(MaxTotalDelayLength)]
		public virtual string TotalDelay { get; set; }

		public virtual DateTime? ShippingTime { get; set; }

		[StringLength(MaxVehicleIdLength)]
		public virtual string VehicleId { get; set; }

		[StringLength(MaxTestNoLength)]
		public virtual string TestNo { get; set; }

		[StringLength(MaxIsPrintedQrcodeLength)]
		public virtual string IsPrintedQrcode { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? PrintedQrcodeDate { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? UpdatedDate { get; set; }

		[StringLength(MaxIsProjectLength)]
		public virtual string IsProject { get; set; }

        //new 2023/05/26

        [StringLength(MaxCfcLength)]
        public virtual string Cfc { get; set; }

        //[StringLength(MaxVinNoLength)]
        //public virtual string VinNo { get; set; }

        [StringLength(MaxEngineIdLength)]
        public virtual string EngineId { get; set; }

        [StringLength(MaxTransIdLength)]
        public virtual string TransId { get; set; }

        [StringLength(MaxSalesSfxLength)]
        public virtual string SalesSfx { get; set; }

        [StringLength(MaxColorTypeLength)]
        public virtual string ColorType { get; set; }

        [StringLength(MaxIndentLineLength)]
        public virtual string IndentLine { get; set; }

        [StringLength(MaxSsNoLength)]
        public virtual string SsNo { get; set; }

        [StringLength(MaxEdNumberLength)]
        public virtual string EdNumber { get; set; }

        [StringLength(MaxGoshiCarLength)]
        public virtual string GoshiCar { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? LineoffDatetime { get; set; }

        [Column(TypeName = "date")]
        public virtual DateTime? LineoffDate { get; set; }

        [Column(TypeName = "time(7)")]
        public virtual TimeSpan? LineoffTime { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? PdiDatetime { get; set; }

        [Column(TypeName = "date")]
        public virtual DateTime? PdiDate { get; set; }

        [Column(TypeName = "time(7)")]
        public virtual TimeSpan? PdiTime { get; set; }


    }
}




