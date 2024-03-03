using System.Collections.Generic;

using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Cmm.Dto;
using prod.Master.Cmm.Exporting;
using prod.Storage;

namespace vovina.Master.Cmm.Exporting
{
    public class MstCmmBusinessParterExcelExporter : NpoiExcelExporterBase, IMstCmmBusinessParterExcelExporter
    {
        public MstCmmBusinessParterExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstCmmBusinessParterDto> businessparter)
        {
            return CreateExcelPackage(
                "MasterCmmBusinessParter.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("BusinessParter");
                    AddHeader(
                                sheet,
                                ("Nation"),
                                ("BusinessPartnerCategory"),
                                ("BusinessPartnerGroup"),
                                ("BusinessPartnerRole"),
                                ("BusinessPartnerCd"),
                                ("EmailAddress1"),
                                ("SuppSearcgTerm"),
                                ("BusinessPartnerName1"),
                                ("BusinessPartnerName2"),
                                ("BusinessPartnerName3"),
                                ("BusinessPartnerName4"),
                                ("Address1"),
                                ("Address2"),
                                ("Address3"),
                                ("District"),
                                ("City"),
                                ("PostalCd"),
                                ("Country"),
                                ("PhoneNo"),
                                ("FaxNo"),
                                ("TaxNo"),
                                ("TaxCate"),
                                ("CompanyCode"),
                                ("PaymentMethodCd"),
                                ("PaymentMethodNm"),
                                ("PaymentTermCd"),
                                ("PaymentTermNm"),
                                ("OrderCurrency"),
                                ("TypeOfIndustry"),
                                ("PreviousMasterRecordNumber"),
                                ("TextIdTitle"),
                                ("UniqueBankId"),
                                ("SuppBankKey"),
                                ("SuppBankCountry"),
                                ("SuppAccount"),
                                ("AccountHolder"),
                                ("Accname"),
                                ("PartnerBankName"),
                                ("ExternalId"),
                                ("StatusFlagAb"),
                                ("StatusFlagCb"),
                                ("StatusFlagAd"),
                                ("StatusFlagCd")

                               );

                    AddObjects(
                         sheet, businessparter,
                                _ => _.Nation,
                                _ => _.BusinessPartnerCategory,
                                _ => _.BusinessPartnerGroup,
                                _ => _.BusinessPartnerRole,
                                _ => _.BusinessPartnerCd,
                                _ => _.EmailAddress1,
                                _ => _.SuppSearcgTerm,
                                _ => _.BusinessPartnerName1,
                                _ => _.BusinessPartnerName2,
                                _ => _.BusinessPartnerName3,
                                _ => _.BusinessPartnerName4,
                                _ => _.Address1,
                                _ => _.Address2,
                                _ => _.Address3,
                                _ => _.District,
                                _ => _.City,
                                _ => _.PostalCd,
                                _ => _.Country,
                                _ => _.PhoneNo,
                                _ => _.FaxNo,
                                _ => _.TaxNo,
                                _ => _.TaxCate,
                                _ => _.CompanyCode,
                                _ => _.PaymentMethodCd,
                                _ => _.PaymentMethodNm,
                                _ => _.PaymentTermCd,
                                _ => _.PaymentTermNm,
                                _ => _.OrderCurrency,
                                _ => _.TypeOfIndustry,
                                _ => _.PreviousMasterRecordNumber,
                                _ => _.TextIdTitle,
                                _ => _.UniqueBankId,
                                _ => _.SuppBankKey,
                                _ => _.SuppBankCountry,
                                _ => _.SuppAccount,
                                _ => _.AccountHolder,
                                _ => _.Accname,
                                _ => _.PartnerBankName,
                                _ => _.ExternalId,
                                _ => _.StatusFlagAb,
                                _ => _.StatusFlagCb,
                                _ => _.StatusFlagAd,
                                _ => _.StatusFlagCd

                                );

                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });

        }
    }
}