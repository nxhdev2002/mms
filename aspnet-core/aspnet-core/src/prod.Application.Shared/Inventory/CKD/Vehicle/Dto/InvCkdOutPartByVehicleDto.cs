 using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Inventory.CKD.Vehicle.Dto
{
    public class InvCkdOutPartByVehicleDto : EntityDto<long?>
    {
        public virtual string VinNo { get; set; }

        public virtual string LotNo { get; set; }

        public virtual int? NoInLot { get; set; }

        public virtual string SequenceNo { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string Grade { get; set; }

        public virtual string Shop { get; set; }

        public virtual string BodyColor { get; set; }

        public virtual string UsageQty { get; set; }

    }

    public class InvCkdOutPartByVehicleInput : PagedAndSortedResultRequestDto
    {
        public virtual string LotNo { get; set; }

        public virtual int? NoInLot { get; set; }

        public virtual string PartType { get; set; }
    }

    public class InvCkdProductionActualReportInput
    {
        public virtual int p_mode { get; set; }
        public virtual DateTime FromDate { get; set; }

        public virtual DateTime ToDate { get; set; }
    }

    public class InvCkdProductionActualReportDataDto
    {
        public virtual string CFC { get; set; }
        public virtual string Grade { get; set; }
        public virtual string Color { get; set; }
        public virtual DateTime PdiDate { get; set; }
        public virtual int Count { get; set; }

        public virtual string PdiDateStr { get; set; }
    }
}
