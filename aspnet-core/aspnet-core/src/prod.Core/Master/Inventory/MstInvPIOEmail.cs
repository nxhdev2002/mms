using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Inventory
{
    [Table("MstInvPIOEmail")]
    public class MstInvPIOEmail : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxSubjectLength = 200;

        public const int MaxToLength = 1000;

        public const int MaxCcLength = 1000;

        public const int MaxBodyMessLength = 5000;

        public const int MaxSupplierCdLength = 200;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxSubjectLength)]
        public virtual string Subject { get; set; }

        [StringLength(MaxToLength)]
        public virtual string To { get; set; }

        [StringLength(MaxCcLength)]
        public virtual string Cc { get; set; }

        [StringLength(MaxBodyMessLength)]
        public virtual string BodyMess { get; set; }

        [StringLength(MaxSupplierCdLength)]
        public virtual string SupplierCd { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}