using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CKD.Dto
{

    public class InvCkdStockIssuingDto : EntityDto<long?>
    {

        public virtual string PartNo { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string LotNo { get; set; }

        public virtual int? NoInLot { get; set; }

        public virtual string VinNo { get; set; }

        public virtual string BodyNo { get; set; }

        public virtual string Color { get; set; }

        public virtual string PartNoNormalized { get; set; }

        public virtual string PartName { get; set; }

        public virtual string PartNoNormalizedS4 { get; set; }

        public virtual string ColorSfx { get; set; }

        public virtual long? PartListId { get; set; }

        public virtual long? PartListGradeId { get; set; }

        public virtual long? MaterialId { get; set; }

        public virtual int? Qty { get; set; }

        public virtual DateTime? TransactionDatetime { get; set; }

        public virtual string TransactionDatetime_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy HH:mm:ss}", TransactionDatetime);
                }
                catch 
                {
                    return "";
                }
            }
            set { }
        }


        public virtual long? ReferenceId { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        public virtual long? PeriodId { get; set; }

        public virtual int? GrandTotal { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditInvCkdStockIssuingDto : EntityDto<long?>
    {

        [StringLength(InvCkdStockIssuingConsts.MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(InvCkdStockIssuingConsts.MaxPartNoNormalizedLength)]
        public virtual string PartNoNormalized { get; set; }

        [StringLength(InvCkdStockIssuingConsts.MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(InvCkdStockIssuingConsts.MaxPartNoNormalizedS4Length)]
        public virtual string PartNoNormalizedS4 { get; set; }

        [StringLength(InvCkdStockIssuingConsts.MaxColorSfxLength)]
        public virtual string ColorSfx { get; set; }

        public virtual long? PartListId { get; set; }

        public virtual long? PartListGradeId { get; set; }

        public virtual long? MaterialId { get; set; }

        public virtual int? Qty { get; set; }

        public virtual DateTime? TransactionDatetime { get; set; }

        public virtual long? ReferenceId { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        public virtual long? PeriodId { get; set; }

        [StringLength(InvCkdStockIssuingConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetInvCkdStockIssuingInput : PagedAndSortedResultRequestDto
    {
        public virtual string PartNo { get; set; }
        public virtual string ColorSfx { get; set; }
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string VinNo { get; set; }
        public virtual string Cfc { get; set; }
        public virtual string LotNo { get; set; }
        public virtual int? NoInLot { get; set; }
        public virtual string PartType { get; set; }
    }
    public class InvCkdStockIssuingValidateDto : EntityDto<long?>
    { 
        public virtual string PartNo { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string LotNo { get; set; }

        public virtual string NoInLot { get; set; }

        public virtual string VinNo { get; set; }

        public virtual string MessagesError { get; set; }
    }


    public class GetInvCkdStockIssuingViewInput : PagedAndSortedResultRequestDto
    {
        public virtual DateTime? WorkingDateFrom { get; set; }

        public virtual DateTime? WorkingDateTo { get; set; }

        public virtual string PartType { get; set; }

    }

    public class GetInvCkdStockIssuingValidateInput : PagedAndSortedResultRequestDto
    {
        public virtual DateTime? WorkingDateFrom { get; set; }

        public virtual DateTime? WorkingDateTo { get; set; }

    }

    public class InvCkdStockIssuingTranslocDto : EntityDto<long?>
    {
        public virtual long? RunningNo { get; set; }
        public virtual string VinNo { get; set; }
        public virtual string DocumentDate { get; set; }
        public virtual string PostingDate { get; set; }
        public virtual string DocumentHeaderText { get; set;}
        public virtual string MovementType { get; set;}
        public virtual string MaterialCodeFrom { get; set;}
        public virtual string PlantFrom { get; set;}
        public virtual string ValuationTypeFrom { get; set;}
        public virtual string StorageLocationFrom { get; set;}
        public virtual string ProductionVersion { get; set;}
        public virtual int? Quantity { get; set;}
        public virtual string UnitOfEntry { get; set;}
        public virtual string ItemText { get; set;}
        public virtual string GlAccount { get; set;}
        public virtual string CostCenter { get; set;}
        public virtual string Wbs { get; set;}
        public virtual string MaterialCodeTo { get; set; }
        public virtual string PlantTo { get; set;}
        public virtual string ValuationTypeTo { get; set;}
        public virtual string StorageLocationTo { get; set;}
        public virtual string BfPc { get; set;}
        public virtual string CancelFlag { get; set;}
        public virtual string ReffMatDocNo { get; set;}
        public virtual string VendorNo { get; set;}
        public virtual string ProfitCenter { get; set;}
        public virtual string ShipemntCat { get; set;}
        public virtual string Reference { get; set;}
        public virtual string AssetNo { get; set;}
        public virtual string SubAssetNo { get; set;}
        public virtual string EndOfRecord { get; set;}

        public virtual int? GrandTotal { get; set; }

    }

    public class GetInvCkdStockIssuingHistoryInput : PagedAndSortedResultRequestDto
    {
        public virtual long Id { get; set; }

        public virtual string TableName { get; set; }
    }

    public class GetInvCkdStockIssuingHistoryExcelInput
    {
        public virtual long Id { get; set; }

        public virtual string TableName { get; set; }


    }
}

