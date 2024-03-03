using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace prod.Inventory.CKD
{
    [Table("InvCkdPart_T")]
    public class InvCkdPart_T : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxGuidLength = 128;

        public const int MaxPartNoLength = 50;

        public const int MaxPartNoNormalizedLength = 20;

        public const int MaxPartNameLength = 500;

        public const int MaxSupplierNoLength = 50;

        public const int MaxSupplierCdLength = 50;

        public const int MaxColorSfxLength = 10;

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

        public const int MaxOrderPatternLength = 50;

        public const int MaxRemarkLength = 5000;

        public const int MaxIsActiveLength = 1;


        public const int MaxBackNoLength = 10;

        public const int MaxModuleNoLength = 10;

        public const int MaxRenbanLength = 10;

        public const int MaxReExportCdLength = 10;

        public const int MaxTypeLength = 50;

        public const int MaxCommonLength = 50;

        public const int MaxIcoFlagLength = 50;






        [StringLength(MaxGuidLength)]
        public virtual string Guid { get; set; }

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxPartNoNormalizedLength)]
        public virtual string PartNoNormalized { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(MaxSupplierCdLength)]
        public virtual string SupplierCd { get; set; }

        [StringLength(MaxColorSfxLength)]
        public virtual string ColorSfx { get; set; }

        public virtual long? SupplierId { get; set; }

        public virtual long? MaterialId { get; set; }


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

        [StringLength(MaxBackNoLength)]
        public virtual string BackNo { get; set; }

        [StringLength(MaxModuleNoLength)]
        public virtual string ModuleNo { get; set; }

        [StringLength(MaxRenbanLength)]
        public virtual string Renban { get; set; }

        public virtual int? BoxSize { get; set; }

        [StringLength(MaxTypeLength)]
        public virtual string Type { get; set; }

        [StringLength(MaxCommonLength)]
        public virtual string Common { get; set; }

        [StringLength(MaxIcoFlagLength)]
        public virtual string IcoFlag { get; set; }

        [StringLength(MaxReExportCdLength)]
        public virtual string ReExportCd { get; set; }

        [Column(TypeName = "Date")]
        public virtual DateTime? StartPackingMonth { get; set; }

        [Column(TypeName = "Date")]
        public virtual DateTime? EndPackingMonth { get; set; }

        [Column(TypeName = "Date")]
        public virtual DateTime? StartProductionMonth { get; set; }

        [Column(TypeName = "Date")]
        public virtual DateTime? EndProductionMonth { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }

    }
}
