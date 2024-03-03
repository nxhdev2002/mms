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
using prod.Master.LogW.ModelGrade.ImportDto;

namespace prod.Master.LogW
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_LogW_ModelGrade)]
    public class MstLgwModelGradeAppService : prodAppServiceBase, IMstLgwModelGradeAppService
    {
        private readonly IDapperRepository<MstLgwModelGrade, long> _dapperRepo;
        private readonly IRepository<MstLgwModelGrade, long> _repo;
        private readonly IMstLgwModelGradeExcelExporter _calendarListExcelExporter;

        public MstLgwModelGradeAppService(IRepository<MstLgwModelGrade, long> repo,
                                         IDapperRepository<MstLgwModelGrade, long> dapperRepo,
                                        IMstLgwModelGradeExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogW_ModelGrade_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstLgwModelGradeDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstLgwModelGradeDto input)
        {
            var mainObj = ObjectMapper.Map<MstLgwModelGrade>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstLgwModelGradeDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogW_ModelGrade_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstLgwModelGradeDto>> GetAll(GetMstLgwModelGradeInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Model), e => e.Model.Contains(input.Model))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Grade), e => e.Grade.Contains(input.Grade));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstLgwModelGradeDto
                         {
                             Id = o.Id,
                             Model = o.Model,
                             Grade = o.Grade,
                             ModuleUpQty = o.ModuleUpQty,
                             ModuleMkQty = o.ModuleMkQty,
                             UpLeadtime = o.UpLeadtime,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstLgwModelGradeDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetModelGradeToExcel(MstLgwModelGradeExportInput input)
        {
            var filtered = _repo.GetAll()
                 .WhereIf(!string.IsNullOrWhiteSpace(input.Model), e => e.Model.Contains(input.Model))
                 .WhereIf(!string.IsNullOrWhiteSpace(input.Grade), e => e.Grade.Contains(input.Grade));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var query = from o in pageAndFiltered
                         select new MstLgwModelGradeDto
                        {
                            Id = o.Id,
                            Model = o.Model,
                            Grade = o.Grade,
                            ModuleUpQty = o.ModuleUpQty,
                            ModuleMkQty = o.ModuleMkQty,
                            UpLeadtime = o.UpLeadtime,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        public async Task ImportModelGrade(List<MstLgwModelGradeDto> input)
        {
            foreach (MstLgwModelGradeDto item in input)
            {
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    var modelGrade = _repo.GetAll().Where(p => p.Model == item.Model && p.Grade == item.Grade );
                    if (modelGrade.Count() != 0)
                    {
                        var orderModelGrade = modelGrade.OrderByDescending(p => p.CreationTime).FirstOrDefault();
                        orderModelGrade.ModuleUpQty = item.ModuleUpQty;
                        orderModelGrade.ModuleMkQty = item.ModuleMkQty;
                        orderModelGrade.UpLeadtime = item.UpLeadtime;
                        orderModelGrade.IsActive = "Y";
                        _repo.Update(orderModelGrade);
                    }
                    else
                    {
                        MstLgwModelGrade importData = new MstLgwModelGrade();
                        importData.Model = item.Model;
                        importData.Grade = item.Grade;
                        importData.ModuleUpQty = item.ModuleUpQty;
                        importData.ModuleMkQty = item.ModuleMkQty;
                        importData.UpLeadtime = item.UpLeadtime;
                        importData.IsActive = "Y";
                        await _repo.InsertAsync(importData);
                    }

                }
            }
        }

        //Import Data From Excel
        public async Task<List<ImportMstlgwModelGradeDto>> ImportMstLgwModelgradeFromExcel(List<ImportMstlgwModelGradeDto> MstLgwModelGrades)
        {
            try
            {
                List<MstLgwModelGrade_T> MstLgwModelGrade = new List<MstLgwModelGrade_T> { };
                foreach (var item in MstLgwModelGrades)
                {
                    MstLgwModelGrade_T importData = new MstLgwModelGrade_T();
                    {
                        importData.Guid = item.Guid;
                        importData.Model = item.Model;
                        importData.Grade = item.Grade;
                        importData.ModuleUpQty = item.ModuleUpQty;
                        importData.ModuleMkQty = item.ModuleMkQty;
                        importData.UpLeadtime = item.UpLeadtime;
                    }
                    MstLgwModelGrade.Add(importData);
                }
                CurrentUnitOfWork.GetDbContext<prodDbContext>().AddRangeAsync(MstLgwModelGrade);
                return null;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }

        //Merge Data From Temp To PxPUpPlan
        public async Task MergeDataMstLgwModel(string v_Guid)
        {
            string _sql = "Exec MST_LGW_MODEL_GRADE_MERGE @Guid";
            await _dapperRepo.QueryAsync<MstLgwModelGrade>(_sql, new { Guid = v_Guid });
        }


    }
}
