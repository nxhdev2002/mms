using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{

    [Table("InvCkdRequestContentByLot")]
    public class InvCkdRequestContentByLot : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxGradeLength = 2;

        public const int MaxLotNoLength = 10;

        public const int MaxDockLength = 50;

        public const int MaxRemarksLength = 1000;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxGradeLength)]
        public virtual string Grade { get; set; }

        [StringLength(MaxLotNoLength)]
        public virtual string LotNo { get; set; }

        public virtual long? CkdReqId { get; set; }

        [StringLength(MaxDockLength)]
        public virtual string Dock { get; set; }

        [Column(TypeName = "Date")]
        public virtual DateTime? DateTimeReq { get; set; }

        [Column(TypeName = "Date")]
        public virtual DateTime? UpPlanDate { get; set; }

        public virtual int? UpPlanTime { get; set; }

        public virtual int? StdForWA { get; set; }

        [StringLength(MaxRemarksLength)]
        public virtual string Remarks { get; set; }

        public virtual DateTime? UpActualDate { get; set; }

        public virtual int? UpActualTime { get; set; }

        public virtual int? TimeRequest { get; set; }

        public virtual int? Status { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}

