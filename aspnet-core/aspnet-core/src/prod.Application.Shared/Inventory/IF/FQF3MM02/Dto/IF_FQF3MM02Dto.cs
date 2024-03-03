using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Inventory.IF.FQF3MM02.Dto
{
    public class IF_FQF3MM02Dto : EntityDto<long?>
    {
        public virtual string RecordId { get; set; }

        public virtual string CompanyCode { get; set; }

        public virtual string PlantCode { get; set; }

        public virtual string MaruCode { get; set; }

        public virtual string ReceivingStockLine { get; set; }

        public virtual string ProductionDate { get; set; }

        public virtual string PostingDate { get; set; }

        public virtual string PartCode { get; set; }

        public virtual int? Quantity { get; set; }

        public virtual int? SpoiledPartsQuantity { get; set; }

        public virtual int? SpoiledMaterialQuantity1 { get; set; }

        public virtual string MaterialCode { get; set; }

        public virtual int? FreeShotQuantity { get; set; }

        public virtual int? RecycledQuantity { get; set; }

        public virtual string CostCenter { get; set; }

        public virtual string NormalCancelFlag { get; set; }

        public virtual string GrgiNo { get; set; }

        public virtual string GrgiType { get; set; }

        public virtual string MaterialDocType { get; set; }

        public virtual int? MaterialQuantity { get; set; }

        public virtual int? SpoiledMaterialQuantity2 { get; set; }

        public virtual string RelatedPartReceiveNo { get; set; }

        public virtual string RelatedGrType { get; set; }

        public virtual string RelatedGrTransactionType { get; set; }

        public virtual int? InHousePartQuantityReceive { get; set; }

        public virtual string RelatedPartIssueNo { get; set; }

        public virtual string RelatedGiType { get; set; }

        public virtual string RelatedGiTransactionType { get; set; }

        public virtual int? RelatedInHousePartQuantityIssued { get; set; }

        public virtual int? RelatedSpoiledPartQuantityIssued { get; set; }

        public virtual int? Wip { get; set; }

        public virtual string ProductionId { get; set; }

        public virtual int? FinalPrice { get; set; }

        public virtual string Wbs { get; set; }

        public virtual string EarmarkedFund { get; set; }

        public virtual string EarmarkedFundItem { get; set; }

        public virtual string PsmsCode { get; set; }

        public virtual string GiUom { get; set; }

        public virtual string EndingOfRecord { get; set; }
    }

    public class GetIF_FQF3MM02Input : PagedAndSortedResultRequestDto
    {
        public virtual DateTime? PostingDateFrom { get; set; }

        public virtual DateTime? PostingDateTo { get; set; }

        public virtual string PartCode { get; set; }

        public virtual string MaterialCode { get; set; }

    }

    public class GetIF_FQF3MM02ExportInput
    {
        public virtual DateTime? PostingDateFrom { get; set; }

        public virtual DateTime? PostingDateTo { get; set; }

        public virtual string PartCode { get; set; }

        public virtual string MaterialCode { get; set; }

    }

    public class GetIF_FQF3MM02_VALIDATE : EntityDto<long?>
    {
        public virtual string RecordId { get; set; }

        public virtual string CompanyCode { get; set; }

        public virtual string PlantCode { get; set; }

        public virtual string MaruCode { get; set; }

        public virtual string ReceivingStockLine { get; set; }

        public virtual string ProductionDate { get; set; }

        public virtual string PostingDate { get; set; }

        public virtual string PartCode { get; set; }

        public virtual int? Quantity { get; set; }

        public virtual int? SpoiledPartsQuantity { get; set; }

        public virtual int? SpoiledMaterialQuantity1 { get; set; }

        public virtual string MaterialCode { get; set; }

        public virtual int? FreeShotQuantity { get; set; }

        public virtual int? RecycledQuantity { get; set; }

        public virtual string CostCenter { get; set; }

        public virtual string NormalCancelFlag { get; set; }

        public virtual string GrgiNo { get; set; }

        public virtual string GrgiType { get; set; }

        public virtual string MaterialDocType { get; set; }

        public virtual int? MaterialQuantity { get; set; }

        public virtual int? SpoiledMaterialQuantity2 { get; set; }

        public virtual string RelatedPartReceiveNo { get; set; }

        public virtual string RelatedGrType { get; set; }

        public virtual string RelatedGrTransactionType { get; set; }

        public virtual int? InHousePartQuantityReceive { get; set; }

        public virtual string RelatedPartIssueNo { get; set; }

        public virtual string RelatedGiType { get; set; }

        public virtual string RelatedGiTransactionType { get; set; }

        public virtual int? RelatedInHousePartQuantityIssued { get; set; }

        public virtual int? RelatedSpoiledPartQuantityIssued { get; set; }

        public virtual int? Wip { get; set; }

        public virtual string ProductionId { get; set; }

        public virtual int? FinalPrice { get; set; }

        public virtual string Wbs { get; set; }

        public virtual string EarmarkedFund { get; set; }

        public virtual string EarmarkedFundItem { get; set; }

        public virtual string PsmsCode { get; set; }

        public virtual string GiUom { get; set; }

        public virtual string EndingOfRecord { get; set; }
        public virtual long? HeaderFwgId { get; set; }
        public virtual long? HeaderId { get; set; }
        public virtual string TrailerId { get; set; }
        public virtual string ErrorDescription { get; set; }
    }


    public class GetIF_FQF3MM02_VALIDATEInput : PagedAndSortedResultRequestDto
    {
        public DateTime? PostingDateFrom { get; set; }

        public DateTime? PostingDateTo { get; set; }
    }

    public class GetIF_FQF3MM02_VALIDATE_Input
    {
        public DateTime? PostingDateFrom { get; set; }

        public DateTime? PostingDateTo { get; set; }
    }
}
