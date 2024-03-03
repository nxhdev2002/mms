using System;
namespace prod.Inventory.CKD.Dto
{
	public class InvCkdStockPartExportInput
	{
        public virtual string PartNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string ColorSfx { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        public virtual bool NegativeStock { get; set; }
    }
}