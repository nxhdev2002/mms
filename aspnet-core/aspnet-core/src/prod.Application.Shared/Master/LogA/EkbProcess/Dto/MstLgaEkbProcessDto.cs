using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogA.Dto
{

    public class MstLgaEkbProcessDto : EntityDto<long?>
    {

        public virtual string Code { get; set; }

        public virtual string ProcessName { get; set; }

        public virtual string ProcessGroup { get; set; }

        public virtual string ProcessSubgroup { get; set; }

        public virtual string ProdLine { get; set; }

        public virtual int? LeadTime { get; set; }

        public virtual int? Sorting { get; set; }

        public virtual string ProcessType { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditMstLgaEkbProcessDto : EntityDto<long?>
    {

        [StringLength(MstLgaEkbProcessConsts.MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MstLgaEkbProcessConsts.MaxProcessNameLength)]
        public virtual string ProcessName { get; set; }

        [StringLength(MstLgaEkbProcessConsts.MaxProcessGroupLength)]
        public virtual string ProcessGroup { get; set; }

        [StringLength(MstLgaEkbProcessConsts.MaxProcessSubgroupLength)]
        public virtual string ProcessSubgroup { get; set; }

        [StringLength(MstLgaEkbProcessConsts.MaxProdLineLength)]
        public virtual string ProdLine { get; set; }


        [Required]
        public virtual int? LeadTime { get; set; }


        [Required]
        public virtual int? Sorting { get; set; }

        [StringLength(MstLgaEkbProcessConsts.MaxProcessTypeLength)]
        public virtual string ProcessType { get; set; }

        [StringLength(MstLgaEkbProcessConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetMstLgaEkbProcessInput : PagedAndSortedResultRequestDto
    {
        public virtual string Code { get; set; }
        public virtual string ProcessName { get; set; }
        public virtual string ProdLine { get; set; }
    }
    public class GetMstLgaEkbProcessExcelInput
    {
        public virtual string Code { get; set; }
        public virtual string ProcessName { get; set; }
        public virtual string ProdLine { get; set; }
    }
}


