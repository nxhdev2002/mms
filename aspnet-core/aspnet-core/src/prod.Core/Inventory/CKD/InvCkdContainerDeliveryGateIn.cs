using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{

    [Table("InvCkdContainerDeliveryGateIn")]
    public class InvCkdContainerDeliveryGateIn : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxPlateNoLength = 20;

        public const int MaxContainerNoLength = 15;

        public const int MaxRenbanLength = 20;

        public const int MaxDriverNameLength = 50;

        public const int MaxForwarderLength = 100;

        public const int MaxCardNoLength = 10;

        public const int MaxMobileLength = 50;

        public const int MaxIdNoLength = 100;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxPlateNoLength)]
        public virtual string PlateNo { get; set; }

        [StringLength(MaxContainerNoLength)]
        public virtual string ContainerNo { get; set; }

        [StringLength(MaxRenbanLength)]
        public virtual string Renban { get; set; }

        public virtual DateTime? DateIn { get; set; }

        [StringLength(MaxDriverNameLength)]
        public virtual string DriverName { get; set; }

        [StringLength(MaxForwarderLength)]
        public virtual string Forwarder { get; set; }

        public virtual int? TimeIn { get; set; }

        public virtual int? TimeOut { get; set; }

        public virtual long? CkdReqId { get; set; }

        [StringLength(MaxCardNoLength)]
        public virtual string CardNo { get; set; }

        [StringLength(MaxMobileLength)]
        public virtual string Mobile { get; set; }

        public virtual DateTime? CallTime { get; set; }

        public virtual DateTime? CancelTime { get; set; }

        public virtual DateTime? StartTime { get; set; }

        public virtual DateTime? FinishTime { get; set; }

        [StringLength(MaxIdNoLength)]
        public virtual string IdNo { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}

