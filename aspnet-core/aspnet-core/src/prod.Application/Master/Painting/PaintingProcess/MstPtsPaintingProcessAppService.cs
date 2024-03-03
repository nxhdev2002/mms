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
using prod.Master.Painting.Dto;
using prod.Master.Painting.PaintingProcess.Exporting;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.Painting
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_Painting_PaintingProcess)]
    public class MstPtsPaintingProcessAppService : prodAppServiceBase, IMstPtsPaintingProcessAppService
    {
        private readonly IDapperRepository<MstPtsPaintingProcess, long> _dapperRepo;
        private readonly IRepository<MstPtsPaintingProcess, long> _repo;
        private readonly IMstPtsPaintingProcessExcelExporter _calendarListExcelExporter;

        public MstPtsPaintingProcessAppService(IRepository<MstPtsPaintingProcess, long> repo,
                                         IDapperRepository<MstPtsPaintingProcess, long> dapperRepo,
                                        IMstPtsPaintingProcessExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_Painting_PaintingProcess_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstPtsPaintingProcessDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstPtsPaintingProcessDto input)
        {
            var mainObj = ObjectMapper.Map<MstPtsPaintingProcess>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstPtsPaintingProcessDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_Painting_PaintingProcess_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstPtsPaintingProcessDto>> GetAll(GetMstPtsPaintingProcessInput input)
        {
            var filtered = _repo.GetAll()
            // .WhereIf(input.ProcessCode.HasValue, t => t.ProcessCode == input.ProcessCode.Value)
            .WhereIf(!string.IsNullOrWhiteSpace(input.ProcessCode), e => e.ProcessCode.Contains(input.ProcessCode))
            .WhereIf(!string.IsNullOrWhiteSpace(input.ProcessName), e => e.ProcessName.Contains(input.ProcessName));

            var pageAndFiltered = filtered.OrderBy(s => s.ProcessSeq);


            var system = from o in pageAndFiltered
                         select new MstPtsPaintingProcessDto
                         {
                             Id = o.Id,
                             ProcessSeq = o.ProcessSeq,
                             ProcessCode = o.ProcessCode,
                             ProcessName = o.ProcessName,
                             ProcessDesc = o.ProcessDesc,
                             ProcessGroup = o.ProcessGroup,
                             GroupName = o.GroupName,
                             ProcessSubgroup = o.ProcessSubgroup,
                             SubgroupName = o.SubgroupName,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstPtsPaintingProcessDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetPaintingProcessToExcel(MstPtsPaintingProcessExportInput input)
        {
            var filtered = _repo.GetAll()
            // .WhereIf(input.ProcessCode.HasValue, t => t.ProcessCode == input.ProcessCode.Value)
            .WhereIf(!string.IsNullOrWhiteSpace(input.ProcessCode), e => e.ProcessCode.Contains(input.ProcessCode))
            .WhereIf(!string.IsNullOrWhiteSpace(input.ProcessName), e => e.ProcessName.Contains(input.ProcessName));

            var pageAndFiltered = filtered.OrderBy(s => s.ProcessSeq);


            var query = from o in pageAndFiltered
                        select new MstPtsPaintingProcessDto
                        {
                            Id = o.Id,
                            ProcessSeq = o.ProcessSeq,
                            ProcessCode = o.ProcessCode,
                            ProcessName = o.ProcessName,
                            ProcessDesc = o.ProcessDesc,
                            ProcessGroup = o.ProcessGroup,
                            GroupName = o.GroupName,
                            ProcessSubgroup = o.ProcessSubgroup,
                            SubgroupName = o.SubgroupName,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstPtsPaintingProcessConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
