using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace prod.LogA.Ekb
{

    public class LgaEkbProgressScreenDto : EntityDto<long?>
    {

        public virtual string PickingUser { get; set; }
        public virtual string ProdLine { get; set; }
        public virtual DateTime? WorkingDate { get; set; }
        public virtual string Shift { get; set; }
        public virtual int? KanbanNoInDate { get; set; }
        public virtual string  KanbanSeq { get; set; }
        public virtual string PartNo { get; set; }
        public virtual string BackNo { get; set; }
        public virtual string PcAddress { get; set; }
        public virtual string SpsAddress { get; set; }
        public virtual int? Sorting { get; set; }
        public virtual DateTime? PikStartDatetime { get; set; }
        public virtual DateTime? PikFinishDatetime { get; set; }
        public virtual int? RequestQty { get; set; }
        public virtual int? InputQty { get; set; }
        public virtual int? ScanQty { get; set; }
        public virtual int? Qty { get; set; }
        public virtual DateTime? NewtaktDatetime { get; set; }
        public virtual string ProcessCode { get; set; }
        public virtual string ProcessName { get; set; }
        public virtual int? ItemCount { get; set; }
        public virtual int? TotalItem { get; set; }
        public virtual int? ActualTime { get; set; }
        public virtual int? TaktTime { get; set; }
        public virtual int? DelaySecond { get; set; }
        public virtual string IsPaused { get; set; }
        public virtual string IsStoped { get; set; }
        public virtual string IsCallLeader { get; set; }

        public virtual int? PartSequence { get; set; } 
        public virtual string Status { get; set; } // NEWTAKT / STARTED/ FINISHED / COMPLETED / exclude NULL ==> Neu FINISHED hoac COMPLETED ma khong Delay => Xanh la cay
         
    }

}

