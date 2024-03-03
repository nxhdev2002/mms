using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CKD.Dto
{

    public class InvCkdPhysicalStockPeriodDto : EntityDto<long?>
    {

        public virtual string Description { get; set; }

        public virtual DateTime? FromDate { get; set; }

        public virtual string FromTime { get; set; }

        public virtual DateTime? ToDate { get; set; }

        public virtual string ToTime { get; set; }

        public virtual int? Status { get; set; }

        public virtual string InfoPeriod { get; set; }

    }

    public class CreateOrEditInvCkdPhysicalStockPeriodDto : EntityDto<long?>
    {

        [StringLength(InvCkdPhysicalStockPeriodConsts.MaxDescriptionLength)]
        public virtual string Description { get; set; }

        public virtual DateTime? FromDate { get; set; }

        public virtual string FromTime { get; set; }

        public virtual DateTime? ToDate { get; set; }

        public virtual string ToTime { get; set; }

        public virtual int? Status { get; set; }
    }

    public class GetInvCkdPhysicalStockPeriodInput : PagedAndSortedResultRequestDto
    {

        public virtual string Description { get; set; }

        public virtual DateTime? FromDate { get; set; }

        public virtual DateTime? ToDate { get; set; }


    }

    public class GetInvCkdPhysicalStockPeriodPeriosInput 
    {

        public virtual string Description { get; set; }

        public virtual DateTime? FromDate { get; set; }

        public virtual string FromTime { get; set; }

        public virtual DateTime? ToDate { get; set; }

        public virtual string ToTime { get; set; }


    }

}

