using Abp.Application.Services.Dto;
using System;

namespace prod.Inventory.Gps.Mapping.Dto
{
    public class InvGpsMappingDto : EntityDto<long?>
    {
        public virtual long? PartId { get; set; }
        public virtual string PartNo { get; set; }
        public virtual string PartType { get; set; }
        public virtual string PartCatetory { get; set; }
        public virtual string ShopRegister { get; set; }
        public virtual string Location { get; set; }
        public virtual string CostCenter { get; set; }
        public virtual string Wbs { get; set; }
        public virtual string GlAccount { get; set; }
        public virtual string ExpenseAccount { get; set; }
        public virtual DateTime? EffectiveDateTo { get; set; }
        public virtual DateTime? EffectiveDateFrom { get; set; }
        public virtual string LastRenew { get; set; }
        public virtual string RenewBy { get; set; }
        public virtual string Status { get; set; }
        public virtual string IsAcive { get; set; }
    }

    public class GetInvGpsMappingInput : PagedAndSortedResultRequestDto
    {
        public virtual string PartNo { get; set; }
        public virtual string PartCatetory { get; set; }
        public virtual string ShopRegister { get; set; }
        public virtual string CostCenter { get; set; }
        public virtual string Wbs { get; set; }
        public virtual string GlAccount { get; set; }
        
    }

    public class GetInvGpsMappingExportInput
    {
        public virtual string PartNo { get; set; }
        public virtual string PartCatetory { get; set; }
        public virtual string ShopRegister { get; set; }
        public virtual string CostCenter { get; set; }
        public virtual string Wbs { get; set; }
        public virtual string GlAccount { get; set; }
    }
}
