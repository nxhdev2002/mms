using System;
namespace prod.Inventory.CKD.Dto
{
    public class InvCkdStockIssuingExportInput
   {
        public virtual string PartNo { get; set; }
        public virtual string ColorSfx { get; set; }
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string VinNo { get; set; }
        public virtual string Cfc { get; set; }
        public virtual string LotNo { get; set; }
        public virtual int? NoInLot { get; set; }
        public virtual string PartType { get; set; }
    }
}


