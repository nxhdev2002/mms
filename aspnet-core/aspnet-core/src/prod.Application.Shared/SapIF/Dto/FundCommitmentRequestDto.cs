using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace prod.SapIF.Dto
{
    [Display(Name = "MaintainFundCommitReq_MT")]
    public class FundCommitmentRequestDto
    {
        [Display(Name = "REQUEST")]
        public FundCommitmentRequest Request { get; set; }
    }
    public class FundCommitmentRequest
    {
        [Display(Name = "document")]
        public FundCommitmentRequestDocument Document { get; set; }
    }
    public class FundCommitmentRequestDocument
    {
        [Display(Name = "doc_type"), MaxLength(5), Required]
        public string DocumentType { get; set; }
        [Display(Name = "action"), MaxLength(1), Required]
        public string Action { get; set; }
        [Display(Name = "system"), MaxLength(10), Required]
        public string System { get; set; }
        [Display(Name = "test_run"), MaxLength(1)]
        public string TestRun { get; set; }
        [Display(Name = "doc_no"), MaxLength(15), Required]
        public string DocumentNo { get; set; }
        [Display(Name = "closed"), MaxLength(1), Required]
        public string Closed { get; set; }
        [Display(Name = "doc_date"), Required]
        public DateTime DocumentDate { get; set; }        
        [Display(Name = "requestor"), MaxLength(60), Required]
        public string Requestor { get; set; }
        [Display(Name = "company_code"), MaxLength(8), Required]
        public string CompanyCode { get; set; }
        [Display(Name = "currency"), MaxLength(5), Required]
        public string Currency { get; set; }
        [Display(Name = "currency_rate"), MaxLength(12)]
        public string CurrencyRate { get; set; }
        [Display(Name = null, GroupName = "item")]
        public List<FundCommitmentRequestDocumentItem> Items { get; set; }        
    }
    public class FundCommitmentRequestDocumentItem
    {
        [Display(Name = "line_no"), MaxLength(5), Required]
        public string LineNo { get; set; }
        [Display(Name = "closed2"), MaxLength(1), Required]
        public string Closed { get; set; }
        [Display(Name = "ref_doc_no"), MaxLength(11)]
        public string ReferenceDocumentNo { get; set; }
        [Display(Name = "ref_doc_line_item_no"), MaxLength(5)]
        public string ReferenceDocumentLineItemNo { get; set; }
        [Display(Name = "item_code"), MaxLength(10)]
        public string ItemCode { get; set; }
        [Display(Name = "item_description"), MaxLength(100)]
        public string ItemDescription { get; set; }
        [Display(Name = "part_category"), MaxLength(2)]
        public string PartCategory { get; set; }
        [Display(Name = "inventory_type"), MaxLength(20)]
        public string InventoryType { get; set; }
        [Display(Name = "material_type"), MaxLength(20)]
        public string MaterialType { get; set; }
        [Display(Name = "supplier_code"), MaxLength(10)]
        public string SupplierCode { get; set; }
        [Display(Name = "asset"), MaxLength(17)]
        public string Asset { get; set; }
        [Display(Name = "wbs_element"), MaxLength(24), Required]
        public string WbsElement { get; set; }
        [Display(Name = "cost_center_charger"), MaxLength(10), Required]
        public string CostCenterCharger { get; set; }
        [Display(Name = "total_amount"), Range(typeof(decimal), "-99999999999", "999999999999"), Required]
        public decimal TotalAmount { get; set; }
        [Display(Name = "quantity"), Range(typeof(decimal), "-9999999999999", "99999999999999")]
        public decimal Quantity { get; set; }
        [Display(Name = "uom"), MaxLength(3)]
        public string Uom { get; set; }
        [Display(Name = "journal_source"), MaxLength(40)]
        public string JournalSource { get; set; }
        [Display(Name = "gl_account"), MaxLength(10)]
        public string GlAccount { get; set; }
        [Display(Name = "submit_date"), Required]
        public DateTime SubmitDate { get; set; }
        [Display(AutoGenerateField = false)]
        public string Action { get; set; }
    }

}
