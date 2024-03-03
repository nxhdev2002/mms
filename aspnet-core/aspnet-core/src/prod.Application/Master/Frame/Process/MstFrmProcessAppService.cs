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
using prod.Master.Frame.Dto;
using prod.Master.Frame.Exporting;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.Frame
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_Frame_Process)]
    public class MstFrmProcessAppService : prodAppServiceBase, IMstFrmProcessAppService
    {
        private readonly IDapperRepository<MstFrmProcess, long> _dapperRepo;
        private readonly IRepository<MstFrmProcess, long> _repo;
        private readonly IMstFrmProcessExcelExporter _calendarListExcelExporter;

        public MstFrmProcessAppService(IRepository<MstFrmProcess, long> repo,
                                         IDapperRepository<MstFrmProcess, long> dapperRepo,
                                        IMstFrmProcessExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_Frame_Process_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstFrmProcessDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstFrmProcessDto input)
        {
            var mainObj = ObjectMapper.Map<MstFrmProcess>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstFrmProcessDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_Frame_Process_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstFrmProcessDto>> GetAll(GetMstFrmProcessInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProcessCode), e => e.ProcessCode.Contains(input.ProcessCode))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProcessName), e => e.ProcessName.Contains(input.ProcessName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProcessDesc), e => e.ProcessDesc.Contains(input.ProcessDesc))
                .WhereIf(!string.IsNullOrWhiteSpace(input.GroupName), e => e.GroupName.Contains(input.GroupName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SubgroupName), e => e.SubgroupName.Contains(input.SubgroupName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstFrmProcessDto
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

            return new PagedResultDto<MstFrmProcessDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetProcessToExcel(MstFrmProcessExportInput input)
        {
            var filtered = _repo.GetAll()
               .WhereIf(!string.IsNullOrWhiteSpace(input.ProcessCode), e => e.ProcessCode.Contains(input.ProcessCode))
               .WhereIf(!string.IsNullOrWhiteSpace(input.ProcessName), e => e.ProcessName.Contains(input.ProcessName))
               .WhereIf(!string.IsNullOrWhiteSpace(input.ProcessDesc), e => e.ProcessDesc.Contains(input.ProcessDesc))
               .WhereIf(!string.IsNullOrWhiteSpace(input.GroupName), e => e.GroupName.Contains(input.GroupName))
               .WhereIf(!string.IsNullOrWhiteSpace(input.SubgroupName), e => e.SubgroupName.Contains(input.SubgroupName))
               .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive))
               ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var query = from o in pageAndFiltered
                        select new MstFrmProcessDto
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
        //    await _dapperRepo.ExecuteAsync(MstFrmProcessConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
