using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Plm
{
    [Table("MasterPlmMatrix")]
    [Index(nameof(ScreenId), Name = "IX_MasterPlmMatrix_ScreenId")]
    [Index(nameof(PartId), Name = "IX_MasterPlmMatrix_PartId")]
    public class MasterPlmMatrix : FullAuditedEntity<long>, IEntity<long>
    {


        public virtual int? ScreenId { get; set; }

        public virtual int? PartId { get; set; }

        public virtual int? Ordering { get; set; }

        public virtual int? SideId { get; set; }
    }
}
