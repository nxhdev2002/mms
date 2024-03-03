using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Inv
{

    [Table("MstInvGpsScreenSetting")]
    public class MstInvGpsScreenSetting : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxScreenNameLength = 40;

        public const int MaxScreenTypeLength = 40;

        public const int MaxScreenValueLength = 1024;

        public const int MaxDescriptionLength = 255;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxScreenNameLength)]
        public virtual string ScreenName { get; set; }

        [StringLength(MaxScreenTypeLength)]
        public virtual string ScreenType { get; set; }

        [StringLength(MaxScreenValueLength)]
        public virtual string ScreenValue { get; set; }

        [StringLength(MaxDescriptionLength)]
        public virtual string Description { get; set; }

        public virtual int? BarcodeId { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}