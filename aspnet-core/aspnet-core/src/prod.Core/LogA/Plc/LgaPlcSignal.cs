using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using PayPalCheckoutSdk.Orders;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace prod.LogA.Plc
{
    
        [Table("LgaPlcSignal")]
        [Index(nameof(SignalPattern), Name = "IX_LgaPlcSignal_SignalPattern")]
        [Index(nameof(Process), Name = "IX_LgaPlcSignal_Process")]
        [Index(nameof(IsActive), Name = "IX_LgaPlcSignal_IsActive")]
        public class LgaPlcSignal : FullAuditedEntity<long>, IEntity<long>
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
