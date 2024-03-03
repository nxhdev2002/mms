using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace prod.Inventory.CPS
{
    [Table("InvCpsSapAssetMaster")]
    public class InvCpsSapAssetMaster : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxLineItemLength = 4000;
        public const int MaxStatusRemarkLength = 4000;
        public const int MaxCompanyCodeLength = 8;
        public const int MaxFixedAssetNumberLength = 12;
        public const int MaxSubAssetNumberLength = 4;
        public const int MaxAssetDescriptionLength = 50;
        public const int MaxAdditionalAssetDescriptionLength = 50;
        public const int MaxAssetClassLength = 8;
        public const int MaxAssetClassDescriptionLength = 20;
        public const int MaxSerialNumberLength = 18;
        public const int MaxWBSLength = 24;
        public const int MaxCostCenterLength = 10;
        public const int MaxResponsibleCostCenterLength = 10;
        public const int MaxDeactivationDateLength = 10;
        public const int MaxAcquisitionLockLength = 1;
        public const int MaxCostOfAssetLength = 24;

        [StringLength(MaxLineItemLength)]
        public virtual string LineItem { get; set; }

        [StringLength(MaxStatusRemarkLength)]
        public virtual string StatusRemark { get; set; }

        [StringLength(MaxCompanyCodeLength)]
        public virtual string CompanyCode { get; set; }

        [StringLength(MaxFixedAssetNumberLength)]
        public virtual string FixedAssetNumber { get; set; }

        [StringLength(MaxSubAssetNumberLength)]
        public virtual string SubAssetNumber { get; set; }

        [StringLength(MaxAssetDescriptionLength)]
        public virtual string AssetDescription { get; set; }

        [StringLength(MaxAdditionalAssetDescriptionLength)]
        public virtual string AdditionalAssetDescription { get; set; }

        [StringLength(MaxAssetClassLength)]
        public virtual string AssetClass { get; set; }

        [StringLength(MaxAssetClassDescriptionLength)]
        public virtual string AssetClassDescription { get; set; }

        [StringLength(MaxSerialNumberLength)]
        public virtual string SerialNumber { get; set; }

        [StringLength(MaxWBSLength)]
        public virtual string WBS { get; set; }

        [StringLength(MaxCostCenterLength)]
        public virtual string CostCenter { get; set; }

        [StringLength(MaxResponsibleCostCenterLength)]
        public virtual string ResponsibleCostCenter { get; set; }

        [StringLength(MaxDeactivationDateLength)]
        public virtual string DeactivationDate { get; set; }

        [StringLength(MaxAcquisitionLockLength)]
        public virtual string AcquisitionLock { get; set; }

        [StringLength(MaxCostOfAssetLength)]
        public virtual string CostOfAsset { get; set; }

        public virtual long? Ordering { get; set; }
    }
}
