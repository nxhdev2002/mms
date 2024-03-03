using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace prod.Inventory.CKD
{
    [Table("InvCkdProductionPlanMonthly_T")]
    public class InvCkdProductionPlanMonthly_T : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxGuidLength = 128;

        public const int MaxCfcLength = 4;

        public const int MaxGradeLength = 3;

        public const int MaxErrorDescriptionLength = 5000;



        [StringLength(MaxGuidLength)]
        public virtual string Guid { get; set; }

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

        [StringLength(MaxErrorDescriptionLength)]
        public string ErrorDescription { get; set; }
    }
}
