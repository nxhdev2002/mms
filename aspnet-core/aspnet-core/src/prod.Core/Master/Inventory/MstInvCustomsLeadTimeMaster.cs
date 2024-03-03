using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace prod.Master.Inventory
{
    [Table("MstInvCustomsLeadTimeMaster")]
    public class MstInvCustomsLeadTimeMaster : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxExporterLength = 50;

        public const int MaxCarrierLength = 50;

        [StringLength(MaxExporterLength)]
        public virtual string Exporter { get; set; }

        [StringLength(MaxCarrierLength)]
        public virtual string Carrier { get; set; }
        public virtual int Leadtime { get; set; }

    }
}
