using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.EntityFrameworkCore;
using prod.Master.Cmm.Dto;
using prod.Master.Cmm.Exporting;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.Cmm
{
    //[AbpAuthorize(AppPermissions.Pages_Master_Cmm_BusinessParter_View)]
    public class MstCmmBusinessParterAppService : prodAppServiceBase, IMstCmmBusinessParterAppService
    {
        private readonly IDapperRepository<MstCmmBusinessParter, long> _dapperRepo;
        private readonly IRepository<MstCmmBusinessParter, long> _repo;
        private readonly IMstCmmBusinessParterExcelExporter _calendarListExcelExporter;

        public MstCmmBusinessParterAppService(IRepository<MstCmmBusinessParter, long> repo,
                                         IDapperRepository<MstCmmBusinessParter, long> dapperRepo,
                                        IMstCmmBusinessParterExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

       // [AbpAuthorize(AppPermissions.Pages_Master_Cmm_BusinessParter_CreateEdit)]
        public async Task CreateOrEdit(CreateOrEditMstCmmBusinessParterDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstCmmBusinessParterDto input)
        {
            var mainObj = ObjectMapper.Map<MstCmmBusinessParter>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstCmmBusinessParterDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Master_Cmm_BusinessParter_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstCmmBusinessParterDto>> GetAll(GetMstCmmBusinessParterInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessPartnerCategory), e => e.BusinessPartnerCategory.Contains(input.BusinessPartnerCategory))
                .WhereIf(!string.IsNullOrWhiteSpace(input.City), e => e.City.Contains(input.City))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PhoneNo), e => e.PhoneNo.Contains(input.PhoneNo))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstCmmBusinessParterDto
                         {
                             Id = o.Id,
                             Nation = o.Nation,
                             BusinessPartnerCategory = o.BusinessPartnerCategory,
                             BusinessPartnerGroup = o.BusinessPartnerGroup,
                             BusinessPartnerRole = o.BusinessPartnerRole,
                             BusinessPartnerCd = o.BusinessPartnerCd,
                             EmailAddress1 = o.EmailAddress1,
                             SuppSearcgTerm = o.SuppSearcgTerm,
                             BusinessPartnerName1 = o.BusinessPartnerName1,
                             BusinessPartnerName2 = o.BusinessPartnerName2,
                             BusinessPartnerName3 = o.BusinessPartnerName3,
                             BusinessPartnerName4 = o.BusinessPartnerName4,
                             Address1 = o.Address1,
                             Address2 = o.Address2,
                             Address3 = o.Address3,
                             District = o.District,
                             City = o.City,
                             PostalCd = o.PostalCd,
                             Country = o.Country,
                             PhoneNo = o.PhoneNo,
                             FaxNo = o.FaxNo,
                             TaxNo = o.TaxNo,
                             TaxCate = o.TaxCate,
                             CompanyCode = o.CompanyCode,
                             PaymentMethodCd = o.PaymentMethodCd,
                             PaymentMethodNm = o.PaymentMethodNm,
                             PaymentTermCd = o.PaymentTermCd,
                             PaymentTermNm = o.PaymentTermNm,
                             OrderCurrency = o.OrderCurrency,
                             TypeOfIndustry = o.TypeOfIndustry,
                             PreviousMasterRecordNumber = o.PreviousMasterRecordNumber,
                             TextIdTitle = o.TextIdTitle,
                             UniqueBankId = o.UniqueBankId,
                             SuppBankKey = o.SuppBankKey,
                             SuppBankCountry = o.SuppBankCountry,
                             SuppAccount = o.SuppAccount,
                             AccountHolder = o.AccountHolder,
                             Accname = o.Accname,
                             PartnerBankName = o.PartnerBankName,
                             ExternalId = o.ExternalId,
                             StatusFlagAb = o.StatusFlagAb,
                             StatusFlagCb = o.StatusFlagCb,
                             StatusFlagAd = o.StatusFlagAd,
                             StatusFlagCd = o.StatusFlagCd,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstCmmBusinessParterDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        /*public async Task<FileDto> GetBusinessParterToExcel(MstCmmBusinessParterExportInput input)
        {
            var query = from o in _repo.GetAll()
                        select new MstCmmBusinessParterDto
                        {
                            Id = o.Id,
                            Nation = o.Nation,
                            BusinessPartnerCategory = o.BusinessPartnerCategory,
                            BusinessPartnerGroup = o.BusinessPartnerGroup,
                            BusinessPartnerRole = o.BusinessPartnerRole,
                            BusinessPartnerCd = o.BusinessPartnerCd,
                            EmailAddress1 = o.EmailAddress1,
                            SuppSearcgTerm = o.SuppSearcgTerm,
                            BusinessPartnerName1 = o.BusinessPartnerName1,
                            BusinessPartnerName2 = o.BusinessPartnerName2,
                            BusinessPartnerName3 = o.BusinessPartnerName3,
                            BusinessPartnerName4 = o.BusinessPartnerName4,
                            Address1 = o.Address1,
                            Address2 = o.Address2,
                            Address3 = o.Address3,
                            District = o.District,
                            City = o.City,
                            PostalCd = o.PostalCd,
                            Country = o.Country,
                            PhoneNo = o.PhoneNo,
                            FaxNo = o.FaxNo,
                            TaxNo = o.TaxNo,
                            TaxCate = o.TaxCate,
                            CompanyCode = o.CompanyCode,
                            PaymentMethodCd = o.PaymentMethodCd,
                            PaymentMethodNm = o.PaymentMethodNm,
                            PaymentTermCd = o.PaymentTermCd,
                            PaymentTermNm = o.PaymentTermNm,
                            OrderCurrency = o.OrderCurrency,
                            TypeOfIndustry = o.TypeOfIndustry,
                            PreviousMasterRecordNumber = o.PreviousMasterRecordNumber,
                            TextIdTitle = o.TextIdTitle,
                            UniqueBankId = o.UniqueBankId,
                            SuppBankKey = o.SuppBankKey,
                            SuppBankCountry = o.SuppBankCountry,
                            SuppAccount = o.SuppAccount,
                            AccountHolder = o.AccountHolder,
                            Accname = o.Accname,
                            PartnerBankName = o.PartnerBankName,
                            ExternalId = o.ExternalId,
                            StatusFlagAb = o.StatusFlagAb,
                            StatusFlagCb = o.StatusFlagCb,
                            StatusFlagAd = o.StatusFlagAd,
                            StatusFlagCd = o.StatusFlagCd,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }*/

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstCmmBusinessParterConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
