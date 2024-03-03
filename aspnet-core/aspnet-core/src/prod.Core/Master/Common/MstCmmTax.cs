using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Cmm
{

    [Table("MstCmmTax")]
    public class MstCmmTax : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxCodeLength = 10;

        public const int MaxDescriptionLength = 200;

        public const int MaxRateLength = 10;

        [StringLength(MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MaxDescriptionLength)]
        public virtual string Description { get; set; }

        [StringLength(MaxRateLength)]
        public virtual string Rate { get; set; }
    }

}
