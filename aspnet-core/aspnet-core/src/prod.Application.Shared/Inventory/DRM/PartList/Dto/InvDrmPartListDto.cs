using Abp.Application.Services.Dto;
using prod.Inventory.CKD.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Inventory.DRM.Dto
{
    public class InvDrmPartListDto : EntityDto<long?>
    {

        public virtual string SupplierType { get; set; }

        public virtual string SupplierCd { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string MaterialCode { get; set; }

        public virtual string MaterialSpec { get; set; }

        public virtual string PartSpec { get; set; }

        public virtual string PartSize { get; set; }

        public virtual int? BoxQty { get; set; }

        public virtual string PartCode { get; set; }

        public virtual DateTime? FirstDayProduct { get; set; }

        public virtual DateTime? LastDayProduct { get; set; }

        public virtual string Sourcing { get; set; }

        public virtual string Cutting { get; set; }

        public virtual int? Packing { get; set; }

        public virtual decimal? SheetWeight { get; set; }

        public virtual decimal? YiledRation { get; set; }

        //new
        public virtual long? AssetId { get; set; }
        public virtual string MainAssetNumber { get; set; }
        public virtual string WBS { get; set; }
        public virtual string CostCenter { get; set; }
        public virtual string ResponsibleCostCenter { get; set; }
        public virtual string CostOfAsset { get; set; }
        public virtual string AssetSubNumber { get; set; }
        public virtual string FixedAssetNumber { get; set; }
        public virtual string AssetDescription { get; set; }
        public virtual string AdditionalAssetDescription { get; set; }
        public virtual string SerialNumber { get; set; }
        public virtual string AssetClass { get; set; }

        public virtual string AssetClassDescription { get; set; }

         public virtual decimal? FinPartPrice { get; set; }
        public virtual string FinMaterialNumber { get; set; }
         public virtual string FinMaterialCode { get; set; }
        public virtual string FinSpec { get; set; }
        public virtual string FinPartSize { get; set; }
        public virtual string Model { get; set; }
        public virtual string GradeName { get; set; }
        public virtual string ModelCode { get; set; }
        public virtual long MaterialId { get; set; }
        public virtual string CreateMaterial { get; set; }

        public virtual string SizeCode { get; set; }

    }
    public class GetDrmPartListId
    {
        public virtual long? DrmPartListId { get; set; }
    }

    public class GetInvDrmPartListInput : PagedAndSortedResultRequestDto
    {
        public virtual string SupplierType { get; set; }

        public virtual string SupplierCd { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string MaterialCode { get; set; }

        public virtual string MaterialSpec { get; set; }


    }
    public class CreateOrEditInvDrmPartListDto : EntityDto<long?>
    {
        public virtual string SupplierType { get; set; }

        public virtual string SupplierCd { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string MaterialCode { get; set; }

        public virtual string MaterialSpec { get; set; }

        public virtual string PartSpec { get; set; }

        public virtual string PartSize { get; set; }

        public virtual int? BoxQty { get; set; }

        public virtual string PartCode { get; set; }

        public virtual DateTime? FirstDayProduct { get; set; }

        public virtual DateTime? LastDayProduct { get; set; }

        public virtual string Sourcing { get; set; }

        public virtual string Cutting { get; set; }

        public virtual int? Packing { get; set; }

        public virtual decimal? SheetWeight { get; set; }

        public virtual decimal? YiledRation { get; set; }
        public virtual decimal? FinPartPrice { get; set; }
        public virtual string FinMaterialNumber { get; set; }
         public virtual string FinMaterialCode { get; set; }
        public virtual string FinSpec { get; set; }
        public virtual string FinPartSize { get; set; }
        public virtual string Model { get; set; }
        public virtual string GradeName { get; set;}
        public virtual string ModelCode { get; set;}
        public virtual long MaterialId { get; set; }
        public virtual long AssetId { get; set; }
        public virtual string MainAssetNumber { get; set; }
        public virtual string AssetSubNumber { get; set; }
        public virtual string WBS { get; set; }
        public virtual string CostCenter { get; set; }
        public virtual string ResponsibleCostCenter { get; set; }
        public virtual string CostOfAsset { get; set;}
        public virtual string CreateMaterial { get; set; }

        public virtual string SizeCode { get; set; }

    }
    public class CreateOrEditAsset : EntityDto<long?>
    {
        public virtual string AssetId { get; set;}
        public virtual string MainAssetNumber { get; set; }
        public virtual string WBS { get; set; }
        public virtual string CostCenter { get; set; }
        public virtual string ResponsibleCostCenter { get; set; }
        public virtual string CostOfAsset { get; set; }
    }

    public class GetInvDrmPartListExportInput
    {
        public virtual string SupplierType { get; set; }

        public virtual string SupplierCd { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string MaterialCode { get; set; }

        public virtual string MaterialSpec { get; set; }

    }

    public class InvDrmIhpPartImportDto
    {
        public virtual long? ROW_NO { get; set; }

        public virtual string Guid { get; set; }

        public virtual string SupplierType { get; set; }

        public virtual string SupplierDrm { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string MaterialCode { get; set; }

        public virtual string MaterialSpec { get; set; }

        public virtual int? BoxQty { get; set; }

        public virtual string SupplierIhp { get; set; }

        public virtual string CfcIhp { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual string PartCode { get; set; }

        public virtual string Spec { get; set; }

        public virtual string Size { get; set; }

        public virtual DateTime? FirstDayProduct { get; set; }

        public virtual DateTime? LastDayProduct { get; set; }

        public virtual string Grade { get; set; }

        public virtual int? UsageQty { get; set; }

        public virtual string ErrorDescription { get; set; }

        public virtual string DrmOrIhp { get; set; }

        public virtual string Sourcing { get; set; }

        public virtual string Cutting { get; set; }
        public virtual string AssetId { get; set; }
        public virtual string MainAssetNumber { get; set; }
        public virtual string WBS { get; set; }
        public virtual string CostCenter { get; set; }
        public virtual string ResponsibleCostCenter { get; set; }
        public virtual string CostOfAsset { get; set; }

        public virtual int? Packing { get; set; }

        public virtual decimal? SheetWeight { get; set; }

        public virtual decimal? PanelWeight { get; set; }

        public virtual decimal? YiledRation { get; set; }


    }


    public class InvCpsSapAssetInput : EntityDto<long?>
    {
        public virtual string CurrentLineItemId { get; set; }
        public virtual string LineItem { get; set; }
        public virtual string SubAssetNumber { get; set; }
        public virtual string WBS { get; set; }
        public virtual string CostCenter { get; set; }
        public virtual string ResponsibleCostCenter { get; set; }
        public virtual string CostOfAsset { get; set; }
        public virtual long? DrmPartListId { get; set; }
        public virtual string IsUse { get; set; }

    }
    public class GetInvDrmPartListHistoryInput : PagedAndSortedResultRequestDto
    {
        public virtual long Id { get; set; }
        public virtual string TableName { get; set; }
    }
    public class GetInvDrmPartListHistoryExcelInput 
    {
        public virtual long Id { get; set; }
        public virtual string TableName { get; set; }
    }

}
