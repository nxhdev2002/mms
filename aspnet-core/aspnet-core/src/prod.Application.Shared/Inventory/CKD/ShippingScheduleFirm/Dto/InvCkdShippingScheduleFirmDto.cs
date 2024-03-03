using Abp.Application.Services.Dto;
using prod.Master.CKD;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Inventory.CKD.ShippingScheduleFirm.Dto
{
    public class InvCkdShippingScheduleFirmDto : EntityDto<long?>
    {
        public long? OriginalId { get; set; }
        public string ShipmentNo { get; set; }
        public string Segment { get; set; }
        public string DateSs { get; set; }
        public string Seller { get; set; }
        public string Buyer { get; set; }
        public int? Index { get; set; }
        public string ShippingMonth { get; set; }
        public string EkanbanFlag { get; set; }
        public string RevisionNo { get; set; }
        public string PortOfLoading { get; set; }
        public string VesselName1st { get; set; }
        public string VesselNo1st { get; set; }
        public string VesselEta1st { get; set; }
        public string TranShipmentPort1 { get; set; }
        public string VesselEtd1St { get; set; }
        public string VesselEtd2nd { get; set; }
        public string VesselName2nd { get; set; }
        public string VesselNo2nd { get; set; }
        public string VesselEta2nd { get; set; }
        public string TranShipmentPort2 { get; set; }
        public string VesselEtd3rd { get; set; }
        public string VesselName3rd { get; set; }
        public string VesselNo3rd { get; set; }
        public string VesselEta3rd { get; set; }
        public string TranShipmentPort3 { get; set; }
        public string VesselEtd4th { get; set; }
        public string VesselName4th { get; set; }
        public string VesselNo4th { get; set; }
        public string VesselEts { get; set; }
        public string PortOfDischarge { get; set; }
        public string TotalOfCargoes { get; set; }
        public string TotalM3OfCargoes { get; set; }
        public string TotalContainers20 { get; set; }
        public string TotalContainers40 { get; set; }
        public string ShippingCompany { get; set; }
        public string VesselBookingRefNo { get; set; }
        public string ValuationTypeFrom { get; set; }
        public string Status { get; set; }
    }


    public class InvCkdShippingScheduleFirmInput : PagedAndSortedResultRequestDto
    {
        public string PartNo { get; set; }
        public string ModuleNo { get; set; }
        public string Cfc { get; set; }
        public string RenbanNo { get; set; }
        public string Seller { get; set; }
        public string Buyer { get; set; }
        public DateTime? PackingMonth { get; set; }
        public string ValuationTypeFrom { get; set; }
        public string PortOfLoading { get; set; }
        public string VesselEtd1st { get; set; }
        public string VesselName1st { get; set; }
        public string VesselNo1st { get; set; }
        public string PortOfDischarge { get; set; }
        public string EkanbanFlag { get; set; }
        public string RevisionNo { get; set; }
        public string ShipmentNo { get; set; }
    }

    public class InvCkdShipmentDetailsFirmDto : EntityDto<long?>
    {
        public string DateSs { get; set; }
        public string Segment { get; set; }
        public string Seller { get; set; }
        public string Buyer { get; set; }
        public string ShippingMonth { get; set; }
        public string EkanbanFlag { get; set; }
        public string PortOfLoading { get; set; }
        public string PackingDate { get; set; }
        public string PortOfDischarge { get; set; }
        public string VesselEtd1st { get; set; }
        public string VesselName1st { get; set; }
        public string VesselNo1st { get; set; }
        public string ExporterCode { get; set; }
        public string ImporterCode { get; set; }
        public string OrderControlNo { get; set; }
        public string RenbanNo { get; set; }
        public string ContainerSize { get; set; }
        public string CarFamilyCode { get; set; }
        public string PackingMonth { get; set; }
        public string ModelCode { get; set; }
        public string LotNo { get; set; }
        public string CaseNo { get; set; }
        public string ModuleNo { get; set; }
        public string FormDRenban { get; set; }
        public string M3OfModule { get; set; }
        public string GrossWeightOfModule { get; set; }
        public string ReExportCode { get; set; }
        public string SsNo { get; set; }
        public string LineCode { get; set; }
        public string PartNo { get; set; }
        public string PartName { get; set; }
        public string LotCode { get; set; }
        public string ExteriorColorCode { get; set; }
        public string InteriorColorCode { get; set; }
        public string OrderLot { get; set; }
        public string ScheduleQty { get; set; }
        public string ValuationTypeFrom { get; set; }
        public string RevisionNo { get; set; }
        public DateTime? ETD { get; set; }
        public DateTime? ETA { get; set; }
        public DateTime? ETADelay { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
        public string ManualUpdateStatus { get; set; }

    }

    public class CreateOrEditInvCkdShippingScheduleDetailsFirmDto : EntityDto<long?>
    {
        public virtual long? ShipmentHeaderId { get;set;}
        [StringLength(20)]
        public virtual string Status { get; set; }

        [StringLength(1)]
        public virtual string Segment { get; set; }

        [StringLength(3)]
        public virtual string Seller { get; set; }

        [StringLength(3)]
        public virtual string Buyer { get; set; }

        [StringLength(6)]
        public virtual string ShippingMonth { get; set; }

        [StringLength(1)]
        public virtual string EkanbanFlag { get; set; }

        [StringLength(3)]
        public virtual string PortOfLoading { get; set; }

        [StringLength(50)]
        public virtual string PortOfDischarge { get; set; }

        [StringLength(8)]
        public virtual string VesselEtd1st { get; set; }

        [StringLength(30)]
        public virtual string VesselName1st { get; set; }

        [StringLength(10)]
        public virtual string VesselNo1st { get; set; }

        public virtual int? RevisionNo { get; set; }

        [StringLength(4)]
        public virtual string ExporterCode { get; set; }

        [StringLength(4)]
        public virtual string ImporterCode { get; set; }

        [StringLength(21)]
        public virtual string OrderControlNo { get; set; }

        [StringLength(6)]
        public virtual string RenbanNo { get; set; }

        public virtual int? ContainerSize { get; set; }

        [StringLength(6)]
        public virtual string CarFamilyCode { get; set; }

        [StringLength(6)]
        public virtual string PackingMonth { get; set; }

        [StringLength(20)]
        public virtual string ModelCode { get; set; }

        [StringLength(6)]
        public virtual string LotNo { get; set; }

        [StringLength(4)]
        public virtual string CaseNo { get; set; }

        [StringLength(6)]
        public virtual string ModuleNo { get; set; }

        [StringLength(7)]
        public virtual string FormDRenban { get; set; }

        public virtual int? M3OfModule { get; set; }

        public virtual int? GrossWeightOfModule { get; set; }

        [StringLength(1)]
        public virtual string ReExportCode { get; set; }

        [StringLength(2)]
        public virtual string SsNo { get; set; }

        [StringLength(1)]
        public virtual string LineCode { get; set; }

        [StringLength(12)]
        public virtual string PartNo { get; set; }

        [StringLength(2)]
        public virtual string LotCode { get; set; }

        [StringLength(4)]
        public virtual string ExteriorColorCode { get; set; }

        [StringLength(4)]
        public virtual string InteriorColorCode { get; set; }

        public virtual int? OrderLot { get; set; }

        public virtual int? ScheduleQty { get; set; }

        [StringLength(3)]
        public virtual string ValuationTypeFrom { get; set; }

        [StringLength(8)]
        public virtual string PackingDate { get; set; }


    }

    public class InvCkdShipmentDetailsFirmInput : PagedAndSortedResultRequestDto
    {
        //public string PartNo { get; set; }
        //public string ModuleNo { get; set; }
        //public string Cfc { get; set; }
        //public string RenbanNo { get; set; }
        //public string Seller { get; set; }
        //public string Buyer { get; set; }
        //public DateTime? ShippingMonthFrom { get; set; }
        //public DateTime? ShippingMonthTo { get; set; }
        //public string ValuationTypeFrom { get; set; }
        //public string PortOfLoading { get; set; }
        //public string VesselEtd1st { get; set; }
        //public string VesselName1st { get; set; }
        //public string VesselNo1st { get; set; }
        //public string PortOfDischarge { get; set; }
        //public string EkanbanFlag { get; set; }
        //public string RevisionNo { get; set; }
        //public string ShipmentNo { get; set; }

        public long?[] ShipmentHeaderId { get; set; }
        public string PartNo { get; set; }
        public string Cfc { get; set; }
        public string Status { get; set; }

    }

    public class GetInvCkdShippingFirmExport
    {
        public virtual string Exp { get; set; }
        public virtual string Etd { get; set; }
        public virtual string Etd2 { get; set; }
        public virtual string Etd3 { get; set; }
        public virtual string Etd4 { get; set; }

        public virtual string Eta { get; set; }
        public virtual string Eta2 { get; set; }
        public virtual string Eta3 { get; set; }
        public virtual string Eta4 { get; set; }
        public virtual string ShipmentNo { get; set; }

        public virtual string ModelCode { get; set; }
        public virtual string IdLine { get; set; }

        public virtual string PartNo { get; set; }
        public virtual string PartName { get; set; }
        public virtual int Qty { get; set; }
        public virtual string PackingMonth { get; set; }

    }

    public class GetInvCkdShippingFirmExportInput
    {
        public virtual DateTime? PackingMonth { get; set; }
    }

    public class GetInvCkdShippingFirmExportNew
    {
        public virtual string Exp { get; set; }
        public virtual DateTime? Etd { get; set; }
        public virtual DateTime? Eta { get; set; }
        public virtual string ShipmentNo { get; set; }
        public virtual string ModelCode { get; set; }

        public virtual string IDLine { get; set; }
        public virtual string PartNo { get; set; }
        public virtual string PartName { get; set; }
        public virtual int Qty { get; set; }
        public virtual string PackingMonth { get; set; }
        public virtual DateTime? ETADelay { get; set; }
        public virtual string Version { get; set; }
        public virtual string Remark { get; set; }
        public virtual string ShippingMonth { get; set; }

    }

    public class GetShipmentFirmDetailsEditDtoInput
    {
        public virtual long? Id { get; set; }
        public virtual DateTime? ETA { get; set; }
        public virtual DateTime? ETD { get; set; }
        public virtual DateTime? ETADelay { get; set; }
        public virtual int? Quantity { get; set; }
        public virtual string Remark { get; set; }

    }

    //public class GetInvCkdShippingCancelInput
    //{
    //    public virtual string ShipmentNo { get; set; }
    //    public virtual string ShippingMonth { get; set; }
    //    public virtual string VesselETD1st { get; set; }
    //    public virtual long? Index { get; set; }
    //}
    //public class InvShippingScheduleSheet1ImportDto
    //{
    //    public virtual string Dest { get; set; }
    //    public virtual string Importer { get; set; }
    //    public virtual string Model { get; set; }
    //    public virtual string PartNo { get; set; }
    //    public virtual string Mod { get; set; }
    //    public virtual int? OrderLot { get; set; }
    //    public virtual int? TotalBox { get; set; }
    //    public virtual DateTime? Date { get; set; }
    //    public virtual int? BoxQty { get; set; }
    //    public virtual string Guid { get; set; }
    //    public virtual string ValuationTypeFrom { get; set; }
    //}


    //public class InvShippingScheduleSheet2ImportDto
    //{
    //    public virtual string Haisen { get; set; }
    //    public virtual string ETD { get; set; }
    //    public virtual string ETA { get; set; }
    //    public virtual string Vessel { get; set; }
    //    public virtual string Carrier { get; set; }
    //    public virtual string VNo { get; set; }
    //    public virtual string CSign { get; set; }
    //    public virtual string BNo { get; set; }
    //    public virtual string Kakutei { get; set; }
    //    public virtual string FromDate { get; set; }
    //    public virtual string ToDate { get; set; }
    //    public virtual string PackingMonth { get; set; }
    //    public virtual int? IsCancelled { get; set; }
    //    public virtual string Guid { get; set; }
    //    public virtual string ValuationTypeFrom { get; set; }
    //    public virtual int? Index { get; set; }
    //    public virtual string ToPort { get; set; }
    //}

    //public class InvShippingScheduleImportDto
    //{

    //    public virtual string EXP { get; set; }

    //    public virtual DateTime? ETD { get; set; }

    //    public virtual string ETA { get; set; }

    //    public virtual string ShipmentNo { get; set; }

    //    public virtual string ModelCode { get; set; }

    //    public virtual string IDLine { get; set; }

    //    public virtual string PartNo { get; set; }

    //    public virtual string PartName { get; set; }

    //    public virtual int? Qty { get; set; }

    //    public virtual DateTime? PackingMonth { get; set; }
    //}
}
