using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Inv.Dto
{

    public class MstInvGpsTruckSupplierDto : EntityDto<long?>
    {

        public virtual int? SupplierId { get; set; }

        public virtual string TruckName { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditMstInvGpsTruckSupplierDto : EntityDto<long?>
    {
        public virtual int? SupplierId { get; set; }

        [StringLength(MstInvGpsTruckSupplierConsts.MaxTruckNameLength)]
        public virtual string TruckName { get; set; }

        [StringLength(MstInvGpsTruckSupplierConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetMstInvGpsTruckSupplierInput : PagedAndSortedResultRequestDto
    {
        public virtual string TruckName { get; set; }

    }

    public class MstInvGpsTruckSupplierExportInput
    {
        public virtual string TruckName { get; set; }
    }

}


