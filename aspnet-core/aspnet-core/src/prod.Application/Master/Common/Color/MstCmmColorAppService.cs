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
using prod.Master.Cmm.Dto;
using prod.Master.Cmm.Exporting;
using prod.Inventory.CKD.Dto;
using prod.HistoricalData;

namespace prod.Master.Cmm
{
    [AbpAuthorize(AppPermissions.Pages_Master_Common_Color_View)]
    public class MstCmmColorAppService : prodAppServiceBase, IMstCmmColorAppService
    {
        private readonly IDapperRepository<MstCmmColor, long> _dapperRepo;
        private readonly IRepository<MstCmmColor, long> _repo;
        private readonly IMstCmmColorExcelExporter _calendarListExcelExporter;
        private readonly IHistoricalDataAppService _historicalDataAppService;

        public MstCmmColorAppService(IRepository<MstCmmColor, long> repo,
                                         IDapperRepository<MstCmmColor, long> dapperRepo,
                                        IMstCmmColorExcelExporter calendarListExcelExporter,
                                        IHistoricalDataAppService historicalDataAppService
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
            _historicalDataAppService = historicalDataAppService;
        }

        public async Task<List<string>> GetMstCmmColorHistory(GetMstCmmColorHistoryInput input)
        {
            return await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
        }

        public async Task<FileDto> GetHistoricalDataToExcel(GetMstCmmColorHistoryExcelInput input)
        {
            var data = await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
            return _calendarListExcelExporter.ExportToHistoricalFile(data);
        }

        public async Task<List<long?>> GetChangedRecords()
        {
            /*            var data = await _historicalDataAppService.GetChangedRecordIds("InvCkdProdPlanDaily");
                        return data;*/
            return await _historicalDataAppService.GetChangedRecordIds("MstCmmColor");
        }


        [AbpAuthorize(AppPermissions.Pages_Master_Common_Color_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstCmmColorDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstCmmColorDto input)
        {
            var mainObj = ObjectMapper.Map<MstCmmColor>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstCmmColorDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Master_Common_Color_Edit)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstCmmColorDto>> GetAll(GetMstCmmColorInput input)
        {
            string _sql = "Exec MST_CMM_COLOR_SEARCH @p_Color, @p_NameEn, @p_NameVn, @p_ColorType";

            IEnumerable<MstCmmColorDto> result = await _dapperRepo.QueryAsync<MstCmmColorDto>(_sql, new
            {
                p_Color = input.Color,
                p_NameEn = input.NameEn,
                p_NameVn = input.NameVn,
                p_ColorType = input.ColorType,
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<MstCmmColorDto>(
                totalCount,
                pagedAndFiltered);
        }


        public async Task<FileDto> GetColorToExcel(MstCmmColorExportInput input)
        {
            string _sql = "Exec MST_CMM_COLOR_SEARCH @p_Color, @p_NameEn, @p_NameVn, @p_ColorType";

            IEnumerable<MstCmmColorDto> result = await _dapperRepo.QueryAsync<MstCmmColorDto>(_sql, new
            {
                p_Color = input.Color,
                p_NameEn = input.NameEn,
                p_NameVn = input.NameVn,
                p_ColorType = input.ColorType,
            });

            var listResult = result.ToList(); 

            return _calendarListExcelExporter.ExportToFile(listResult);
        }
    }
}
