using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Inventory
{
    [Table("InvLotPart")]
    public class MstInvLotPart : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxPartNoLength = 12;

        public const int MaxSourceLength = 10;

        public const int MaxCarfamilyCodeLength = 4;

        public const int MaxCarfamilyNameLength = 30;

        public const int MaxLineCodeLength = 2;

        public const int MaxColorLength = 10;

        public const int MaxPartNameLength = 500;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxPartNoLength)]
        public virtual string Part_No { get; set; }

        [StringLength(MaxSourceLength)]
        public virtual string Source { get; set; }

        [StringLength(MaxCarfamilyCodeLength)]
        public virtual string Carfamily_Code { get; set; }

        [StringLength(MaxCarfamilyNameLength)]
        public virtual string Carfamily_Name { get; set; }

        [StringLength(MaxLineCodeLength)]
        public virtual string Line_Code { get; set; }

        [StringLength(MaxColorLength)]
        public virtual string Color { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string Part_Name { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string Active { get; set; }
    }
}
