using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Inventory.Dto
{

    public class MstInvCpsInventoryItemsDto : EntityDto<long?>
    {

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual string Color { get; set; }

        public virtual string Puom { get; set; }

    }

    public class CreateOrEditMstInvCpsInventoryItemsDto : EntityDto<long?>
    {

        [StringLength(MstInvCpsInventoryItemsConsts.MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MstInvCpsInventoryItemsConsts.MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(MstInvCpsInventoryItemsConsts.MaxColorLength)]
        public virtual string Color { get; set; }

        [StringLength(MstInvCpsInventoryItemsConsts.MaxPuomLength)]
        public virtual string Puom { get; set; }
    }

    public class GetMstInvCpsInventoryItemsInput : PagedAndSortedResultRequestDto
    {

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual string Color { get; set; }

        public virtual string Puom { get; set; }

    }

}