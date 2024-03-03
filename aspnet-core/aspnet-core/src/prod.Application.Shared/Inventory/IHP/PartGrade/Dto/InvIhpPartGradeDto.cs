using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Inventory.IHP.PartGrade.Dto
{
    public class InvIhpPartGradeDto : EntityDto<long?>
    {
        public virtual string Grade { get; set; }

        public virtual long? IhpPartId { get; set; }

        public virtual int? UsageQty { get; set; }

        public virtual DateTime? FirstDayProduct { get; set; }

        public virtual DateTime? LastDayProduct { get; set; }
    }

    public class GetInvIhpPartGradeInput : PagedAndSortedResultRequestDto
    {
        public virtual int? PartListId { get; set; }
    }

    public class GetInvIhpPartGradeExportInput
    {
        public virtual int? PartListId { get; set; }
    }

    public class ValidateIhpPartListDto : PagedAndSortedResultRequestDto
    {
        public virtual long? PartListId { get; set; }
        public virtual long? PartGradeId { get; set; }
        public virtual long? DrmPartListId { get; set; }
        public virtual string SupplierType { get; set; }
        public virtual string PartNo { get; set; }
        public virtual string PartName { get; set; }
        public virtual string Model { get; set; }
        public virtual string Cfc { get; set; }
        public virtual long? MaterialId { get; set; }
        public virtual string MaterialCode { get; set; }
        public virtual int? Packing { get; set; }
        public virtual decimal? SheetWeight { get; set; }
        public virtual decimal? YiledRation { get; set; }
        public virtual string Grade { get; set; }
        public virtual decimal? UsageQty { get; set; }
        public virtual DateTime? FirstDayProduct { get; set; }
        public virtual DateTime? LastDayProduct { get; set; }
        public virtual string ErrorDescription { get; set; }

    }
}
