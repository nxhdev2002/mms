using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.LogW.Ado
{

    [Table("LgwAdoCallingLightStatus")]
    [Index(nameof(Code), Name = "IX_LgwAdoCallingLightStatus_Code")]
    [Index(nameof(LightName), Name = "IX_LgwAdoCallingLightStatus_LightName")]
    public class LgwAdoCallingLightStatus : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxCodeLength = 50;

        public const int MaxLightNameLength = 200;

        public const int MaxProdLineLength = 50;

        public const int MaxProcessLength = 50;

        public const int MaxBlockCodeLength = 50;

        public const int MaxBlockDescriptionLength = 250;

        public const int MaxSortingLength = 50;

        public const int MaxSignalCodeLength = 50;

        public const int MaxStatusLength = 50;

        public const int MaxShiftLength = 50;



        [StringLength(MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MaxLightNameLength)]
        public virtual string LightName { get; set; }

        [StringLength(MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        [StringLength(MaxProcessLength)]
        public virtual string Process { get; set; }

        [StringLength(MaxBlockCodeLength)]
        public virtual string BlockCode { get; set; }

        [StringLength(MaxBlockDescriptionLength)]
        public virtual string BlockDescription { get; set; }

        [StringLength(MaxSortingLength)]
        public virtual string Sorting { get; set; }

        public virtual int? SignalId { get; set; }

        [StringLength(MaxSignalCodeLength)]
        public virtual string SignalCode { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? StartDate { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? FinshDate { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? WorkingDate { get; set; }

        [StringLength(MaxShiftLength)]
        public virtual string Shift { get; set; }
        public virtual int? NoInDate { get; set; }
        public virtual int? NoInShift { get; set; }
    }

}