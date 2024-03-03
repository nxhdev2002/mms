using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Cmm
{
    [Table("MstCmmBusinessParter")]
    public class MstCmmBusinessParter : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxNationLength = 1;

        public const int MaxBusinessPartnerCategoryLength = 1;

        public const int MaxBusinessPartnerGroupLength = 4;

        public const int MaxBusinessPartnerRoleLength = 6;

        public const int MaxBusinessPartnerCdLength = 10;

        public const int MaxEmailAddress1Length = 241;

        public const int MaxSuppSearcgTermLength = 20;

        public const int MaxBusinessPartnerName1Length = 35;

        public const int MaxBusinessPartnerName2Length = 35;

        public const int MaxBusinessPartnerName3Length = 35;

        public const int MaxBusinessPartnerName4Length = 35;

        public const int MaxAddress1Length = 35;

        public const int MaxAddress2Length = 35;

        public const int MaxAddress3Length = 35;

        public const int MaxDistrictLength = 40;

        public const int MaxCityLength = 40;

        public const int MaxPostalCdLength = 10;

        public const int MaxCountryLength = 3;

        public const int MaxPhoneNoLength = 30;

        public const int MaxFaxNoLength = 30;

        public const int MaxTaxNoLength = 20;

        public const int MaxTaxCateLength = 4;

        public const int MaxCompanyCodeLength = 4;

        public const int MaxPaymentMethodCdLength = 10;

        public const int MaxPaymentMethodNmLength = 30;

        public const int MaxPaymentTermCdLength = 4;

        public const int MaxPaymentTermNmLength = 50;

        public const int MaxOrderCurrencyLength = 3;

        public const int MaxTypeOfIndustryLength = 30;

        public const int MaxPreviousMasterRecordNumberLength = 10;

        public const int MaxTextIdTitleLength = 40;

        public const int MaxUniqueBankIdLength = 4;

        public const int MaxSuppBankKeyLength = 15;

        public const int MaxSuppBankCountryLength = 3;

        public const int MaxSuppAccountLength = 18;

        public const int MaxAccountHolderLength = 60;

        public const int MaxAccnameLength = 40;

        public const int MaxPartnerBankNameLength = 30;

        public const int MaxExternalIdLength = 10;

        public const int MaxStatusFlagAbLength = 1;

        public const int MaxStatusFlagCbLength = 1;

        public const int MaxStatusFlagAdLength = 1;

        public const int MaxStatusFlagCdLength = 1;

        [StringLength(MaxNationLength)]
        public virtual string Nation { get; set; }

        [StringLength(MaxBusinessPartnerCategoryLength)]
        public virtual string BusinessPartnerCategory { get; set; }

        [StringLength(MaxBusinessPartnerGroupLength)]
        public virtual string BusinessPartnerGroup { get; set; }

        [StringLength(MaxBusinessPartnerRoleLength)]
        public virtual string BusinessPartnerRole { get; set; }

        [StringLength(MaxBusinessPartnerCdLength)]
        public virtual string BusinessPartnerCd { get; set; }

        [StringLength(MaxEmailAddress1Length)]
        public virtual string EmailAddress1 { get; set; }

        [StringLength(MaxSuppSearcgTermLength)]
        public virtual string SuppSearcgTerm { get; set; }

        [StringLength(MaxBusinessPartnerName1Length)]
        public virtual string BusinessPartnerName1 { get; set; }

        [StringLength(MaxBusinessPartnerName2Length)]
        public virtual string BusinessPartnerName2 { get; set; }

        [StringLength(MaxBusinessPartnerName3Length)]
        public virtual string BusinessPartnerName3 { get; set; }

        [StringLength(MaxBusinessPartnerName4Length)]
        public virtual string BusinessPartnerName4 { get; set; }

        [StringLength(MaxAddress1Length)]
        public virtual string Address1 { get; set; }

        [StringLength(MaxAddress2Length)]
        public virtual string Address2 { get; set; }

        [StringLength(MaxAddress3Length)]
        public virtual string Address3 { get; set; }

        [StringLength(MaxDistrictLength)]
        public virtual string District { get; set; }

        [StringLength(MaxCityLength)]
        public virtual string City { get; set; }

        [StringLength(MaxPostalCdLength)]
        public virtual string PostalCd { get; set; }

        [StringLength(MaxCountryLength)]
        public virtual string Country { get; set; }

        [StringLength(MaxPhoneNoLength)]
        public virtual string PhoneNo { get; set; }

        [StringLength(MaxFaxNoLength)]
        public virtual string FaxNo { get; set; }

        [StringLength(MaxTaxNoLength)]
        public virtual string TaxNo { get; set; }

        [StringLength(MaxTaxCateLength)]
        public virtual string TaxCate { get; set; }

        [StringLength(MaxCompanyCodeLength)]
        public virtual string CompanyCode { get; set; }

        [StringLength(MaxPaymentMethodCdLength)]
        public virtual string PaymentMethodCd { get; set; }

        [StringLength(MaxPaymentMethodNmLength)]
        public virtual string PaymentMethodNm { get; set; }

        [StringLength(MaxPaymentTermCdLength)]
        public virtual string PaymentTermCd { get; set; }

        [StringLength(MaxPaymentTermNmLength)]
        public virtual string PaymentTermNm { get; set; }

        [StringLength(MaxOrderCurrencyLength)]
        public virtual string OrderCurrency { get; set; }

        [StringLength(MaxTypeOfIndustryLength)]
        public virtual string TypeOfIndustry { get; set; }

        [StringLength(MaxPreviousMasterRecordNumberLength)]
        public virtual string PreviousMasterRecordNumber { get; set; }

        [StringLength(MaxTextIdTitleLength)]
        public virtual string TextIdTitle { get; set; }

        [StringLength(MaxUniqueBankIdLength)]
        public virtual string UniqueBankId { get; set; }

        [StringLength(MaxSuppBankKeyLength)]
        public virtual string SuppBankKey { get; set; }

        [StringLength(MaxSuppBankCountryLength)]
        public virtual string SuppBankCountry { get; set; }

        [StringLength(MaxSuppAccountLength)]
        public virtual string SuppAccount { get; set; }

        [StringLength(MaxAccountHolderLength)]
        public virtual string AccountHolder { get; set; }

        [StringLength(MaxAccnameLength)]
        public virtual string Accname { get; set; }

        [StringLength(MaxPartnerBankNameLength)]
        public virtual string PartnerBankName { get; set; }

        [StringLength(MaxExternalIdLength)]
        public virtual string ExternalId { get; set; }

        [StringLength(MaxStatusFlagAbLength)]
        public virtual string StatusFlagAb { get; set; }

        [StringLength(MaxStatusFlagCbLength)]
        public virtual string StatusFlagCb { get; set; }

        [StringLength(MaxStatusFlagAdLength)]
        public virtual string StatusFlagAd { get; set; }

        [StringLength(MaxStatusFlagCdLength)]
        public virtual string StatusFlagCd { get; set; }
    }

}

