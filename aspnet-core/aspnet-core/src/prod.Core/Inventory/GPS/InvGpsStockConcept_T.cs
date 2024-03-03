using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.GPS
{

    [Table("InvGpsStockConcept_T")]
    public class InvGpsStockConcept_T : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxGuidLength = 128;

        public const int MaxSupplierCodeLength = 10;

        public const int MaxStkConceptLength = 10;

        public const int MaxIsActiveLength = 1;


        [StringLength(MaxGuidLength)]
        public virtual string Guid { get; set; }

        [StringLength(MaxSupplierCodeLength)]
        public virtual string SupplierCode { get; set; }

        [Column(TypeName = "date")]
        public virtual DateTime? MonthStk { get; set; }

        public virtual decimal? MinStk1 { get; set; }

        public virtual decimal? MinStk2 { get; set; }

        public virtual decimal? MinStk3 { get; set; }

        public virtual decimal? MinStk4 { get; set; }

        public virtual decimal? MinStk5 { get; set; }

        public virtual decimal? MinStk6 { get; set; }

        public virtual decimal? MinStk7 { get; set; }

        public virtual decimal? MinStk8 { get; set; }

        public virtual decimal? MinStk9 { get; set; }

        public virtual decimal? MinStk10 { get; set; }

        public virtual decimal? MinStk11 { get; set; }

        public virtual decimal? MinStk12 { get; set; }

        public virtual decimal? MinStk13 { get; set; }

        public virtual decimal? MinStk14 { get; set; }

        public virtual decimal? MinStk15 { get; set; }

        public virtual decimal? MaxStk1 { get; set; }

        public virtual decimal? MaxStk2 { get; set; }

        public virtual decimal? MaxStk3 { get; set; }

        public virtual decimal? MaxStk4 { get; set; }

        public virtual decimal? MaxStk5 { get; set; }

        public virtual decimal? MinStkConcept { get; set; }

        public virtual decimal? MaxStkConcept { get; set; }

        [StringLength(MaxStkConceptLength)]
        public virtual string StkConcept { get; set; }

        public virtual int? StkConceptFrq { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}
