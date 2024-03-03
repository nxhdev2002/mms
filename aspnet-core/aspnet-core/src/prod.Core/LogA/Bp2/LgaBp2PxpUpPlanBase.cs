using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Castle.MicroKernel.SubSystems.Conversion;
using Microsoft.EntityFrameworkCore;
using PayPalCheckoutSdk.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.LogA.Bp2
{

    [Table("LgaBp2PxpUpPlanBase")]
    [Index(nameof(WorkingDate), Name = "IX_LgaBp2PxpUpPlanBase_WorkingDate")]
    [Index(nameof(ProdLine), Name = "IX_LgaBp2PxpUpPlanBase_ProdLine")]
    [Index(nameof(IsActive), Name = "IX_LgaBp2PxpUpPlanBase_IsActive")]
    public class LgaBp2PxpUpPlanBase : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxProdLineLength = 50;

        public const int MaxIsActiveLength = 1;

        [Column(TypeName = "date")]
        public virtual DateTime? WorkingDate { get; set; }

        [StringLength(MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        public virtual int? Shift1 { get; set; }

        public virtual int? Shift2 { get; set; }

        public virtual int? Shift3 { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }
}




