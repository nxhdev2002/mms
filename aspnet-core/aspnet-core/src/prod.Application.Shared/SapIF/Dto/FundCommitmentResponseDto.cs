using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace prod.SapIF.Dto
{
    [Display(Name = "MaintainFundCommitResp_MT")]
    public class FundCommitmentResponseDto
    {
        [Display(Name = "RESPONSE")]
        public FundCommitmentResponse Response { get; set; }
    }
    public class FundCommitmentResponse
    {
        [Display(Name = null, GroupName = "document")]
        public List<FundCommitmentResponseDocument> Documents { get; set; }
    }
    public class FundCommitmentResponseDocument
    {
        [Display(Name = "doc_no")]
        public string DocumentNo { get; set; }
        [Display(Name = "doc_line_item_no")]
        public string DocumentLineItemNo { get; set; }
        [Display(Name = "fund_cmmt_doc")]
        public string FundsCommitmentDocument { get; set; }
        [Display(Name = "fund_cmmt_doc_line_item")]
        public string FundsCommitmentDocumentLineItem { get; set; }
        [Display(Name = "msg_type")]
        public string MessageType { get; set; }
        [Display(Name = "msg_id")]
        public string MessageID { get; set; }
        [Display(Name = "msg_no")]
        public string MessageNo { get; set; }
        [Display(Name = "msg")]
        public string Message { get; set; }
    }
}
