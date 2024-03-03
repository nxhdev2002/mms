using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace prod.SapIF
{
    public class SapIFLoggingResponseDetailsFundCommitment : FullAuditedEntity<long>, IEntity<long>
    {
        public long ItemId { get; set; }
        public long LoggingId { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentLineItemNo { get; set; }
        public string FundsCommitmentDocument { get; set; }
        public string FundsCommitmentDocumentLineItem { get; set; }
        public string MessageType { get; set; }
        public string MessageID { get; set; }
        public string MessageNo { get; set; }
        public string Message { get; set; }
    }
}
