using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Dapper;
using Microsoft.EntityFrameworkCore;
using NUglify.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Master.Spp.Dto;
using prod.Inventory.SPP.Cost.Dto;
using prod.Master.Spp.Exporting;

namespace prod.Master.Spp
{
    [AbpAuthorize(AppPermissions.Pages_SPP_Master_Customer)]
    public class MstSppCustomerAppService : prodAppServiceBase, IMstSppCustomerAppService
    {
        private readonly IDapperRepository<MstSppCustomer, long> _dapperRepo;
        private readonly IRepository<MstSppCustomer, long> _repo;
        private readonly IMstSppCustomerExcelExporter _calendarListExcelExporter;

        public MstSppCustomerAppService(IRepository<MstSppCustomer, long> repo,
                                         IDapperRepository<MstSppCustomer, long> dapperRepo,
                                         IMstSppCustomerExcelExporter calendarListExcelExporter)
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        /*[AbpAuthorize(AppPermissions.Pages_Master_Spp_Customer_CreateEdit)]
        public async Task CreateOrEdit(CreateOrEditMstSppCustomerDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstSppCustomerDto input)
        {
            var mainObj = ObjectMapper.Map<MstSppCustomer>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstSppCustomerDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Master_Spp_Customer_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }
*/
        /*public async Task<PagedResultDto<MstSppCustomerDto>> GetAll(GetMstSppCustomerInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code));
                string _ProdMonth = (input.ProdMonth.ToString("dd-MM-yyyy") != "01-01-0001") ? input.ProdMonth.ToString("yyyy-MM") : "";
            ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstSppCustomerDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Name = o.Name,
                             Rep = o.Rep,
                             FromMonth = o.FromMonth,
*//*                             FromYear = o.FromYear,
                             ToMonth = o.ToMonth,*/
/*                             ToYear = o.ToYear,*/
/*                             FromPeriodId = o.FromPeriodId,
                             ToPeriodId = o.ToPeriodId,*/
/*                             IsNew = o.IsNew,*/
/*                             OraCustomerId = o.OraCustomerId,*//*
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstSppCustomerDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }*/

        public async Task<PagedResultDto<MstSppCustomerDto>> GetAll(GetMstSppCustomerInput input)
        {
            string _sql = "Exec MASTER_SPP_CUSTOMER_SEARCH @p_code, @p_frommonthyear, @p_tomonthyear";

            IEnumerable<MstSppCustomerDto> result = await _dapperRepo.QueryAsync<MstSppCustomerDto>(_sql, new
            {
                p_code = input.Code,
                p_frommonthyear = input.FromMonthYear,
                p_tomonthyear = input.ToMonthYear
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<MstSppCustomerDto>(
                totalCount,
                pagedAndFiltered);
        }
        public async Task<FileDto> GetCustomerToExcel(MstSppCustomerExportInput input)
        {
            string _sql = "Exec MASTER_SPP_CUSTOMER_SEARCH @p_code, @p_frommonthyear, @p_tomonthyear";

            IEnumerable<MstSppCustomerDto> result = await _dapperRepo.QueryAsync<MstSppCustomerDto>(_sql, new
            {
                p_code = input.Code,
                p_frommonthyear = input.FromMonthYear,
                p_tomonthyear = input.ToMonthYear
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }
        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstSppCustomerConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
