using System;
namespace prod.Inventory.CKD.Dto
{
    public class InvCkdProdPlanDailyExportInput
    {
        public virtual string Vin { get; set; }

        public virtual string LotNo { get; set; }

        public virtual int? NoInLot { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string BodyNo { get; set; }

        public virtual DateTime? DateFrom { get; set; }

        public virtual DateTime? DateTo { get; set; }

        public virtual string IsPdiDate { get; set; }

        public virtual int SelectDate { get; set; }

    }
}


