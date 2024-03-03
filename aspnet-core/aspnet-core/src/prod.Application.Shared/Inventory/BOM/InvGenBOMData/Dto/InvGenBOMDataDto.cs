using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Inventory.BOM.Dto
{
    public class InvGenBOMDataDto : EntityDto<long?>
    {
        public string DiscriminationSign { get; set; }
        public string OrderCycle { get; set; }
        public string PSC { get; set; }
        public string EDNO { get; set; }
        public string URN { get; set; }
        public string ProductionSFX { get; set; }
        public string ExportType { get; set; }
        public string IDLineCode { get; set; }
        public string ProductionYear { get; set; }
        public string ProductionMonth { get; set; }
        public string ProductionWeek { get; set; }
        public string ProductionDay { get; set; }
        public string FrameTypeCode { get; set; }
        public string VIN { get; set; }
        public string WMI { get; set; }
        public string VDS { get; set; }
        public string ModelYearCode { get; set; }
        public string VINType { get; set; }
        public string StampModelCode { get; set; }
        public string FrameSequenceNumberDigits { get; set; }
        public string SpecSheetNumber { get; set; }
        public string KATASHIKICode { get; set; }
        public string DisplayKATASHIKI { get; set; }
        public string CTLKATASHIKI { get; set; }
        public string LineOffKATASHIKICode { get; set; }
        public string LineOffKATASHIKI { get; set; }
        public string ExteriorCode { get; set; }
        public string InteriorCode { get; set; }
        public string ProductionSpec { get; set; }
        public string CarFamilyCode { get; set; }
        public string DestinationCountryCode { get; set; }
        public string DestinationCountryName { get; set; }
        public string KDLotCode { get; set; }
        public string KDLotNo { get; set; }
        public string KDSubType { get; set; }
        public string PAMSLotSFX { get; set; }
        public string EngineBasicKATASHIKI { get; set; }
        public string MotorBasicKATASHIKI { get; set; }
        public string ProductionLotSize { get; set; }
        public string MakerCode { get; set; }
        public string PackingYearMonth { get; set; }
        public string VehicleNameCode { get; set; }
        public string PackingStyle { get; set; }
        public string DestinationDestinction { get; set; }
        public string DestinationDetails { get; set; }
        public string GovernmentApproval { get; set; }
        public string VAR1 { get; set; }
        public string SpecType { get; set; }
        public string PlantCode { get; set; }
        public string SpecialSign { get; set; }
        public string UnitType { get; set; }
        public string SecondAssemblyLine { get; set; }
        public string SecondAssemblyVehicleType { get; set; }
        public string OffOptionType { get; set; }
        public string ProductionRequestYearMonth { get; set; }
        public string DistributerCode { get; set; }
        public string SalesSFX { get; set; }
        public string ImportDutyExemption { get; set; }
        public string OrderNo { get; set; }
        public string TypeApprovalNumber { get; set; }
        public string Dummy { get; set; }
        public string TypeMPP { get; set; }
        public DateTime? PeriodMpp { get; set; }
    }

    public class GetInvGenBOMDataInput : PagedAndSortedResultRequestDto
    {
        public string TypeMPP { get; set; }
        public DateTime? PeriodMpp { get; set; }
    }

    public class GetInvGenBOMDataExportInput
    {
        public string TypeMPP { get; set; }
        public DateTime? PeriodMpp { get; set; }
    }
}
