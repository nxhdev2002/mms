using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.LogA.Sps.Dto
{

    public class LgaSpsStockDto : EntityDto<long?>
    {

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string BackNo { get; set; }

        public virtual string SpsRackAddress { get; set; }

        public virtual string PcRackAddress { get; set; }

        public virtual int? RackCapBox { get; set; }

        public virtual string PcPicKingMember { get; set; }

        public virtual int? EkbQty { get; set; }

        public virtual int? StockQty { get; set; }

        public virtual int? BoxQty { get; set; }

        public virtual string Process { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditLgaSpsStockDto : EntityDto<long?>
    {

        [StringLength(LgaSpsStockConsts.MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(LgaSpsStockConsts.MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(LgaSpsStockConsts.MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(LgaSpsStockConsts.MaxBackNoLength)]
        public virtual string BackNo { get; set; }

        [StringLength(LgaSpsStockConsts.MaxSpsRackAddressLength)]
        public virtual string SpsRackAddress { get; set; }

        [StringLength(LgaSpsStockConsts.MaxPcRackAddressLength)]
        public virtual string PcRackAddress { get; set; }

        public virtual int? RackCapBox { get; set; }

        [StringLength(LgaSpsStockConsts.MaxPcPicKingMemberLength)]
        public virtual string PcPicKingMember { get; set; }

        public virtual int? EkbQty { get; set; }

        public virtual int? StockQty { get; set; }

        public virtual int? BoxQty { get; set; }

        [StringLength(LgaSpsStockConsts.MaxProcessLength)]
        public virtual string Process { get; set; }

        [StringLength(LgaSpsStockConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetLgaSpsStockInput : PagedAndSortedResultRequestDto
    {

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string BackNo { get; set; }

        public virtual string SpsRackAddress { get; set; }

        public virtual string PcRackAddress { get; set; }

        public virtual int? RackCapBox { get; set; }

        public virtual int? PcPickingMember { get; set; }

        public virtual int? EkbQty { get; set; }

        public virtual int? StockQty { get; set; }

        public virtual int? BoxQty { get; set; }

        public virtual string Process { get; set; }

        public virtual string IsActive { get; set; }

    }

}


