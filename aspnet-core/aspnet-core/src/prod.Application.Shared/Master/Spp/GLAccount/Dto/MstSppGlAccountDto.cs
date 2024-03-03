using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Spp.Dto
{

    public class MstSppGlAccountDto : EntityDto<long?>
    {

        public virtual string GlAccountNo { get; set; }

        public virtual string GlAccountNoS4 { get; set; }

        public virtual string GlType { get; set; }

        public virtual DateTime? StartDate { get; set; }

        public virtual DateTime? EndDate { get; set; }

        public virtual string GlDescEn { get; set; }

        public virtual string GlDesc { get; set; }

        public virtual string CrDb { get; set; }

    }

    public class CreateOrEditMstSppGlAccountDto : EntityDto<long?>
    {

        [StringLength(MstSppGlAccountConsts.MaxGlAccountNoLength)]
        public virtual string GlAccountNo { get; set; }

        [StringLength(MstSppGlAccountConsts.MaxGlAccountNoS4Length)]
        public virtual string GlAccountNoS4 { get; set; }

        [StringLength(MstSppGlAccountConsts.MaxGlTypeLength)]
        public virtual string GlType { get; set; }

        public virtual DateTime? StartDate { get; set; }

        public virtual DateTime? EndDate { get; set; }

        [StringLength(MstSppGlAccountConsts.MaxGlDescEnLength)]
        public virtual string GlDescEn { get; set; }

        [StringLength(MstSppGlAccountConsts.MaxGlDescLength)]
        public virtual string GlDesc { get; set; }

        [StringLength(MstSppGlAccountConsts.MaxCrDbLength)]
        public virtual string CrDb { get; set; }
    }

    public class GetMstSppGlAccountInput : PagedAndSortedResultRequestDto
    {

        public virtual string GlAccountNo { get; set; }

        public virtual string GlType { get; set; }

        public virtual DateTime? StartDate { get; set; }

        public virtual DateTime? EndDate { get; set; }


    }

}


