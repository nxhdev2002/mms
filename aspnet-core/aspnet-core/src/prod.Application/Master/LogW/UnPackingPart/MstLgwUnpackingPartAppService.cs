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
using prod.LogW.Pup;
using prod.Master.LogW.Dto;
using prod.Master.LogW.Exporting;
using prod.Master.LogW.UnPackingPart.ImportDto;

namespace prod.Master.LogW
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_LogW_UnpackingPart)]
    public class MstLgwUnpackingPartAppService : prodAppServiceBase,IMstLgwUnpackingPartAppService
    {
        private readonly IDapperRepository<MstLgwUnpackingPart, long> _dapperRepo;
        private readonly IRepository<MstLgwUnpackingPart, long> _repo;
        private readonly IMstLgwUnpackingPartExcelExporter _calendarListExcelExporter;

        public MstLgwUnpackingPartAppService(IRepository<MstLgwUnpackingPart, long> repo,
                                         IDapperRepository<MstLgwUnpackingPart, long> dapperRepo,
                                        IMstLgwUnpackingPartExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogW_UnpackingPart_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstLgwUnpackingPartDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstLgwUnpackingPartDto input)
        {
            var mainObj = ObjectMapper.Map<MstLgwUnpackingPart>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstLgwUnpackingPartDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogW_UnpackingPart_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstLgwUnpackingPartDto>> GetAll(GetMstLgwUnpackingPartInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Cfc), e => e.Cfc.Contains(input.Cfc))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Model), e => e.Model.Contains(input.Model))
                .WhereIf(!string.IsNullOrWhiteSpace(input.BackNo), e => e.BackNo.Contains(input.BackNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo), e => e.PartNo.Contains(input.PartNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartName), e => e.PartName.Contains(input.PartName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ModuleCode), e => e.ModuleCode.Contains(input.ModuleCode))
                .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstLgwUnpackingPartDto
                         {
                             Id = o.Id,
                             Cfc = o.Cfc,
                             Model = o.Model,
                             BackNo = o.BackNo,
                             PartNo = o.PartNo,
                             PartName = o.PartName,
                             ModuleCode = o.ModuleCode,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstLgwUnpackingPartDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetUnpackingPartToExcel(MstLgwUnpackingPartExportInput input)
        {
            var filtered = _repo.GetAll()
                 .WhereIf(!string.IsNullOrWhiteSpace(input.Cfc), e => e.Cfc.Contains(input.Cfc))
                 .WhereIf(!string.IsNullOrWhiteSpace(input.Model), e => e.Model.Contains(input.Model))
                 .WhereIf(!string.IsNullOrWhiteSpace(input.BackNo), e => e.BackNo.Contains(input.BackNo))
                 .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo), e => e.PartNo.Contains(input.PartNo))
                 .WhereIf(!string.IsNullOrWhiteSpace(input.PartName), e => e.PartName.Contains(input.PartName))
                 .WhereIf(!string.IsNullOrWhiteSpace(input.ModuleCode), e => e.ModuleCode.Contains(input.ModuleCode))
                 .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive))
                 ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var query = from o in pageAndFiltered
                         select new MstLgwUnpackingPartDto
                        {
                            Id = o.Id,
                            Cfc = o.Cfc,
                            Model = o.Model,
                            BackNo = o.BackNo,
                            PartNo = o.PartNo,
                            PartName = o.PartName,
                            ModuleCode = o.ModuleCode,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        //Import Data From Excel
        public async Task<List<ImportMstLgwUnpackingPartDto>> ImportUnpackingPartFromExcel(List<ImportMstLgwUnpackingPartDto> unpackingParts)
        {
            try
            {
                List<MstLgwUnpackingPart_T> pxpUpPlanList = new List<MstLgwUnpackingPart_T> { };
                foreach (var item in unpackingParts)
                {
                    MstLgwUnpackingPart_T importData = new MstLgwUnpackingPart_T();
                    {
                        importData.Guid = item.Guid;
                        importData.Cfc = item.Cfc;
                        importData.PartNo = item.PartNo;
                        importData.PartName = item.PartName;
                        importData.BackNo = item.BackNo;
                        importData.ModuleCode = item.ModuleCode;
                    }
                    pxpUpPlanList.Add(importData);
                }
                CurrentUnitOfWork.GetDbContext<prodDbContext>().AddRangeAsync(pxpUpPlanList);
                return null;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }

        //Merge Data From Temp To UnpackingPart
        public async Task MergeDataUnpackingPart(string v_Guid)
        {
            string _sql = "Exec MST_LGW_UNPACKING_PART_MERGE @Guid";
             await _dapperRepo.QueryAsync<MstLgwUnpackingPart>(_sql, new { Guid = v_Guid });
        }

    }
}

