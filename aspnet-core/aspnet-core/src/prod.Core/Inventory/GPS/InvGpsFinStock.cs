using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.GPS
{
    public class InvGpsFinStock :  FullAuditedEntity<long>, IEntity<long>
    {
        public string PartNo { get; set; }
        public string PartName { get; set; }
        public long? PartId { get; set; }
        public long? Qty { get; set; }
        public long? Price { get; set; }
        public DateTime? WorkingDate { get; set; }
        public long? TransactionId { get; set; }
        public string Dock { get; set; }
        public string Location { get; set; }
    }
}
