using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{

    [Table("InvCkdPhysicalConfirmLot_T")]
    [Index(nameof(ModelCode), Name = "IX_InvCkdPhysicalConfirmLot_T_ModelCode")]
  
    public class InvCkdPhysicalConfirmLot_T : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxGuidLength = 128;

        public const int MaxModelCodeLength = 1;

        public const int MaxProdLineLength = 2;

        public const int MaxGradeLength = 2;

        public const int MaxErrorDescriptionLength = 5000;


        [StringLength(MaxGuidLength)]
        public virtual string Guid { get; set; }

        [StringLength(MaxModelCodeLength)]
        public virtual string ModelCode { get; set; }

        [StringLength(MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        [StringLength(MaxGradeLength)]
        public virtual string Grade { get; set; }

        public virtual int? StartLot { get; set; }

        public virtual int? StartRun { get; set; }

        public virtual DateTime? StartProcessDate { get; set; }

        public virtual int? EndLot { get; set; }

        public virtual int? EndRun { get; set; }

        public virtual DateTime? EndProcessDate { get; set; }

        public virtual int PeriodId { get; set; }


        [StringLength(MaxErrorDescriptionLength)]
        public virtual string ErrorDescription { get; set; }

    }

}
