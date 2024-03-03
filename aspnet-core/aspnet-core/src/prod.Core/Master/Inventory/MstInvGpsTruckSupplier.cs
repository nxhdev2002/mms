using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Inv
{

    [Table("MstInvGpsTruckSupplier")]
    public class MstInvGpsTruckSupplier : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxTruckNameLength = 20;

        public const int MaxIsActiveLength = 1;

        public virtual int? SupplierId { get; set; }

        [StringLength(MaxTruckNameLength)]
        public virtual string TruckName { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}
