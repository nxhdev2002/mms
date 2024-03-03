using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Painting
{

    [Table("MstPtsBmpPartType")]
    [Index(nameof(PartType), Name = "IX_MstPtsBmpPartType_PartType")]
    [Index(nameof(PartTypeName), Name = "IX_MstPtsBmpPartType_PartTypeName")]
    [Index(nameof(ProdLine), Name = "IX_MstPtsBmpPartType_ProdLine")]
    public class MstPtsBmpPartType : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxPartTypeLength = 50;

        public const int MaxPartTypeNameLength = 50;

        public const int MaxProdLineLength = 50;

        public const int MaxIsBumperLength = 1;

        public const int MaxIsActiveLength = 1;


        [StringLength(MaxPartTypeLength)]
        public virtual string PartType { get; set; }

        [StringLength(MaxPartTypeNameLength)]
        public virtual string PartTypeName { get; set; }

        [StringLength(MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        [Required]
        public virtual int? Sorting { get; set; }

        [StringLength(MaxIsBumperLength)]
        public virtual string IsBumper { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }

    }

}

