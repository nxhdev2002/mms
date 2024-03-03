using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CKD.Dto
{

    public class InvCkdPhysicalStockPartPeriodDto : EntityDto<long?>
    {

        public virtual string PartNo { get; set; }

        public virtual string PartNoNormalized { get; set; }

        public virtual string PartName { get; set; }

        public virtual string PartNoNormalizedS4 { get; set; }

        public virtual string ColorSfx { get; set; }

        public virtual string LotNo { get; set; }

        public virtual long? PartListId { get; set; }

        public virtual long? MaterialId { get; set; }

        public virtual int? BeginQty { get; set; }

        public virtual int? ReceiveQty { get; set; }

        public virtual int? IssueQty { get; set; }

        public virtual int? CalculatorQty { get; set; }

        public virtual int? ActualQty { get; set; }

        public virtual long? PeriodId { get; set; }

        public virtual DateTime? LastCalDatetime { get; set; }

        public virtual int? Transtype { get; set; }

        public virtual string Remark { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditInvCkdPhysicalStockPartPeriodDto : EntityDto<long?>
    {

        [StringLength(InvCkdPhysicalStockPartPeriodConsts.MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(InvCkdPhysicalStockPartPeriodConsts.MaxPartNoNormalizedLength)]
        public virtual string PartNoNormalized { get; set; }

        [StringLength(InvCkdPhysicalStockPartPeriodConsts.MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(InvCkdPhysicalStockPartPeriodConsts.MaxPartNoNormalizedS4Length)]
        public virtual string PartNoNormalizedS4 { get; set; }

        [StringLength(InvCkdPhysicalStockPartPeriodConsts.MaxColorSfxLength)]
        public virtual string ColorSfx { get; set; }

        [StringLength(InvCkdPhysicalStockPartPeriodConsts.MaxLotNoLength)]
        public virtual string LotNo { get; set; }

        public virtual long? PartListId { get; set; }

        public virtual long? MaterialId { get; set; }

        public virtual int? BeginQty { get; set; }

        public virtual int? ReceiveQty { get; set; }

        public virtual int? IssueQty { get; set; }

        public virtual int? CalculatorQty { get; set; }

        public virtual decimal? ActualQty { get; set; }

        public virtual long? PeriodId { get; set; }

        public virtual DateTime? LastCalDatetime { get; set; }

        public virtual int? Transtype { get; set; }

        [StringLength(InvCkdPhysicalStockPartPeriodConsts.MaxRemarkLength)]
        public virtual string Remark { get; set; }

        [StringLength(InvCkdPhysicalStockPartPeriodConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetInvCkdPhysicalStockPartPeriodInput : PagedAndSortedResultRequestDto
    {

        public virtual string PartNo { get; set; }

        public virtual string PartNoNormalized { get; set; }

        public virtual string PartName { get; set; }

        public virtual string PartNoNormalizedS4 { get; set; }

        public virtual string ColorSfx { get; set; }

        public virtual string LotNo { get; set; }

        public virtual string Remark { get; set; }

        public virtual string IsActive { get; set; }

    }

}

