using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.IF
{

    [Table("IF_FQF3MM01")]
    public class IF_FQF3MM01 : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxVinLength = 17;

        public const int MaxUrnLength = 10;

        public const int MaxSpecSheetNoLength = 2;

        public const int MaxIdLineLength = 2;

        public const int MaxKatashikiLength = 20;

        public const int MaxSaleKatashikiLength = 20;

        public const int MaxSaleSuffixLength = 2;

        public const int MaxSpec200DigitsLength = 200;

        public const int MaxProductionSuffixLength = 2;

        public const int MaxLotCodeLength = 2;

        public const int MaxEnginePrefixLength = 5;

        public const int MaxEngineNoLength = 30;

        public const int MaxPlantCodeLength = 4;

        public const int MaxCurrentStatusLength = 2;

        public const int MaxLineOffDatetimeLength = 14;

        public const int MaxInteriorColorLength = 4;

        public const int MaxExteriorColorLength = 4;

        public const int MaxDestinationCodeLength = 5;

        public const int MaxEdOdnoLength = 5;

        public const int MaxCancelFlagLength = 1;

        public const int MaxSmsCarFamilyCodeLength = 4;

        public const int MaxOrderTypeLength = 1;

        public const int MaxKatashikiCodeLength = 5;

        public const int MaxEndOfRecordLength = 1;

        public virtual int? RecordId { get; set; }

        [StringLength(MaxVinLength)]
        public virtual string Vin { get; set; }

        [StringLength(MaxUrnLength)]
        public virtual string Urn { get; set; }

        [StringLength(MaxSpecSheetNoLength)]
        public virtual string SpecSheetNo { get; set; }

        [StringLength(MaxIdLineLength)]
        public virtual string IdLine { get; set; }

        [StringLength(MaxKatashikiLength)]
        public virtual string Katashiki { get; set; }

        [StringLength(MaxSaleKatashikiLength)]
        public virtual string SaleKatashiki { get; set; }

        [StringLength(MaxSaleSuffixLength)]
        public virtual string SaleSuffix { get; set; }

        [StringLength(MaxSpec200DigitsLength)]
        public virtual string Spec200Digits { get; set; }

        [StringLength(MaxProductionSuffixLength)]
        public virtual string ProductionSuffix { get; set; }

        [StringLength(MaxLotCodeLength)]
        public virtual string LotCode { get; set; }

        [StringLength(MaxEnginePrefixLength)]
        public virtual string EnginePrefix { get; set; }

        [StringLength(MaxEngineNoLength)]
        public virtual string EngineNo { get; set; }

        [StringLength(MaxPlantCodeLength)]
        public virtual string PlantCode { get; set; }

        [StringLength(MaxCurrentStatusLength)]
        public virtual string CurrentStatus { get; set; }

        [StringLength(MaxLineOffDatetimeLength)]
        public virtual string LineOffDatetime { get; set; }

        [StringLength(MaxInteriorColorLength)]
        public virtual string InteriorColor { get; set; }

        [StringLength(MaxExteriorColorLength)]
        public virtual string ExteriorColor { get; set; }

        [StringLength(MaxDestinationCodeLength)]
        public virtual string DestinationCode { get; set; }

        [StringLength(MaxEdOdnoLength)]
        public virtual string EdOdno { get; set; }

        [StringLength(MaxCancelFlagLength)]
        public virtual string CancelFlag { get; set; }

        [StringLength(MaxSmsCarFamilyCodeLength)]
        public virtual string SmsCarFamilyCode { get; set; }

        [StringLength(MaxOrderTypeLength)]
        public virtual string OrderType { get; set; }

        [StringLength(MaxKatashikiCodeLength)]
        public virtual string KatashikiCode { get; set; }

        [StringLength(MaxEndOfRecordLength)]
        public virtual string EndOfRecord { get; set; }
    }

}


