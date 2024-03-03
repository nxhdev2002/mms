using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Asy.Plm
{
    [Table("AsyPlmAssemblyProcessBase")]
    [Index(nameof(Line), Name = "IX_AsyPlmAssemblyProcessBase_Line")]
    [Index(nameof(Name), Name = "IX_AsyPlmAssemblyProcessBase_Name")]
    [Index(nameof(Dolly), Name = "IX_AsyPlmAssemblyProcessBase_Dolly")]
    [Index(nameof(IsPicking), Name = "IX_AsyPlmAssemblyProcessBase_IsPicking")]
    public class AsyPlmAssemblyProcessBase : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxLineLength = 5;

        public const int MaxNameLength = 10;

        public const int MaxProcessLength = 10;

        public const int MaxDollyLength = 30;


        [StringLength(MaxLineLength)]
        public virtual string Line { get; set; }

        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }

        [StringLength(MaxProcessLength)]
        public virtual string Process { get; set; }

        public virtual int? Sort { get; set; }

        [StringLength(MaxDollyLength)]
        public virtual string Dolly { get; set; }

        public virtual bool IsPicking { get; set; }
    }

}

