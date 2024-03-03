using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogA.Dto
{
    public class MstLgaEkbUserDto : EntityDto<long?>
    {

        public virtual string UserCode { get; set; }

        public virtual string UserName { get; set; }

        public virtual int? ProcessId { get; set; }

        public virtual string ProcessCode { get; set; }

        public virtual string ProcessGroup { get; set; }

        public virtual string ProcessSubgroup { get; set; }

        public virtual string ProdLine { get; set; }

        public virtual string UserType { get; set; }

        public virtual int? LeadTime { get; set; }

        public virtual int? Sortingg { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditMstLgaEkbUserDto : EntityDto<long?>
    {

        [StringLength(MstLgaEkbUserConsts.MaxUserCodeLength)]
        public virtual string UserCode { get; set; }

        [StringLength(MstLgaEkbUserConsts.MaxUserNameLength)]
        public virtual string UserName { get; set; }

        public virtual int? ProcessId { get; set; }

        [StringLength(MstLgaEkbUserConsts.MaxProcessCodeLength)]
        public virtual string ProcessCode { get; set; }

        [StringLength(MstLgaEkbUserConsts.MaxProcessGroupLength)]
        public virtual string ProcessGroup { get; set; }

        [StringLength(MstLgaEkbUserConsts.MaxProcessSubgroupLength)]
        public virtual string ProcessSubgroup { get; set; }

        [StringLength(MstLgaEkbUserConsts.MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        [StringLength(MstLgaEkbUserConsts.MaxUserTypeLength)]
        public virtual string UserType { get; set; }

        public virtual int? LeadTime { get; set; }

        public virtual int? Sortingg { get; set; }

        [StringLength(MstLgaEkbUserConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetMstLgaEkbUserInput : PagedAndSortedResultRequestDto
    {

        public virtual string UserCode { get; set; }

        public virtual string UserName { get; set; }

        public virtual int? ProcessId { get; set; }

        public virtual string ProcessCode { get; set; }

        public virtual string ProcessGroup { get; set; }

        public virtual string ProcessSubgroup { get; set; }

        public virtual string ProdLine { get; set; }

        public virtual string UserType { get; set; }

        public virtual int? LeadTime { get; set; }

        public virtual int? Sortingg { get; set; }

        public virtual string IsActive { get; set; }

    }

}


