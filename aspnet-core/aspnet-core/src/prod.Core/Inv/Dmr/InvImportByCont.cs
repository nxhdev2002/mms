using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inv.Dmr
{

    [Table("InvImportByCont")]
    [Index(nameof(ContainerNo), Name = "IX_InvImportByCont_ContainerNo")]
    [Index(nameof(InvoiceNo), Name = "IX_InvImportByCont_InvoiceNo")]
    [Index(nameof(Fob), Name = "IX_InvImportByCont_Fob")]
    [Index(nameof(InlandCharge), Name = "IX_InvImportByCont_InlandCharge")]
    [Index(nameof(Amount), Name = "IX_InvImportByCont_Amount")]
    [Index(nameof(CifVn), Name = "IX_InvImportByCont_CifVn")]
    [Index(nameof(AmountVn), Name = "IX_InvImportByCont_AmountVn")]
    [Index(nameof(PriceVn), Name = "IX_InvImportByCont_PriceVn")]
    [Index(nameof(ContSize), Name = "IX_InvImportByCont_ContSize")]
    public class InvImportByCont : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxContainerNoLength = 50;

        public const int MaxCaseNoLength = 50;

        public const int MaxLotNoLength = 50;

        public const int MaxPartNoLength = 50;

        public const int MaxSupplierNoLength = 50;

        public const int MaxInvoiceNoLength = 50;

        

        public virtual decimal? PeriodId { get; set; }

        [StringLength(MaxContainerNoLength)]
        public virtual string ContainerNo { get; set; }


        [StringLength(MaxInvoiceNoLength)]
        public virtual string InvoiceNo { get; set; }

        [StringLength(MaxCaseNoLength)]
        public virtual string CaseNo { get; set; }

        [StringLength(MaxLotNoLength)]
        public virtual string LotNo { get; set; }

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        public virtual DateTime? DateIn { get; set; }

        public virtual decimal? Fob { get; set; }

        public virtual decimal? Cif { get; set; }

        public virtual decimal? ImportTax { get; set; }

        public virtual decimal? InlandCharge { get; set; }

        public virtual decimal? Amount { get; set; }

        public virtual decimal? Qty { get; set; }

        public virtual decimal? Price { get; set; }

        public virtual decimal? FobVn { get; set; }

        public virtual decimal? CifVn { get; set; }

        public virtual decimal? ImportTaxVn { get; set; }

        public virtual decimal? InlandChargeVn { get; set; }

        public virtual decimal? AmountVn { get; set; }

        public virtual decimal? PriceVn { get; set; }

        public virtual DateTime? InvoiceDate { get; set; }

        public virtual DateTime? ReceiveDate { get; set; }

        public virtual int? ContSize { get; set; }

        public virtual DateTime? Eta { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }
    }

}


