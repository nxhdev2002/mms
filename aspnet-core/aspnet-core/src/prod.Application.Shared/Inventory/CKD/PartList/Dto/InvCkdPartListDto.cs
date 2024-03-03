using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Castle.MicroKernel.SubSystems.Conversion;
using prod.MultiTenancy.Payments.Stripe.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Inventory.CKD.Dto
{

    public class InvCkdPartListDto : EntityDto<long?>
    {

        public virtual string PartNo { get; set; }

        public virtual long? PartId { get; set; }

        public virtual string PartNoNormalized { get; set; }

        public virtual string PartNoNormalizedS4 { get; set; }

        public virtual string PartName { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string SupplierCd { get; set; }

        public virtual string ColorSfx { get; set; }

        public virtual long? SupplierId { get; set; }

        public virtual long? MaterialId { get; set; }

        public virtual string OrderPattern { get; set; }

        public virtual string IsActive { get; set; }

        public virtual string Grade { get; set; }

        public virtual string Qty { get; set; }

        public virtual string Shop { get; set; }

        public virtual string BoxSize { get; set; }

        public virtual string BodyColor { get; set; }


    }
    public class InvCkdPartGradeDto : EntityDto<long?>
    {
        public virtual string Guid { get; set; }

        public virtual string PartNo { get; set; }

        public virtual long? PartId { get; set; }
        
        public virtual string PartName { get; set; }

        public virtual string Model { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string Grade { get; set; }

        public virtual string IdLine { get; set; }

        public virtual string Shop { get; set; }

        public virtual string BodyColor { get; set; }

        public virtual decimal? UsageQty { get; set; }

        public virtual string StartLot { get; set; }

        public virtual string EndLot { get; set; }

        public virtual string StartRun { get; set; }

        public virtual string EndRun { get; set; }

        public virtual DateTime? EfFromPart { get; set; }

        public virtual DateTime? EfToPart { get; set; }

        public virtual string OrderPattern { get; set; }

        public virtual string Remark { get; set; }

        public virtual string IsActive { get; set; }

        public virtual string isECI { get; set; }

        public virtual string UserName { get; set; }
        public virtual DateTime? UserUpdate { get; set; }
        public virtual string Boxsize { get; set; }
        public virtual string ECIFromPart { get; set; }
        public virtual string ECIToPart { get; set; }
        public string ErrorDescription { get; set; }
        public bool CanBeImported()
        {
            return string.IsNullOrEmpty(ErrorDescription);
        }
        public string SupplierNo { get; set; }
    }

    public class ExportCkdPartListGradeDto
    {
        public virtual string PartNo { get; set; }

        public virtual string Grade { get; set; }

        public virtual string Cfc { get; set; }

        public virtual decimal? UsageQty { get; set; }
    }
    public class InvPartPackingDetailDto : EntityDto<long?>
    {
        public virtual long? Id { get; set; }
        public virtual string PartNo { get; set; }
        public virtual string Cfc { get; set; }
        public virtual string Grade { get; set; }

        public virtual long? PartId { get; set; }

        public virtual string BackNo { get; set; }

        public virtual string ModuleNo { get; set; }

        public virtual string Renban { get; set; }

        public virtual int? BoxSize { get; set; }

        public virtual string Type { get; set; }

        public virtual string Common { get; set; }

        public virtual string IcoFlag { get; set; }

        public virtual string ReExportCd { get; set; }

        public virtual DateTime? StartPackingMonth { get; set; }

        public virtual DateTime? EndPackingMonth { get; set; }

        public virtual DateTime? StartProductionMonth { get; set; }

        public virtual DateTime? EndProductionMonth { get; set; }

        public virtual string IsActive { get; set; }
    }
    public class GetInvCkdPartListHistoryInput : PagedAndSortedResultRequestDto
    {
        public virtual long Id { get; set; }

        public virtual string TableName { get; set; }
    }

    public class GetInvCkdPartListHistoryFilterInput : PagedAndSortedResultRequestDto
    {
        public virtual int?[] CDCAction { get; set; }
        public virtual string PartNo { get; set; }
        public virtual DateTime? FromDate { get; set; }
        public virtual DateTime? ToDate { get; set; }
    }

    public class GetInvCkdPartListHistoryFilterExcelExporterInput
    {
        public virtual int?[] CDCAction { get; set; }
        public virtual string PartNo { get; set; }
        public virtual DateTime? FromDate { get; set; }
        public virtual DateTime? ToDate { get; set; }
    }


    public class GetInvCkdPartListHistoryExcelInput
    {
        public virtual long Id { get; set; }

        public virtual string TableName { get; set; }
    }

    public class GetInvCkdPartListInput : PagedAndSortedResultRequestDto
    {
        public virtual string PartNo { get; set; }
        public virtual string Cfc { get; set; }
        public virtual string Model { get; set; }
        public virtual string Grade { get; set; }
        public virtual string Shop { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string OrderPattern { get; set; }
        public virtual string IsActive { get; set; }

    }
    public class GetInvCkdPartListDetailInput : PagedAndSortedResultRequestDto
    {
        public virtual long? PartId { get; set; }
        public virtual string OrderPatern { get; set; }


    }


    public class GetInvCkdPartListGradeInput : PagedAndSortedResultRequestDto
    {
        public virtual long? PartId { get; set; }


    }

    public class ExportCkdPartListDto
    {

        public virtual string Common { get; set; }
        public virtual string Type { get; set; }
        public virtual string Cfc { get; set; }
        public virtual string Shop { get; set; }
        public virtual string IdLine { get; set; }

        public virtual string Grade { get; set; }

        public virtual decimal? UsageQty { get; set; }
        public virtual string PartNo { get; set; }
        public virtual string PartName { get; set; }
        public virtual string SupplierNo { get; set; }

        public virtual string SupplierCd { get; set; }

        public virtual string BodyColor { get; set; }
        public virtual string StartLot { get; set; }
        public virtual string EndLot { get; set; }
        public virtual string StartRun { get; set; }
        public virtual string EndRun { get; set; }
        public virtual string BackNo { get; set; }
        public virtual string ModuleNo { get; set; }

        public virtual string Renban { get; set; }

        public virtual int? BoxSize { get; set; }

        public virtual string ReExportCd { get; set; }

        public virtual string IcoFlag { get; set; }

        public virtual DateTime? StartPackingMonth { get; set; }

        public virtual DateTime? EndPackingMonth { get; set; }

        public virtual DateTime? StartProductionMonth { get; set; }

        public virtual DateTime? EndProductionMonth { get; set; }

        public virtual string Blank { get; set; }
        public virtual string Remark { get; set; }
        public virtual string Value { get; set; }

    }
        
    public class ImportCkdPartListDto
    {
        public virtual long? ROW_NO { get; set; }

        [StringLength(128)]
        public virtual string Guid { get; set; }

        [StringLength(50)]
        public virtual string PartNo { get; set; }

        [StringLength(20)]
        public virtual string PartNoNormalized { get; set; }

        [StringLength(500)]
        public virtual string PartName { get; set; }

        [StringLength(50)]
        public virtual string SupplierNo { get; set; }

        [StringLength(50)]
        public virtual string SupplierCd { get; set; }

        [StringLength(10)]
        public virtual string ColorSfx { get; set; }

        public virtual long? SupplierId { get; set; }

        public virtual long? MaterialId { get; set; }


        public virtual long? PartId { get; set; }

        [StringLength(1)]
        public virtual string Model { get; set; }

        [StringLength(4)]
        public virtual string Cfc { get; set; }

        [StringLength(3)]
        public virtual string Grade { get; set; }

        [StringLength(10)]
        public virtual string IdLine { get; set; }

        [StringLength(1)]
        public virtual string Shop { get; set; }

        [StringLength(100)]
        public virtual string BodyColor { get; set; }

        public virtual decimal? UsageQty { get; set; }

        [StringLength(10)]
        public virtual string StartLot { get; set; }

        [StringLength(10)]
        public virtual string EndLot { get; set; }

        [StringLength(10)]
        public virtual string StartRun { get; set; }

        [StringLength(10)]
        public virtual string EndRun { get; set; }

        public virtual DateTime? EfFromPart { get; set; }

        public virtual DateTime? EfToPart { get; set; }

        [StringLength(50)]
        public virtual string OrderPattern { get; set; }

        [StringLength(5000)]
        public virtual string Remark { get; set; }

        [StringLength(10)]
        public virtual string BackNo { get; set; }

        [StringLength(10)]
        public virtual string ModuleNo { get; set; }

        [StringLength(10)]
        public virtual string Renban { get; set; }

        public virtual int? BoxSize { get; set; }

        [StringLength(50)]
        public virtual string Type { get; set; }

        [StringLength(50)]
        public virtual string Common { get; set; }

        [StringLength(50)]
        public virtual string IcoFlag { get; set; }

        [StringLength(10)]
        public virtual string ReExportCd { get; set; }

        public virtual DateTime? StartPackingMonth { get; set; }

        public virtual DateTime? EndPackingMonth { get; set; }

        public virtual DateTime? StartProductionMonth { get; set; }

        public virtual DateTime? EndProductionMonth { get; set; }

        [StringLength(1)]

        public virtual string IsActive { get; set; }

        public string ErrorDescription { get; set; }
        public bool CanBeImported()
        {
            return string.IsNullOrEmpty(ErrorDescription);
        }
    }

    public class GetCfcDto
    {
        public string Cfc { get; set; }

        public string Model { get; set; }
    }

    public class GetPartNoDto
    {
        public string PartNo { get; set; }
    }

    public class GetGradeDto
    {
        public string Cfc { get; set; }

        public string Grade { get; set; }

        public string PartNo { get; set; }

        public string Model { get; set; }
    }

    public class GetColorDto
    {
        public string Cfc { get; set; }

        public string Grade { get; set; }

        public string Color { get; set; }

    }

    public class GetSupplierDto
    {
        public string SupplierNo { get; set; }
    }

    public class GetPartListGradeDto : EntityDto<long?>
    {
        [StringLength(50)]
        public virtual string PartNo { get; set; }

        [StringLength(500)]
        public virtual string PartName { get; set; }

        [StringLength(50)]
        public virtual string SupplierNo { get; set; }

        [StringLength(4)]
        public virtual string Cfc { get; set; }

        [StringLength(50)]
        public virtual string OrderPattern { get; set; }

        public List<GetGradebyPartListDto> listGrade { get; set; }

    }

    public class GetGradebyPartListDto : EntityDto<long?>
    {

        public virtual long PartId { get; set; }

        [StringLength(50)]
        public virtual string PartNo { get; set; }

        [StringLength(1)]
        public virtual string Model { get; set; }

        [StringLength(3)]
        public virtual string Grade { get; set; }

        [StringLength(4)]
        public virtual string Cfc { get; set; }

        [StringLength(1)]
        public virtual string Shop { get; set; }

        [StringLength(100)]
        public virtual string BodyColor { get; set; }

        public virtual decimal? UsageQty { get; set; }

        [StringLength(10)]
        public virtual string StartLot { get; set; }

        [StringLength(10)]
        public virtual string EndLot { get; set; }

        [StringLength(10)]
        public virtual string StartRun { get; set; }

        [StringLength(10)]
        public virtual string EndRun { get; set; }

        public virtual DateTime? EfFromPart { get; set; }

        public virtual DateTime? EfToPart { get; set; }

        [StringLength(50)]
        public virtual string OrderPattern { get; set; }

        [StringLength(5000)]
        public virtual string Remark { get; set; }

        public virtual string Selected { get; set; }

    }

    public class GetPartListId
    {
        public virtual long? PartListId { get; set; }
    }
     
    public class ValidatePartListDto : PagedAndSortedResultRequestDto
    {
        public virtual long? PartListId { get; set; }
        public virtual long? PartGradeId { get; set; }
        public virtual string ErrorDescription { get; set; }
        public virtual string PartNo { get; set; }
        public virtual string PartNoNormalizedS4 { get; set; }
        public virtual string PartName { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string Model { get; set; }
        public virtual string Cfc { get; set; }
        public virtual long? MaterialId { get; set; }
        public virtual string OrderPattern { get; set; }
        public virtual string Grade { get; set; }
        public virtual string Shop { get; set; }
        public virtual string BodyColor { get; set; }
        public virtual decimal? UsageQty { get; set; }
        public virtual string StartLot { get; set; }
        public virtual string EndLot { get; set; }
        public virtual string StartRun { get; set; }
        public virtual string EndRun { get; set; }
        public virtual DateTime? CreationTime { get; set; }
        public virtual long? CreatorUserId { get; set; }

    }

    public class CheckExistDto
    {
        public virtual string ResultCheck { get; set; }
    }

    public class ValidatePartGradeBodyColorInput
    {
        public virtual long Id { get; set; }
        public virtual string Cfc { get; set; }
        public virtual string Shop { get; set; }
        public virtual string Model { get; set; }
        public virtual string Grade { get; set; }
        public virtual string BodyColor { get; set; }       

    }

    public class ChangedRecordIdsDto
    {
        public virtual List<long?> PartList { get; set; }
        public virtual List<long?> PartGrade { get; set; }
        public virtual List<long?> Bill { get; set; }
        public virtual List<long?> PartPacking { get; set; }

    }



    public class InvCkdPartDetailsDto : EntityDto<long?>
    {
        public virtual string PartStatus { get; set; }
        public virtual string Cfc { get; set; }
        public virtual string Shop { get; set; }
        public virtual string IdLine { get; set; }
        public virtual string PartNo { get; set; }
        public virtual string PartName { get; set; }
        public virtual string Exporter { get; set; }
        public virtual string ExporterCode { get; set; }
        public virtual string BodyColor { get; set; }

        public virtual string Grade { get; set; }
        public virtual decimal? UsageQty { get; set; }
        public virtual string StartLot { get; set; }
        public virtual string StartNoInLot { get; set; }
        public virtual string EndLot { get; set; }
        public virtual string EndNoInLot { get; set; }
        public virtual string ECIFromPart { get; set; }
        public virtual string ECIToPart { get; set; }
        public virtual string OrderPattern { get; set; }
        public virtual string Remark { get; set; }
        public virtual DateTime? UserUpdate { get; set; }
        public virtual string UserName { get; set; }


    }

    public class InvCkdPartNormalDto : EntityDto<long?>
    {
        public virtual string PartStatus { get; set; }
        public virtual string Model { get; set; }
        public virtual string Shop { get; set; }
        public virtual string IdLine { get; set; }
        public virtual string PartNo { get; set; }
        public virtual string PartName { get; set; }
        public virtual string Exporter { get; set; }
        public virtual string ExporterCode { get; set; }
        public virtual string BodyColor { get; set; }
        public virtual string BackNo { get; set; }
        public virtual string ModuleNo { get; set; }
        public virtual string Renban { get; set; }
        public virtual int? BoxSize { get; set; }
        public virtual string ReExportCd { get; set; }
        public virtual string IcoFlag { get; set; }
        public virtual DateTime? StartPackingMonth { get; set; }
        public virtual DateTime? EndPackingMonth { get; set; }
        public virtual string UserUpdate { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Value { get; set; }
        public virtual string Remark { get; set; }

    }



    public class ImportCkdPartListNormalDto
    {
        public virtual string MaintenanceCode { get; set; }
        public virtual long? ROW_NO { get; set; }

        [StringLength(128)]
        public virtual string Guid { get; set; }

        [StringLength(50)]
        public virtual string PartNo { get; set; }

        [StringLength(500)]
        public virtual string PartName { get; set; }

        [StringLength(50)]
        public virtual string SupplierNo { get; set; }

        [StringLength(50)]
        public virtual string SupplierCd { get; set; }

        [StringLength(10)]
        public virtual string ColorSfx { get; set; }

        public virtual long? SupplierId { get; set; }

        public virtual long? MaterialId { get; set; }


        public virtual long? PartId { get; set; }

        [StringLength(1)]
        public virtual string Model { get; set; }

        [StringLength(4)]
        public virtual string Cfc { get; set; }

        [StringLength(3)]
        public virtual string Grade { get; set; }

        [StringLength(10)]
        public virtual string IdLine { get; set; }

        [StringLength(1)]
        public virtual string Shop { get; set; }

        [StringLength(100)]
        public virtual string BodyColor { get; set; }

        public virtual decimal? UsageQty { get; set; }

        [StringLength(10)]
        public virtual string StartLot { get; set; }

        [StringLength(10)]
        public virtual string EndLot { get; set; }

        [StringLength(10)]
        public virtual string StartRun { get; set; }

        [StringLength(10)]
        public virtual string EndRun { get; set; }

        public virtual DateTime? EfFromPart { get; set; }

        public virtual DateTime? EfToPart { get; set; }

        [StringLength(50)]
        public virtual string OrderPattern { get; set; }

        [StringLength(5000)]
        public virtual string Remark { get; set; }

        [StringLength(10)]
        public virtual string BackNo { get; set; }

        [StringLength(10)]
        public virtual string ModuleNo { get; set; }

        [StringLength(10)]
        public virtual string Renban { get; set; }

        public virtual int? BoxSize { get; set; }

        [StringLength(50)]
        public virtual string Type { get; set; }

        [StringLength(50)]
        public virtual string Common { get; set; }

        [StringLength(50)]
        public virtual string IcoFlag { get; set; }

        [StringLength(10)]
        public virtual string ReExportCd { get; set; }

        public virtual DateTime? StartPackingMonth { get; set; }

        public virtual DateTime? EndPackingMonth { get; set; }

        public virtual DateTime? StartProductionMonth { get; set; }

        public virtual DateTime? EndProductionMonth { get; set; }

        [StringLength(1)]

        public virtual string IsActive { get; set; }

        public string ErrorDescription { get; set; }
        public bool CanBeImported()
        {
            return string.IsNullOrEmpty(ErrorDescription);
        }


    }

    public class ImportInvCkdPartGradeDto : EntityDto<long?>
    {
        public virtual long? ROW_NO { get; set; }
        public virtual string Guid { get; set; }
        public virtual string PartNo { get; set; }
        public virtual string PartName { get; set; }
        public virtual string Model { get; set; }
        public virtual string Cfc { get; set; }
        public virtual string Grade { get; set; }
        public virtual string IdLine { get; set; }
        public virtual string Shop { get; set; }
        public virtual string BodyColor { get; set; }
        public virtual decimal? UsageQty { get; set; }
        public virtual string StartLot { get; set; }
        public virtual string StartNoInLot { get; set; }
        public virtual string EndLot { get; set; }
        public virtual string EndNoInLot { get; set; }
        public virtual string MaintenanceCode { get; set; }
        public virtual string PartStatus { get; set; }
        public virtual string Exporter { get; set; }
        public virtual string ExporterCode { get; set; }
        public virtual DateTime? UpdateDate { get; set; }
        public virtual string UpdateUser { get; set; }
        public string ErrorDescription { get; set; }
        public bool CanBeImported()
        {
            return string.IsNullOrEmpty(ErrorDescription);
        }

        public virtual string OrderPattern { get; set; }
        public virtual string ECIFromPart { get; set; }
        public virtual string ECIToPart { get; set; }
        public virtual string Remark { get; set; }
        public virtual long? CreatorUserId { get; set; }
    }


    public class InvCkdPartPackingDetailsDto : EntityDto<long?>
    {
        public virtual string PartStatus { get; set; }
        public virtual string Model { get; set; }
        public virtual string Shop { get; set; }
        public virtual string IdLine { get; set; }
        public virtual string PartNo { get; set; }
        public virtual string PartName { get; set; }
        public virtual string Cfc { get; set; }
        public virtual string Grade { get; set; }
        public virtual string Exporter { get; set; }
        public virtual string ExporterCode { get; set; }
        public virtual string BodyColor { get; set; }
        public virtual string BackNo { get; set; }
        public virtual string ModuleNo { get; set; }
        public virtual string Renban { get; set; }
        public virtual int? Boxsize { get; set; }
        public virtual string ReExportCd { get; set; }
        public virtual string IcoFlag { get; set; }
        public virtual DateTime? StartPackingMonth { get; set; }
        public virtual DateTime? EndProductionMonth { get; set; }

    }
}


