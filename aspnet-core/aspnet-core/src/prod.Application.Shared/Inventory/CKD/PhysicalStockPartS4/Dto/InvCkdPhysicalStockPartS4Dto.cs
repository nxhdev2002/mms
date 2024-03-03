using Abp.Application.Services.Dto;
using prod.Inventory.CKD;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inv.CKD.Dto
{
    public class InvCkdPhysicalStockPartS4Dto : EntityDto<long?>
    {

        public virtual string MaterialCode { get; set; }

        public virtual string MaterialCodeS4 { get; set; }

        public virtual long? PeriodId { get; set; }

        public virtual int? Qty { get; set; }

        public virtual int QtyS4 { get; set; }

        public virtual DateTime? FromDate { get; set; }

        public virtual DateTime? ToDate { get; set; }

        public virtual string Description { get; set; }

        public virtual int Diff { get; set; }

        public virtual string Guid { get; set; }

        public virtual string ErrorDescription { get; set; }

        

    }

    public class GetInvCkdPhysicalStockPartS4Input : PagedAndSortedResultRequestDto
    {

        public virtual string MaterialCode { get; set; }

        public virtual int? PeriodId { get; set; }


    }

}