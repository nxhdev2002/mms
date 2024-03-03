using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Inv
{

    [Table("MstInvGpsSupplierInfo")]
    public class MstInvGpsSupplierInfo : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxSupplierCodeLength = 10;

        public const int MaxSupplierPlantCodeLength = 1;

        public const int MaxSupplierNameLength = 100;

        public const int MaxAddressLength = 100;

        public const int MaxDockXLength = 20;

        public const int MaxDockXAddressLength = 100;

        public const int MaxDeliveryMethodLength = 20;

        public const int MaxDeliveryFrequencyLength = 3;

        public const int MaxCdLength = 40;

        public const int MaxOrderDateTypeLength = 5;

        public const int MaxKeihenTypeLength = 5;

        public const int MaxProductionShiftLength = 10;

        public const int MaxSupplierNameEnLength = 100;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxSupplierCodeLength)]
        public virtual string SupplierCode { get; set; }

        [StringLength(MaxSupplierPlantCodeLength)]
        public virtual string SupplierPlantCode { get; set; }

        [StringLength(MaxSupplierNameLength)]
        public virtual string SupplierName { get; set; }

        [StringLength(MaxAddressLength)]
        public virtual string Address { get; set; }

        [StringLength(MaxDockXLength)]
        public virtual string DockX { get; set; }

        [StringLength(MaxDockXAddressLength)]
        public virtual string DockXAddress { get; set; }

        [StringLength(MaxDeliveryMethodLength)]
        public virtual string DeliveryMethod { get; set; }

        [StringLength(MaxDeliveryFrequencyLength)]
        public virtual string DeliveryFrequency { get; set; }

        [StringLength(MaxCdLength)]
        public virtual string Cd { get; set; }

        [StringLength(MaxOrderDateTypeLength)]
        public virtual string OrderDateType { get; set; }

        [StringLength(MaxKeihenTypeLength)]
        public virtual string KeihenType { get; set; }

        public virtual decimal? StkConceptTmvMin { get; set; }

        public virtual decimal? StkConceptTmvMax { get; set; }

        public virtual decimal? StkConceptSupMMin { get; set; }

        public virtual decimal? StkConceptSupMMax { get; set; }

        public virtual decimal? StkConceptSupPMin { get; set; }

        public virtual decimal? StkConceptSupPMax { get; set; }

        public virtual int? TmvProductPercentage { get; set; }

        public virtual int? PicMainId { get; set; }

        public virtual decimal? DeliveryLt { get; set; }


        [StringLength(MaxProductionShiftLength)]
        public virtual string ProductionShift { get; set; }

        public virtual DateTime? TcFrom { get; set; }

        public virtual DateTime? TcTo { get; set; }

        public virtual int? OrderTrip { get; set; }

        [StringLength(MaxSupplierNameEnLength)]
        public virtual string SupplierNameEn { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}