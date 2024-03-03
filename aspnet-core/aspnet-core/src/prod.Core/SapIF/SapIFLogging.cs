using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace prod.SapIF
{
    public class SapIFLogging : FullAuditedEntity<long>, IEntity<long>
    {
        public string Type { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public string Exception { get; set; }
        public string DataType { get; set; }
        public long? DataId { get; set; }
    }
}
