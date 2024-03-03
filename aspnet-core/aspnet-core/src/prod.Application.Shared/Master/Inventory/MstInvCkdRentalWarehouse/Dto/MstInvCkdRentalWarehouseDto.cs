using Abp.Application.Services.Dto;
using prod.Master.Inventory.MstInvCpsInventoryGroups;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Inventory.Dto
{

    public class MstInvCkdRentalWarehouseDto : EntityDto<long?>
    {

        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
        public virtual string IsActive { get; set; }
        public virtual DateTime? FromDate { get; set; }
        public virtual DateTime? ToDate { get; set; }
	}
    public class CreateOrEditMstInvCkdRentalWarehouseDto : EntityDto<long?>
    {
        public const int MaxCodeLength = 3;

        public const int MaxNameLength = 50;

        public const int MaxIsActiveLength = 1;

		[StringLength(MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }

        public virtual DateTime? FromDate { get; set; }
        public virtual DateTime? ToDate { get; set; }
	}

    public class GetMstInvCkdRentalWarehouseInput : PagedAndSortedResultRequestDto
    {

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual string IsActive { get; set; }

    }

}

