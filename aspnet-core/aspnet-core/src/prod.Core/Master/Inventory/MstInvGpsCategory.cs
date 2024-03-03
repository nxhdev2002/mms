using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace prod.Master.Inventory
{
    [Table("MstInvGpsCategory")]
    public class MstInvGpsCategory : FullAuditedEntity<long>, IEntity<long>
    {

            public const int MaxCodeLength = 10;

            public const int MaxNameLength = 50;

            [StringLength(MaxCodeLength)]
            public virtual string Code { get; set; }

            [StringLength(MaxNameLength)]
            public virtual string Name { get; set; }
        
    }
}
