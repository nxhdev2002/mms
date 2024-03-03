using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Inventory.Dto
{

    public class MstInvPIOEmailDto : EntityDto<long?>
    {

        public virtual string Subject { get; set; }

        public virtual string To { get; set; }

        public virtual string Cc { get; set; }

        public virtual string BodyMess { get; set; }

        public virtual string SupplierCd { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditMstInvPIOEmailDto : EntityDto<long?>
    {

        [StringLength(MstInvPIOEmailConsts.MaxSubjectLength)]
        public virtual string Subject { get; set; }

        [StringLength(MstInvPIOEmailConsts.MaxToLength)]
        public virtual string To { get; set; }

        [StringLength(MstInvPIOEmailConsts.MaxCcLength)]
        public virtual string Cc { get; set; }

        [StringLength(MstInvPIOEmailConsts.MaxBodyMessLength)]
        public virtual string BodyMess { get; set; }

        [StringLength(MstInvPIOEmailConsts.MaxSupplierCdLength)]
        public virtual string SupplierCd { get; set; }

        [StringLength(MstInvPIOEmailConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetMstInvPIOEmailInput : PagedAndSortedResultRequestDto
    {

        public virtual string Subject { get; set; }

        public virtual string To { get; set; }

        public virtual string Cc { get; set; }

        public virtual string BodyMess { get; set; }

        public virtual string SupplierCd { get; set; }

        public virtual string IsActive { get; set; }

    }

}


