using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Inventory.PIO.PartListOff.Dto
{
    public class InvPioPartListOffDto :  EntityDto<long?>
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
    }

    public class InvPioPartGradeOffDto : EntityDto<long?>
    {

        public virtual string PartNo { get; set; }

        public virtual long? PartId { get; set; }

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
        public virtual string PartName { get; set; }
        public virtual string IsECI { get; set; }

    }

    public class GetInvPioPartListOffInput : PagedAndSortedResultRequestDto
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

    public class GetInvPioPartListGradeOffInput : PagedAndSortedResultRequestDto
    {
        public virtual long? PartId { get; set; }


    }

    public class GetCfcPartListOffDto
    {
        public string Cfc { get; set; }

        public string Model { get; set; }
    }

    public class GetPartNoDto
    {
        public string PartNo { get; set; }
    }
    public class GetGradePartListOffDto
    {
        public string Cfc { get; set; }

        public string Grade { get; set; }

        public string PartNo { get; set; }

        public string Model { get; set; }
    }

    public class GetSupplierPartListOffDto
    {
        public string SupplierNo { get; set; }
    }

    public class GetPartListGradePartListOffDto : EntityDto<long?>
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

        public List<GetGradebyPartListOffDto> listGrade { get; set; }

    }
    public class GetGradebyPartListOffDto : EntityDto<long?>
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

    public class GetPartNoPartListOffDto
    {
        public string PartNo { get; set; }
    }
    public class GetPartListOffId
    {
        public virtual long? PartListId { get; set; }
    }

    public class CheckExistPartListOffDto
    {
        public virtual string ResultCheck { get; set; }
    }


    public class ExportPioPartListOffDto
    {
        public virtual int? Id { get; set; }
        public virtual string Cfc { get; set; }
        public virtual string Model { get; set; }
        public virtual string Shop { get; set; }
        public virtual string IdLine { get; set; }
        public virtual string PartNo { get; set; }
        public virtual string PartName { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string SupplierCd { get; set; }
        public virtual string BodyColor { get; set; }
        public virtual string StartLot { get; set; }
        public virtual string EndLot { get; set; }
        public virtual string StartRun { get; set; }
        public virtual string EndRun { get; set; }
        public virtual string Blank { get; set; }
        public virtual string Remark { get; set; }
        public virtual string Value { get; set; }

    }

    public class ImportPioPartListOffDto
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

        [StringLength(1)]

        public virtual string IsActive { get; set; }

        public string ErrorDescription { get; set; }
        public bool CanBeImported()
        {
            return string.IsNullOrEmpty(ErrorDescription);
        }
    }
}
