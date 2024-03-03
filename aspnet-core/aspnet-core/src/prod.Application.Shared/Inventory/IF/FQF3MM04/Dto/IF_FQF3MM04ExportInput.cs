using System;
namespace prod.Inventory.IF.Dto
{
    public class IF_FQF3MM04ExportInput
    {
        public virtual DateTime? DevaningDateFrom { get; set; }

        public virtual DateTime? DevaningDateTo { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual string Renban { get; set; }

        public virtual string ContainerNo { get; set; }

        public virtual string ModuleNo { get; set; }

    }

}


