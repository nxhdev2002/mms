using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace prod.Master.Inv
{

    [Table("MstInvGpsTmvPic")]
    public class MstInvGpsTmvPic : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxPicUserAccountLength = 40;

        public const int MaxPicNameLength = 40;

        public const int MaxPicTelephoneLength = 100;

        public const int MaxPicEmailLength = 100;

        public const int MaxIsMainPicLength = 50;

        public const int MaxPicTelephone2Length = 50;

        public const int MaxSuppliersLength = 2048;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxPicUserAccountLength)]
        public virtual string PicUserAccount { get; set; }

        [StringLength(MaxPicNameLength)]
        public virtual string PicName { get; set; }

        [StringLength(MaxPicTelephoneLength)]
        public virtual string PicTelephone { get; set; }

        [StringLength(MaxPicEmailLength)]
        public virtual string PicEmail { get; set; }

        [StringLength(MaxIsMainPicLength)]
        public virtual string IsMainPic { get; set; }

        [StringLength(MaxPicTelephone2Length)]
        public virtual string PicTelephone2 { get; set; }

        [StringLength(MaxSuppliersLength)]
        public virtual string Suppliers { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}