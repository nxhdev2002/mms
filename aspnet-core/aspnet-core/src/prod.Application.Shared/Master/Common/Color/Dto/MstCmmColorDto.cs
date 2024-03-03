using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Cmm.Dto
{

    public class MstCmmColorDto : EntityDto<long?>
    {

        public virtual string Color { get; set; }

        public virtual string NameEn { get; set; }

        public virtual string NameVn { get; set; }

        public virtual string ColorType { get; set; }

        public virtual string NameColorType { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditMstCmmColorDto : EntityDto<long?>
    {

        [StringLength(MstCmmColorConsts.MaxColorLength)]
        public virtual string Color { get; set; }

        [StringLength(MstCmmColorConsts.MaxNameEnLength)]
        public virtual string NameEn { get; set; }

        [StringLength(MstCmmColorConsts.MaxNameVnLength)]
        public virtual string NameVn { get; set; }

        [StringLength(MstCmmColorConsts.MaxColorTypeLength)]
        public virtual string ColorType { get; set; }

        [StringLength(MstCmmColorConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetMstCmmColorInput : PagedAndSortedResultRequestDto
    {

        public virtual string Color { get; set; }

        public virtual string NameEn { get; set; }

        public virtual string NameVn { get; set; }

        public virtual string ColorType { get; set; }

    }
    public class GetMstCmmColorHistoryInput : PagedAndSortedResultRequestDto
    {
        public virtual long Id { get; set; }
        public virtual string TableName { get; set; }
    }
    public class GetMstCmmColorHistoryExcelInput 
    {
        public virtual long Id { get; set; }
        public virtual string TableName { get; set; }
    }

}


