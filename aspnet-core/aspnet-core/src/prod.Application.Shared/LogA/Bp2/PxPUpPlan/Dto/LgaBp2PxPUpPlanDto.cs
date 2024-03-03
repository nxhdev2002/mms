using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.LogA.Bp2.PxPUpPlan.Dto
{
    public class LgaBp2PxPUpPlanDto : EntityDto<long?>
    {
        public virtual string ProdLine { get; set; }

        public virtual int? NoOfALineIn { get; set; }

        public virtual string UnpackingTime { get; set; }

        public virtual DateTime? UnpackingDate { get; set; }

        public virtual string CaseNo { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string Model { get; set; }

        public virtual int? TotalNoInShift { get; set; }

        public virtual DateTime? UnpackingDatetime { get; set; }

        public virtual DateTime? WorkingDate { get; set; }
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }

        public virtual string Shift { get; set; }

        public virtual string UpTable { get; set; }

        public virtual DateTime? FinishDatetime { get; set; }

        public virtual int? UpLt { get; set; }

        public virtual DateTime? UnpackingStartDatetime { get; set; }

        public virtual DateTime? UnpackingFinishDatetime { get; set; }

        public virtual int? UnpackingSecond { get; set; }

        public virtual string UnpackingBy { get; set; }

        public virtual int? DelaySecond { get; set; }

        public virtual int? TimeOffSecond { get; set; }

        public virtual DateTime? StartPauseTime { get; set; }

        public virtual DateTime? EndPauseTime { get; set; }

        public virtual DateTime? DelayConfirmFlag { get; set; }

        public virtual DateTime? FinishConfirmFlag { get; set; }

        public virtual int? DelayConfirmSecond { get; set; }

        public virtual int? TimeOffConfirmSecond { get; set; }

        public virtual string WhLocation { get; set; }

        public virtual DateTime? InvoiceDate { get; set; }

        public virtual string Remarks { get; set; }

        public virtual string IsNewPart { get; set; }

        public virtual int? Status { get; set; }

        public virtual string IsActive { get; set; }

        public virtual string ScreenStatus { get; set; }
    }
    public class GetLgaBp2PxPUpPlanInput : PagedAndSortedResultRequestDto
    {
        

        public virtual string ProdLine { get; set; }

        public virtual string CaseNo { get; set; }

        public virtual string SupplierNo { get; set; }
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }


    }

    public class GetLgaBp2PxPUpPlanExportInput 
    {
        public virtual DateTime? WorkingDate { get; set; }
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }

        public virtual string ProdLine { get; set; }

        public virtual string CaseNo { get; set; }

        public virtual string SupplierNo { get; set; }

    }

}
