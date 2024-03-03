using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Inventory.IF.FQF3MM05.Dto
{
    public class IF_FQF3MM05Dto : EntityDto<long?>
    {
        public virtual int? RunningNo { get; set; }

        public virtual string DocumentDate { get; set; }

        public virtual string PostingDate { get; set; }

        public virtual string DocumentHeaderText { get; set; }

        public virtual string MovementType { get; set; }

        public virtual string MaterialCodeFrom { get; set; }

        public virtual string PlantFrom { get; set; }

        public virtual string ValuationTypeFrom { get; set; }

        public virtual string StorageLocationFrom { get; set; }

        public virtual string ProductionVersion { get; set; }

        public virtual int? Quantity { get; set; }

        public virtual string UnitOfEntry { get; set; }

        public virtual string ItemText { get; set; }

        public virtual string GlAccount { get; set; }

        public virtual string CostCenter { get; set; }

        public virtual string Wbs { get; set; }

        public virtual string MaterialCodeTo { get; set; }

        public virtual string PlantTo { get; set; }

        public virtual string ValuationTypeTo { get; set; }

        public virtual string StorageLocationTo { get; set; }

        public virtual string BfPc { get; set; }

        public virtual string CancelFlag { get; set; }

        public virtual string ReffMatDocNo { get; set; }

        public virtual string VendorNo { get; set; }

        public virtual string ProfitCenter { get; set; }

        public virtual string ShipemntCat { get; set; }

        public virtual string Reference { get; set; }

        public virtual string AssetNo { get; set; }

        public virtual string SubAssetNo { get; set; }

        public virtual string EndOfRecord { get; set; }

    }

    public class GetIF_FQF3MM05Input : PagedAndSortedResultRequestDto
    {
        public virtual DateTime? PostingDateFrom { get; set; }

        public virtual DateTime? PostingDateTo { get; set; }

        public virtual string MaterialCodeFrom { get; set; }

        public virtual string ValuationTypeFrom { get; set; }

    }

    public class GetIF_FQF3MM05ExportInput
    {
        public virtual DateTime? PostingDateFrom { get; set; }

        public virtual DateTime? PostingDateTo { get; set; }

        public virtual string MaterialCodeFrom { get; set; }

        public virtual string ValuationTypeFrom { get; set; }

    }

    public class GetIF_FQF3MM05_VALIDATE : EntityDto<long?>
    {
        public virtual int? RunningNo { get; set; }

        public virtual string DocumentDate { get; set; }

        public virtual string PostingDate { get; set; }

        public virtual string DocumentHeaderText { get; set; }

        public virtual string MovementType { get; set; }

        public virtual string MaterialCodeFrom { get; set; }

        public virtual string PlantFrom { get; set; }

        public virtual string ValuationTypeFrom { get; set; }

        public virtual string StorageLocationFrom { get; set; }

        public virtual string ProductionVersion { get; set; }

        public virtual int? Quantity { get; set; }

        public virtual string UnitOfEntry { get; set; }

        public virtual string ItemText { get; set; }

        public virtual string GlAccount { get; set; }

        public virtual string CostCenter { get; set; }

        public virtual string Wbs { get; set; }

        public virtual string MaterialCodeTo { get; set; }

        public virtual string PlantTo { get; set; }

        public virtual string ValuationTypeTo { get; set; }

        public virtual string StorageLocationTo { get; set; }

        public virtual string BfPc { get; set; }

        public virtual string CancelFlag { get; set; }

        public virtual string ReffMatDocNo { get; set; }

        public virtual string VendorNo { get; set; }

        public virtual string ProfitCenter { get; set; }

        public virtual string ShipemntCat { get; set; }

        public virtual string Reference { get; set; }

        public virtual string AssetNo { get; set; }

        public virtual string SubAssetNo { get; set; }

        public virtual string EndOfRecord { get; set; }

        public virtual string ErrorDescription { get; set; }
    }


    public class GetIF_FQF3MM05_VALIDATEInput : PagedAndSortedResultRequestDto
    {
        public DateTime? PostingDateFrom { get; set; }
        public DateTime? PostingDateTo { get; set; }
    }

    public class GetIF_FQF3MM05VALIDATEExportInput
    {
        public virtual DateTime? PostingDateFrom { get; set; }

        public virtual DateTime? PostingDateTo { get; set; }

    }
}
