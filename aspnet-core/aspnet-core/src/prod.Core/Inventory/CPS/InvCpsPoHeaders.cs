using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CPS
{

    [Table("InvCpsPoHeaders")]
    public class InvCpsPoHeaders : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxTypeLookupCodeLength = 25;

        public const int MaxPoNumberLength = 20;

        public const int MaxCurrencyCodeLength = 15;

        public const int MaxAuthorizationStatusLength = 25;

        public const int MaxCommentsLength = 240;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxTypeLookupCodeLength)]
        public virtual string TypeLookupCode { get; set; }

        [StringLength(MaxPoNumberLength)]
        public virtual string PoNumber { get; set; }

        public virtual long? VendorId { get; set; }

        [StringLength(MaxCurrencyCodeLength)]
        public virtual string CurrencyCode { get; set; }

        [StringLength(MaxAuthorizationStatusLength)]
        public virtual string AuthorizationStatus { get; set; }

        public virtual decimal? TotalPrice { get; set; }

        public virtual decimal? TotalPriceUsd { get; set; }

        public virtual DateTime? ApprovedDate { get; set; }

        [StringLength(MaxCommentsLength)]
        public virtual string Comments { get; set; }

        public virtual DateTime? SubmitDate { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}

