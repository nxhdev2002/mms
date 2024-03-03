using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Cmm
{

    [Table("MstCmmVehicleCBUColor")]
    public class MstCmmVehicleCBUColor : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxExtColorLength = 50;

        public const int MaxIntColorLength = 50;


        [StringLength(MaxExtColorLength)]
        public virtual string ExtColor { get; set; }

        [StringLength(MaxIntColorLength)]
        public virtual string IntColor { get; set; }

        public virtual long? VehicleId { get; set; }
    }

}


