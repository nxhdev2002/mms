using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Plm
{

    [Table("MasterPlmPart")]
    [Index(nameof(PartName), Name = "IX_MasterPlmPart_PartName")]
    [Index(nameof(PartCd), Name = "IX_MasterPlmPart_PartCd")]
    public class MasterPlmPart : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxPartNameLength = 500;

        public const int MaxPartCdLength = 50;


        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(MaxPartCdLength)]
        public virtual string PartCd { get; set; }

        public virtual int? R { get; set; }

        public virtual int? G { get; set; }

        public virtual int? B { get; set; }
    }

}

