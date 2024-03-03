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
using Abp.Runtime.Caching.Redis;
using StackExchange.Redis;
using prod.Common;
using prod.HistoricalData;

namespace prod.Master.Common
{
    [AbpAuthorize(AppPermissions.Pages_Master_Common_Lookup_View)]
    public class MstCmmLookupAppService : prodAppServiceBase, IMstCmmLookupAppService
    {
        private readonly IDapperRepository<MstCmmLookup, long> _dapperRepo;
        private readonly IRepository<MstCmmLookup, long> _repo;
        private readonly IMstCmmLookupExcelExporter _calendarListExcelExporter;
        private readonly IAbpRedisCacheDatabaseProvider _cacheService;
        private readonly IDatabase _redisDb;
        private readonly IHistoricalDataAppService _historicalDataAppService;

        public MstCmmLookupAppService(IRepository<MstCmmLookup, long> repo,
                                      IDapperRepository<MstCmmLookup, long> dapperRepo, 
                                      IMstCmmLookupExcelExporter calendarListExcelExporter,
                                      IAbpRedisCacheDatabaseProvider cacheService,
                                      IHistoricalDataAppService historicalDataAppService
        )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
            _cacheService = cacheService;
            _redisDb = _cacheService.GetDatabase();
            _historicalDataAppService = historicalDataAppService;
        }
        public async Task<List<string>> GetMstCmmLookupHistory(GetMstCmmLookupHistoryInput input)
        {
            return await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
        }

        public async Task<FileDto> GetHistoricalDataToExcel(GetMstCmmLookupHistoryExcelInput input)
        {
            var data = await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
            return _calendarListExcelExporter.ExportToHistoricalFile(data);
        }

        public async Task<List<long?>> GetChangedRecords()
        {
            return await _historicalDataAppService.GetChangedRecordIds("MstCmmLookup");
        }

        [AbpAuthorize(AppPermissions.Pages_Master_Common_Lookup_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstCmmLookupDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstCmmLookupDto input)
        {
            var mainObj = ObjectMapper.Map<MstCmmLookup>(input); 
            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
            await _redisDb.KeyDeleteAsync(input.DomainCode);
        }

        // EDIT
        private async Task Update(CreateOrEditMstCmmLookupDto input)
        {
            await _redisDb.KeyDeleteAsync(input.DomainCode);
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                MstCmmLookup mainObj = await _repo.FirstOrDefaultAsync(e => e.Id == input.Id);
                mainObj.DomainCode = input.DomainCode;
                mainObj.ItemCode = input.ItemCode;
                mainObj.ItemValue = input.ItemValue;
                mainObj.ItemOrder = input.ItemOrder;
                mainObj.Description = input.Description;
                mainObj.IsUse = input.IsUse;
                mainObj.IsRestrict = input.IsRestrict;
                await _repo.UpdateAsync(mainObj);
                
            }
        }

        // SAVE ALL
        [AbpAuthorize(AppPermissions.Pages_Master_Common_Lookup_Edit)]
        public async Task SaveAll(List<CreateOrEditMstCmmLookupDto>  listData)
        {           
            foreach (var item in listData)
            {
                if (item.Id == null) {
                    var mainObj = ObjectMapper.Map<MstCmmLookup>(item);
                    await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
                }
                else
                { 
                    MstCmmLookup mainObj = await _repo.FirstOrDefaultAsync(e => e.Id == item.Id);
                        mainObj.DomainCode = item.DomainCode;
                        mainObj.ItemCode = item.ItemCode;
                        mainObj.ItemValue = item.ItemValue;
                        mainObj.ItemOrder = item.ItemOrder;
                        mainObj.Description = item.Description; 
                        mainObj.IsUse = item.IsUse;
                        mainObj.IsRestrict = item.IsRestrict;

                    await _repo.UpdateAsync(mainObj);
                }
            } 
        }


        public async Task<List<long>> GetALLMstCmmLookupById()
        {
            var a = await _dapperRepo.QueryAsync<MstCmmLookup>(@"select id from MstCmmLookup");
            return a.Select(e => e.Id).ToList();
        }


        [AbpAuthorize(AppPermissions.Pages_Master_Common_Lookup_Edit)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstCmmLookupDto>> GetAll(GetMstCmmLookupInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.DomainCode), e => e.DomainCode.Contains(input.DomainCode))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ItemCode), e => e.ItemCode.Contains(input.ItemCode))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ItemValue), e => e.ItemValue.Contains(input.ItemValue))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Description), e => e.Description.Contains(input.Description))
                .WhereIf(!string.IsNullOrWhiteSpace(input.IsUse), e => e.IsUse.Contains(input.IsUse))
                .WhereIf(!string.IsNullOrWhiteSpace(input.IsRestrict), e => e.IsRestrict.Contains(input.IsRestrict))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstCmmLookupDto
                         {
                             Id = o.Id,
                             DomainCode = o.DomainCode,
                             ItemCode = o.ItemCode,
                             ItemValue = o.ItemValue,
                             ItemOrder = o.ItemOrder,
                             Description = o.Description,
                             IsUse = o.IsUse,
                             IsRestrict = o.IsRestrict,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstCmmLookupDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<List<MstCmmLookupDto>> GetsByDomainCode(string DomainCode)
        {

            string token = await _redisDb.StringGetAsync(DomainCode);
            if (token == null)
            {
                var expires = TimeSpan.FromMinutes(10);
                string _sql = "Exec MST_CMM_LOOKUP_GET_BY_DOMAINCODE @DomainCode";

                var results = await _dapperRepo.QueryAsync<MstCmmLookupDto>(_sql, new
                {
                    DomainCode = DomainCode,
                });

                await _redisDb.StringSetAsync(DomainCode, Newtonsoft.Json.JsonConvert.SerializeObject(results.ToList()), expires);
                return results.ToList();
            }
            else
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<MstCmmLookupDto>>(token);
            }
        }

        public async Task<FileDto> GetLookupToExcel(MstCmmLookupExportInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.DomainCode), e => e.DomainCode.Contains(input.DomainCode))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ItemCode), e => e.ItemCode.Contains(input.ItemCode))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ItemValue), e => e.ItemValue.Contains(input.ItemValue))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Description), e => e.Description.Contains(input.Description))
                .WhereIf(!string.IsNullOrWhiteSpace(input.IsUse), e => e.IsUse.Contains(input.IsUse))
                .WhereIf(!string.IsNullOrWhiteSpace(input.IsRestrict), e => e.IsRestrict.Contains(input.IsRestrict));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var query = from o in pageAndFiltered
                         select new MstCmmLookupDto
                        {
                            Id = o.Id,
                            DomainCode = o.DomainCode,
                            ItemCode = o.ItemCode,
                            ItemValue = o.ItemValue,
                            ItemOrder = o.ItemOrder,
                            Description = o.Description,
                            IsUse = o.IsUse,
                            IsRestrict = o.IsRestrict,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstCmmLookupConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
