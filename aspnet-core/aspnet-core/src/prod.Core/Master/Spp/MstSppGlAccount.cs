using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using PayPalCheckoutSdk.Orders;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Spp
{

    [Table("MstSppGlAccount")]
    [Index(nameof(GlAccountNo), Name = "IX_MstSppGlAccount_GlAccountNo")]
    [Index(nameof(GlType), Name = "IX_MstSppGlAccount_GlType")]

    public class MstSppGlAccount : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxGlAccountNoLength = 100;

        public const int MaxGlAccountNoS4Length = 100;

        public const int MaxGlTypeLength = 100;

        public const int MaxGlDescEnLength = 200;

        public const int MaxGlDescLength = 200;

        public const int MaxCrDbLength = 1;

        [StringLength(MaxGlAccountNoLength)]
        public virtual string GlAccountNo { get; set; }

        [StringLength(MaxGlAccountNoS4Length)]
        public virtual string GlAccountNoS4 { get; set; }

        [StringLength(MaxGlTypeLength)]
        public virtual string GlType { get; set; }

        public virtual DateTime StartDate { get; set; }

        public virtual DateTime EndDate { get; set; }

        [StringLength(MaxGlDescEnLength)]
        public virtual string GlDescEn { get; set; }

        [StringLength(MaxGlDescLength)]
        public virtual string GlDesc { get; set; }

        [StringLength(MaxCrDbLength)]
        public virtual string CrDb { get; set; }
    }

}

