using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Asy.Plm
{

    [Table("AsyPlmQrcodeAssembly")]
    [Index(nameof(Line), Name = "IX_AsyPlmQrcodeAssembly_Line")]
    [Index(nameof(Process), Name = "IX_AsyPlmQrcodeAssembly_Process")]
    [Index(nameof(Shift), Name = "IX_AsyPlmQrcodeAssembly_Shift")]
    [Index(nameof(StartDatetime), Name = "IX_AsyPlmQrcodeAssembly_StartDatetime")]
    [Index(nameof(StartTimeOff), Name = "IX_AsyPlmQrcodeAssembly_StartTimeOff")]
    [Index(nameof(WorkingDate), Name = "IX_AsyPlmQrcodeAssembly_WorkingDate")]
    public class AsyPlmQrcodeAssembly : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxLineLength = 5;

        public const int MaxProcessLength = 10;

        public const int MaxShiftLength = 5;


        [StringLength(MaxLineLength)]
        public virtual string Line { get; set; }

        [StringLength(MaxProcessLength)]
        public virtual string Process { get; set; }

        public virtual int? NoInDate { get; set; }

        public virtual int? NoInShift { get; set; }

        [StringLength(MaxShiftLength)]
        public virtual string Shift { get; set; }

        public virtual DateTime? StartDatetime { get; set; }

        public virtual TimeSpan? TackTime { get; set; }

        public virtual int? TimeOff { get; set; }

        public virtual DateTime? StartTimeOff { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        public virtual bool Status { get; set; }
    }

}


