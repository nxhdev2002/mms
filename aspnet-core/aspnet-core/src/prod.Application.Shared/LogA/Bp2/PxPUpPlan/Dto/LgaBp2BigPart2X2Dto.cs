using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.LogA.Bp2.Dto
{
    public class LgaBp2PxpUpPlanUpCaseDto : EntityDto<long?>
    {
        public virtual string ProdLine { get; set; }
        public virtual int? NoOfALineIn { get; set; }
        public virtual string CaseNo { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual DateTime? WorkingDate { get; set; }
        public virtual string Shift { get; set; }
        public virtual string UpTable { get; set; }
        public virtual int? UpLt { get; set; }
        public virtual string IsNewPart { get; set; }
        public virtual string IsActive { get; set; }
        public virtual TimeSpan? UnpackingTime { get; set; }
        public virtual DateTime? UnpackingDatetime { get; set; }
        public virtual TimeSpan? CycleTime { get; set; }
        public virtual int? CycleSecond { get; set; }
        public virtual string TaktTime { get; set; }
        public virtual int? TaktTimeSecond { get; set; }
        public virtual float TackTime { get; set; }
        public virtual int? MaxNoInDate { get; set; }
        public virtual string WhLocation { get; set; }
        public virtual int? IsCurrentDate { get; set; }
        public virtual float MaxNoOfALineIn { get; set; }    
    }

    public class LgaBp2AQvgGetLastScanDto : EntityDto<long?>
    {
        public virtual DateTime? ScanTime { get; set; }
        public virtual string ProdLine { get; set; }
        public virtual string SequenceNo { get; set; }
        public virtual int? NoInDate { get; set; }
        public virtual DateTime? WorkingDate { get; set; }
        public virtual int? IsManual { get; set; }
        public virtual int? AdjNo { get; set; }
        public virtual DateTime? CreateDate { get; set; }

        public virtual int? DelayConfirmSecond { get; set; }
    }
}
