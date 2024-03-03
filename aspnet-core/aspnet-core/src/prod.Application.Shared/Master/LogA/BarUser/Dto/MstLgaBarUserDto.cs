using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogA.Dto
{

    public class MstLgaBarUserDto : EntityDto<long?>
    {

        public virtual string UserId { get; set; }

        public virtual string UserName { get; set; }

        public virtual string UserDescription { get; set; }

        public virtual string IsNeedPass { get; set; }

        public virtual string Pwd { get; set; }

        public virtual long? ProcessId { get; set; }

        public virtual string ProcessCode { get; set; }

        public virtual string ProcessGroup { get; set; }

        public virtual string ProcessSubgroup { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditMstLgaBarUserDto : EntityDto<long?>
    {

        [StringLength(MstLgaBarUserConsts.MaxUserIdLength)]
        public virtual string UserId { get; set; }

        [StringLength(MstLgaBarUserConsts.MaxUserNameLength)]
        public virtual string UserName { get; set; }

        [StringLength(MstLgaBarUserConsts.MaxUserDescriptionLength)]
        public virtual string UserDescription { get; set; }

        [StringLength(MstLgaBarUserConsts.MaxIsNeedPassLength)]
        public virtual string IsNeedPass { get; set; }

        [StringLength(MstLgaBarUserConsts.MaxPwdLength)]
        public virtual string Pwd { get; set; }

        public virtual long? ProcessId { get; set; }

        [StringLength(MstLgaBarUserConsts.MaxProcessCodeLength)]
        public virtual string ProcessCode { get; set; }

        [StringLength(MstLgaBarUserConsts.MaxProcessGroupLength)]
        public virtual string ProcessGroup { get; set; }

        [StringLength(MstLgaBarUserConsts.MaxProcessSubgroupLength)]
        public virtual string ProcessSubgroup { get; set; }

        [StringLength(MstLgaBarUserConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetMstLgaBarUserInput : PagedAndSortedResultRequestDto
    {
        public virtual string UserName { get; set; }
        public virtual string ProcessCode { get; set; }
    }

    public class GetMstLgaBarUserExcelInput
    {
        public virtual string UserName { get; set; }
        public virtual string ProcessCode { get; set; }
    }
}


