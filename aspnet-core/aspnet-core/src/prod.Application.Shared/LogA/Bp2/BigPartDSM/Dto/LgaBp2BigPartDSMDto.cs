using System;


namespace prod.LogA.Bp2.Dto
{
    public class LgaBp2BigPartDSMDto
    {
        public virtual string Title { get; set; }
        public virtual string ProdLine { get; set; }
        public virtual DateTime? WorkingDate { get; set; }
        public virtual string NoInShift { get; set; }
        public virtual string EcarName { get; set; }
        public virtual string Code { get; set; }
        public virtual string ProcessName { get; set; }
        public virtual int? Sorting { get; set; }

        public virtual int? TotalCycle { get; set; }

        public virtual int? SequenceNo { get; set; }
        public virtual int? Efficiency { get; set; }

        public virtual int? DelaySecond { get; set; }

        public virtual int? TaktTime { get; set; }

        public virtual string IsPaused { get; set; }

        public virtual string IsStoped { get; set; }

        public virtual string Status { get; set; }

        public virtual string IsDelay { get; set; }

    }
}
