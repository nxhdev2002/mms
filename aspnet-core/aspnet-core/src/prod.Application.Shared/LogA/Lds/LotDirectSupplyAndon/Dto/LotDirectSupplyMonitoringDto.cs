using System;
using System.Collections.Generic;
using System.Text;

namespace prod.LogA.Lds.Dto
{
    public class LotDirectSupplyMonitoringDto
    {
		public virtual string Title { get; set; }
		public virtual int? TotalCycle { get; set; }
		public virtual int? SequenceNo { get; set; }
		public virtual int? Efficiency { get; set; }
		public virtual int? DelaySecond { get; set; }
		public virtual string IsDelay { get; set; }
		public virtual int? SeqLineIn { get; set; }
		public virtual int? TaktTime { get; set; }
		public virtual string IsPaused { get; set; }
		public virtual string IsStoped { get; set; }
		public virtual string Status { get; set; }

	}
}
