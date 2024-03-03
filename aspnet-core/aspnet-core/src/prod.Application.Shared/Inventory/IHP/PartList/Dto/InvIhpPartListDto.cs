using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace prod.Inventory.IHP.Dto
{
    public class InvIhpPartListDto : EntityDto<long?>
    {
        public virtual string SupplierType { get; set; }

        public virtual string SupplierCd { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual string MaterialCode { get; set; }

        public virtual string MaterialSpec { get; set; }

        public virtual string Sourcing { get; set; }

        public virtual string Cutting { get; set; }

        public virtual int? Packing { get; set; }

        public virtual decimal? SheetWeight { get; set; }

        public virtual decimal? YiledRation { get; set; }

        public virtual long? DrmPartListId { get; set; }
    }

    public class GetInvIhpPartListInput : PagedAndSortedResultRequestDto
    {

        public virtual string SupplierCd { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string Grade { get; set; }

        public virtual string PartName { get; set; }

        public virtual string MaterialCode { get; set; }

        public virtual string MaterialSpec { get; set; }

    }

    public class GetInvIhpPartListExportInput
    {
        public virtual string SupplierCd { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string Grade { get; set; }

        public virtual string PartName { get; set; }

        public virtual string MaterialCode { get; set; }

        public virtual string MaterialSpec { get; set; }


    }


    public class GetEditIhpPartListDto : EntityDto<long?>
    {
        [StringLength(20)]
        public virtual string SupplierType { get; set; }

        [StringLength(20)]
        public virtual string SupplierCd { get; set; }

        [StringLength(4)]
        public virtual string Cfc { get; set; }

        [StringLength(20)]
        public virtual string PartNo { get; set; }

        [StringLength(200)]
        public virtual string PartName { get; set; }

        [StringLength(40)]
        public virtual string MaterialCode { get; set; }

        [StringLength(40)]
        public virtual string MaterialSpec { get; set; }

        [StringLength(40)]
        public virtual string Sourcing { get; set; }

        [StringLength(40)]
        public virtual string Cutting { get; set; }

        public virtual int? Packing { get; set; }

        public virtual decimal? SheetWeight { get; set; }

        public virtual decimal? YiledRation { get; set; }

        public virtual long? DrmPartListId { get; set; }

        public List<GetEditIhpPartGradeDto> listGrade { get; set; }

    }

    public class GetEditIhpPartGradeDto : EntityDto<long?>
    {
        [StringLength(3)]
        public virtual string Grade { get; set; }

        public virtual long? IhpPartId { get; set; }

        public virtual int? UsageQty { get; set; }

        public virtual DateTime? FirstDayProduct { get; set; }

        public virtual DateTime? LastDayProduct { get; set; }
    }

    public class GetListMaterialDto
    {
        public virtual long? Id { get; set; }
        public virtual string MaterialCode { get; set; }
        public virtual string MaterialSpec { get; set; }
        public virtual string SupplierType { get; set; }
        public virtual string SupplierCd { get; set; }
        public virtual string PartSpec { get; set; }
    }

    public class GetInvIhpPartListHistoryInput : PagedAndSortedResultRequestDto
    {
        public virtual long Id { get; set; }
        public virtual string TableName { get; set; }
    }

    public class GetInvIhpPartListHistoryExcelInput
    {
        public virtual long Id { get; set; }

        public virtual string TableName { get; set; }
    }
}
