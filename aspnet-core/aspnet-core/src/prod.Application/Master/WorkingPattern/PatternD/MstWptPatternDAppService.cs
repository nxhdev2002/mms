using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Master.WorkingPattern.Dto;
using prod.Master.WorkingPattern.Exporting;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.WorkingPattern
{
    [AbpAuthorize(AppPermissions.Pages_WorkingPattern_PatternD_View)]
    public class MstWptPatternDAppService : prodAppServiceBase, IMstWptPatternDAppService
    {
        private readonly IDapperRepository<MstWptPatternD, long> _dapperRepo;
        private readonly IRepository<MstWptPatternD, long> _repo;
        private readonly IMstWptPatternDExcelExporter _calendarListExcelExporter;

        public MstWptPatternDAppService(IRepository<MstWptPatternD, long> repo,
                                         IDapperRepository<MstWptPatternD, long> dapperRepo,
                                        IMstWptPatternDExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_WorkingPattern_PatternD_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstWptPatternDDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstWptPatternDDto input)
        {
            if (input.StartTime <= input.EndTime)
            {
                var mainObj = ObjectMapper.Map<MstWptPatternD>(input);

                await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
            }
            else
            {
                throw new UserFriendlyException(400, L("End Time phải lớn hơn Start Time"));
            }

        }

        // EDIT
        private async Task Update(CreateOrEditMstWptPatternDDto input)
        {
            if (input.StartTime <= input.EndTime)
            {
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    var mainObj = await _repo.GetAll()
                    .FirstOrDefaultAsync(e => e.Id == input.Id);

                    var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
                }
            }
            else
            {
                throw new UserFriendlyException(400, L("End Date phải lớn hơn Start Time "));
            }
        }

        [AbpAuthorize(AppPermissions.Pages_WorkingPattern_PatternD_Edit)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstWptPatternDDto>> GetAll(GetMstWptPatternDInput input)
        {
            var filtered = _repo.GetAll()
            .WhereIf(!string.IsNullOrWhiteSpace(input.ShiftName), e => e.ShiftName.Contains(input.ShiftName));
            var pageAndFiltered = filtered.OrderBy(s => s.PatternHId);


            var system = from o in pageAndFiltered
                         select new MstWptPatternDDto
                         {
                             Id = o.Id,
                             PatternHId = o.PatternHId,
                             ShiftNo = o.ShiftNo,
                             ShiftName = o.ShiftName,
                             StartTime = o.StartTime,
                             EndTime = o.EndTime,
                             DayOfWeek = o.DayOfWeek,
                             SeasonType = (
                               o.SeasonType == "L" ? "Low" :
                               o.SeasonType == "H" ? "High" :
                               o.SeasonType == "N" ? "Normal" : "Unknown"
                             ),
                             IsActive = o.IsActive,

                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstWptPatternDDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetPatternDToExcel(MstWptPatternDExportInput input)
        {
            var filtered = _repo.GetAll()
            .WhereIf(!string.IsNullOrWhiteSpace(input.ShiftName), e => e.ShiftName.Contains(input.ShiftName));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var query = from o in pageAndFiltered
                        select new MstWptPatternDDto
                        {
                            Id = o.Id,
                            PatternHId = o.PatternHId,
                            ShiftNo = o.ShiftNo,
                            ShiftName = o.ShiftName,
                            StartTime = o.StartTime,
                            EndTime = o.EndTime,
                            DayOfWeek = o.DayOfWeek,
                            SeasonType = o.SeasonType,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstWptPatternDConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
