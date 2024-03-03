using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Inv.Dto
{

    public class MstInvGpsCalendarDto : EntityDto<long?>
    {

        public virtual string SupplierCode { get; set; }

        public virtual DateTime? WorkingDate { get; set; }
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }

        public virtual string WorkingType { get; set; }

        public virtual string WorkingStatus { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditMstInvGpsCalendarDto : EntityDto<long?>
    {

        [StringLength(MstInvGpsCalendarConsts.MaxSupplierCodeLength)]
        public virtual string SupplierCode { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        [StringLength(MstInvGpsCalendarConsts.MaxWorkingTypeLength)]
        public virtual string WorkingType { get; set; }

        [StringLength(MstInvGpsCalendarConsts.MaxWorkingStatusLength)]
        public virtual string WorkingStatus { get; set; }

        [StringLength(MstInvGpsCalendarConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetMstInvGpsCalendarInput : PagedAndSortedResultRequestDto
    {

        public virtual string SupplierCode { get; set; }

        public virtual DateTime? WorkingDate { get; set; }
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }
        public virtual string WorkingType { get; set; }

        public virtual string WorkingStatus { get; set; }

        public virtual string IsActive { get; set; }

    }

}
