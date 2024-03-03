using Abp.Application.Services.Dto;
using prod.Inventory.Invoice;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Inventory.CKD.Invoice.Dto
{
    public class InvCkdInvoiceCustomsDto : EntityDto<long?>
    {
        [StringLength(InvCkdInvoiceConsts.MaxInvoiceNoLength)]
        public virtual string InvoiceNo { get; set; }

        public virtual DateTime? InvoiceDate { get; set; }

        [StringLength(InvCkdInvoiceConsts.MaxShipmentNoLength)]
        public virtual string ShipmentNo { get; set; }

        [StringLength(InvCkdInvoiceConsts.MaxOrdertypeCodeLength)]
        public virtual string OrdertypeCode { get; set; }

        public virtual decimal? Fob { get; set; }
        public virtual DateTime? DeclareDate { get; set; }
    }
}
