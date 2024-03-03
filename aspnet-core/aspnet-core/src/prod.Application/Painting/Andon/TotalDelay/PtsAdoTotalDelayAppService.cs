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
using prod.Painting.Andon.Dto;
using prod.Painting.Andon.Exporting;

namespace prod.Painting.Andon
{
    //  [AbpAuthorize(AppPermissions.Pages_Painting_Andon_TotalDelay)]
    public class PtsAdoTotalDelayAppService : prodAppServiceBase, IPtsAdoTotalDelayAppService
    {
        private readonly IDapperRepository<PtsAdoTotalDelay, long> _dapperRepo;
        private readonly IRepository<PtsAdoTotalDelay, long> _repo;
        private readonly IPtsAdoTotalDelayExcelExporter _calendarListExcelExporter;

        public PtsAdoTotalDelayAppService(IRepository<PtsAdoTotalDelay, long> repo,
                                         IDapperRepository<PtsAdoTotalDelay, long> dapperRepo,
                                        IPtsAdoTotalDelayExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Painting_Andon_TotalDelay_Edit)]
        public async Task CreateOrEdit(CreateOrEditPtsAdoTotalDelayDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditPtsAdoTotalDelayDto input)
        {
            var mainObj = ObjectMapper.Map<PtsAdoTotalDelay>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditPtsAdoTotalDelayDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Painting_Andon_TotalDelay_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<PtsAdoTotalDelayDto>> GetAll(GetPtsAdoTotalDelayInput input)
        {
             var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.BodyNo), e => e.BodyNo.Contains(input.BodyNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Mode), e => e.Mode.Contains(input.Mode))
                ;
            var pageAndFiltered = filtered.OrderByDescending(s => s.StartRepair);


            var system = from o in pageAndFiltered
                         select new PtsAdoTotalDelayDto
                         {
                             Id = o.Id,
                             WipId = o.WipId,
                             ProgressId = o.ProgressId,
                             BodyNo = o.BodyNo,
                             LotNo = o.LotNo,
                             Color = o.Color,
                             Mode = o.Mode,
                             TargetRepair = o.TargetRepair,
                             StartRepair = o.StartRepair,
                             Location = o.Location,
                             AInPlanDate = o.AInPlanDate,
                             EdInAct = o.EdInAct,
                             RepairIn = o.RepairIn,
                             Leadtime = o.Leadtime,
                             LeadtimePlus = o.LeadtimePlus,
                             Etd = o.Etd,
                             RecoatIn = o.RecoatIn,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<PtsAdoTotalDelayDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetTotalDelayToExcel(PtsAdoTotalDelayExportInput input)
        {
            var filtered = _repo.GetAll()
               .WhereIf(!string.IsNullOrWhiteSpace(input.BodyNo), e => e.BodyNo.Contains(input.BodyNo))
               .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo))
               .WhereIf(!string.IsNullOrWhiteSpace(input.Mode), e => e.Mode.Contains(input.Mode))
               ;
            var pageAndFiltered = filtered.OrderByDescending(s => s.StartRepair);


            var query = from o in pageAndFiltered
                         select new PtsAdoTotalDelayDto
                        {
                            Id = o.Id,
                            WipId = o.WipId,
                            ProgressId = o.ProgressId,
                            BodyNo = o.BodyNo,
                            LotNo = o.LotNo,
                            Color = o.Color,
                            Mode = o.Mode,
                            TargetRepair = o.TargetRepair,
                            StartRepair = o.StartRepair,
                            Location = o.Location,
                            AInPlanDate = o.AInPlanDate,
                            EdInAct = o.EdInAct,
                            RepairIn = o.RepairIn,
                            Leadtime = o.Leadtime,
                            LeadtimePlus = o.LeadtimePlus,
                            Etd = o.Etd,
                            RecoatIn = o.RecoatIn,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(PtsAdoTotalDelayConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
