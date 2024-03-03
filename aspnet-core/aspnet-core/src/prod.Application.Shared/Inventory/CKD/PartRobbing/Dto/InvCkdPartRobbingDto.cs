using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Inventory.CKD.PartRobbing.Dto
{
    public class InvCkdPartRobbingDto : EntityDto<long?>
    {
        public virtual string PartNo { get; set; }

        public virtual string PartNoNormalized { get; set; }

        public virtual string PartName { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string Shop { get; set; }

        public virtual string Case { get; set; }

        public virtual string Box { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual int? RobbingQty { get; set; }

        public virtual int? UnitQty { get; set; }

        public virtual int? EffectVehQty { get; set; }

        public virtual string DetailModel { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class GetPartRobbingInput: PagedAndSortedResultRequestDto
    {
        public virtual string PartNo { get; set; }

        public virtual string Cfc { get; set; }

    }

    public class GetPartRobbingToExcel
    {
        public virtual string PartNo { get; set; }

        public virtual string Cfc { get; set; }

    }

    public class ExportCkdPartRobbingDto
    {
        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string Shop { get; set; }

        public virtual string Case { get; set; }

        public virtual string Box { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual int? RobbingQty { get; set; }

        public virtual int? UnitQty { get; set; }

        public virtual int? EffectVheQty { get; set; }

        public virtual string DetailModel { get; set; }
    }

    public class InvCkdPartRobbingImportDto
    {
        [StringLength(128)]
        public virtual string Guid { get; set; }

        [StringLength(50)]
        public virtual string PartNo { get; set; }

        [StringLength(50)]
        public virtual string PartNoNormalized { get; set; }

        [StringLength(500)]
        public virtual string PartName { get; set; }

        [StringLength(4)]
        public virtual string Cfc { get; set; }

        [StringLength(10)]
        public virtual string Shop { get; set; }

        [StringLength(50)]
        public virtual string Case { get; set; }

        [StringLength(50)]
        public virtual string Box { get; set; }

        [StringLength(50)]
        public virtual string SupplierNo { get; set; }

        public virtual int? RobbingQty { get; set; }

        public virtual int? UnitQty { get; set; }

        public virtual int? EffectVehQty { get; set; }

        [StringLength(5000)]
        public virtual string DetailModel { get; set; }

        [StringLength(1)]
        public virtual string IsActive { get; set; }

        public string ErrorDescription { get; set; }
        public bool CanBeImported()
        {
            return string.IsNullOrEmpty(ErrorDescription);
        }
    }

    public class InvCkdPartRobbingDetailsDto : EntityDto<long?>
    {      
        public virtual string Grade { get; set; }

        public virtual decimal? RobbingQty { get; set; }
    }

    public class GetPartRobbingDetailsInput : PagedAndSortedResultRequestDto
    {
        public virtual long? PartId { get; set; }
    }

    public class GetPartRobbingDetailsToExcel
    {
        public virtual long? PartId { get; set; }
    }

    public class ExportCkdPartRobbingDetailsDto
    {
        public virtual string PartNo { get; set; }       

        public virtual string Grade { get; set; }

        public virtual string Cfc { get; set; }

        public virtual int RobbingQty { get; set; }
    }
}
