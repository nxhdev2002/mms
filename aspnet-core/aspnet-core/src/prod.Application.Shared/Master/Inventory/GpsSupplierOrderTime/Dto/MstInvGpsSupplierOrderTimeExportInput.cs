using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Master.Inv.Dto
{
    public class MstInvGpsSupplierOrderTimeExportInput
    {
        public virtual long? SupplierId { get; set; }

        public virtual int? OrderSeq { get; set; }

        public virtual string OrderType { get; set; }

        public virtual TimeSpan? OrderTime { get; set; }

        public virtual int? ReceivingDay { get; set; }

        public virtual TimeSpan? ReceiveTime { get; set; }

        public virtual TimeSpan? KeihenTime { get; set; }

        public virtual int? KeihenDay { get; set; }

        public virtual string IsActive { get; set; }
    }
}
