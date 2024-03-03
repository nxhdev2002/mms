using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Inventory.CKD.StockReceiving.Dto
{
    public class InvCkdStockReceivingValidateInput
    {
        public virtual string PartNo { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string ContainerNo { get; set; }

        public virtual string Renban { get; set; }

        public virtual DateTime? ReceiveDate { get; set; }

        public virtual string Status { get; set; }

        public virtual DateTime?  CdDate { get; set; }

        public virtual string ErrDesc { get; set; }
    }
}
