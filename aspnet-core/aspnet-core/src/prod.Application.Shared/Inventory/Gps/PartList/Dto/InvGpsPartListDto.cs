using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Inventory.Gps.PartList.Dto
{
    public class InvGpsPartListDto : EntityDto<long?>
    {
        public virtual string PartNo { get; set; }

        public virtual string PartNoNormalized { get; set; }

        public virtual string PartName { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string Uom { get; set; }

        public virtual string Type { get; set; }

        public virtual int? BoxQty { get; set; }

        public virtual string Color { get; set; }

        public virtual decimal? SummerRadio { get; set; }

        public virtual decimal? WinterRatio { get; set; }

        public virtual decimal? DiffRatio { get; set; }

        public virtual string SeasonType { get; set; }

        public virtual string Remark { get; set; }
        public virtual string Remark1 { get; set; }

        public virtual int? MinLot { get; set; }

        public virtual string Category { get; set; }

        public virtual DateTime? StartDate { get; set; }

        public virtual DateTime? EndDate { get; set; }

        public virtual string IsPartColor { get; set; }
    }

    public class GetInvGpsPartListInput : PagedAndSortedResultRequestDto
    {
        public virtual string PartNo { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string IsPartColor { get; set; }

    }
    public class InvGpsPartGradeByPartListDto : EntityDto<long?>
    {

        public virtual int? PartListId { get; set; }

        public virtual string Grade { get; set; }

        public virtual string BodyColor { get; set; }

        public virtual decimal? UsageQty { get; set; }

        public virtual string ProcessUse { get; set; }

        public virtual string Shop { get; set; }

        public virtual int? VehicleId { get; set; }

    }

    public class GetInvGpsPartListInputExport
    {
        public virtual string PartNo { get; set; }

        public virtual string Grade { get; set; }

    }

    public class GetInvGpsPartGradeByPartListInput : PagedAndSortedResultRequestDto
    {

        public virtual int? PartListId { get; set; }

        public virtual string Grade { get; set; }

    }

    public class InvGpsPartListImportDto
    {
        public virtual long? ROW_NO { get; set; }

        public virtual string IsPartNo { get; set; }

        public virtual string Guid { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string PartNoNormalized { get; set; }

        public virtual string PartName { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string Uom { get; set; }

        public virtual string Type { get; set; }

        public virtual int? BoxQty { get; set; }

        public virtual string Color { get; set; }

        public virtual decimal? SummerRadio { get; set; }

        public virtual decimal? WinterRatio { get; set; }

        public virtual decimal? DiffRatio { get; set; }

        public virtual string SeasonType { get; set; }

        public virtual string Remark { get; set; }

        public virtual string Remark1 { get; set; }

        //

        public virtual int? PartListId { get; set; }

        public virtual string Grade { get; set; }

        public virtual string BodyColor { get; set; }

        public virtual decimal? UsageQty { get; set; }

        public virtual string ProcessUse { get; set; }

        public virtual string Shop { get; set; }

        public virtual int? VehicleId { get; set; }

        //

        public virtual string ErrorDescription { get; set; }

        public virtual int? MinLot { get; set; }

        public virtual string Category { get; set; }

        public virtual DateTime? StartDate { get; set; }

        public virtual DateTime? EndDate { get; set; }

        public virtual string IsPartColor { get; set; }

        public virtual DateTime? CreationTime { get; set; }

        public virtual int CreatorUserId { get; set; }

        public virtual int IsDeleted { get; set; }
    }

    public class InvGpsPartListTitleExportDto
    {
        public virtual string Model { get; set; }
        public virtual string Grade { get; set; }
        public virtual string BodyColor { get; set; }
    }

    public class InvGpsPartListContentExportDto
    {
        public virtual string PartNo { get; set; }
        public virtual string PartName { get; set; }
        public virtual string Grade { get; set; }
        public virtual string BodyColor { get; set; }
        public virtual double UsageQty { get; set; }
        public virtual string Uom { get; set; }
        public virtual int BoxQty { get; set; }
        public virtual string Remark1 { get; set; }
        public virtual string ProcessUse { get; set; }
        public virtual string Type { get; set; }
        public virtual string SeasonType { get; set; }
        public virtual double WinterRatio { get; set; }
        public virtual double SummerRadio { get; set; }
        public virtual double DiffRatio { get; set; }
        public virtual string Remark { get; set; }

    }


    public class ValidateGpsPartListDto : PagedAndSortedResultRequestDto
    {
        public virtual long PartListId { get; set; }
        public virtual string ErrorDescription { get; set; }
        public virtual string PartNo { get; set; }
        public virtual string PartNoNormalized { get; set; }
        public virtual string PartName { get; set; }
        public virtual string Grade { get; set; }
        public virtual string Shop { get; set; }
        public virtual string BodyColor { get; set; }
        //public virtual decimal? UsageQty { get; set; }
        public virtual DateTime? CreationTime { get; set; }
        public virtual long? CreatorUserId { get; set; }

    }

}
