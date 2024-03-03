using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Inv.Dto
{

    public class MstInvGpsCalendarExportInput
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


