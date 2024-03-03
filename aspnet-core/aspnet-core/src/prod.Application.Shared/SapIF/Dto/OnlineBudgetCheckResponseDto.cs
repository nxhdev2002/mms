using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace prod.SapIF.Dto
{
    [Display(Name = "OnlineBudgetCheckRes_MT")]
    public class OnlineBudgetCheckResponseDto
    {
        [Display(Name = "RESPONSE")]
        public OnlineBudgetCheckResponse Response { get; set; }
    }
    public class OnlineBudgetCheckResponse
    {
        [Display(Name = "BUDGETCHECK")]
        public OnlineBudgetCheckAvailableBudget AvailableBudget { get; set; }
        [Display(Name = "DATAVALIDATION")]
        public OnlineBudgetCheckDataValidation DataValidation { get; set; }
    }
    public class OnlineBudgetCheckAvailableBudget
    {
        [Display(Name = "wbs_master_data")]
        public string AvailableBudgetWBSMasterData { get; set; }
        [Display(Name = "fiscal_year")]
        public int AvailableBudgetFiscalYear { get; set; }
        [Display(Name = "available_amount")]
        public decimal AvailableBudgetAvailableAmount { get; set; }
        [Display(Name = "message_type")]
        public string AvailableBudgetMessageType { get; set; }
        [Display(Name = "message_id")]
        public string AvailableBudgetMessageID { get; set; }
        [Display(Name = "message_no")]
        public string AvailableBudgetMessageNo { get; set; }
        [Display(Name = "message")]
        public string AvailableBudgetMessage { get; set; }
    }
    public class OnlineBudgetCheckDataValidation
    {
        [Display(Name = "wbs_master_data")]
        public string DataValidationWBSMasterData { get; set; }
        [Display(Name = "cost_center")]
        public string DataValidationCostCenter { get; set; }
        [Display(Name = "fixed_asset_no")]
        public string DataValidationFixedAssetNo { get; set; }
        [Display(Name = "result")]
        public string DataValidationResult { get; set; }
        [Display(Name = "message_type")]
        public string DataValidationMessageType { get; set; }
        [Display(Name = "message_id")]
        public string DataValidationMessageID { get; set; }
        [Display(Name = "message_no")]
        public string DataValidationMessageNo { get; set; }
        [Display(Name = "message")]
        public string DataValidationMessage { get; set; }
    }
}
