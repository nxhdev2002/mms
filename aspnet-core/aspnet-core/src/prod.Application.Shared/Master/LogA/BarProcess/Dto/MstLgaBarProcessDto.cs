using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogA.Dto
{

    public class MstLgaBarProcessDto : EntityDto<long?>
    {

        public virtual string Code { get; set; }

        public virtual string ProcessName { get; set; }

        public virtual string ProcessGroup { get; set; }

        public virtual string ProcessSubgroup { get; set; }

        public virtual string ProdLine { get; set; }

        public virtual string ProcessType { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditMstLgaBarProcessDto : EntityDto<long?>
    {

        [StringLength(MstLgaBarProcessConsts.MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MstLgaBarProcessConsts.MaxProcessNameLength)]
        public virtual string ProcessName { get; set; }

        [StringLength(MstLgaBarProcessConsts.MaxProcessGroupLength)]
        public virtual string ProcessGroup { get; set; }

        [StringLength(MstLgaBarProcessConsts.MaxProcessSubgroupLength)]
        public virtual string ProcessSubgroup { get; set; }

        [StringLength(MstLgaBarProcessConsts.MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        [StringLength(MstLgaBarProcessConsts.MaxProcessTypeLength)]
        public virtual string ProcessType { get; set; }

        [StringLength(MstLgaBarProcessConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetMstLgaBarProcessInput : PagedAndSortedResultRequestDto
    {
        public virtual string Code { get; set; }

        public virtual string ProcessName { get; set; }

        public virtual string ProdLine { get; set; }
    }
    public class GetMstLgaBarProcesExcelInput
    {
        public virtual string Code { get; set; }

        public virtual string ProcessName { get; set; }

        public virtual string ProdLine { get; set; }
    }
}