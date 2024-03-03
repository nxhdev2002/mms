using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.PIO.PartList.Exporting
{
    public class InvPioPartListExportInput
    {
        public virtual string FullModel { get; set; }
        public virtual string MktCode { get; set; }
        public virtual string PartNo { get; set; }
        public virtual string IsActive { get; set; }

    }
}
