using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace prod.Inventory.CKD
{
    [Table("InvCkdProductionPlanMonthly")]
    public class InvCkdProductionPlanMonthly : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxCfcLength = 4;

        public const int MaxGradeLength = 3;


        [StringLength(MaxCfcLength)]
        public virtual string Cfc { get; set; }

        [StringLength(MaxGradeLength)]
        public virtual string Grade { get; set; }

        public virtual DateTime? ProductionMonth { get; set; }

        public virtual int? N_3 { get; set; }

        public virtual int? N_2 { get; set; }

        public virtual int? N_1 { get; set; }

        public virtual int? N { get; set; }

        public virtual int? N1 { get; set; }

        public virtual int? N2 { get; set; }

        public virtual int? N3 { get; set; }

        public virtual int? N4 { get; set; }

        public virtual int? N5 { get; set; }

        public virtual int? N6 { get; set; }

        public virtual int? N7 { get; set; }

        public virtual int? N8 { get; set; }

        public virtual int? N9 { get; set; }

        public virtual int? N10 { get; set; }

        public virtual int? N11 { get; set; }

        public virtual int? N12 { get; set; }
	}

}

