using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.CKD
{
    public class InvCkdStockRundown : FullAuditedEntity<long>, IEntity<long>
    {
        public string PartNo { get; set; }
        public string PartName { get; set; }
        public string  Cfc { get; set; }
        public string Supplier { get; set; }
        public string Qty { get; set; }
        public DateTime? WorkingDate { get; set; }
    }
}
