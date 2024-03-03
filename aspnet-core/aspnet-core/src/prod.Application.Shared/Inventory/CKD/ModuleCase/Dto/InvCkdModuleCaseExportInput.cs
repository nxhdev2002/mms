using System;
namespace prod.Inventory.CKD.Dto
{
    public class InvCkdModuleCaseExportInput
    {
        public virtual string ModuleCaseNo { get; set; }
        public virtual string ContainerNo { get; set; }
        public virtual string Renban { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string InvoiceNo { get; set; }
        public virtual string CaseNo { get; set; }
        public virtual DateTime? DevanningFromDate { get; set; }
        public virtual DateTime? DevanningToDate { get; set; }
        public virtual DateTime? UnpackingFromDate { get; set; }
        public virtual DateTime? UnpackingToDate { get; set; }
        public virtual string StorageLocationCode { get; set; }
        public virtual string radio { get; set; }
        public virtual DateTime? BillDateFrom { get; set; }
        public virtual DateTime? BillDateTo { get; set; }

        public virtual string CkdPio { get; set; }
        public virtual string OrderTypeCode { get; set; }
        public virtual string BillNo { get; set; }
        public virtual string LotNo { get; set; }
    }

}
