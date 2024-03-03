using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CKD.Dto
{

    public class InvCkdContainerInvoiceExportInput
    {

        public virtual string ContainerNo { get; set; }

        public virtual string Renban { get; set; }

        public virtual long? InvoiceId { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string SealNo { get; set; }

        public virtual int? ContainerSize { get; set; }

        public virtual DateTime? PlandedvanningDate { get; set; }

        public virtual DateTime? ActualvanningDate { get; set; }

        public virtual decimal? Thc { get; set; }

        public virtual decimal? Inland { get; set; }

        public virtual DateTime? CdDate { get; set; }

        public virtual string Status { get; set; }

        public virtual decimal? ThcVn { get; set; }

        public virtual decimal? InlandVn { get; set; }

        public virtual DateTime? PeriodDate { get; set; }

        public virtual long? PeriodId { get; set; }

        public virtual string DateStatus { get; set; }

        public virtual decimal? Fob { get; set; }

        public virtual decimal? Freight { get; set; }

        public virtual decimal? Insurance { get; set; }

        public virtual decimal? Amount { get; set; }

        public virtual decimal? InvoiceNo { get; set; }

        public virtual string IsActive { get; set; }

    }

}


