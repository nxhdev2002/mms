using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;

namespace prod.SapIF
{
    public class SapIFFundCommitmentHeader : FullAuditedEntity<long>, IEntity<long>
    {
        public string FundCommitmentHeaderType { get; set; }
        public long FundCommitmentHeaderId { get; set; }
        public string DocumentType { get; set; }
        public string Action { get; set; }
        public string System { get; set; }
        public string TestRun { get; set; }
        public string DocumentNo { get; set; }
        public string Closed { get; set; }
        public DateTime? DocumentDate { get; set; }        
        public string Requestor { get; set; }
        public string CompanyCode { get; set; }
        public string Currency { get; set; }
        public string CurrencyRate { get; set; }
    }

    public class SapIFFundCommitmentHeaderDto : SapIFFundCommitmentHeader
    {
        public string SqlMessage { get; set; }
    }
}
