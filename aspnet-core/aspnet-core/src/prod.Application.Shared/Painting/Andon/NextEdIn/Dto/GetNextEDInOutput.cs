using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Welding.Andon.NextEdIn.Dto
{
    public class GetNextEDInOutput
    {
        public virtual string BodyNo { get; set; }
        public virtual string ScanLocation { get; set; }
    }
}
