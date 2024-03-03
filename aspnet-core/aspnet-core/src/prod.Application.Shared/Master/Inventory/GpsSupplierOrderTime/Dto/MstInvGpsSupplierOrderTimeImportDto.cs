using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Master.Inventory.GpsSupplierOrderTime.Dto
{
    public class MstInvGpsSupplierOrderTimeImportDto
    {
        public virtual string RowNumber { get; set; }
        public virtual string Guid { get; set; }

        public virtual long? SupplierId { get; set; }

        public virtual int? OrderSeq { get; set; }

        public virtual string OrderType { get; set; }

        public virtual TimeSpan? OrderTime { get; set; }

        public virtual int? ReceivingDay { get; set; }

        public virtual TimeSpan? ReceiveTime { get; set; }

        public virtual TimeSpan? KeihenTime { get; set; }

        public virtual int? KeihenDay { get; set; }


        public virtual string ErrorDescription { get; set; }
    }
}
