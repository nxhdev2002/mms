using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Common
{
    [Table("CmmReportRequest")]
    public class CmmReportRequest : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxReportListLength = 500;
        public const int MaxReportParamsLength = 4000;
        public const int MaxReqStatusLength = 50;
        public const int MaxIsActiveLength = 1;

        [MaxLength(MaxReportListLength)]
        public virtual string ReportList { get; set; }

        [MaxLength(MaxReportParamsLength)]
        public virtual string ReportParams { get; set; }

        [MaxLength(MaxReqStatusLength)]
        public virtual string ReqStatus { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public virtual DateTime? ReqTime { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }

    }
}


