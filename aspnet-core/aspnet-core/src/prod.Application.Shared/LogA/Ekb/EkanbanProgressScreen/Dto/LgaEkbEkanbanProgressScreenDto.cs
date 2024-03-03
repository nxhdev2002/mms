using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.LogA.Ekb.Dto
{

    public class LgaEkbEkanbanProgressScreenDto : EntityDto<long?>
    {

        public virtual string Title { get; set; }

        public virtual string PickingUser { get; set; }

        public virtual string ProdLine { get; set; }

        public virtual DateTime? WorkingDate { get; set; }    

        public virtual string Shift { get; set; }

        public virtual int? NoInShift { get; set; }

        public virtual int? NoInDate { get; set; }

        public virtual DateTime? NewTaktTime { get; set; }

        public virtual string ProcessCode { get; set; }

        public virtual string ProcessName { get; set; }

        public virtual int? Sorting { get; set; }

        public virtual int? TotalCycle { get; set; }

        public virtual string IsDelay { get; set; }

        public virtual string IsPaused { get; set; }

        public virtual string IsStoped { get; set; }

        public virtual int? SequenceNo { get; set; }

        public virtual int? NumberNo { get; set; }

        public virtual int? Efficiency { get; set; }
        public virtual DateTime? StartDatetime { get; set; }

        public virtual DateTime? FinishDatetime { get; set; }

        public virtual int? DelaySecond { get; set; }

        public virtual int? TaktTime { get; set; }

        public virtual string Status { get; set; }

        //

    }
    
}

