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
using prod.Master.LogW.Dto;
using prod.Master.LogW.Exporting;
using prod.Master.LogW;

namespace prod.Master.LogW
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_LogW_ContDevanningLT)]
    public class MstLgwContDevanningLTAppService : prodAppServiceBase, IMstLgwContDevanningLTAppService
    {
        private readonly IDapperRepository<MstLgwContDevanningLT, long> _dapperRepo;
        private readonly IRepository<MstLgwContDevanningLT, long> _repo;
        private readonly IMstLgwContDevanningLTExcelExporter _calendarListExcelExporter;

        public MstLgwContDevanningLTAppService(IRepository<MstLgwContDevanningLT, long> repo,
                                         IDapperRepository<MstLgwContDevanningLT, long> dapperRepo,
                                        IMstLgwContDevanningLTExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogW_ContDevanningLT_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstLgwContDevanningLTDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstLgwContDevanningLTDto input)
        {
            var mainObj = ObjectMapper.Map<MstLgwContDevanningLT>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstLgwContDevanningLTDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogW_ContDevanningLT_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstLgwContDevanningLTDto>> GetAll(GetMstLgwContDevanningLTInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.RenbanCode), e => e.RenbanCode.Contains(input.RenbanCode))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Source), e => e.Source.Contains(input.Source))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstLgwContDevanningLTDto
                         {
                             Id = o.Id,
                             RenbanCode = o.RenbanCode,
                             Source = o.Source,
                             DevLeadtime = o.DevLeadtime,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstLgwContDevanningLTDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetContDevanningLTToExcel(MstLgwContDevanningLTExportInput input)
        {
            var filtered = _repo.GetAll()
                   .WhereIf(!string.IsNullOrWhiteSpace(input.RenbanCode), e => e.RenbanCode.Contains(input.RenbanCode))
                   .WhereIf(!string.IsNullOrWhiteSpace(input.Source), e => e.Source.Contains(input.Source))
                   ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var query = from o in pageAndFiltered
                         select new MstLgwContDevanningLTDto
                        {
                            Id = o.Id,
                            RenbanCode = o.RenbanCode,
                            Source = o.Source,
                            DevLeadtime = o.DevLeadtime,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }
        //Import Data From Excel
        public async Task<List<ImportContDevanningLTDto>> ImportContDevanningLTFromExcel(List<ImportContDevanningLTDto> LotUpPlans)
        {
            try
            {
                List<MstLgwContDevanningLT_T> LotUpPlan = new List<MstLgwContDevanningLT_T> { };
                foreach (var item in LotUpPlans)
                {
                    MstLgwContDevanningLT_T importData = new MstLgwContDevanningLT_T();
                    {
                        importData.Guid = item.Guid;
                        importData.RenbanCode = item.RenbanCode;
                        importData.Source = item.Source;
                        importData.DevLeadtime = item.DevLeadtime;                    
                    }
                    LotUpPlan.Add(importData);
                }
                CurrentUnitOfWork.GetDbContext<prodDbContext>().AddRangeAsync(LotUpPlan);
                return null;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }

        //Merge Data From Temp To ContDevanningLT
        public async Task MergeDataContDevanningLT(string v_Guid)
        {
            string _sql = "Exec MST_LGW_CONT_DEVANNING_LT_MERGE @Guid";
            await _dapperRepo.QueryAsync<MstLgwContDevanningLT>(_sql, new { Guid = v_Guid });
        }

        //get Data Screen


    }


}
