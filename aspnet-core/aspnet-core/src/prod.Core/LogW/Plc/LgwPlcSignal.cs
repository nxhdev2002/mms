using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PayPalCheckoutSdk.Orders;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace prod.LogW.Plc
{
    [Table("LgwPlcSignal")]
    [Index(nameof(SignalPattern), Name = "IX_LgwPlcSignal_PickingTabletId")]
    [Index(nameof(Process), Name = "IX_LgwPlcSignal_TabletProcessId")]
    [Index(nameof(IsActive), Name = "IX_LgwPikPickingSignal_IsActive")]
    public class LgwPlcSignal : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxSignalPatternLength = 200;

        public const int MaxProdLineLength = 50;

        public const int MaxProcessLength = 50;

        public const int MaxIsActiveLength = 1;


        public virtual int? SignalIndex { get; set; }

        [StringLength(MaxSignalPatternLength)]
        public virtual string SignalPattern { get; set; }

        [Column(TypeName = "date")]
        public virtual DateTime? SignalTime { get; set; }

        [StringLength(MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        [StringLength(MaxProcessLength)]
        public virtual string Process { get; set; }

        public virtual long? RefId { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }
}
