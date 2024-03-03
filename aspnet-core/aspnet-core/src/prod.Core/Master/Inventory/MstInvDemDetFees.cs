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
    [Table("MstInvDemDetFees")]
    public class MstInvDemDetFees : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxSourceLength = 50;

        public const int MaxCarrierLength = 50;
        public const int MaxIsActiveLength = 1;


        [StringLength(MaxSourceLength)]
        public virtual string Source { get; set; }

        [StringLength(MaxCarrierLength)]
        public virtual string Carrier { get; set; }
        public virtual int? ContType { get; set; }
        public virtual int? NoOfDayOVF { get; set; }
        public virtual decimal? DemFee { get; set; }
        public virtual decimal? DetFee { get; set; }
        public virtual decimal? DemAndDetFee { get; set; }
        public virtual string IsMax { get; set; }
        public virtual DateTime? EffectiveDateFrom { get; set; }
        public virtual DateTime? EffectiveDateTo { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
        public virtual int? MinDay { get; set; }
        public virtual int? MaxDay { get; set; }
    }
}
