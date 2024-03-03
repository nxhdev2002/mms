using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace prod.Master.Inv
{
    [Table("MstInvGpsSupplierPic")]
    public class MstInvGpsSupplierPic : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxPicNameLength = 40;

        public const int MaxPicTelephoneLength = 100;

        public const int MaxPicEmailLength = 100;

        public const int MaxIsMainPicLength = 50;

        public const int MaxIsSendEmailLength = 1;

        public const int MaxIsActiveLength = 1;

        public virtual long? SupplierId { get; set; }

        [StringLength(MaxPicNameLength)]
        public virtual string PicName { get; set; }

        [StringLength(MaxPicTelephoneLength)]
        public virtual string PicTelephone { get; set; }

        [StringLength(MaxPicEmailLength)]
        public virtual string PicEmail { get; set; }

        [StringLength(MaxIsMainPicLength)]
        public virtual string IsMainPic { get; set; }

        [StringLength(MaxIsSendEmailLength)]
        public virtual string IsSendEmail { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }
}
