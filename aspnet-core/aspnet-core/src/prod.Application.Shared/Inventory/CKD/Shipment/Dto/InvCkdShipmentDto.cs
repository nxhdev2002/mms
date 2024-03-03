using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CKD.Dto
{

    public class InvCkdShipmentDto : EntityDto<long?>
    {

        public virtual string ShipmentNo { get; set; }

        public virtual string ShippingcompanyCode { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string Buyer { get; set; }

        public virtual string FromPort { get; set; }

        public virtual string ToPort { get; set; }

        public virtual string ShipmentDate { get; set; }

        public virtual DateTime? Etd { get; set; }

        public virtual DateTime? Eta { get; set; }

        public virtual DateTime? Ata { get; set; }

        public virtual string FeedervesselName { get; set; }

        public virtual string OceanvesselName { get; set; }

        public virtual string OceanvesselvoyageNo { get; set; }

        public virtual int? NoofinvoicewithinshipmentNo { get; set; }

        public virtual int? Noof20Ftcontainers { get; set; }

        public virtual int? Noof40Ftcontainers { get; set; }

        public virtual int? Lclvolume { get; set; }

        public virtual DateTime? Atd { get; set; }

        public virtual string Status { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditInvCkdShipmentDto : EntityDto<long?>
    {

        [StringLength(InvCkdShipmentConsts.MaxShipmentNoLength)]
        public virtual string ShipmentNo { get; set; }

        [StringLength(InvCkdShipmentConsts.MaxShippingcompanyCodeLength)]
        public virtual string ShippingcompanyCode { get; set; }

        [StringLength(InvCkdShipmentConsts.MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(InvCkdShipmentConsts.MaxBuyerLength)]
        public virtual string Buyer { get; set; }

        [StringLength(InvCkdShipmentConsts.MaxFromPortLength)]
        public virtual string FromPort { get; set; }

        [StringLength(InvCkdShipmentConsts.MaxToPortLength)]
        public virtual string ToPort { get; set; }

        [StringLength(InvCkdShipmentConsts.MaxShipmentDateLength)]
        public virtual string ShipmentDate { get; set; }

        public virtual DateTime? Etd { get; set; }

        public virtual DateTime? Eta { get; set; }

        public virtual DateTime? Ata { get; set; }

        [StringLength(InvCkdShipmentConsts.MaxFeedervesselNameLength)]
        public virtual string FeedervesselName { get; set; }

        [StringLength(InvCkdShipmentConsts.MaxOceanvesselNameLength)]
        public virtual string OceanvesselName { get; set; }

        [StringLength(InvCkdShipmentConsts.MaxOceanvesselvoyageNoLength)]
        public virtual string OceanvesselvoyageNo { get; set; }

        public virtual int? NoofinvoicewithinshipmentNo { get; set; }

        public virtual int? Noof20Ftcontainers { get; set; }

        public virtual int? Noof40Ftcontainers { get; set; }

        public virtual int? Lclvolume { get; set; }

        public virtual DateTime? Atd { get; set; }

        [StringLength(InvCkdShipmentConsts.MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(InvCkdShipmentConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetInvCkdShipmentInput : PagedAndSortedResultRequestDto
    {

        public virtual string ShipmentNo { get; set; }

        public virtual string ShippingcompanyCode { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string FromPort { get; set; }

        public virtual string ToPort { get; set; }

        public virtual string ShipmentDate { get; set; }

        public virtual string CkdPio { get; set; }

        public virtual string OrderTypeCode { get; set; }
    }

    public class InvCkdShipmentExportInput 
    {

        public virtual string ShipmentNo { get; set; }

        public virtual string ShippingcompanyCode { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string FromPort { get; set; }

        public virtual string ToPort { get; set; }

        public virtual string ShipmentDate { get; set; }

        public virtual string CkdPio { get; set; }

        public virtual string OrderTypeCode { get; set; }
    }
    



    

}


