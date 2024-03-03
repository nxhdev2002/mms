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
using prod.Master.Plm.Dto;
using prod.Master.Plm.Exporting;

namespace prod.Master.Plm
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_Plm_LotCodeGrade)]
    public class MstPlmLotCodeGradeAppService : prodAppServiceBase, IMstPlmLotCodeGradeAppService
    {
        private readonly IDapperRepository<MstPlmLotCodeGrade, long> _dapperRepo;
        private readonly IRepository<MstPlmLotCodeGrade, long> _repo;
        private readonly IMstPlmLotCodeGradeExcelExporter _calendarListExcelExporter;

        public MstPlmLotCodeGradeAppService(IRepository<MstPlmLotCodeGrade, long> repo,
                                         IDapperRepository<MstPlmLotCodeGrade, long> dapperRepo,
                                        IMstPlmLotCodeGradeExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_Plm_LotCodeGrade_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstPlmLotCodeGradeDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstPlmLotCodeGradeDto input)
        {
            var mainObj = ObjectMapper.Map<MstPlmLotCodeGrade>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstPlmLotCodeGradeDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_Plm_LotCodeGrade_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstPlmLotCodeGradeDto>> GetAll(GetMstPlmLotCodeGradeInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.LotCode), e => e.LotCode.Contains(input.LotCode))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Cfc), e => e.Cfc.Contains(input.Cfc))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Grade), e => e.Grade.Contains(input.Grade))         
                .WhereIf(!string.IsNullOrWhiteSpace(input.ModeCode), e => e.ModeCode.Contains(input.ModeCode))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ModelVin), e => e.ModelVin.Contains(input.ModelVin))
                .WhereIf(!string.IsNullOrWhiteSpace(input.MaLotCode), e => e.MaLotCode.Contains(input.MaLotCode))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstPlmLotCodeGradeDto
                         {
                             Id = o.Id,
                             ModelId = o.ModelId,
                             LotCode = o.LotCode,
                             Cfc = o.Cfc,
                             Grade = o.Grade,
                             Odering = o.Odering,
                             GradeName = o.GradeName,
                             ModeCode = o.ModeCode,
                             ModelVin = o.ModelVin,
                             VisStart = o.VisStart,
                             VisEnd = o.VisEnd,
                             MaLotCode = o.MaLotCode,
                             VehicleId = o.VehicleId,
                             TestNo = o.TestNo,
                             Dab = o.Dab,
                             Pab = o.Pab,
                             EngCode = o.EngCode,
                             Lab = o.Lab,
                             Rab = o.Rab,
                             Kab = o.Kab,
                             IsFcLabel = o.IsFcLabel,
                             IsActive = o.IsActive,
                             R = o.R,
                             G = o.G,
                             B = o.B,
                             Clab = o.Clab,
                             CharStr = o.CharStr,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstPlmLotCodeGradeDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetLotCodeGradeToExcel(GetMstPlmLotCodeGradeInput input)
        {
            var filtered = _repo.GetAll()
               .WhereIf(!string.IsNullOrWhiteSpace(input.LotCode), e => e.LotCode.Contains(input.LotCode))
               .WhereIf(!string.IsNullOrWhiteSpace(input.Cfc), e => e.Cfc.Contains(input.Cfc))
               .WhereIf(!string.IsNullOrWhiteSpace(input.Grade), e => e.Grade.Contains(input.Grade))
               .WhereIf(!string.IsNullOrWhiteSpace(input.ModeCode), e => e.ModeCode.Contains(input.ModeCode))
               .WhereIf(!string.IsNullOrWhiteSpace(input.ModelVin), e => e.ModelVin.Contains(input.ModelVin))
               .WhereIf(!string.IsNullOrWhiteSpace(input.MaLotCode), e => e.MaLotCode.Contains(input.MaLotCode))
               ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var query = from o in pageAndFiltered
                        select new MstPlmLotCodeGradeDto
                        {
                            Id = o.Id,
                            ModelId = o.ModelId,
                            LotCode = o.LotCode,
                            Cfc = o.Cfc,
                            Grade = o.Grade,
                            Odering = o.Odering,
                            GradeName = o.GradeName,
                            ModeCode = o.ModeCode,
                            ModelVin = o.ModelVin,
                            VisStart = o.VisStart,
                            VisEnd = o.VisEnd,
                            MaLotCode = o.MaLotCode,
                            VehicleId = o.VehicleId,
                            TestNo = o.TestNo,
                            Dab = o.Dab,
                            Pab = o.Pab,
                            EngCode = o.EngCode,
                            Lab = o.Lab,
                            Rab = o.Rab,
                            Kab = o.Kab,
                            IsFcLabel = o.IsFcLabel,
                            IsActive = o.IsActive,
                            R = o.R,
                            G = o.G,
                            B = o.B,
                            Clab = o.Clab,
                            CharStr = o.CharStr,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstPlmLotCodeGradeConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
