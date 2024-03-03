using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.GPS.Dto
{

    public class InvGpsMaterialDto : EntityDto<long?>
    {
        public virtual string PartNo { get; set; }
        public virtual string PartNoNormalized { get; set; }
        public virtual string PartName { get; set; }
        public virtual string PartNameVn { get; set; }
        public virtual string ColorSfx { get; set; }
        public virtual string PartType { get; set; }
        public virtual string Category { get; set; }
        public virtual string Location { get; set; }
        public virtual string PurposeOfUse { get; set; }
        public virtual string Spec { get; set; }
        public virtual string HasExpiryDate { get; set; }
        public virtual string PartGroup { get; set; }
        public virtual decimal? Price { get; set; }
        public virtual string Currency { get; set; }
        public virtual int? ConvertPrice { get; set; }
        public virtual string UOM { get; set; } 
        public virtual string SupplierName { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string LocalImport { get; set; }
        public virtual int? LeadTime { get; set; }
        public virtual int? LeadTimeForecast { get; set; }
        public virtual decimal? MinLot { get; set; }
        public virtual decimal? BoxQty { get; set; }
        public virtual string Remark { get; set; }
        public virtual int? PalletL { get; set; }
        public virtual int? PalletR { get; set; }
        public virtual int? PalletH { get; set; }
        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditInvGpsMaterialDto : EntityDto<long?>
    {

        [StringLength(InvGpsMaterialConsts.MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(InvGpsMaterialConsts.MaxPartNoNormalizedLength)]
        public virtual string PartNoNormalized { get; set; }

        [StringLength(InvGpsMaterialConsts.MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(InvGpsMaterialConsts.MaxPartNameVnLength)]
        public virtual string PartNameVn { get; set; }

        [StringLength(InvGpsMaterialConsts.MaxColorSfxLength)]
        public virtual string ColorSfx { get; set; }

        [StringLength(InvGpsMaterialConsts.MaxPartTypeLength)]
        public virtual string PartType { get; set; }
        
        [StringLength(InvGpsMaterialConsts.MaxCategoryLength)]
        public virtual string Category { get; set; }
        
        [StringLength(InvGpsMaterialConsts.MaxLocationLength)]
        public virtual string Location { get; set; }

        [StringLength(InvGpsMaterialConsts.MaxPurposeOfUseLength)]
        public virtual string PurposeOfUse { get; set; }

        [StringLength(InvGpsMaterialConsts.MaxSpecLength)]
        public virtual string Spec { get; set; }

        [StringLength(InvGpsMaterialConsts.MaxHasExpiryDateLength)]
        public virtual string HasExpiryDate { get; set; }

        [StringLength(InvGpsMaterialConsts.MaxPartGroupLength)]
        public virtual string PartGroup { get; set; }

        public virtual decimal? Price { get; set; }

        [StringLength(InvGpsMaterialConsts.MaxCurrencyLength)]
        public virtual string Currency { get; set; }

        public virtual int? ConvertPrice { get; set; }

        [StringLength(InvGpsMaterialConsts.MaxUOMLength)]
        public virtual string UOM { get; set; }

        [StringLength(InvGpsMaterialConsts.MaxSupplierNameLength)]
        public virtual string SupplierName { get; set; }

        [StringLength(InvGpsMaterialConsts.MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(InvGpsMaterialConsts.MaxLocalImportLength)]
        public virtual string LocalImport { get; set; }

        public virtual int? LeadTime { get; set; }

        public virtual int? LeadTimeForecast { get; set; }

        public virtual decimal? MinLot { get; set; }

        public virtual decimal? BoxQty { get; set; }

        [StringLength(InvGpsMaterialConsts.MaxRemarkLength)]
        public virtual string Remark { get; set; }

        public virtual int? PalletL { get; set; }

        public virtual int? PalletR { get; set; }

        public virtual int? PalletH { get; set; }

        [StringLength(InvGpsMaterialConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetInvGpsMaterialInput : PagedAndSortedResultRequestDto
    {
        public virtual string PartNo { get; set; }
        public virtual string Supplier { get; set; }
        public virtual string PartType { get; set; }
        public virtual string IsExpDate { get; set; }
        public virtual string PartGroup { get; set; }
        public virtual string LocalImport { get; set; }

        public virtual string IsActive { get; set; }
        public virtual string PartName { get; set; }
        public virtual string Category { get; set; }
        public virtual string Location { get; set; }

    }
    public class ImportInvGpsMaterialDto : EntityDto<long?>
    {
        public virtual long? ROW_NO { get; set; }
        public virtual string Guid { get; set; }
        public virtual string PartNo { get; set; }
        public virtual string PartNoNormalized { get; set; }
        public virtual string PartName { get; set; }
        public virtual string PartNameVn { get; set; }
        public virtual string ColorSfx { get; set; }
        public virtual string PartType { get; set; }
        public virtual string Category { get; set; }
        public virtual string Location { get; set; }
        public virtual string PurposeOfUse { get; set; }
        public virtual string Spec { get; set; }
        public virtual string HasExpiryDate { get; set; }
        public virtual string PartGroup { get; set; }
        public virtual decimal? Price { get; set; }
        public virtual string Currency { get; set; }
        public virtual int? ConvertPrice { get; set; }
        public virtual string UOM { get; set; }
        public virtual string SupplierName { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string LocalImport { get; set; }
        public virtual int? LeadTime { get; set; }
        public virtual int? LeadTimeForecast { get; set; }
        public virtual decimal? MinLot { get; set; }
        public virtual decimal? BoxQty { get; set; }
        public virtual string Remark { get; set; }
        public virtual int? PalletL { get; set; }
        public virtual int? PalletR { get; set; }
        public virtual int? PalletH { get; set; }
        public virtual string IsActive { get; set; }
        public virtual string ErrorDescription { get; set; }
        public virtual long? CreatorUserId { get; set; }

    }
    public class GetInvGpsMaterialHistoryInput : PagedAndSortedResultRequestDto
    {
        public virtual long Id { get; set; }
        public virtual string TableName { get; set; }
    }
     public class GetInvGpsMaterialHistoryExcelInput 
    {
        public virtual long Id { get; set; }
        public virtual string TableName { get; set; }
    }

    

}


