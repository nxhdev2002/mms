using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Inventory.CPS.SapAssetMaster.Dto
{
    public class InvCpsSapAssetMasterDto : EntityDto<long?>
    {
        public virtual string LineItem { get; set; }
        public virtual string StatusRemark { get; set; }
        public virtual string CompanyCode { get; set; }
        public virtual string FixedAssetNumber { get; set; }
        public virtual string SubAssetNumber { get; set; }
        public virtual string AssetDescription { get; set; }
        public virtual string AdditionalAssetDescription { get; set; }
        public virtual string AssetClass { get; set; }
        public virtual string AssetClassDescription { get; set; }
        public virtual string SerialNumber { get; set; }
        public virtual string WBS { get; set; }
        public virtual string CostCenter { get; set; }
        public virtual string ResponsibleCostCenter { get; set; }
        public virtual string DeactivationDate { get; set; }
        public virtual string AcquisitionLock { get; set; }
        public virtual string CostOfAsset { get; set; }
        public virtual long? Ordering { get; set; }
    }

    public class GetInvCpsSapAssetMasterInput : PagedAndSortedResultRequestDto
    {
        public virtual string CompanyCode { get; set; }
        public virtual string FixedAssetNumber { get; set; }
        public virtual string SerialNumber { get; set; }
        public virtual string WBS { get; set; }
        public virtual string CostCenter { get; set; }
    }

    public class GetInvCpsSapAssetMasterExportInput : PagedAndSortedResultRequestDto
    {
        public virtual string CompanyCode { get; set; }
        public virtual string FixedAssetNumber { get; set; }
        public virtual string SerialNumber { get; set; }
        public virtual string WBS { get; set; }
        public virtual string CostCenter { get; set; }
    }
}
