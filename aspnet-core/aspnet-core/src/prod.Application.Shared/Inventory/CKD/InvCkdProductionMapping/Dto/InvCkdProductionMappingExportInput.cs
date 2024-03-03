using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CKD.Dto
{

    public class InvCkdProductionMappingExportInput
    {

        public virtual long? PlanSequence { get; set; }

        public virtual string Shop { get; set; }

        public virtual string Model { get; set; }

        public virtual string LotNo { get; set; }

        public virtual string NoInLot { get; set; }

        public virtual string Grade { get; set; }

        public virtual string BodyNo { get; set; }

        public virtual DateTime? DateIn { get; set; }

        public virtual string TimeIn { get; set; }

        public virtual string UseLotNo { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual long? PartId { get; set; }

        public virtual decimal? Quantity { get; set; }

        public virtual long? PeriodId { get; set; }

        public virtual long? WipId { get; set; }

        public virtual long? InStockId { get; set; }

        public virtual long? MappingId { get; set; }

    }

}


