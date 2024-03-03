using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CPS.Dto
{

    public class InvCpsInvoiceHeadersDto : EntityDto<long?>
    {

        public virtual string InvoiceNum { get; set; }

        public virtual string InvoiceSymbol { get; set; }

        public virtual string Description { get; set; }

        public virtual DateTime? InvoiceDate { get; set; }

        public virtual long? VendorId { get; set; }

        public virtual string VendorName { get; set; }

        public virtual string CurrencyCode { get; set; }

        public virtual decimal? InvoiceAmount { get; set; }

        public virtual decimal? AmountVat { get; set; }

        public virtual decimal? TaxRate { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class InvCpsInvoiceHeadersGrid : EntityDto<long?>
    {
        public virtual string InvoiceSymbol { get; set; }

        public virtual string InvoiceNum { get; set; }

        public virtual DateTime? CreationTime { get; set; }

        public virtual string Productgroupname { get; set; }

        public virtual string SupplierName { get; set; }

        public virtual string VatregistrationInvoice { get; set; }

        public virtual string VatregistrationNum { get; set; }

        public virtual decimal? Amount { get; set; }

        public virtual string CurrencyCode { get; set; }

        public virtual DateTime? TransactionDatetime { get; set; }

        public virtual long? FormatAmount
        {
            get
            {
                return Amount == null ? 0 : (long)Amount;
            }
            set { }
        }
        public virtual string TransactionDatetime_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", CreationTime);
                }
                catch
                {
                    return "";
                }
            }
            set { }
        }

    }

    public class GetInvCpsInvoiceHeadersInput : PagedAndSortedResultRequestDto
    {

        public virtual string PoNumber { get; set; }

        public virtual long? InventoryGroup { get; set; }


        public virtual long? Supplier { get; set; }

        public virtual DateTime? CreationTimeFrom { get; set; }


        public virtual DateTime? CreationTimeTo { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual string InvoiceSymbol { get; set; }

        public virtual string PartNo { get; set; }


    }

    public class CbxInventoryGroup : EntityDto<long?>
    {
        public virtual long? Id { get; set; }

        public virtual string Productgroupcode { get; set; }

        public virtual string Productgroupname { get; set; }

    }
    public class CbxSupplier : EntityDto<long?>
    {
        public virtual long? Id { get; set; }

        public virtual string SupplierName { get; set; }

        public virtual string SupplierNumber { get; set; }

    }

}