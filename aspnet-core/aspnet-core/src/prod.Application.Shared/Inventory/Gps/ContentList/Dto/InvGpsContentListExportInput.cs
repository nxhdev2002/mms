using System;
namespace prod.Inventory.GPS.Dto
{

    public class InvGpsContentListExportInput
    {
        public virtual DateTime? WorkingDateFrom { get; set; }

        public virtual DateTime? WorkingDateTo { get; set; }
        public virtual string Shift { get; set; }

        public virtual string SupplierCode { get; set; }

        public virtual string DockNo { get; set; }

    }

}


