using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Spp
{

    [Table("MstSppCustomer")]
    [Index(nameof(Code), Name = "IX_MstSppCustomer_Code")]
    [Index(nameof(Rep), Name = "IX_MstSppCustomer_Rep")]
    [Index(nameof(IsNew), Name = "IX_MstSppCustomer_IsNew")]
    public class MstSppCustomer : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxCodeLength = 25;

        public const int MaxNameLength = 50;

        public const int MaxRepLength = 100;

        public const int MaxIsNewLength = 1;


        [StringLength(MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }

        [StringLength(MaxRepLength)]
        public virtual string Rep { get; set; }

        public virtual int FromMonth { get; set; }

        public virtual int FromYear { get; set; }

        public virtual int ToMonth { get; set; }

        public virtual int ToYear { get; set; }

        public virtual long FromPeriodId { get; set; }

        public virtual long ToPeriodId { get; set; }

        [StringLength(MaxIsNewLength)]
        public virtual string IsNew { get; set; }

        public virtual long OraCustomerId { get; set; }
    }

}

