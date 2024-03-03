using System;
namespace prod.Inventory.GPS.Dto
{

    public class InvGpsIssuingExportInput
    {

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual string Oum { get; set; }

        public virtual int? Boxqty { get; set; }

        public virtual int? Box { get; set; }

        public virtual int? Qty { get; set; }

        public virtual string LotNo { get; set; }

        public virtual DateTime? ProdDate { get; set; }

        public virtual DateTime? ExpDate { get; set; }

        public virtual DateTime? ReceivedDate { get; set; }

        public virtual string Supplier { get; set; }

        public virtual string CostCenter { get; set; }

        public virtual int? QtyIssue { get; set; }

        public virtual string IsIssue { get; set; }

        public virtual string Status { get; set; }

        public virtual string IsGentani { get; set; }

    }

}


