using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.LogA.Pcs.Dto
{

    public class LgaPcsStockExportInput
    {

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string BackNo { get; set; }

        public virtual string PcRackAddress { get; set; }

        public virtual int? UsagePerHour { get; set; }

        public virtual int? RackCapBox { get; set; }

        public virtual string OutType { get; set; }

        public virtual int? StockQty { get; set; }

        public virtual int? BoxQty { get; set; }

        public virtual string IsActive { get; set; }

    }

}


