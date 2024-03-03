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
using prod.LogW.Pup.Dto;
using prod.LogW.Pup.Exporting;
using prod.LogW.Pup.ImportDto;

namespace prod.LogW.Pup
{
    //  [AbpAuthorize(AppPermissions.Pages_LogW_Pup_PxPUpPlanBase)]
    public class LgwPupPxPUpPlanBaseAppService : prodAppServiceBase, ILgwPupPxPUpPlanBaseAppService
    {
        private readonly IDapperRepository<LgwPupPxPUpPlanBase, long> _dapperRepo;
        private readonly IRepository<LgwPupPxPUpPlanBase, long> _repo;
        private readonly ILgwPupPxPUpPlanBaseExcelExporter _calendarListExcelExporter;

        public LgwPupPxPUpPlanBaseAppService(IRepository<LgwPupPxPUpPlanBase, long> repo,
                                         IDapperRepository<LgwPupPxPUpPlanBase, long> dapperRepo,
                                        ILgwPupPxPUpPlanBaseExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_LogW_Pup_PxPUpPlanBase_Edit)]
        public async Task CreateOrEdit(CreateOrEditLgwPupPxPUpPlanBaseDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditLgwPupPxPUpPlanBaseDto input)
        {
            var mainObj = ObjectMapper.Map<LgwPupPxPUpPlanBase>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditLgwPupPxPUpPlanBaseDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_LogW_Pup_PxPUpPlanBase_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<LgwPupPxPUpPlanBaseDto>> GetAll(GetLgwPupPxPUpPlanBaseInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;
           
            var filtered = _repo.GetAll()
                .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
                .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom.Value.Date <= t.WorkingDate)
                .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo.Value.Date.AddDays(1) > t.WorkingDate);

            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate).ThenBy(a => a.ProdLine);

            var system = from o in pageAndFiltered
                         select new LgwPupPxPUpPlanBaseDto
                         {
                             Id = o.Id,
                             WorkingDate = o.WorkingDate,
                             ProdLine = o.ProdLine,
                             Shift1 = o.Shift1,
                             Shift2 = o.Shift2,
                             Shift3 = o.Shift3,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<LgwPupPxPUpPlanBaseDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetPxPUpPlanBaseToExcel(GetLgwPupPxPUpPlanBaseExportInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;
           
            var filtered = _repo.GetAll()
                .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
                .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom.Value.Date <= t.WorkingDate)
                .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo.Value.Date.AddDays(1) > t.WorkingDate);

            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate).ThenBy(a => a.ProdLine);

            var query = from o in pageAndFiltered
                        select new LgwPupPxPUpPlanBaseDto
                        {
                            Id = o.Id,
                            WorkingDate = o.WorkingDate,
                            ProdLine = o.ProdLine,
                            Shift1 = o.Shift1,
                            Shift2 = o.Shift2,
                            Shift3 = o.Shift3,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }


        //Import Data From PxPUpPlanBase Excel
        public async Task<List<ImportPxPUpPlanBaseDto>> ImportPxPUpPlanBaseFromExcel(List<ImportPxPUpPlanBaseDto> PxPUpPlanBases)
        {
            try
            {
                List<LgwPupPxPUpPlanBase_T> PxPUpPlanBase = new List<LgwPupPxPUpPlanBase_T> { };
                foreach (var item in PxPUpPlanBases)
                {
                    LgwPupPxPUpPlanBase_T importDataBase = new LgwPupPxPUpPlanBase_T();
                    {
                        importDataBase.Guid = item.Guid;
                        importDataBase.WorkingDate = item.WorkingDate;
                        importDataBase.ProdLine = item.ProdLine;
                        importDataBase.Shift1 = item.Shift1;
                        importDataBase.Shift2 = item.Shift2;
                        importDataBase.Shift3 = item.Shift3;
                        importDataBase.IsActive = "Y";
                    }
                    PxPUpPlanBase.Add(importDataBase);
                }
                CurrentUnitOfWork.GetDbContext<prodDbContext>().AddRange(PxPUpPlanBase);
                return null;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }

    }
}
