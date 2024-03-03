using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inv.Dmr.Dto
{

    public class InvImportByContDto : EntityDto<long?>
    {

        public virtual decimal? PeriodId { get; set; }

        public virtual string ContainerNo { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual string CaseNo { get; set; }

        public virtual string LotNo { get; set; }

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

        public virtual string SupplierNo { get; set; }

        //

        public virtual decimal? TotalFob { get; set; }

        public virtual decimal? TotalCif { get; set; }

        public virtual decimal? TotalImportTax { get; set; }

        public virtual decimal? TotalInlandCharge { get; set; }

        public virtual decimal? TotalAmount { get; set; }

    }   

    public class GetInvImportByContInput : PagedAndSortedResultRequestDto
    {
        public virtual decimal? PeriodId { get; set; }

        public virtual string ContainerNo { get; set; }

        public virtual string CaseNo { get; set; }

        public virtual string LotNo { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string SupplierNo { get; set; }

    }

    public class GetInvImportByContExportInput
    { 
        public virtual decimal? PeriodId { get; set; }

        public virtual string ContainerNo { get; set; }

        public virtual string CaseNo { get; set; }

        public virtual string LotNo { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string SupplierNo { get; set; }

    }

}


