using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CPS
{

    [Table("InvCpsRcvShipmentLines")]
    public class InvCpsRcvShipmentLines : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxUnitOfMeasureLength = 25;

        public const int MaxPartNoLength = 255;

        public const int MaxItemDescriptionLength = 240;

        public const int MaxShipmentLineStatusCodeLength = 25;

        public const int MaxIsActiveLength = 1;

        public virtual long? ShipmentHeaderId { get; set; }

        public virtual int? LineNum { get; set; }

        public virtual long? CategoryId { get; set; }

        public virtual decimal? QuantityShipped { get; set; }

        public virtual decimal? QuantityReceived { get; set; }

        [StringLength(MaxUnitOfMeasureLength)]
        public virtual string UnitOfMeasure { get; set; }

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxItemDescriptionLength)]
        public virtual string ItemDescription { get; set; }

        public virtual long? ItemId { get; set; }

        [StringLength(MaxShipmentLineStatusCodeLength)]
        public virtual string ShipmentLineStatusCode { get; set; }

        public virtual long? Poheaderid { get; set; }

        public virtual long? PoLineId { get; set; }

        public virtual long? PoLineShipmentId { get; set; }

        public virtual long? EmployeeId { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}


