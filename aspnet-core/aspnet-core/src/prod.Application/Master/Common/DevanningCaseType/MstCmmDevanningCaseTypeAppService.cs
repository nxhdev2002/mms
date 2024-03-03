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
using prod.Master.Common.Dto;
using prod.Master.Common.Exporting;
using prod.HistoricalData;

namespace prod.Master.Common
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_Common_DevanningCaseType)]
    public class MstCmmDevanningCaseTypeAppService : prodAppServiceBase, IMstCmmDevanningCaseTypeAppService
    {
        private readonly IDapperRepository<MstCmmDevanningCaseType, long> _dapperRepo;
        private readonly IRepository<MstCmmDevanningCaseType, long> _repo;
        private readonly IMstCmmDevanningCaseTypeExcelExporter _calendarListExcelExporter;
        private readonly IHistoricalDataAppService _historicalDataAppService;

        public MstCmmDevanningCaseTypeAppService(IRepository<MstCmmDevanningCaseType,long> repo, 
                                         IDapperRepository<MstCmmDevanningCaseType,long> dapperRepo,
                                        IMstCmmDevanningCaseTypeExcelExporter calendarListExcelExporter,
                                        IHistoricalDataAppService historicalDataAppService
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
            _historicalDataAppService = historicalDataAppService;
        }

        public async Task<List<string>> GetMstCmmDevanningCaseTypeHistory(GetMstCmmDevanningCaseTypeHistoryInput input)
        {
            return await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
        }

        public async Task<FileDto> GetHistoricalDataToExcel(GetMstCmmDevanningCaseTypeHistoryExcelInput input)
        {
            var data = await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
            return _calendarListExcelExporter.ExportToHistoricalFile(data);
        }

        public async Task<List<long?>> GetChangedRecords()
        {
            return await _historicalDataAppService.GetChangedRecordIds("MstCmmDevanningCaseType");
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_Common_DevanningCaseType_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstCmmDevanningCaseTypeDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstCmmDevanningCaseTypeDto input)
        {          
            var mainObj = ObjectMapper.Map<MstCmmDevanningCaseType>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstCmmDevanningCaseTypeDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_Common_DevanningCaseType_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstCmmDevanningCaseTypeDto>> GetAll(GetMstCmmDevanningCaseTypeInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.CaseNo), e => e.CaseNo.Contains(input.CaseNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.CarFamilyCode), e => e.CarFamilyCode.Contains(input.CarFamilyCode))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ShoptypeCode), e => e.ShoptypeCode.Contains(input.ShoptypeCode))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierNo), e => e.SupplierNo.Contains(input.SupplierNo))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);
                                                                     

            var system = from o in pageAndFiltered
                         select new MstCmmDevanningCaseTypeDto
                        {
							Id = o.Id,
							CaseNo = o.CaseNo,
							CarFamilyCode = o.CarFamilyCode,
							ShoptypeCode = o.ShoptypeCode,
							SupplierNo = o.SupplierNo,
							IsActive = o.IsActive,
                        };

            var totalCount = await filtered.CountAsync();
			var paged = system.PageBy(input);

            return new PagedResultDto<MstCmmDevanningCaseTypeDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetDevanningCaseTypeToExcel(MstCmmDevanningCaseTypeExportInput input)
        {
            var query = from o in _repo.GetAll()
                        select new MstCmmDevanningCaseTypeDto
                        {
							Id = o.Id,
							CaseNo = o.CaseNo,
                            CarFamilyCode = o.CarFamilyCode,
							ShoptypeCode = o.ShoptypeCode,
							SupplierNo = o.SupplierNo,
							IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

       // public async Task GenerateAsync()
      //  {
        //    await _dapperRepo.ExecuteAsync(MstCmmDevanningCaseTypeConsts.SP_MST_WPT_CALENDAR_GENERATE);
       // }

    }
}
