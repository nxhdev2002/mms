using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.GPS.Dto
{

    public class InvGpsKanbanDto : EntityDto<long?>
    {

        public virtual long? ContentListId { get; set; }

        public virtual string BackNo { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string ColorSfx { get; set; }

        public virtual string PartName { get; set; }

        public virtual int? BoxSize { get; set; }

        public virtual int? BoxQty { get; set; }

        public virtual string PcAddress { get; set; }

        public virtual string WhSpsPicking { get; set; }

        public virtual int? ActualBoxQty { get; set; }

        public virtual int? RenbanNo { get; set; }

        public virtual int? NoInRenban { get; set; }

        public virtual string PackagingType { get; set; }

        public virtual int? ActualBoxSize { get; set; }

        public virtual string GeneratedBy { get; set; }

        public virtual string Status { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditInvGpsKanbanDto : EntityDto<long?>
    {

        public virtual long? ContentListId { get; set; }

        [StringLength(InvGpsKanbanConsts.MaxBackNoLength)]
        public virtual string BackNo { get; set; }

        [StringLength(InvGpsKanbanConsts.MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(InvGpsKanbanConsts.MaxColorSfxLength)]
        public virtual string ColorSfx { get; set; }

        [StringLength(InvGpsKanbanConsts.MaxPartNameLength)]
        public virtual string PartName { get; set; }

        public virtual int? BoxSize { get; set; }

        public virtual int? BoxQty { get; set; }

        [StringLength(InvGpsKanbanConsts.MaxPcAddressLength)]
        public virtual string PcAddress { get; set; }

        [StringLength(InvGpsKanbanConsts.MaxWhSpsPickingLength)]
        public virtual string WhSpsPicking { get; set; }

        public virtual int? ActualBoxQty { get; set; }

        public virtual int? RenbanNo { get; set; }

        public virtual int? NoInRenban { get; set; }

        [StringLength(InvGpsKanbanConsts.MaxPackagingTypeLength)]
        public virtual string PackagingType { get; set; }

        public virtual int? ActualBoxSize { get; set; }

        [StringLength(InvGpsKanbanConsts.MaxGeneratedByLength)]
        public virtual string GeneratedBy { get; set; }

        [StringLength(InvGpsKanbanConsts.MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(InvGpsKanbanConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetInvGpsKanbanInput : PagedAndSortedResultRequestDto
    {
        public virtual string BackNo { get; set; }

        public virtual string PartNo { get; set; }
    }
}


