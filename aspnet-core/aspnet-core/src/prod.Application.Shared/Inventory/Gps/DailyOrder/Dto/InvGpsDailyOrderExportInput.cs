using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.GPS.Dto
{

    public class InvGpsDailyOrderExportInput
    {
        public virtual DateTime? WorkingDateFrom { get; set; }

        public virtual DateTime? WorkingDateTo { get; set; }

        public virtual string Shift { get; set; }

        public virtual string SupplierCode { get; set; }

        public virtual string OrderNo { get; set; }

        public virtual string TruckNo { get; set; }

    }

}


