using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Cmm.Dto
{

    public class MstCmmMMCheckingRuleDto : EntityDto<long?>
    {

        public virtual string RuleCode { get; set; }

        public virtual string RuleDescription { get; set; }

        public virtual string RuleItem { get; set; }

        public virtual string Field1Name { get; set; }

        public virtual string Field1Value { get; set; }

        public virtual string Field2Name { get; set; }

        public virtual string Field2Value { get; set; }

        public virtual string Field3Name { get; set; }

        public virtual string Field3Value { get; set; }

        public virtual string Field4Name { get; set; }

        public virtual string Field4Value { get; set; }

        public virtual string Field5Name { get; set; }

        public virtual string Field5Value { get; set; }

        public virtual string Option { get; set; }

        public virtual string Resultfield { get; set; }

        public virtual string Expectedresult { get; set; }

        public virtual string Checkoption { get; set; }

        public virtual string Errormessage { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditMstCmmMMCheckingRuleDto : EntityDto<long?>
    {

        [StringLength(MstCmmMMCheckingRuleConsts.MaxRuleCodeLength)]
        public virtual string RuleCode { get; set; }

        [StringLength(MstCmmMMCheckingRuleConsts.MaxRuleDescriptionLength)]
        public virtual string RuleDescription { get; set; }

        [StringLength(MstCmmMMCheckingRuleConsts.MaxRuleItemLength)]
        public virtual string RuleItem { get; set; }

        [StringLength(MstCmmMMCheckingRuleConsts.MaxField1NameLength)]
        public virtual string Field1Name { get; set; }

        [StringLength(MstCmmMMCheckingRuleConsts.MaxField1ValueLength)]
        public virtual string Field1Value { get; set; }

        [StringLength(MstCmmMMCheckingRuleConsts.MaxField2NameLength)]
        public virtual string Field2Name { get; set; }

        [StringLength(MstCmmMMCheckingRuleConsts.MaxField2ValueLength)]
        public virtual string Field2Value { get; set; }

        [StringLength(MstCmmMMCheckingRuleConsts.MaxField3NameLength)]
        public virtual string Field3Name { get; set; }

        [StringLength(MstCmmMMCheckingRuleConsts.MaxField3ValueLength)]
        public virtual string Field3Value { get; set; }

        [StringLength(MstCmmMMCheckingRuleConsts.MaxField4NameLength)]
        public virtual string Field4Name { get; set; }

        [StringLength(MstCmmMMCheckingRuleConsts.MaxField4ValueLength)]
        public virtual string Field4Value { get; set; }

        [StringLength(MstCmmMMCheckingRuleConsts.MaxField5NameLength)]
        public virtual string Field5Name { get; set; }

        [StringLength(MstCmmMMCheckingRuleConsts.MaxField5ValueLength)]
        public virtual string Field5Value { get; set; }

        [StringLength(MstCmmMMCheckingRuleConsts.MaxOptionLength)]
        public virtual string Option { get; set; }

        [StringLength(MstCmmMMCheckingRuleConsts.MaxResultfieldLength)]
        public virtual string Resultfield { get; set; }

        [StringLength(MstCmmMMCheckingRuleConsts.MaxExpectedresultLength)]
        public virtual string Expectedresult { get; set; }

        [StringLength(MstCmmMMCheckingRuleConsts.MaxCheckoptionLength)]
        public virtual string Checkoption { get; set; }

        [StringLength(MstCmmMMCheckingRuleConsts.MaxErrormessageLength)]
        public virtual string Errormessage { get; set; }

        [StringLength(MstCmmMMCheckingRuleConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetMstCmmMMCheckingRuleInput : PagedAndSortedResultRequestDto
    {
        public virtual string RuleCode { get; set; }

        public virtual string RuleItem { get; set; }

        public virtual string FieldName { get; set; }

        public virtual string Resultfield { get; set; }

        public virtual string IsActive { get; set; }
    }

    public class MstCmmMMCheckingRuleImportDto 
    {

        public virtual long? ROW_NO { get; set; }
        public virtual string Guid { get; set; }
        public virtual string RuleCode { get; set; }

        public virtual string RuleDescription { get; set; }

        public virtual string RuleItem { get; set; }

        public virtual string Field1Name { get; set; }

        public virtual string Field1Value { get; set; }

        public virtual string Field2Name { get; set; }

        public virtual string Field2Value { get; set; }

        public virtual string Field3Name { get; set; }

        public virtual string Field3Value { get; set; }

        public virtual string Field4Name { get; set; }

        public virtual string Field4Value { get; set; }

        public virtual string Field5Name { get; set; }

        public virtual string Field5Value { get; set; }

        public virtual string Option { get; set; }

        public virtual string Resultfield { get; set; }

        public virtual string Expectedresult { get; set; }

        public virtual string Checkoption { get; set; }

        public virtual string Errormessage { get; set; }

        public virtual string IsActive { get; set; }

        public string ErrorDescription { get; set; }

        public virtual string IsDetail { get; set; }



    }

    public class GetMstCmmMMCheckingRuleHistoryInput : PagedAndSortedResultRequestDto
    {
        public virtual long Id { get; set; }
        public virtual string TableName { get; set; }
    }
    public class GetMstCmmMMCheckingRuleHistoryExcelInput
    {
        public virtual long Id { get; set; }
        public virtual string TableName { get; set; }
    }

}


