using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace prod.SapIF
{
    public class SapIFLoggingResponseDetailsOnlineBudgetCheck : FullAuditedEntity<long>, IEntity<long>
    {
        public long? ItemId { get; set; }
        public long LoggingId { get; set; }
        public string AvailableBudgetWBSMasterData { get; set; }
        public string AvailableBudgetFiscalYear { get; set; }
        public decimal? AvailableBudgetAvailableAmount { get; set; }
        public string AvailableBudgetMessageType { get; set; }
        public string AvailableBudgetMessageID { get; set; }
        public string AvailableBudgetMessageNo { get; set; }
        public string AvailableBudgetMessage { get; set; }
        public string DataValidationWBSMasterData { get; set; }
        public string DataValidationCostCenter { get; set; }
        public string DataValidationFixedAssetNo { get; set; }
        public string DataValidationResult { get; set; }
        public string DataValidationMessageType { get; set; }
        public string DataValidationMessageID { get; set; }
        public string DataValidationMessageNo { get; set; }
        public string DataValidationMessage { get; set; }
    }
}
