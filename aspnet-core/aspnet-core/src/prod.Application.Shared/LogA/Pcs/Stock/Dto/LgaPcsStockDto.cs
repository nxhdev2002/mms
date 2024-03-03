using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
using prod.LogA.Pcs.Stock;

namespace prod.LogA.Pcs.Dto
{

    public class LgaPcsStockDto : EntityDto<long?>
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

    public class CreateOrEditLgaPcsStockDto : EntityDto<long?>
    {

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
    }

    public class GetLgaPcsStockInput : PagedAndSortedResultRequestDto
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


