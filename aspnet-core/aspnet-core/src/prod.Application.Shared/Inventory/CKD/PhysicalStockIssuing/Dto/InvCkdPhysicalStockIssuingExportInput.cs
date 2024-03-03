using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CKD.Dto
{

    public class InvCkdPhysicalStockIssuingExportInput
    {

        public virtual string PartNo { get; set; }

        public virtual DateTime? WorkingDateFrom { get; set; }

        public virtual DateTime? WorkingDateTo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string ColorSfx { get; set; }

        public virtual string VinNo { get; set; }

        public virtual string LotNo { get; set; }

        public virtual string UseLot { get; set; }

        public virtual int? NoInLot { get; set; }

        public virtual int? p_mode { get; set; }

        public virtual int? PeriodId { get; set; }
    }

}


