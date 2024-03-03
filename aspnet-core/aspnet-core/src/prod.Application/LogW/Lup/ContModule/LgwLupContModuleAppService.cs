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
using prod.LogW.Lup.ContModule;
using prod.LogW.Lup.Dto;
using prod.LogW.Lup.Exporting;

namespace prod.LogW.Lup
{
    //  [AbpAuthorize(AppPermissions.Pages_LogW_Lup_ContModule)]
    public class LgwLupContModuleAppService : prodAppServiceBase, ILgwLupContModuleAppService
    {
        private readonly IDapperRepository<LgwLupContModule, long> _dapperRepo;
        private readonly IRepository<LgwLupContModule, long> _repo;
        private readonly ILgwLupContModuleExcelExporter _calendarListExcelExporter;

        public LgwLupContModuleAppService(IRepository<LgwLupContModule, long> repo,
                                         IDapperRepository<LgwLupContModule, long> dapperRepo,
                                        ILgwLupContModuleExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_LogW_Lup_ContModule_Edit)]
        public async Task CreateOrEdit(CreateOrEditLgwLupContModuleDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditLgwLupContModuleDto input)
        {
            var mainObj = ObjectMapper.Map<LgwLupContModule>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditLgwLupContModuleDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_LogW_Lup_ContModule_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<LgwLupContModuleDto>> GetAll(GetLgwLupContModuleInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.ContainerNo), e => e.ContainerNo.Contains(input.ContainerNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ModuleNo), e => e.ModuleNo.Contains(input.ModuleNo))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new LgwLupContModuleDto
                         {
                             Id = o.Id,
                             InvoiceNo = o.InvoiceNo,
                             SupplierNo = o.SupplierNo,
                             ContainerNo = o.ContainerNo,
                             Renban = o.Renban,
                             LotNo = o.LotNo,
                             ModuleNo = o.ModuleNo,
                             Status = o.Status,
                             SortingType = o.SortingType,
                             SortingStatus = o.SortingStatus,
                             UpdatedSortingStatus = o.UpdatedSortingStatus,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<LgwLupContModuleDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetContModuleToExcel(GetLgwLupContModuleExportInput input)
        {
            var filtered = _repo.GetAll()
               .WhereIf(!string.IsNullOrWhiteSpace(input.ContainerNo), e => e.ContainerNo.Contains(input.ContainerNo))
               .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo))
               .WhereIf(!string.IsNullOrWhiteSpace(input.ModuleNo), e => e.ModuleNo.Contains(input.ModuleNo))
               ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var query = from o in pageAndFiltered
                        select new LgwLupContModuleDto
                        {
                            Id = o.Id,
                            InvoiceNo = o.InvoiceNo,
                            SupplierNo = o.SupplierNo,
                            ContainerNo = o.ContainerNo,
                            Renban = o.Renban,
                            LotNo = o.LotNo,
                            ModuleNo = o.ModuleNo,
                            Status = o.Status,
                            SortingType = o.SortingType,
                            SortingStatus = o.SortingStatus,
                            UpdatedSortingStatus = o.UpdatedSortingStatus,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }


    }
}
