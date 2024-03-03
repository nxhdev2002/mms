using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.PIO
{

    [Table("InvPIOStockReceiving")]
    public class InvPIOStockReceiving : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxPartNoLength = 40;

        public const int MaxPartNameLength = 200;

        public const int MaxMktCodeLength = 100;

        public const int MaxVinNoLength = 40;

        public const int MaxPartTypeLength = 40;

        public const int MaxShopLength = 20;

        public const int MaxCartTypeLength = 20;

        public const int MaxInteriorColorLength = 20;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        public virtual long? PartId { get; set; }

        [StringLength(MaxMktCodeLength)]
        public virtual string MktCode { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        public virtual int? Qty { get; set; }

        public virtual long? TransId { get; set; }

        public virtual DateTime? TransDatetime { get; set; }

        public virtual long? VehicleId { get; set; }

        [StringLength(MaxVinNoLength)]
        public virtual string VinNo { get; set; }

        [StringLength(MaxPartTypeLength)]
        public virtual string PartType { get; set; }

        [StringLength(MaxShopLength)]
        public virtual string Shop { get; set; }

        [StringLength(MaxCartTypeLength)]
        public virtual string CartType { get; set; }

        [StringLength(MaxInteriorColorLength)]
        public virtual string InteriorColor { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}

