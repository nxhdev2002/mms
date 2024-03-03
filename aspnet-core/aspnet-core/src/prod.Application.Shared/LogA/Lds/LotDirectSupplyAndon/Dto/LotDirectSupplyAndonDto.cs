using System;
namespace prod.LogA.Lds.Dto
{
	public class LotDirectSupplyAndonDto
	{
		public virtual string Title { get; set; }
		public virtual int? DelaySecond { get; set; }
		public virtual int? ActualTrim { get; set; }
		public virtual int? PlanTrim { get; set; }
		public virtual string Shift { get; set; }
		public virtual int? SeqLineIn { get; set; }
		public virtual int? Trip { get; set; }
		public virtual string Dolly { get; set; }
		public virtual DateTime? StartDatetime { get; set; }
		public virtual DateTime? FinishDatetime { get; set; }
		public virtual string NextDolly { get; set; }
		public virtual int? TotalTaktTime { get; set; }
		public virtual int? TripTatkTime { get; set; }
		public virtual int? TripActualTime { get; set; }
		public virtual DateTime? NewTaktStartTime { get; set; }
		public virtual string IsDelayStart { get; set; }
		public virtual string ScreenStatus { get; set; }
		public virtual string Status { get; set; }
		public virtual int? TotalTrip { get; set; }

		public virtual int? PlanVolCount { get; set; }
		public virtual int? NTSignalCount { get; set; }


	}	
}


