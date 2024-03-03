using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Inventory.CKD.ProductionPlanMonthly.Dto
{
    public class InvCkdProductionPlanMonthlyExportInput
    {
        public virtual string Cfc { get; set; }

        public virtual string Grade { get; set; }

        public virtual DateTime ProdMonth { get; set; }
    }
}
