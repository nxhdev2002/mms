using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{
    [Table("InvCkdShipment")]
    [Index(nameof(ShipmentNo), Name = "IX_InvCkdShipment_ShipmentNo")]
    [Index(nameof(SupplierNo), Name = "IX_InvCkdShipment_SupplierNo")]
    [Index(nameof(Buyer), Name = "IX_InvCkdShipment_Buyer")]
    [Index(nameof(ShipmentDate), Name = "IX_InvCkdShipment_ShipmentDate")]
    [Index(nameof(IsActive), Name = "IX_InvCkdShipment_IsActive")]
    public class InvCkdShipment : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxShipmentNoLength = 10;

        public const int MaxShippingcompanyCodeLength = 10;

        public const int MaxSupplierNoLength = 10;

        public const int MaxBuyerLength = 4;

        public const int MaxFromPortLength = 50;

        public const int MaxToPortLength = 50;

        public const int MaxShipmentDateLength = 10;

        public const int MaxFeedervesselNameLength = 30;

        public const int MaxOceanvesselNameLength = 30;

        public const int MaxOceanvesselvoyageNoLength = 10;

        public const int MaxStatusLength = 50;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxShipmentNoLength)]
        public virtual string ShipmentNo { get; set; }

        [StringLength(MaxShippingcompanyCodeLength)]
        public virtual string ShippingcompanyCode { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(MaxBuyerLength)]
        public virtual string Buyer { get; set; }

        [StringLength(MaxFromPortLength)]
        public virtual string FromPort { get; set; }

        [StringLength(MaxToPortLength)]
        public virtual string ToPort { get; set; }

        [StringLength(MaxShipmentDateLength)]
        public virtual string ShipmentDate { get; set; }

        [Column(TypeName = "date")]
        public virtual DateTime? Etd { get; set; }

        [Column(TypeName = "date")]
        public virtual DateTime? Eta { get; set; }

        [Column(TypeName = "date")]
        public virtual DateTime? Ata { get; set; }

        [StringLength(MaxFeedervesselNameLength)]
        public virtual string FeedervesselName { get; set; }

        [StringLength(MaxOceanvesselNameLength)]
        public virtual string OceanvesselName { get; set; }

        [StringLength(MaxOceanvesselvoyageNoLength)]
        public virtual string OceanvesselvoyageNo { get; set; }

        public virtual int? NoofinvoicewithinshipmentNo { get; set; }

        public virtual int? Noof20Ftcontainers { get; set; }

        public virtual int? Noof40Ftcontainers { get; set; }

        public virtual int? Lclvolume { get; set; }

        [Column(TypeName = "date")]
        public virtual DateTime? Atd { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}

