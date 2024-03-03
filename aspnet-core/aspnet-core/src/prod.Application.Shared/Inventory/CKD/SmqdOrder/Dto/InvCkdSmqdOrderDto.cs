using Abp.Application.Services.Dto;
using System;
using System.Reflection.Emit;

namespace prod.Inventory.CKD.Dto
{
    public class InvCkdSmqdOrderDto : EntityDto<long?>
    {

        public virtual string Shop { get; set; }

        public virtual DateTime? SmqdDate { get; set; }

        public virtual string RunNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual string OrderNo { get; set; }

        public virtual int? OrderQty { get; set; }

        public virtual DateTime? OrderDate { get; set; }

        public virtual string Transport { get; set; }

        public virtual string ReasonCode { get; set; }

        public virtual DateTime? EtaRequest { get; set; }

        public virtual string ActualEtaPort { get; set; }

        public virtual DateTime? EtaExpReply { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual DateTime? ReceiveDate { get; set; }

        public virtual int? ReceiveQty { get; set; }

        public virtual string Remark { get; set; }

        public virtual int? OrderType { get; set; }

    }

    public class CreateOrEditInvCkdSmqdOrderDto : EntityDto<long?> {
        public virtual string Shop { get; set; }

    public virtual DateTime? SmqdDate { get; set; }

    public virtual string RunNo { get; set; }

    public virtual string Cfc { get; set; }

    public virtual string SupplierNo { get; set; }

    public virtual string PartNo { get; set; }

    public virtual string PartName { get; set; }

    public virtual string OrderNo { get; set; }

    public virtual int? OrderQty { get; set; }

    public virtual DateTime? OrderDate { get; set; }

    public virtual string Transport { get; set; }

    public virtual string ReasonCode { get; set; }

    public virtual DateTime? EtaRequest { get; set; }

    public virtual string ActualEtaPort { get; set; }

    public virtual DateTime? EtaExpReply { get; set; }

    public virtual string InvoiceNo { get; set; }

    public virtual DateTime? ReceiveDate { get; set; }

    public virtual int? ReceiveQty { get; set; }

    public virtual string Remark { get; set; }

    public virtual int? OrderType { get; set; }
}

    public class GetInvCkdSmqdOrderInput : PagedAndSortedResultRequestDto
    {

        public virtual string Shop { get; set; }

        public virtual DateTime? SmqdDateFrom { get; set; }

        public virtual DateTime? SmqdDateTo { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string OrderNo { get; set; }

        public virtual DateTime? OrderDateFrom { get; set; }

        public virtual DateTime? OrderDateTo { get; set; }

        public virtual string InvoiceNo { get; set; }

    }
    
    public class InvCkdSmqdOrderImportDto
    {
          public virtual string Shop { get; set; }

        public virtual DateTime? SmqdDate { get; set; }

        public virtual string RunNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual string OrderNo { get; set; }

        public virtual int? OrderQty { get; set; }

        public virtual DateTime? OrderDate { get; set; }

        public virtual string Transport { get; set; }

        public virtual string ReasonCode { get; set; }

        public virtual DateTime? EtaRequest { get; set; }

        public virtual string ActualEtaPort { get; set; }

        public virtual DateTime? EtaExpReply { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual DateTime? ReceiveDate { get; set; }

        public virtual int? ReceiveQty { get; set; }

        public virtual string Remark { get; set; }

        public virtual int? OrderType { get; set; }

        public virtual string Guid { get; set; }
        public virtual string ErrorDescription { get; set; }
    }

    public class GetInvCkdSmqdOrderExportInput 
    {

        public virtual string Shop { get; set; }

        public virtual DateTime? SmqdDateFrom { get; set; }

        public virtual DateTime? SmqdDateTo { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string OrderNo { get; set; }

        public virtual DateTime? OrderDateFrom { get; set; }

        public virtual DateTime? OrderDateTo { get; set; }

        public virtual string InvoiceNo { get; set; }

    }

}

