using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{

    [Table("InvCkdCustomsDeclare")]
    public class InvCkdCustomsDeclare : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxCustomsDeclareNoLength = 30;

        public const int MaxBillofladingNoLength = 20;

        public const int MaxCustomsPortLength = 12;

        public const int MaxTaxNoteLength = 500;

        public const int MaxStatusLength = 4;

        public const int MaxOrdertypeCodeLength = 2;

        public const int MaxForwarderLength = 10;

        public const int MaxBusinessTypeLength = 12;

        public const int MaxDeclareTypeLength = 10;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxCustomsDeclareNoLength)]
        public virtual string CustomsDeclareNo { get; set; }

        [StringLength(MaxBillofladingNoLength)]
        public virtual string BillofladingNo { get; set; }

        public virtual decimal? TotalcifCept { get; set; }

        public virtual decimal? TotalcifNoncept { get; set; }

        public virtual decimal? TotaltaxCept { get; set; }

        public virtual decimal? TotaltaxNoncept { get; set; }

        public virtual decimal? TotalvatCept { get; set; }

        public virtual decimal? TotalvatNoncept { get; set; }

        public virtual decimal? ExchangeRate { get; set; }

        public virtual DateTime? DeclareDate { get; set; }

        public virtual decimal? ActualTax { get; set; }

        public virtual decimal? ActualVat { get; set; }

        [StringLength(MaxCustomsPortLength)]
        public virtual string CustomsPort { get; set; }

        [StringLength(MaxTaxNoteLength)]
        public virtual string TaxNote { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        public virtual DateTime? InputDate { get; set; }

        public virtual DateTime? ActualPaymentDate { get; set; }

        [StringLength(MaxOrdertypeCodeLength)]
        public virtual string OrdertypeCode { get; set; }

        public virtual decimal? CompleteTax { get; set; }

        public virtual decimal? CompleteVat { get; set; }

        [StringLength(MaxForwarderLength)]
        public virtual string Forwarder { get; set; }

        [StringLength(MaxBusinessTypeLength)]
        public virtual string BusinessType { get; set; }

        public virtual int? NoPerGroup { get; set; }

        public virtual int? NumOfGroup { get; set; }

        public virtual int? Isvnaccs { get; set; }

        public virtual int? StatusCo { get; set; }

        public virtual int? IsFromEcus { get; set; }

        [StringLength(MaxDeclareTypeLength)]
        public virtual string DeclareType { get; set; }

        public virtual DateTime? CreatedDate { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}


