using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CKD.Dto
{

    public class InvCkdPhysicalStockPartPeriodExportInput
    {

        public virtual string PartNo { get; set; }

        public virtual string PartNoNormalized { get; set; }

        public virtual string PartName { get; set; }

        public virtual string PartNoNormalizedS4 { get; set; }

        public virtual string ColorSfx { get; set; }

        public virtual string LotNo { get; set; }

        public virtual long? PartListId { get; set; }

        public virtual long? MaterialId { get; set; }

        public virtual decimal? BeginQty { get; set; }

        public virtual decimal? ReceiveQty { get; set; }

        public virtual decimal? IssueQty { get; set; }

        public virtual decimal? CalculatorQty { get; set; }

        public virtual decimal? ActualQty { get; set; }

        public virtual long? PeriodId { get; set; }

        public virtual DateTime? LastCalDatetime { get; set; }

        public virtual int? Transtype { get; set; }

        public virtual string Remark { get; set; }

        public virtual string IsActive { get; set; }

    }

}

