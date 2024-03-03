using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace prod.Inventory.PIO
{
    [Table("InvPioPartGradeInl")]
    public class InvPioPartGradeInl : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxPartNoLength = 50;

        public const int MaxModelLength = 10;

        public const int MaxCfcLength = 4;

        public const int MaxGradeLength = 3;

        public const int MaxIdLineLength = 10;

        public const int MaxShopLength = 1;

        public const int MaxBodyColorLength = 100;

        public const int MaxStartLotLength = 10;

        public const int MaxEndLotLength = 10;

        public const int MaxStartRunLength = 10;

        public const int MaxEndRunLength = 10;

        public const int MaxOrderPatternLength = 10;

        public const int MaxRemarkLength = 5000;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        public virtual long? PartId { get; set; }

        [StringLength(MaxModelLength)]
        public virtual string Model { get; set; }

        [StringLength(MaxCfcLength)]
        public virtual string Cfc { get; set; }

        [StringLength(MaxGradeLength)]
        public virtual string Grade { get; set; }

        [StringLength(MaxIdLineLength)]
        public virtual string IdLine { get; set; }

        [StringLength(MaxShopLength)]
        public virtual string Shop { get; set; }

        [StringLength(MaxBodyColorLength)]
        public virtual string BodyColor { get; set; }

        public virtual decimal? UsageQty { get; set; }

        [StringLength(MaxStartLotLength)]
        public virtual string StartLot { get; set; }

        [StringLength(MaxEndLotLength)]
        public virtual string EndLot { get; set; }

        [StringLength(MaxStartRunLength)]
        public virtual string StartRun { get; set; }

        [StringLength(MaxEndRunLength)]
        public virtual string EndRun { get; set; }

        [Column(TypeName = "DATE")]
        public virtual DateTime? EfFromPart { get; set; }

        [Column(TypeName = "DATE")]
        public virtual DateTime? EfToPart { get; set; }

        [StringLength(MaxOrderPatternLength)]
        public virtual string OrderPattern { get; set; }

        [StringLength(MaxRemarkLength)]
        public virtual string Remark { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }
}
