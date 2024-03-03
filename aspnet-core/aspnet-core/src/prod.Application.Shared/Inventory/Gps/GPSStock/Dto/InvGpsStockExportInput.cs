using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.GPS.Dto
{

    public class InvGpsStockExportInput
    {

        public virtual string PartNo { get; set; }

        public virtual DateTime? WorkingDateFrom { get; set; }

        public virtual DateTime? WorkingDateTo { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string VinNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string LotNo { get; set; }

        public virtual int? NoInLot { get; set; }

    }

}


