using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Inventory.DRM.StockPart.Dto
{
    public class InvDrmStockPartDto : EntityDto<long?>
    {
        public virtual string SupplierNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string MaterialCode { get; set; }

        public virtual string MaterialSpec { get; set; }

        public virtual string PartCode { get; set; }

        public virtual long? DrmMaterialId { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual long? PartId { get; set; }

        public virtual long? MaterialId { get; set; }

        public virtual int? Qty { get; set; }

        public virtual DateTime? WorkingDate { get; set; }
    }

    public class GetInvDrmStockPartInput : PagedAndSortedResultRequestDto
    {

        public virtual string MaterialCode { get; set; }

        public virtual string MaterialSpec { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

    }

    public class GetInvDrmStockPartExportInput
    {

        public virtual string MaterialCode { get; set; }

        public virtual string MaterialSpec { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

    }

    public class InvDrmStockPartImportDto
    {
        public virtual long? ROW_NO { get; set; }

        public virtual string Guid { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string MaterialCode { get; set; }

        public virtual string MaterialSpec { get; set; }

        public virtual string PartCode { get; set; }

        public virtual int? Qty { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        public virtual string ErrorDescription { get; set; }

    }

}
