using Abp.Application.Services.Dto;
using Abp.ObjectComparators.DateTimeComparators;
using System;

namespace prod.Master.Common.Dto
{
    public class MstCommonMMValidationResultDto : EntityDto<long?>
    {

        public virtual long? MateriaId { get; set; }

        public virtual string MaterialCode { get; set; }

        public virtual string MaterialName { get; set; }

        public virtual string MaterialGroup { get; set; }

        public virtual string ValuationClass { get; set; }

        public virtual string ValuationType { get; set; }

        public virtual long? RuleId { get; set; }

        public virtual string RuleCode { get; set; }

        public virtual string RuleDescription { get; set; }

        public virtual string RuleItem { get; set; }

        public virtual string Option { get; set; }

        public virtual string ResultField { get; set; }

        public virtual string ExpectedResult { get; set; }

        public virtual string ActualResult { get; set; }

        public virtual DateTime? LastValidationDatetime { get; set; }

        public virtual string Lastvalidationby { get; set; }

        public virtual long? LastValidationId { get; set; }

        public virtual string Status { get; set; }

        public virtual string ErrorMessage { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class GetMstCommonMMValidationResultInput : PagedAndSortedResultRequestDto
    {
        public virtual string MaterialCode { get; set; }
        public virtual string MaterialGroup { get; set; }
        public virtual string RuleCode { get; set; }
        public virtual string RuleItem { get; set; }
        public virtual string Resultfield { get; set; }
    }

    public class GetMstCommonMMValidationResultExportInput
    {
        public virtual string MaterialCode { get; set; }
        public virtual string MaterialGroup { get; set; }
        public virtual string RuleCode { get; set; }
        public virtual string RuleItem { get; set; }  
        public virtual string Resultfield { get; set; }


    }

    public class GetMstCommonMMValidationResultHistoryInput : PagedAndSortedResultRequestDto
    {
        public virtual long Id { get; set; }
        public virtual string TableName { get; set; }
    }
    public class GetMstCommonMMValidationResultHistoryExcelInput
    {
        public virtual long Id { get; set; }
        public virtual string TableName { get; set; }
    }
}
