using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Cmm.Dto
{

    public class MstCmmBusinessParterDto : EntityDto<long?>
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

    public class CreateOrEditMstCmmBusinessParterDto : EntityDto<long?>
    {

        [StringLength(MstCmmBusinessParterConsts.MaxNationLength)]
        public virtual string Nation { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxBusinessPartnerCategoryLength)]
        public virtual string BusinessPartnerCategory { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxBusinessPartnerGroupLength)]
        public virtual string BusinessPartnerGroup { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxBusinessPartnerRoleLength)]
        public virtual string BusinessPartnerRole { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxBusinessPartnerCdLength)]
        public virtual string BusinessPartnerCd { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxEmailAddress1Length)]
        public virtual string EmailAddress1 { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxSuppSearcgTermLength)]
        public virtual string SuppSearcgTerm { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxBusinessPartnerName1Length)]
        public virtual string BusinessPartnerName1 { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxBusinessPartnerName2Length)]
        public virtual string BusinessPartnerName2 { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxBusinessPartnerName3Length)]
        public virtual string BusinessPartnerName3 { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxBusinessPartnerName4Length)]
        public virtual string BusinessPartnerName4 { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxAddress1Length)]
        public virtual string Address1 { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxAddress2Length)]
        public virtual string Address2 { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxAddress3Length)]
        public virtual string Address3 { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxDistrictLength)]
        public virtual string District { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxCityLength)]
        public virtual string City { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxPostalCdLength)]
        public virtual string PostalCd { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxCountryLength)]
        public virtual string Country { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxPhoneNoLength)]
        public virtual string PhoneNo { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxFaxNoLength)]
        public virtual string FaxNo { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxTaxNoLength)]
        public virtual string TaxNo { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxTaxCateLength)]
        public virtual string TaxCate { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxCompanyCodeLength)]
        public virtual string CompanyCode { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxPaymentMethodCdLength)]
        public virtual string PaymentMethodCd { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxPaymentMethodNmLength)]
        public virtual string PaymentMethodNm { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxPaymentTermCdLength)]
        public virtual string PaymentTermCd { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxPaymentTermNmLength)]
        public virtual string PaymentTermNm { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxOrderCurrencyLength)]
        public virtual string OrderCurrency { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxTypeOfIndustryLength)]
        public virtual string TypeOfIndustry { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxPreviousMasterRecordNumberLength)]
        public virtual string PreviousMasterRecordNumber { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxTextIdTitleLength)]
        public virtual string TextIdTitle { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxUniqueBankIdLength)]
        public virtual string UniqueBankId { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxSuppBankKeyLength)]
        public virtual string SuppBankKey { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxSuppBankCountryLength)]
        public virtual string SuppBankCountry { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxSuppAccountLength)]
        public virtual string SuppAccount { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxAccountHolderLength)]
        public virtual string AccountHolder { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxAccnameLength)]
        public virtual string Accname { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxPartnerBankNameLength)]
        public virtual string PartnerBankName { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxExternalIdLength)]
        public virtual string ExternalId { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxStatusFlagAbLength)]
        public virtual string StatusFlagAb { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxStatusFlagCbLength)]
        public virtual string StatusFlagCb { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxStatusFlagAdLength)]
        public virtual string StatusFlagAd { get; set; }

        [StringLength(MstCmmBusinessParterConsts.MaxStatusFlagCdLength)]
        public virtual string StatusFlagCd { get; set; }
    }

    public class GetMstCmmBusinessParterInput : PagedAndSortedResultRequestDto
    {
        public virtual string BusinessPartnerCategory { get; set; }
        public virtual string City { get; set; }
        public virtual string PhoneNo { get; set; }
    }

}
