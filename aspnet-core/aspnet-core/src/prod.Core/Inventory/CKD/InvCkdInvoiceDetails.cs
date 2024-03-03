using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD
{

    [Table("InvCkdInvoiceDetails")]
    public class InvCkdInvoiceDetails : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxPartNoLength = 12;

        public const int MaxLotNoLength = 10;

        public const int MaxFixlotLength = 4;

        public const int MaxCaseNoLength = 30;

        public const int MaxModuleNoLength = 30;

        public const int MaxContainerNoLength = 15;

        public const int MaxSupplierNoLength = 50;

        public const int MaxCeptTypeLength = 10;

        public const int MaxPartNameLength = 300;

        public const int MaxCarfamilyCodeLength = 4;

        public const int MaxOrderNoLength = 12;

        public const int MaxReexportCodeLength = 2;

        public const int MaxStatusLength = 10;

        public const int MaxHsCodeLength = 20;

        public const int MaxPartnameVnLength = 300;

        public const int MaxCarNameLength = 200;

        public const int MaxEcus5HsCodeLength = 20;

        public const int MaxDeclareTypeLength = 10;

        public const int MaxIsActiveLength = 1;

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxLotNoLength)]
        public virtual string LotNo { get; set; }

        [StringLength(MaxFixlotLength)]
        public virtual string Fixlot { get; set; }

        [StringLength(MaxCaseNoLength)]
        public virtual string CaseNo { get; set; }

        [StringLength(MaxModuleNoLength)]
        public virtual string ModuleNo { get; set; }

        public virtual decimal? Insurance { get; set; }

        [StringLength(MaxContainerNoLength)]
        public virtual string ContainerNo { get; set; }

        public virtual long? InvoiceId { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        public virtual decimal? Fi { get; set; }

        public virtual decimal? Fob { get; set; }

        public virtual decimal? Freight { get; set; }

        public virtual decimal? Thc { get; set; }

        public virtual decimal? Cif { get; set; }

        public virtual decimal? Tax { get; set; }

        public virtual decimal? TaxRate { get; set; }

        public virtual decimal? Vat { get; set; }

        public virtual decimal? VatRate { get; set; }

        public virtual decimal? Inland { get; set; }

        public virtual int? UsageQty { get; set; }
        public virtual int? RemainQty { get; set; }

        [StringLength(MaxCeptTypeLength)]
        public virtual string CeptType { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(MaxCarfamilyCodeLength)]
        public virtual string CarfamilyCode { get; set; }

        public virtual decimal? PartNetWeight { get; set; }

        [StringLength(MaxOrderNoLength)]
        public virtual string OrderNo { get; set; }

        public virtual DateTime? Firmpackingmonth { get; set; }

        [StringLength(MaxReexportCodeLength)]
        public virtual string ReexportCode { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        public virtual decimal? FobVn { get; set; }

        public virtual decimal? FreightVn { get; set; }

        public virtual decimal? InsuranceVn { get; set; }

        public virtual decimal? FiVn { get; set; }

        public virtual decimal? ThcVn { get; set; }

        public virtual decimal? CifVn { get; set; }

        public virtual decimal? TaxVn { get; set; }

        public virtual decimal? VatVn { get; set; }

        public virtual decimal? InlandVn { get; set; }

        public virtual long? InvoiceParentId { get; set; }

        public virtual DateTime? PeriodDate { get; set; }

        public virtual decimal? PeriodId { get; set; }

        [StringLength(MaxHsCodeLength)]
        public virtual string HsCode { get; set; }

        [StringLength(MaxPartnameVnLength)]
        public virtual string PartnameVn { get; set; }

        [StringLength(MaxCarNameLength)]
        public virtual string CarName { get; set; }

        public virtual long? PmhistId { get; set; }

        public virtual long? PreCustomsId { get; set; }

        public virtual int? Ecus5TaxRate { get; set; }

        public virtual int? Ecus5VatRate { get; set; }

        [StringLength(MaxEcus5HsCodeLength)]
        public virtual string Ecus5HsCode { get; set; }

        [StringLength(MaxDeclareTypeLength)]
        public virtual string DeclareType { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

}
