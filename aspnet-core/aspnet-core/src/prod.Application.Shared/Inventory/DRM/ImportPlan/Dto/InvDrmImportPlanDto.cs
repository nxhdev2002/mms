using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.DRM.Dto
{

    public class InvDrmImportPlanDto : EntityDto<long?>
    {

        public virtual string SupplierNo { get; set; }

        public virtual DateTime? Etd { get; set; }

        public virtual DateTime? Eta { get; set; }


        public virtual string ShipmentNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string PartCode { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual int? Qty { get; set; }

        public virtual DateTime? PackingMonth { get; set; }

        public virtual DateTime? DelayEtd { get; set; }

        public virtual DateTime? DelayEta { get; set; }

        public virtual string Remark { get; set; }

        public virtual DateTime? Ata { get; set; }  
        
        public virtual string Status { get; set; }

    }

    public class CreateOrEditInvDrmImportPlanDto : EntityDto<long?>
    {
        public virtual string Status { get; set; }
        [StringLength(InvDrmImportPlanConsts.MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        public virtual DateTime? Etd { get; set; }

        public virtual DateTime? Eta { get; set; }

        [StringLength(InvDrmImportPlanConsts.MaxShipmentNoLength)]
        public virtual string ShipmentNo { get; set; }

        [StringLength(InvDrmImportPlanConsts.MaxCfcLength)]
        public virtual string Cfc { get; set; }

        [StringLength(InvDrmImportPlanConsts.MaxPartCodeLength)]
        public virtual string PartCode { get; set; }

        [StringLength(InvDrmImportPlanConsts.MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(InvDrmImportPlanConsts.MaxPartNameLength)]
        public virtual string PartName { get; set; }

        public virtual int? Qty { get; set; }

        public virtual DateTime? PackingMonth { get; set; }

        public virtual DateTime? DelayEtd { get; set; }

        public virtual DateTime? DelayEta { get; set; }

        [StringLength(InvDrmImportPlanConsts.MaxRemarkLength)]
        public virtual string Remark { get; set; }

        public virtual DateTime? Ata { get; set; }
    }

    public class GetInvDrmImportPlanInput : PagedAndSortedResultRequestDto
    {

        public virtual string SupplierNo { get; set; }

        public virtual DateTime? Etd { get; set; }

        public virtual DateTime? Eta { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string PartCode { get; set; }

        public virtual string PartNo { get; set; }

        
    }

    public class GetDrmImportPlanExportInput : PagedAndSortedResultRequestDto
    {

        public virtual string SupplierNo { get; set; }

        public virtual DateTime? Etd { get; set; }

        public virtual DateTime? Eta { get; set; }

        public virtual string ShipmentNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string PartCode { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual int? Qty { get; set; }

        public virtual DateTime? PackingMonth { get; set; }

        public virtual DateTime? DelayEtd { get; set; }

        public virtual DateTime? DelayEta { get; set; }

        public virtual string Remark { get; set; }

        public virtual DateTime? Ata { get; set; }


    }
    public class InvDrmImportPlanImportDto 
    {
        public virtual long? ROW_NO { get; set; }
        public virtual string Guid { get; set; }
        public virtual string SupplierNo { get; set; }

        public virtual DateTime? Etd { get; set; }

        public virtual DateTime? Eta { get; set; }

        public virtual string ShipmentNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string PartCode { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual int? Qty { get; set; }

        public virtual DateTime? PackingMonth { get; set; }

        public virtual DateTime? DelayEtd { get; set; }

        public virtual DateTime? DelayEta { get; set; }

        public virtual string Remark { get; set; }

        public virtual DateTime? Ata { get; set; }

        public virtual string ErrorDescription { get; set; }

    }
}


