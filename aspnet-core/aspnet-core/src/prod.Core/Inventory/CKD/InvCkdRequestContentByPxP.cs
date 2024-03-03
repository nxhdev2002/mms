using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{

    [Table("InvCkdRequestContentByPxP")]
    public class InvCkdRequestContentByPxP : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxModelLength = 50;

        public const int MaxSourceLength = 50;

        public const int MaxPartNoLength = 12;

        public const int MaxPartNameLength = 300;

        public const int MaxRemarksLength = 1000;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxModelLength)]
        public virtual string Model { get; set; }

        [StringLength(MaxSourceLength)]
        public virtual string Source { get; set; }

        public virtual long? CkdReqId { get; set; }


        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        public virtual int? Balance { get; set; }

        public virtual int? ReqQuantity { get; set; }

        public virtual int? MinstockAtTmv { get; set; }

        public virtual int? StockendofReqDate { get; set; }

        [Column(TypeName = "Date")]
        public virtual DateTime? UpPlanDate { get; set; }

        public virtual int? UpPlanTime { get; set; }

        [Column(TypeName = "Date")]
        public virtual DateTime? UpActualDate { get; set; }

        public virtual int? UpActualTime { get; set; }

        public virtual int? TimeRequest { get; set; }

        [StringLength(MaxRemarksLength)]
        public virtual string Remarks { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}

