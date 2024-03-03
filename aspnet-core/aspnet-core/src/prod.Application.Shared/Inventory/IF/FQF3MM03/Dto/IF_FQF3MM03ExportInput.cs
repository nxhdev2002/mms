using System;

namespace prod.Inventory.IF.Dto
{
    public class IF_FQF3MM03ExportInput
    {
        public virtual string PartNo { get; set; }

        public virtual string SupplierCode { get; set; }

        public virtual DateTime? PostingDateFrom { get; set; }

        public virtual DateTime? PostingDateTo { get; set; }
        public virtual string PdsNo { get; set; }

        public virtual DateTime? SequenceDateFrom { get; set; }

        public virtual DateTime? SequenceDateTo { get; set; }

    }

}


