using Abp.Application.Services.Dto;
using System;
namespace prod.Inventory.DRM.Dto
{
    public class InvDrmLocalPlanDto : EntityDto<long?>
    {
        public virtual string SupplierNo { get; set; }

        public virtual DateTime? DeliveryDate { get; set; }

        public virtual string ShipmentNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string PartCode { get; set; }

        public virtual string MaterialCode { get; set; }

        public virtual string MaterialSpec { get; set; }

        public virtual int? Qty { get; set; }

        public virtual DateTime? DeliveryMonth { get; set; }

        public virtual DateTime? DelayDelivery { get; set; }

        public virtual string Remark { get; set; }

    }

    public class GetInvDrmLocalPlanInput : PagedAndSortedResultRequestDto
    {
        public virtual string SupplierNo { get; set; }

        public virtual string ShipmentNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string PartCode { get; set; }

        public virtual string MaterialCode { get; set; }

    }

    public class GetInvDrmLocalPlanExportInput
    {
        public virtual string SupplierNo { get; set; }

        public virtual string ShipmentNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string PartCode { get; set; }

        public virtual string MaterialCode { get; set; }

    }

}

