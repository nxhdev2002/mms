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
using prod.Master.LogA.Bp2PartList.ImportDto;
using prod.Master.LogA.Dto;
using prod.Master.LogA.Exporting;

namespace prod.Master.LogA
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_Bp2PartList)]
    public class MstLgaBp2PartListAppService : prodAppServiceBase, IMstLgaBp2PartListAppService
    {
        private readonly IDapperRepository<MstLgaBp2PartList, long> _dapperRepo;
        private readonly IRepository<MstLgaBp2PartList, long> _repo;
        private readonly IMstLgaBp2PartListExcelExporter _calendarListExcelExporter;

        public MstLgaBp2PartListAppService(IRepository<MstLgaBp2PartList, long> repo,
                                         IDapperRepository<MstLgaBp2PartList, long> dapperRepo,
                                        IMstLgaBp2PartListExcelExporter calendarListExcelExporter)
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_Bp2PartList_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstLgaBp2PartListDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstLgaBp2PartListDto input)
        {
            var mainObj = ObjectMapper.Map<MstLgaBp2PartList>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstLgaBp2PartListDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_Bp2PartList_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstLgaBp2PartListDto>> GetAll(GetMstLgaBp2PartListInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartName), e => e.PartName.Contains(input.PartName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ShortName), e => e.ShortName.Contains(input.ShortName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine));

            var pageAndFiltered = filtered.OrderBy(s => s.PikSorting);

            var system = from o in pageAndFiltered
                         select new MstLgaBp2PartListDto
                         {
                             Id = o.Id,
                             PartName = o.PartName,
                             ShortName = o.ShortName,
                             ProdLine = o.ProdLine,
                             PikProcess = o.PikProcess,
                             PikSorting = o.PikSorting,
                             DelProcess = o.DelProcess,
                             DelSorting = o.DelSorting,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstLgaBp2PartListDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }

        public async Task<FileDto> GetBp2PartListToExcel(GetMstLgaBp2PartListExcelInput input)
        {
            var filtered = _repo.GetAll()
               .WhereIf(!string.IsNullOrWhiteSpace(input.PartName), e => e.PartName.Contains(input.PartName))
               .WhereIf(!string.IsNullOrWhiteSpace(input.ShortName), e => e.ShortName.Contains(input.ShortName))
               .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine));

            var pageAndFiltered = filtered.OrderBy(s => s.PikSorting);

            var query = from o in pageAndFiltered
                         select new MstLgaBp2PartListDto
                        {
                            Id = o.Id,
                            PartName = o.PartName,
                            ShortName = o.ShortName,
                            ProdLine = o.ProdLine,
                            PikProcess = o.PikProcess,
                            PikSorting = o.PikSorting,
                            DelProcess = o.DelProcess,
                            DelSorting = o.DelSorting,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        //Import Data From Excel
        public async Task<List<ImportMstLgaBp2PartListDto>> ImportBp2PartListFromExcel(List<ImportMstLgaBp2PartListDto> bp2partlists)
        {
            try
            {
                List<MstLgaBp2PartList_T> MstLgaBp2PartList = new List<MstLgaBp2PartList_T> { };
                foreach (var item in bp2partlists)
                {
                    MstLgaBp2PartList_T importData = new MstLgaBp2PartList_T();
                    {
                        importData.Guid = item.Guid;
                        importData.ProdLine = item.ProdLine;
                        importData.PartName = item.PartName;
                        importData.ShortName = item.ShortName;
                        importData.PikProcess = item.PikProcess;
                        importData.PikSorting = item.PikSorting;
                        importData.DelProcess = item.DelProcess;
                        importData.DelSorting = item.DelSorting;
                        importData.Remark = item.Remark;

                    }
                    MstLgaBp2PartList.Add(importData);
                }
                CurrentUnitOfWork.GetDbContext<prodDbContext>().AddRangeAsync(MstLgaBp2PartList);
                return null;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }

        //Merge Data From Temp To bp2partlist
        public async Task MergeDataBp2PartList(string v_Guid)
        {
            string _sql = "Exec MST_LGA_BP2_PARTLIST_MERGE @Guid";
            await _dapperRepo.QueryAsync<MstLgaBp2PartList>(_sql, new { Guid = v_Guid });
        }


    }
}
