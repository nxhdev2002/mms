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
using prod.Master.LogW.RenbanModule.ImportDto;

namespace prod.Master.LogW
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_LogW_RenbanModule)]
    public class MstLgwRenbanModuleAppService : prodAppServiceBase, IMstLgwRenbanModuleAppService
    {
        private readonly IDapperRepository<MstLgwRenbanModule, long> _dapperRepo;
        private readonly IRepository<MstLgwRenbanModule, long> _repo;
        private readonly IMstLgwRenbanModuleExcelExporter _calendarListExcelExporter;

        public MstLgwRenbanModuleAppService(IRepository<MstLgwRenbanModule, long> repo,
                                         IDapperRepository<MstLgwRenbanModule, long> dapperRepo,
                                        IMstLgwRenbanModuleExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogW_RenbanModule_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstLgwRenbanModuleDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstLgwRenbanModuleDto input)
        {
            var mainObj = ObjectMapper.Map<MstLgwRenbanModule>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstLgwRenbanModuleDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogW_RenbanModule_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstLgwRenbanModuleDto>> GetAll(GetMstLgwRenbanModuleInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Renban), e => e.Renban.Contains(input.Renban))
                .WhereIf(!string.IsNullOrWhiteSpace(input.CaseNo), e => e.CaseNo.Contains(input.CaseNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierNo), e => e.SupplierNo.Contains(input.SupplierNo))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstLgwRenbanModuleDto
                         {
                             Id = o.Id,
                             Renban = o.Renban,
                             CaseNo = o.CaseNo,
                             SupplierNo = o.SupplierNo,
                             MinModule = o.MinModule,
                             MaxModule = o.MaxModule,
                             ModuleCapacity = o.ModuleCapacity,
                             ModuleType = o.ModuleType,
                             ModuleSize = o.ModuleSize,
                             SortingType = o.SortingType,
                             MinMod = o.MinMod,
                             MaxMod = o.MaxMod,
                             MonitorVisualize = o.MonitorVisualize,
                             CaseOrder = o.CaseOrder,
                             CaseType = o.CaseType,
                             ProdLine = o.ProdLine,
                             Model = o.Model,
                             Cfc = o.Cfc,
                             WhLoc = o.WhLoc,
                             IsUsePxpData = o.IsUsePxpData,
                             UpLeadtime = o.UpLeadtime,
                             Remark = o.Remark,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstLgwRenbanModuleDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetRenbanModuleToExcel(MstLgwRenbanModuleExportInput input)
        {
            var filtered = _repo.GetAll()
                    .WhereIf(!string.IsNullOrWhiteSpace(input.Renban), e => e.Renban.Contains(input.Renban))
                    .WhereIf(!string.IsNullOrWhiteSpace(input.CaseNo), e => e.CaseNo.Contains(input.CaseNo))
                    .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierNo), e => e.SupplierNo.Contains(input.SupplierNo))
                    ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var query = from o in pageAndFiltered
                         select new MstLgwRenbanModuleDto
                        {
                            Id = o.Id,
                            Renban = o.Renban,
                            CaseNo = o.CaseNo,
                            SupplierNo = o.SupplierNo,
                            MinModule = o.MinModule,
                            MaxModule = o.MaxModule,
                            ModuleCapacity = o.ModuleCapacity,
                            ModuleType = o.ModuleType,
                            ModuleSize = o.ModuleSize,
                            SortingType = o.SortingType,
                            MinMod = o.MinMod,
                            MaxMod = o.MaxMod,
                            MonitorVisualize = o.MonitorVisualize,
                            CaseOrder = o.CaseOrder,
                            CaseType = o.CaseType,
                            ProdLine = o.ProdLine,
                            Model = o.Model,
                            Cfc = o.Cfc,
                            WhLoc = o.WhLoc,
                            IsUsePxpData = o.IsUsePxpData,
                            UpLeadtime = o.UpLeadtime,
                            Remark = o.Remark,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        /*    public async Task ImportRenbanModule(List<MstLgwRenbanModuleDto> input)
            {
                foreach (MstLgwRenbanModuleDto item in input)
                {
                    using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
                    {
                        var dealerRenbanModule = _repo.GetAll().Where(p => p.Renban == item.Renban && p.CaseNo == item.CaseNo && p.SupplierNo == item.SupplierNo);
                        if (dealerRenbanModule.Count() != 0)
                        {
                            var orderRenbanModule = dealerRenbanModule.OrderByDescending(p => p.CreationTime).FirstOrDefault();

                            orderRenbanModule.MinModule = item.MinModule;
                            orderRenbanModule.MaxModule = item.MaxModule;
                            orderRenbanModule.ModuleCapacity = item.ModuleCapacity;
                            orderRenbanModule.ModuleType = item.ModuleType;
                            orderRenbanModule.ModuleSize = item.ModuleSize;
                            orderRenbanModule.SortingType = item.SortingType;
                            orderRenbanModule.MinMod = item.MinMod;
                            orderRenbanModule.MaxMod = item.MaxMod;
                            orderRenbanModule.MonitorVisualize = item.MonitorVisualize;
                            orderRenbanModule.CaseOrder = item.CaseOrder;
                            orderRenbanModule.CaseType = item.CaseType;
                            orderRenbanModule.ProdLine = item.ProdLine;
                            orderRenbanModule.Model = item.Model;
                            orderRenbanModule.Cfc = item.Cfc;
                            orderRenbanModule.WhLoc = item.WhLoc;
                            orderRenbanModule.IsUsePxpData = item.IsUsePxpData;
                            orderRenbanModule.UpLeadtime = item.UpLeadtime;
                            orderRenbanModule.Remark = item.Remark;
                            orderRenbanModule.IsActive = item.IsActive;
                            _repo.Update(orderRenbanModule);
                        }
                        else
                        {
                            MstLgwRenbanModule importData = new MstLgwRenbanModule();
                            importData.Renban = item.Renban;
                            importData.CaseNo = item.CaseNo;
                            importData.SupplierNo = item.SupplierNo;
                            importData.MinModule = item.MinModule;
                            importData.MaxModule = item.MaxModule;
                            importData.ModuleCapacity = item.ModuleCapacity;
                            importData.ModuleType = item.ModuleType;
                            importData.ModuleSize = item.ModuleSize;
                            importData.SortingType = item.SortingType;
                            importData.MinMod = item.MinMod;
                            importData.MaxMod = item.MaxMod;
                            importData.MonitorVisualize = item.MonitorVisualize;
                            importData.CaseOrder = item.CaseOrder;
                            importData.CaseType = item.CaseType;
                            importData.ProdLine = item.ProdLine;
                            importData.Model = item.Model;
                            importData.Cfc = item.Cfc;
                            importData.WhLoc = item.WhLoc;
                            importData.IsUsePxpData = item.IsUsePxpData;
                            importData.UpLeadtime = item.UpLeadtime;
                            importData.Remark = item.Remark;
                            importData.IsActive = item.IsActive;
                            await _repo.InsertAsync(importData);
                        }

                    }
                }
            }*/

        //Import Data From Excel
        public async Task<List<ImportMstLgwRenbanModuleDto>> ImportMstLgwRenbanModuleFromExcel(List<ImportMstLgwRenbanModuleDto> MstLgwRenbanModules)
        {
            try
            {
                List<MstLgwRenbanModule_T> MstLgwRenbanModule = new List<MstLgwRenbanModule_T> { };
                foreach (var item in MstLgwRenbanModules)
                {
                    MstLgwRenbanModule_T importData = new MstLgwRenbanModule_T();
                    {
                        importData.Guid = item.Guid ;
                        importData.CaseNo = item.CaseNo;
                        importData.SupplierNo = item.SupplierNo;
                        importData.Renban = item.Renban;
                        importData.ProdLine = item.ProdLine;
                        importData.Model = item.Model;
                        importData.Cfc = item.Cfc;
                        importData.ModuleType = item.ModuleType;
                        importData.ModuleSize = item.ModuleSize ;
                        importData.SortingType = item.SortingType ;
                        importData.WhLoc = item.WhLoc;
                        importData.MinModule = item.MinModule;
                        importData.MaxModule = item.MaxModule;
                        importData.ModuleCapacity = item.ModuleCapacity;
                        importData.IsActive =item.IsActive;
                        importData.MonitorVisualize = item.MonitorVisualize;
                        importData.CaseOrder = item.CaseOrder;
                        importData.IsUsePxpData = item.IsUsePxpData;
                        importData.UpLeadtime = item.UpLeadtime;
                    }
                    MstLgwRenbanModule.Add(importData);
                }
                await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddRangeAsync(MstLgwRenbanModule);
                return null;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }

        //Merge Data From Temp To PxPUpPlan
        public async Task MergeDataMstLgwRenbanModule(string v_Guid)
        {
            string _sql = "Exec MST_LGW_RENBAN_MODEL_MERGE @Guid";
            await _dapperRepo.QueryAsync<MstLgwRenbanModule>(_sql, new { Guid = v_Guid });
        }

    }
}
