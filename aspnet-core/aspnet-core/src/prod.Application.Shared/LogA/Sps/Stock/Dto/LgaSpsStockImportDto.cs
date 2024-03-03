using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using prod.LogA.Pcs.Stock;

namespace prod.LogA.Sps.Stock.Dto
{
    public class LgaSpsStockImportDto : EntityDto<long?>
    {
        [StringLength(LgaSpsStockConsts.MaxGuidLength)]
        public virtual string Guid { get; set; }

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
        public string Exception { get; set; }
        public bool CanBeImported()
        {
            return string.IsNullOrEmpty(Exception);
        }
    }
}
