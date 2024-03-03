using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Inventory.IHP.StockPart.Dto
{
    public class InvIhpStockPartDto : EntityDto<long?>
    {
        public virtual long? DrmMaterialId { get; set; }
        public virtual string MaterialCode { get; set; }
        public virtual string MaterialSpec { get; set; }
        public virtual string PartCode { get; set; }
        public virtual int? Qty { get; set; }
        public virtual DateTime WorkingDate { get; set; }
        public virtual string PartNo { get; set; }
        public virtual string PartNo5Digits { get; set; }
        public virtual string Cfc { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string Model { get; set; }
        public virtual string GradeName { get; set; }
        public virtual int? UsePress { get; set; }
        public virtual int? Press { get; set; }
        public virtual int? IhpOh { get; set; }
        public virtual int? PressBroken { get; set; }
        public virtual int? Hand { get; set; }
        public virtual int? HandOh { get; set; }
        public virtual int? HandBroken { get; set; }
        public virtual int? MaterialIn { get; set; }
        public virtual int? MaterialInAddition { get; set; }
        public virtual string Shift { get; set; }
        public virtual DateTime CreationTime { get; set; }
        public virtual long? CreatorUserId { get; set; }
        public virtual DateTime LastModificationTime { get; set; }
        public virtual long? LastModifierUserId { get; set; }
        public virtual long? PartId { get; set; }

    }

    public class GetInvIhpStockPartInput : PagedAndSortedResultRequestDto
    {
        public virtual string PartNo { get; set; }
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }
        public virtual string Model { get; set; }
    }


    public class GetInvInpIfViewDto : EntityDto<long?>
    {
        public virtual long? DrmMaterialId { get; set; }
        public virtual string PartCode { get; set; }
        public virtual string PartNo { get; set; }
        public virtual int? Press { get; set; }
        public virtual DateTime? WorkingDate { get; set; }
        public virtual string SupplierType { get; set; }
        public virtual string SupplierCd { get; set; }
        public virtual string Cfc { get; set; }
        public virtual string MaterialCode { get; set; }
        public virtual string MaterialSpec { get; set; }
        public virtual string FinMaterialNumber { get; set; }
        public virtual string FinMaterialCode { get; set; }
        public virtual string FinMaterialFinSpec { get; set; }
        public virtual string FinPartSize { get; set; }
        public virtual string FinPartPrice { get; set; }
        public virtual string PartSpec { get; set; }
        public virtual string SizeCode { get; set; }
        public virtual string PartSize { get; set; }
        public virtual string BoxQty { get; set; }
        public virtual string FirstDayProduct { get; set; }
        public virtual string LastDayProduct { get; set; }
        public virtual string Sourcing { get; set; }
        public virtual string Cutting { get; set; }
        public virtual string Packing { get; set; }
        public virtual string SheetWeight { get; set; }
        public virtual string YiledRation { get; set; }
        public virtual string Model { get; set; }
        public virtual string GradeName { get; set; }
        public virtual string ModelCode { get; set; }

    }

    public class GetInvInpIfViewInput : PagedAndSortedResultRequestDto
    {
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }
    }
}
