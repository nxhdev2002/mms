using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CKD.Dto
{
    public class InvCkdPhysicalStockPartExportInput
    {
        public virtual string PartNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string ColorSfx { get; set; }

        public virtual long? PeriodId { get; set; }

        public virtual string LotNo { get; set; }

        public virtual int? p_mode { get; set; }


    }

}

