using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Inventory.CKD.Dto
{
    public class InvCkdStockBalanceDto : EntityDto<long?>
    {
        public virtual string PartNoNormalizedS4 { get; set; }
        public virtual string Cfc { get; set; }
        public virtual string Source { get; set; }
        public virtual string ColorSfx { get; set; }
        public virtual int? Begining  { get; set; }
        public virtual int? Receiving { get; set; }
        public virtual int? Issuing { get; set; }
        public virtual int? Closing { get; set; }
        public virtual int? Concept { get; set; }
        public virtual int? Diff { get; set; }
        public virtual int? GrandBegining { get; set; }
        public virtual int? GrandReceiving { get; set; }
        public virtual int? GrandIssuing { get; set; }
        public virtual int? GrandClosing { get; set; }
        public virtual int? GrandConcept { get; set; }
        public virtual int? GrandDiff { get; set; }

    }

    public class InvCkdStockBalanceInputDto : PagedAndSortedResultRequestDto
    {
        public virtual string PartNo { get; set; }
        public virtual string ColorSfx { get; set; }
        public virtual string Cfc { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }           
        public virtual bool Diff { get; set; }
       
        //public virtual int? PeriodId { get; set; }

    }

    public class GetStockBalanceExportInput
    {
        public virtual string PartNo { get; set; }
        public virtual string ColorSfx { get; set; }
        public virtual string Cfc { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }
        public virtual bool Diff { get; set; }
    }
}
