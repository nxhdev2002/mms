using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Inventory.CKD.Vehicle.Dto
{
    public class InvCkdVehicleOutDetailsDto
    {
        public virtual string LotNo { get; set; }
        public virtual int? NoInLot { get; set; }
        public virtual string PartNo { get; set; }
        public virtual string Cfc { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string Grade { get; set; }
        public virtual string Shop { get; set; }
        public virtual string BodyColor { get; set; }
        public virtual string UsageQty { get; set; }
    }
}
