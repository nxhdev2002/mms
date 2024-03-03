using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Welding.Andon.PunchQueueIndicator.Dto
{
    public class WldAdoPunchQueueIndicatorDto
    {
        public virtual string Filler { get; set; }
        public int Seq { get; set; }
        public string ProcessCd { get; set; }
        public string BodyNo { get; set; }
        public string Mode { get; set; }
        public string Line { get; set; }
        public virtual string LotNo { get; set; }
        public virtual string Color { get; set; }
        public int ProcessGroup { get; set; }
        public DateTime? ScanTime { get; set; }
        public virtual string PunchFlag { get; set; }
        public virtual string PunchIndicator { get; set; }
        public int WTalkTime { get; set; }
        public virtual string WeldSignal { get; set; }
    }

    public class WldAdoPunchQueueIndicatorV2Dto
    {
        public virtual string Filler { get; set; }
        public int Seq { get; set; }
        public string ProcessCd { get; set; }
        public string BodyNo { get; set; }
        public string Mode { get; set; }
        public string Line { get; set; }
        public virtual string LotNo { get; set; }
        public virtual string Color { get; set; }
        public int ProcessGroup { get; set; }
        public DateTime? ScanTime { get; set; }
        public virtual string PunchFlag { get; set; }
        public virtual string PunchIndicator { get; set; }
        public int WTalkTime { get; set; }
        public virtual string WeldSignal { get; set; }
    }
}
