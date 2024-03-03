using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Cmm.Dto
{

    public class MstCmmBusinessParterExportInput
    {

        public virtual string Nation { get; set; }

        public virtual string BusinessPartnerCategory { get; set; }

        public virtual string BusinessPartnerGroup { get; set; }

        public virtual string BusinessPartnerRole { get; set; }

        public virtual string BusinessPartnerCd { get; set; }

        public virtual string EmailAddress1 { get; set; }

        public virtual string SuppSearcgTerm { get; set; }

        public virtual string BusinessPartnerName1 { get; set; }

        public virtual string BusinessPartnerName2 { get; set; }

        public virtual string BusinessPartnerName3 { get; set; }

        public virtual string BusinessPartnerName4 { get; set; }

        public virtual string Address1 { get; set; }

        public virtual string Address2 { get; set; }

        public virtual string Address3 { get; set; }

        public virtual string District { get; set; }

        public virtual string City { get; set; }

        public virtual string PostalCd { get; set; }

        public virtual string Country { get; set; }

        public virtual string PhoneNo { get; set; }

        public virtual string FaxNo { get; set; }

        public virtual string TaxNo { get; set; }

        public virtual string TaxCate { get; set; }

        public virtual string CompanyCode { get; set; }

        public virtual string PaymentMethodCd { get; set; }

        public virtual string PaymentMethodNm { get; set; }

        public virtual string PaymentTermCd { get; set; }

        public virtual string PaymentTermNm { get; set; }

        public virtual string OrderCurrency { get; set; }

        public virtual string TypeOfIndustry { get; set; }

        public virtual string PreviousMasterRecordNumber { get; set; }

        public virtual string TextIdTitle { get; set; }

        public virtual string UniqueBankId { get; set; }

        public virtual string SuppBankKey { get; set; }

        public virtual string SuppBankCountry { get; set; }

        public virtual string SuppAccount { get; set; }

        public virtual string AccountHolder { get; set; }

        public virtual string Accname { get; set; }

        public virtual string PartnerBankName { get; set; }

        public virtual string ExternalId { get; set; }

        public virtual string StatusFlagAb { get; set; }

        public virtual string StatusFlagCb { get; set; }

        public virtual string StatusFlagAd { get; set; }

        public virtual string StatusFlagCd { get; set; }

    }

}

