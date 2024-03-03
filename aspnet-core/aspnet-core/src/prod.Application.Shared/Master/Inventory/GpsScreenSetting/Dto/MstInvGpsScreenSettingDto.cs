using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Inv.Dto
{

    public class MstInvGpsScreenSettingDto : EntityDto<long?>
    {

        public virtual string ScreenName { get; set; }

        public virtual string ScreenType { get; set; }

        public virtual string ScreenValue { get; set; }

        public virtual string Description { get; set; }

        public virtual int? BarcodeId { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditMstInvGpsScreenSettingDto : EntityDto<long?>
    {

        [StringLength(MstInvGpsScreenSettingConsts.MaxScreenNameLength)]
        public virtual string ScreenName { get; set; }

        [StringLength(MstInvGpsScreenSettingConsts.MaxScreenTypeLength)]
        public virtual string ScreenType { get; set; }

        [StringLength(MstInvGpsScreenSettingConsts.MaxScreenValueLength)]
        public virtual string ScreenValue { get; set; }

        [StringLength(MstInvGpsScreenSettingConsts.MaxDescriptionLength)]
        public virtual string Description { get; set; }

        public virtual int? BarcodeId { get; set; }

        [StringLength(MstInvGpsScreenSettingConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetMstInvGpsScreenSettingInput : PagedAndSortedResultRequestDto
    {

        public virtual string ScreenName { get; set; }

        public virtual string ScreenType { get; set; }

        public virtual string ScreenValue { get; set; }

    }

}

