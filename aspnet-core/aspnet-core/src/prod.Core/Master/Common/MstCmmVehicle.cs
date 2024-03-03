
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Cmm
{
    [Table("MstCmmVehicle")]
    public class MstCmmVehicle : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxVehicleCodeLength = 10;

        public const int MaxVehicleNameLength = 50;

        [StringLength(MaxVehicleCodeLength)]
        public virtual string VehicleCode { get; set; }

        [StringLength(MaxVehicleNameLength)]
        public virtual string VehicleName { get; set; }
    }

}

