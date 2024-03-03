using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CKD.Dto
{
    public class InvCkdProductionMappingDto : EntityDto<long?>
    {
        public virtual long? ROW_NO { get; set; }
        public virtual string Guid { get; set; }

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

        public virtual DateTime? CreationTime { get; set; }

        public virtual string ErrorDescription { get; set; }

        public virtual long? CreatorUserId { get; set; }

    }

    public class CreateOrEditInvCkdProductionMappingDto : EntityDto<long?>
    {

        public virtual long? PlanSequence { get; set; }

        [StringLength(InvCkdProductionMappingConsts.MaxShopLength)]
        public virtual string Shop { get; set; }

        [StringLength(InvCkdProductionMappingConsts.MaxModelLength)]
        public virtual string Model { get; set; }

        [StringLength(InvCkdProductionMappingConsts.MaxLotNoLength)]
        public virtual string LotNo { get; set; }

        [StringLength(InvCkdProductionMappingConsts.MaxNoInLotLength)]
        public virtual string NoInLot { get; set; }

        [StringLength(InvCkdProductionMappingConsts.MaxGradeLength)]
        public virtual string Grade { get; set; }

        [StringLength(InvCkdProductionMappingConsts.MaxBodyNoLength)]
        public virtual string BodyNo { get; set; }

        public virtual DateTime? DateIn { get; set; }

        [StringLength(InvCkdProductionMappingConsts.MaxTimeInLength)]
        public virtual string TimeIn { get; set; }

        [StringLength(InvCkdProductionMappingConsts.MaxUseLotNoLength)]
        public virtual string UseLotNo { get; set; }

        [StringLength(InvCkdProductionMappingConsts.MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        public virtual long? PartId { get; set; }

        public virtual decimal? Quantity { get; set; }

        public virtual long? PeriodId { get; set; }

        public virtual long? WipId { get; set; }

        public virtual long? InStockId { get; set; }

        public virtual long? MappingId { get; set; }
    }

    public class GetInvCkdProductionMappingInput : PagedAndSortedResultRequestDto
    {
        public virtual decimal? PeriodId { get; set; }

        public virtual string Shop { get; set; }

        public virtual string BodyNo { get; set; }

        public virtual string LotNo { get; set; }

        public virtual string UseLotNo { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual DateTime? DateInFrom { get; set; }

        public virtual DateTime? DateInTo { get; set; }

    }
    public class GetInvProductionMappingHistoryInput : PagedAndSortedResultRequestDto
    {
        public virtual long Id { get; set; }

        public virtual string TableName { get; set; }
    }

    public class GetInvCkdProductionMappingHistoryExcelInput
    {
        public virtual long Id { get; set; }

        public virtual string TableName { get; set; }
    }
}