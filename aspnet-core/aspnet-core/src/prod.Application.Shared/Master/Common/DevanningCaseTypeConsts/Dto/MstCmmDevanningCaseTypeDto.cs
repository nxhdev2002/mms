using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Common.Dto
{

    public class MstCmmDevanningCaseTypeDto : EntityDto<long?>
    {

        public virtual string CaseNo { get; set; }

        public virtual string CarFamilyCode { get; set; }

        public virtual string ShoptypeCode { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditMstCmmDevanningCaseTypeDto : EntityDto<long?>
    {

        [StringLength(MstCmmDevanningCaseTypeConsts.MaxCaseNoLength)]
        public virtual string CaseNo { get; set; }

        [StringLength(MstCmmDevanningCaseTypeConsts.MaxCarFamilyCodeLength)]
        public virtual string CarFamilyCode { get; set; }

        [StringLength(MstCmmDevanningCaseTypeConsts.MaxShoptypeCodeLength)]
        public virtual string ShoptypeCode { get; set; }

        [StringLength(MstCmmDevanningCaseTypeConsts.MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(MstCmmDevanningCaseTypeConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetMstCmmDevanningCaseTypeInput : PagedAndSortedResultRequestDto
    {

        public virtual string CaseNo { get; set; }
        public virtual string CarFamilyCode { get; set; }             

        public virtual string ShoptypeCode { get; set; }

        public virtual string SupplierNo { get; set; }


    }

    public class GetMstCmmDevanningCaseTypeHistoryInput : PagedAndSortedResultRequestDto
    {
        public virtual long Id { get; set; }
        public virtual string TableName { get; set; }
    }

    public class GetMstCmmDevanningCaseTypeHistoryExcelInput
    {
        public virtual long Id { get; set; }

        public virtual string TableName { get; set; }
    }
}


