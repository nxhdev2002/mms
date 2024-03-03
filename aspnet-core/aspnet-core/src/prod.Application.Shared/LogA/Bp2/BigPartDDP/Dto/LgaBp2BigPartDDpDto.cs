using Abp.Application.Services.Dto;
using System; 

namespace prod.LogA.Bp2.Dto
{
    public class LgaBp2BigPartDDpDto
    {
        public virtual string Title { get; set; }      
        public virtual string ProdLine { get; set; }
        public virtual DateTime? WorkingDate { get; set; }
        public virtual string Shift { get; set; }
        public virtual string EcarName { get; set; }
        public virtual string Code { get; set; }
        public virtual string ProcessName { get; set; }
        public virtual int? Sorting { get; set; }
        public virtual int? ActualVolCount { get; set; }
        public virtual int? PlanVolCount { get; set; }
        public virtual int? DelaySecond { get; set; }
        public virtual int? ActualTrim { get; set; }
        public virtual int? PlanTrim { get; set; }
        public virtual DateTime? StartDatetime { get; set; }
        public virtual DateTime? FinishDatetime { get; set; }
        public virtual int? TotalTaktTime { get; set; }

        public virtual int? ProcessTaktTime { get; set; }

        public virtual float ProcessActualTime { get; set; }

        public virtual DateTime? NewTaktStartTime { get; set; }

        public virtual string IsDelayStart { get; set; }

        public virtual string ScreenStatus { get; set; }

        public virtual string Status { get; set; }

        public virtual int? NTSignalCount { get; set; }

        public virtual int? DelayStartSecond { get; set; }

        public virtual int? NewTaktOffset { get; set; }

        public virtual int? RemainingTime { get; set; }

        public virtual int? EcarCount { get; set; }
    }


    public class BigPartTabletAndonOutput
    {
        
        public virtual long? PartId { get; set; }
        public virtual string Title { get; set; }
        public virtual string ProdLine { get; set; }
        public virtual string PartName { get; set; }
        public virtual string ShortName { get; set; }

        public virtual DateTime? WorkingDate { get; set; }
        public virtual string Shift { get; set; }
        public virtual string ScreenStatus { get; set; }
        public virtual string Status { get; set; }
        public virtual int? ActualVolCount { get; set; }
        public virtual int? PlanVolCount { get; set; }
        public virtual int? TotalTaktTime { get; set; }
        public virtual string EcarId { get; set; }
        public virtual string ProgressId { get; set; }
        public virtual string ButtonName { get; set; }
        public virtual string IsCallLeader { get; set; }

        public virtual DateTime? NewtaktDatetime { get; set; }
        public virtual DateTime? StartDatetime { get; set; }
        public virtual DateTime? FinishDatetime { get; set; }
        public virtual int? ActualTaktTime { get; set; }
        public virtual int? ProcessTatkTime { get; set; }

        public virtual string EcarName { get; set; }
        public virtual string ProcessCode { get; set; }
        public virtual string ProcessName { get; set; }
        public virtual int? ProcessSorting { get; set; }
        public virtual string ProcessType { get; set; }

        public virtual string DelAddress { get; set; }
        public virtual string PikAddress { get; set; }
        public virtual string DelAddress2 { get; set; }
        public virtual string PikAddress2 { get; set; }
        public virtual string Remark { get; set; }
        public virtual string Remark2 { get; set; }

        public virtual string RefNo { get; set; }
        public virtual string RefNo2 { get; set; }

        public virtual int? MaxPartPerPage { get; set; }

        public virtual int? ItemCount { get; set; }
        public virtual int? TotalItem { get; set; }

        public virtual int? EcarCount { get; set; }




    }

    public class BigPartTabletEcarOutput : EntityDto<long?>
    {
        public virtual string Code { get; set; }
        public virtual string EcarName { get; set; }
        public virtual string ProdLine { get; set; }
        public virtual string EcarType { get; set; }
        public virtual int? Sorting { get; set; }
        public virtual string IsActive { get; set; }
    }
}
