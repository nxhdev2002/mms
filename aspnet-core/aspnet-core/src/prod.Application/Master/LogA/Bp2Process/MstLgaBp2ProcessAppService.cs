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
using prod.Master.LogA.Bp2Process.ImportDto;
using prod.Master.LogA.Dto;
using prod.Master.LogA.Exporting;

namespace prod.Master.LogA
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_Bp2Process)]
    public class MstLgaBp2ProcessAppService : prodAppServiceBase, IMstLgaBp2ProcessAppService
    {
        private readonly IDapperRepository<MstLgaBp2Process, long> _dapperRepo;
        private readonly IRepository<MstLgaBp2Process, long> _repo;
        private readonly IMstLgaBp2ProcessExcelExporter _calendarListExcelExporter;

        public MstLgaBp2ProcessAppService(IRepository<MstLgaBp2Process, long> repo,
                                         IDapperRepository<MstLgaBp2Process, long> dapperRepo,
                                        IMstLgaBp2ProcessExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_Bp2Process_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstLgaBp2ProcessDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstLgaBp2ProcessDto input)
        {
            var mainObj = ObjectMapper.Map<MstLgaBp2Process>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstLgaBp2ProcessDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_Bp2Process_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstLgaBp2ProcessDto>> GetAll(GetMstLgaBp2ProcessInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProcessName), e => e.ProcessName.Contains(input.ProcessName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine));

            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstLgaBp2ProcessDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             ProcessName = o.ProcessName,
                             ProdLine = o.ProdLine,
                             LeadTime = o.LeadTime,
                             Sorting = o.Sorting,
                             ProcessType = o.ProcessType,
                             IsActive = o.IsActive
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstLgaBp2ProcessDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetBp2ProcessToExcel(GetMstLgaBp2ProcessExcelInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProcessName), e => e.ProcessName.Contains(input.ProcessName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine));

            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var query = from o in pageAndFiltered
                         select new MstLgaBp2ProcessDto
                        {
                            Id = o.Id,
                            Code = o.Code,
                            ProcessName = o.ProcessName,
                            ProdLine = o.ProdLine,
                            LeadTime = o.LeadTime,
                            Sorting = o.Sorting,
                            ProcessType = o.ProcessType,
                            IsActive = o.IsActive
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        public async Task<List<ImportMstLgaBp2ProcessDto>> ImportMstLgaBp2ProcessFromExcel(List<ImportMstLgaBp2ProcessDto> MstLgaBp2ProcessS)
        {
            try
            {
                List<MstLgaBp2Process_T> MstLgaBp2Process = new List<MstLgaBp2Process_T> { };
                foreach (var item in MstLgaBp2ProcessS)
                {
                    MstLgaBp2Process_T importData = new MstLgaBp2Process_T();
                    {
                        importData.Guid = item.Guid;
                        importData.Code = item.Code;
                        importData.ProdLine = item.ProdLine;
                        importData.Sorting = item.Sorting;
                    }
                    MstLgaBp2Process.Add(importData);
                }
                CurrentUnitOfWork.GetDbContext<prodDbContext>().AddRangeAsync(MstLgaBp2Process);
                return null;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }

        //Merge Data From Temp To ContDevanningLT
        public async Task MergeDataMstLgaBp2Process(string v_Guid)
        {
            //string _sql = "Exec MST_LGW_CONT_DEVANNING_LT_MERGE @Guid";
            //await _dapperRepo.QueryAsync<MstLgaBp2Process>(_sql, new { Guid = v_Guid });
        }

    }
}
