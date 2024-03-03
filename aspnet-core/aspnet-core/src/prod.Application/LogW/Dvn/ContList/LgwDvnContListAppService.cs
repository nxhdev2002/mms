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
using prod.LogW.Dvn.Dto;
using prod.LogW.Dvn.Exporting;

namespace prod.LogW.Dvn
{
    //  [AbpAuthorize(AppPermissions.Pages_LogW_Dvn_ContList)]
    public class LgwDvnContListAppService : prodAppServiceBase, ILgwDvnContListAppService
    {
        private readonly IDapperRepository<LgwDvnContList, long> _dapperRepo;
        private readonly IRepository<LgwDvnContList, long> _repo;
        private readonly ILgwDvnContListExcelExporter _calendarListExcelExporter;

        public LgwDvnContListAppService(IRepository<LgwDvnContList, long> repo,
                                         IDapperRepository<LgwDvnContList, long> dapperRepo,
                                        ILgwDvnContListExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_LogW_Dvn_ContList_Edit)]
        public async Task CreateOrEdit(CreateOrEditLgwDvnContListDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditLgwDvnContListDto input)
        {
            var mainObj = ObjectMapper.Map<LgwDvnContList>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditLgwDvnContListDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_LogW_Dvn_ContList_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<LgwDvnContListDto>> GetAll(GetLgwDvnContListInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;

            var filtered = _repo.GetAll()
                .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
                .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom.Value.Date <= t.WorkingDate)
                .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo.Value.Date.AddDays(1) > t.WorkingDate)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ContainerNo), e => e.ContainerNo.Contains(input.ContainerNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Renban), e => e.Renban.Contains(input.Renban));

            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate).ThenBy(n => n.PlanDevanningDate);


            var results = from o in pageAndFiltered
                         select new LgwDvnContListDto
                         {
                             Id = o.Id,
                             ContainerNo = o.ContainerNo,
                             Renban = o.Renban,
                             SupplierNo = o.SupplierNo,
                             LotNo = o.LotNo,
                             WorkingDate = o.WorkingDate,
                             ShiftNo = o.ShiftNo,
                             DevanningDock = o.DevanningDock,
                             PlanDevanningDate = o.PlanDevanningDate,
                             ActDevanningDate = o.ActDevanningDate,
                             ActDevanningDateFinished = o.ActDevanningDateFinished,
                             DevanningType = o.DevanningType,
                             Status = o.Status,
                             DevLeadtime = o.DevLeadtime,
                             PlanDevanningLineOff = o.PlanDevanningLineOff,
                             SortingStatus = o.SortingStatus,
                             IsActive = o.IsActive,
                         };

            var totalCount = filtered.ToList().Count;
            var paged = results.AsQueryable().PageBy(input);

            return new PagedResultDto<LgwDvnContListDto>(
                totalCount,
                paged.ToList()
            );
        }


        public async Task<FileDto> GetContListToExcel(GetLgwDvnContListExportInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;

            var filtered = _repo.GetAll()
               .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
               .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom.Value.Date <= t.WorkingDate)
               .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo.Value.Date.AddDays(1) > t.WorkingDate)
               .WhereIf(!string.IsNullOrWhiteSpace(input.ContainerNo), e => e.ContainerNo.Contains(input.ContainerNo))
               .WhereIf(!string.IsNullOrWhiteSpace(input.Renban), e => e.Renban.Contains(input.Renban));

            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate).ThenBy(n => n.PlanDevanningDate);


            var query = from o in pageAndFiltered
                        select new LgwDvnContListDto
                        {
                            Id = o.Id,
                            ContainerNo = o.ContainerNo,
                            Renban = o.Renban,
                            SupplierNo = o.SupplierNo,
                            LotNo = o.LotNo,
                            WorkingDate = o.WorkingDate,
                            ShiftNo = o.ShiftNo,
                            DevanningDock = o.DevanningDock,
                            PlanDevanningDate = o.PlanDevanningDate,
                            ActDevanningDate = o.ActDevanningDate,
                            ActDevanningDateFinished = o.ActDevanningDateFinished,
                            DevanningType = o.DevanningType,
                            Status = o.Status,
                            DevLeadtime = o.DevLeadtime,
                            PlanDevanningLineOff = o.PlanDevanningLineOff,
                            SortingStatus = o.SortingStatus,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        //screen
        //

        public async Task<PagedResultDto<LgwDvnContListDto>> GetPlanDev()
        {
            string _sql = "Exec LGW_DVN_DEVANNING_GET_PLAN";
            var filtered = await _dapperRepo.QueryAsync<LgwDvnContListDto>(_sql, new { });
            var results = from d in filtered
                          select new LgwDvnContListDto
                          {
                              ContainerNo = d.ContainerNo,
                              Renban = d.Renban,
                              LotNo = d.LotNo,
                              WorkingDate = d.WorkingDate,
                              PlanDevanningDate = d.PlanDevanningDate,
                              DevanningType = d.DevanningType,
                              Status = d.Status
                          };

            var totalCount = filtered.ToList().Count;
            return new PagedResultDto<LgwDvnContListDto>(
                totalCount,
                results.ToList()
            );
        }

        public async Task<PagedResultDto<LgwDvnContListDto>> GetsDailly()
        {
            string _sql = "Exec LGW_DVN_DEVANNING_GET_DAILY";
            var filtered = await _dapperRepo.QueryAsync<LgwDvnContListDto>(_sql, new { });
            var results = from d in filtered
                          select new LgwDvnContListDto
                          {
                              Id = d.Id,
                              ContainerNo = d.ContainerNo,
                              Renban = d.Renban,
                              SupplierNo = d.SupplierNo,
                              LotNo = d.LotNo,
                              WorkingDate = d.WorkingDate,
                              ShiftNo = d.ShiftNo,
                              DevanningDock = d.DevanningDock,
                              PlanDevanningDate = d.PlanDevanningDate,
                              ActDevanningDate = d.ActDevanningDate,
                              ActDevanningDateFinished = d.ActDevanningDateFinished,
                              DevanningType = d.DevanningType,
                              Status = d.Status,
                              IsActive = d.SupplierNo,
                              DevLeadtime = d.DevLeadtime,
                              PlanDevanningLineOff = d.PlanDevanningLineOff,
                              AvgTaktDevLT = d.AvgTaktDevLT,
                              TotalActual = d.TotalActual,
                              IsEci = d.IsEci,
                              IsDelayed = d.IsDelayed
                          };

            var totalCount = filtered.ToList().Count;
            return new PagedResultDto<LgwDvnContListDto>(
                totalCount,
                results.ToList()
            );
        }

    }

}
