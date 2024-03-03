using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.LogA.Pcs.Stock.Dto
{
    public class LgaPcsStockImportDto  : EntityDto<long?>
    {

        [StringLength(LgaPcsStockConsts.MaxGuidLength)]
        public virtual string Guid { get; set; }

        [StringLength(LgaPcsStockConsts.MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(LgaPcsStockConsts.MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(LgaPcsStockConsts.MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(LgaPcsStockConsts.MaxBackNoLength)]
        public virtual string BackNo { get; set; }

        [StringLength(LgaPcsStockConsts.MaxPcRackAddressLength)]
        public virtual string PcRackAddress { get; set; }

        public virtual int? UsagePerHour { get; set; }

        public virtual int? RackCapBox { get; set; }

        [StringLength(LgaPcsStockConsts.MaxOutTypeLength)]
        public virtual string OutType { get; set; }

        public virtual int? StockQty { get; set; }

        public virtual int? BoxQty { get; set; }

        [StringLength(LgaPcsStockConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }

        public string Exception { get; set; }
        public bool CanBeImported()
        {
            return string.IsNullOrEmpty(Exception);
        }
    }
}
