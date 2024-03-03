using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.HistoricalData;
using prod.Inventory.GPS.Dto;
using prod.Master.Cmm.Dto;
using prod.Master.Cmm.Exporting;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.Cmm
{
    [AbpAuthorize(AppPermissions.Pages_Master_Cmm_EngineMaster_View)]
    public class MstCmmEngineMasterAppService : prodAppServiceBase, IMstCmmEngineMasterAppService
    {
        private readonly IDapperRepository<MstCmmEngineMaster, long> _dapperRepo;
        private readonly IRepository<MstCmmEngineMaster, long> _repo;
        private readonly IMstCmmEngineMasterExcelExporter _calendarListExcelExporter;
        private readonly IHistoricalDataAppService _historicalDataAppService;

        public MstCmmEngineMasterAppService(IRepository<MstCmmEngineMaster, long> repo,
                                         IDapperRepository<MstCmmEngineMaster, long> dapperRepo,
                                        IMstCmmEngineMasterExcelExporter calendarListExcelExporter,
                                        IHistoricalDataAppService historicalDataAppService
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
            _historicalDataAppService = historicalDataAppService;
        }

        public async Task<List<string>> GetMstCmmEngineMasterHistory(GetMstCmmEngineMasterHistoryInput input)
        {
            return await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
        }

        public async Task<FileDto> GetHistoricalDataToExcel(GetMstCmmEngineMasterHistoryExcelInput input)
        {
            var data = await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
            return _calendarListExcelExporter.ExportToHistoricalFile(data);
        }

        public async Task<List<long?>> GetChangedRecords()
        {
            return await _historicalDataAppService.GetChangedRecordIds("MstCmmEngineMaster");
        }

        [AbpAuthorize(AppPermissions.Pages_Master_Cmm_EngineMaster_CreateEdit)]
        public async Task CreateOrEdit(CreateOrEditMstCmmEngineMasterDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstCmmEngineMasterDto input)
        {
            var mainObj = ObjectMapper.Map<MstCmmEngineMaster>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstCmmEngineMasterDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Master_Cmm_EngineMaster_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstCmmEngineMasterDto>> GetAll(GetMstCmmEngineMasterInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.MaterialCode), e => e.MaterialCode.Contains(input.MaterialCode))
                .WhereIf(!string.IsNullOrWhiteSpace(input.TransmissionType), e => e.TransmissionType.Contains(input.TransmissionType))
                .WhereIf(!string.IsNullOrWhiteSpace(input.EngineModel), e => e.EngineModel.Contains(input.EngineModel))
                .WhereIf(!string.IsNullOrWhiteSpace(input.EngineType), e => e.EngineType.Contains(input.EngineType))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstCmmEngineMasterDto
                         {
                             Id = o.Id,
                             MaterialCode = o.MaterialCode,
                             ClassType = o.ClassType,
                             ClassName = o.ClassName,
                             TransmissionType = o.TransmissionType,
                             EngineModel = o.EngineModel,
                             EngineType = o.EngineType,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstCmmEngineMasterDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetEngineMasterToExcel(MstCmmEngineMasterExportInput input)
        {
            var query = from o in _repo.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MaterialCode), e => e.MaterialCode.Contains(input.MaterialCode))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TransmissionType), e => e.TransmissionType.Contains(input.TransmissionType))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EngineModel), e => e.EngineModel.Contains(input.EngineModel))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EngineType), e => e.EngineType.Contains(input.EngineType))
                        select new MstCmmEngineMasterDto
                        {
                            Id = o.Id,
                            MaterialCode = o.MaterialCode,
                            ClassType = o.ClassType,
                            ClassName = o.ClassName,
                            TransmissionType = o.TransmissionType,
                            EngineModel = o.EngineModel,
                            EngineType = o.EngineType,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstCmmEngineMasterConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}