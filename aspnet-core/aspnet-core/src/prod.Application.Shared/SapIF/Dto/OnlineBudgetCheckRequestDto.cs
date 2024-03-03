using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace prod.SapIF.Dto
{
    [Display(Name = "OnlineBudgetCheckReq_MT")]
    public class OnlineBudgetCheckRequestDto
    {
        [Display(Name = "REQUEST")]
        public OnlineBudgetCheckRequest Request { get; set; }
    }
    public class OnlineBudgetCheckRequest
    {
        [Display(Name = "wbs_master_data"), MaxLength(24), Required]
        public string WBSMasterData { get; set; }
        [Display(Name = "fiscal_year"), MaxLength(4), Required]
        public string FiscalYear { get; set; }
        [Display(Name = "cost_center"), MaxLength(10), Required]
        public string CostCenter { get; set; }
        [Display(Name = "fixed_asset_no"), MaxLength(17)]
        public string FixedAssetNo { get; set; }
        [Display(Name = "system"), MaxLength(10)]
        public string System { get; set; }
        public List<long> ListItemId { get; set; }

        public int Index1 { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
