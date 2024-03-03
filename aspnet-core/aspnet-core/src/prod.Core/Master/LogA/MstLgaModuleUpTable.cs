using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Castle.MicroKernel.Registration;
using Microsoft.EntityFrameworkCore;
using PayPalCheckoutSdk.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace prod.Master.LogA
{
    [Table("MstLgaModuleUpTable")]
    [Index(nameof(Line), Name = "IX_MstLgaModuleUpTable_Line")]
    [Index(nameof(UpTable), Name = "IX_MstLgaModuleUpTable_UpTable")]
    [Index(nameof(IsActive), Name = "IX_MstLgaBp2Ecar_IsActive")]

    public class MstLgaModuleUpTable : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxLineLength = 10;
        public const int MaxUpTableLength = 20;
        public const int MaxIsActiveLength = 1;

        [StringLength(MaxLineLength)]
        public virtual string Line { get; set; }

        [StringLength(MaxUpTableLength)]
        public virtual string UpTable { get; set; }

        public virtual int? DisplayOrder { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }
}
