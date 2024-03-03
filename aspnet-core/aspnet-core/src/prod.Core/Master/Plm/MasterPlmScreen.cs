using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Plm
{

    [Table("MasterPlmScreen")]
    [Index(nameof(SideCd), Name = "IX_MasterPlmScreen_SideCd")]
    [Index(nameof(ProdLine), Name = "IX_MasterPlmScreen_ProdLine")]
    [Index(nameof(ProcessCd), Name = "IX_MasterPlmScreen_ProcessCd")]
    [Index(nameof(IsActive), Name = "IX_MasterPlmScreen_IsActive")]
    public class MasterPlmScreen : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxScreenCdLength = 50;

        public const int MaxSideCdLength = 50;

        public const int MaxProdLineLength = 50;

        public const int MaxProcessCdLength = 50;

        public const int MaxGradeLength = 50;

        public const int MaxSequenceNoLength = 50;


        [StringLength(MaxScreenCdLength)]
        public virtual string ScreenCd { get; set; }

        [StringLength(MaxSideCdLength)]
        public virtual string SideCd { get; set; }

        [StringLength(MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        [StringLength(MaxProcessCdLength)]
        public virtual string ProcessCd { get; set; }

        public virtual int? Ordering { get; set; }

        public virtual int? IsNeedReload { get; set; }

        [StringLength(MaxGradeLength)]
        public virtual string Grade { get; set; }

        public virtual int? LotcodeGradeId { get; set; }

        [StringLength(MaxSequenceNoLength)]
        public virtual string SequenceNo { get; set; }

        public virtual int? IsActive { get; set; }
    }

}

