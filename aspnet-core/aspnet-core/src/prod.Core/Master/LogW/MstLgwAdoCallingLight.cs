using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.LogW
{

    [Table("MstLgwAdoCallingLight")]
    [Index(nameof(Code), Name = "IX_MstLgwAdoCallingLight_Code")]
    [Index(nameof(LightName), Name = "IX_MstLgwAdoCallingLight_LightName")]
    public class MstLgwAdoCallingLight : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxCodeLength = 50;

        public const int MaxLightNameLength = 200;

        public const int MaxProdLineLength = 50;

        public const int MaxProcessLength = 50;

        public const int MaxBlockCodeLength = 50;

        public const int MaxBlockDescriptionLength = 250;

        public const int MaxSortingLength = 50;

        public const int MaxSignalCodeLength = 50;

        public const int MaxIsActiveLength = 50;



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

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}
