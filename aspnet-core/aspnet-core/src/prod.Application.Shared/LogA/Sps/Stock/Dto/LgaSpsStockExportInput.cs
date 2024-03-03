using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.LogA.Sps.Dto
{

    public class LgaSpsStockExportInput
    {

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string BackNo { get; set; }

        public virtual string SpsRackAddress { get; set; }

        public virtual string PcRackAddress { get; set; }

        public virtual int? RackCapBox { get; set; }

        public virtual int? PcPicKingMember { get; set; }

        public virtual int? EkbQty { get; set; }

        public virtual int? StockQty { get; set; }

        public virtual int? BoxQty { get; set; }

        public virtual string Process { get; set; }

        public virtual string IsActive { get; set; }

    }

}


