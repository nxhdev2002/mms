using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.GPS.Dto
{

    public class InvGpsIssuingDto : EntityDto<long?>
    {

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual string Uom { get; set; }

        public virtual int? Boxqty { get; set; }

        public virtual int? Box { get; set; }

        public virtual int? QtyRequest { get; set; }

        public virtual string LotNo { get; set; }
        public virtual DateTime? IssueDate { get; set; }

        public virtual DateTime? ProdDate { get; set; }

        public virtual DateTime? ExpDate { get; set; }

        public virtual DateTime? ReceivedDate { get; set; }

        public virtual string Supplier { get; set; }

        public virtual string CostCenter { get; set; }

        public virtual int? QtyIssue { get; set; }

        public virtual string IsIssue { get; set; }


        public virtual string Status { get; set; }

        public virtual string IsGentani { get; set; }

    }

    public class CreateOrEditInvGpsIssuingDto : EntityDto<long?>
    {

        [StringLength(InvGpsIssuingConsts.MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(InvGpsIssuingConsts.MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(InvGpsIssuingConsts.MaxOumLength)]
        public virtual string Uom { get; set; }

        public virtual int? Boxqty { get; set; }

        public virtual int? Box { get; set; }

        public virtual int? QtyRequest { get; set; }

        [StringLength(InvGpsIssuingConsts.MaxLotNoLength)]
        public virtual string LotNo { get; set; }

        public virtual DateTime? ProdDate { get; set; }

        public virtual DateTime? ExpDate { get; set; }

        public virtual string CostCenter { get; set; }

        public virtual int? QtyIssue { get; set; }

        [StringLength(InvGpsIssuingConsts.MaxIsIssueLength)]
        public virtual string IsIssue { get; set; }

        public virtual DateTime? IssueDate { get; set; }

        [StringLength(InvGpsIssuingConsts.MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(InvGpsIssuingConsts.MaxIsGentaniLength)]
        public virtual string IsGentani { get; set; }
    }

    public class GetInvGpsIssuingInput : PagedAndSortedResultRequestDto
    {

        public virtual string PartNo { get; set; }

        public virtual string LotNo { get; set; }

        public virtual DateTime? ReqDateFrom { get; set; }

        public virtual DateTime? ReqDateTo { get; set; }

        public virtual DateTime? IssueDateFrom { get; set; }

        public virtual DateTime? IssueDateTo { get; set; }
        public virtual DateTime? Today { get; set; }

        public virtual string Status { get; set; }

        public virtual string IsGentani { get; set; }

    }

    public class GetInvGesIssuingImport : EntityDto<long?>
    {
        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual string Uom { get; set; }

        public virtual int? Boxqty { get; set; }

        public virtual int? Box { get; set; }

        public virtual int? QtyRequest { get; set; }

        public virtual string LotNo { get; set; }

        public virtual DateTime? ProdDate { get; set; }

        public virtual DateTime? ExpDate { get; set; }

        public virtual DateTime? ReceivedDate { get; set; }

        public virtual string Supplier { get; set; }

        public virtual string CostCenter { get; set; }

        public virtual int? QtyIssue { get; set; }

        public virtual string IsIssue { get; set; }

        public virtual string Status { get; set; }

        public virtual string IsGentani { get; set; }

        public virtual DateTime? IssueDate { get; set; }

        public virtual long? ROW_NO { get; set; }
        public virtual string Guid { get; set; }
        public virtual string ErrorDescription { get; set; }

    }

    public class GpsIssuingRequestCheckDto
    {

        public virtual string Result { get; set; }


    }


    public class LoginTestDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string TenancyName { get; set; }

    }

    public class GetGpsIssuingRequetCreateInput
    {
        public int? HeaderId { get; set; }
        public int? QtyRequest { get; set; }
        public string BudgetCheckMessage { get; set; }
        public string FundCommitmentMessage { get; set; }

    }

    public class GetGpsIssuingInput
    {
        public int? IssuingHeaderId { get; set; }
        public string Status { get; set; }
        public int? IssuingDetailsId { get; set; }      
        public int? QtyRequest { get; set; }
        public string BudgetCheckMessage { get; set; }
        public string FundCommitmentMessage { get; set; }
        public string StatusItem { get; set; }
        public int? QtyReject { get; set; }
        public int? QtyIssue { get; set; }

    }

    public class GetGpsIssuingDetailInput
    {
        public int? IdDetails { get; set; }
        public int? QtyRequest { get; set; }
        public int? QtyReject { get; set; }
        public int? QtyIssue { get; set; }
        public string StatusItem { get; set; }
        public string BudgetCheckMessage { get; set; }
        public string FundCommitmentMessage { get; set; }
        public int? QtyRemain { get; set; }
    }

    public class GetGpsIssuingSubmitInput
    {
        public int? IssuingHeaderId { get; set; }
        public int? IssuingDetailsId { get; set; }
        public string Status { get; set; }
        public int? QtyIssue { get; set; }
        public DateTime? IssueDate { get; set; }
    }
    public class MessageDto
    {
        public string ExistMaterial { get; set; }

        public string ExistDocumentNo { get; set; }
    }

    public class GetCostCenter : EntityDto<long?>
    {
        public string CostCenter { get; set; }
    }

    public class GetPartNo
    {
        public string PartNo { get; set; }
    }

    public class GetIssuingImportView
    {
        public string DocumentNo { get; set; }
        public string DocumentDate { get; set; }
        public string Shop { get; set; }
        public string CostCenter { get; set; }
        public string Status { get; set; }
    }

    public class GetNewItemRequestValidateInputDto
    {
        public string DocumentNo { get; set; }

        public string PartNo { get; set; }

        public string Shop { get; set; }

        public string CostCenter { get; set; }
    }

    public class GetNewItemRequestValidateDto
    {
        public string Wbs { get; set; }

        public string WbsMapping { get; set; }

        public string CostCenterMapping { get; set; }

        public string GlAccount { get; set; }

        public string ErrorDescription { get; set; }   

        public string IsBudgetCheck { get; set; }
    }
}

