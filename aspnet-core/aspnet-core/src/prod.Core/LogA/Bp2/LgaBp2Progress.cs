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
using System.Numerics;

namespace prod.LogA.Bp2
{
    [Table("LgaBp2Progress")]   
    [Index(nameof(ProdLine), Name = "IX_LgaBp2Progress_Shift")]
    [Index(nameof(WorkingDate), Name = "IX_LgaBp2Progress_WorkingDate")]
    [Index(nameof(Status), Name = "IX_LgaBp2Progress_Status")]
    [Index(nameof(IsActive), Name = "IX_LgaBp2Progress_IsActive")]
    public class LgaBp2Progress : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxProdLineLength = 50;

        public const int MaxShiftLength = 50;

        public const int MaxStatusLength = 50;

        public const int MaxIsActiveLength = 1;


        public virtual long? ProcessId { get; set; }
        public virtual int? EcarId { get; set; }

        [StringLength(MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        [Column(TypeName = "date")]
        public virtual DateTime? WorkingDate { get; set; }

        [StringLength(MaxShiftLength)]
        public virtual string Shift { get; set; }
        public virtual int? NoInShift { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? NewtaktDatetime { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? StartDatetime { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? FinishDatetime { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }
}
