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
//using NUglify.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Master.Cmm.Dto;
using prod.Master.Cmm.Exporting;
using prod.Master.Common.Dto;
using prod.Master.Common.GradeColor.Dto;

namespace prod.Master.Cmm
{
    [AbpAuthorize(AppPermissions.Pages_Master_Common_Model_View)]
    public class MstCmmModelAppService : prodAppServiceBase, IMstCmmModelAppService
    {
        private readonly IDapperRepository<MstCmmModel, long> _dapperRepo;
        private readonly IRepository<MstCmmModel, long> _repo;
        private readonly IRepository<MstCmmLotCodeGrade, long> _repoLotCodeGrade;
        private readonly IMstCmmModelExcelExporter _calendarListExcelExporter;

        public MstCmmModelAppService(IRepository<MstCmmModel, long> repo,
                                                 IRepository<MstCmmLotCodeGrade, long> repoLotCodeGrade,
                                         IDapperRepository<MstCmmModel, long> dapperRepo,
                                        IMstCmmModelExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _repoLotCodeGrade = repoLotCodeGrade;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_Master_Common_Model_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstCmmModelDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstCmmModelDto input)
        {
            var mainObj = ObjectMapper.Map<MstCmmModel>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstCmmModelDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Master_Common_Model_Edit)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }
        
        public async Task<PagedResultDto<MstCmmModelDto>> GetAllModel(GetMstCmmModelInput input)
        {
            var repairHistory = (from ro in _repo.GetAll().AsNoTracking()
                                join t in _repoLotCodeGrade.GetAll() on ro.Code equals t.Model into k
                                from t in k.DefaultIfEmpty()
                                 where (string.IsNullOrWhiteSpace(input.Cfc) || t.Cfc.ToLower() == input.Cfc.ToLower())
                                 && (string.IsNullOrWhiteSpace(input.ModelVin) || t.ModelVin.ToLower() == input.ModelVin.ToLower())
                                 && (string.IsNullOrWhiteSpace(input.ModelCode) || t.ModelCode.ToLower() == input.ModelCode.ToLower())

                                 //  && (string.IsNullOrWhiteSpace(input.ModelVin) || ro.RegisterNo.ToLower() == input.RegisterNo.ToLower())
                                 orderby ro.Code descending
                                select new MstCmmModelDto
                          {
                                   Id = ro.Id,
                                   Code = ro.Code,
                                   Name = ro.Name,
                                   IsActive = ro.IsActive
                         }).ToList().AsEnumerable().DistinctBy(e=>e.Id);
            var totalCount = repairHistory.Count();
            var paged = repairHistory.Skip(input.SkipCount).Take(input.MaxResultCount);

            return new PagedResultDto<MstCmmModelDto>(
                totalCount,
                 paged.ToList()
            );
        }

        public async Task<PagedResultDto<MstCmmLotCodeGradeDto>> GetAllLotCodeGrade(GetMstCmmLotCodeGradeInput input)
        {
            var filtered = _repoLotCodeGrade.GetAll()
                .Where(e => e.Model.Contains(input.Model))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Cfc), e => e.Cfc.Contains(input.Cfc))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ModelCode), e => e.ModelCode.Contains(input.ModelCode))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ModelVin), e => e.ModelVin.Contains(input.ModelVin));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var system = from lot in pageAndFiltered
                         select new MstCmmLotCodeGradeDto
                         {
                             Id = lot.Id,
                             Model = lot.Model,
                             LotCode = lot.LotCode,
                             Cfc = lot.Cfc,
                             Grade = lot.Grade,
                             GradeName = lot.GradeName,
                             ModelCode = lot.ModelCode,
                             ModelVin = lot.ModelVin,
                             IsActive = lot.IsActive
                         };
            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstCmmLotCodeGradeDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }



        public async Task<FileDto> GetModelToExcel(MstCmmModelExportInput input)
        {
            var subfilter = _repoLotCodeGrade.GetAll()
                                .WhereIf(!string.IsNullOrWhiteSpace(input.Cfc), e => e.Cfc.Contains(input.Cfc))
                                .WhereIf(!string.IsNullOrWhiteSpace(input.ModelCode), e => e.ModelCode.Contains(input.ModelCode))
                                .WhereIf(!string.IsNullOrWhiteSpace(input.ModelVin), e => e.ModelVin.Contains(input.ModelVin))
                                .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive))
                                .Select(p => p.Model).Distinct().ToList();
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name))
                .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive))
                .Where(p => subfilter.Any(o => o == p.Code))
                .OrderBy(s => s.Id).ToList();
            ;


            var query = from o in filtered
                        select new MstCmmModelDto
                        {
                            Id = o.Id,
                            Code = o.Code,
                            Name = o.Name,
                            IsActive = o.IsActive,
                            ListLotCodeGrade = (from lot in _repoLotCodeGrade.GetAll().Where(e => e.Model == o.Code)
                                                select new MstCmmLotCodeGradeDto
                                                {
                                                    Id = lot.Id,
                                                    Model = lot.Model,
                                                    LotCode = lot.LotCode,
                                                    Cfc = lot.Cfc,
                                                    Grade = lot.Grade,
                                                    GradeName = lot.GradeName,
                                                    ModelCode = lot.ModelCode,
                                                    ModelVin = lot.ModelVin,
                                                    IsActive = lot.IsActive
                                                }).ToList()
                        };
            var exportToExcel = query.ToList();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }
        public async Task<FileDto> GetLotCodeGradeToExcel(MstCmmLotCodeGradeInput input)
        {
            var query = from o in _repoLotCodeGrade.GetAll()
                          .WhereIf(!string.IsNullOrWhiteSpace(input.Cfc), e => e.Cfc.Contains(input.Cfc))
                          .WhereIf(!string.IsNullOrWhiteSpace(input.ModelCode), e => e.ModelCode.Contains(input.ModelCode))
                          .WhereIf(!string.IsNullOrWhiteSpace(input.ModelVin), e => e.ModelVin.Contains(input.ModelVin))
                        select new MstCmmLotCodeGradeDto
                        {
                            Model = o.Model,
                            LotCode = o.LotCode,
                            Cfc = o.Cfc,
                            Grade = o.Grade,
                            GradeName = o.GradeName,
                            ModelCode = o.ModelCode,
                            ModelVin = o.ModelVin

                        };
            var exportLotCodeToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportLotCodeToFile(exportLotCodeToExcel);
        }

    }
}
