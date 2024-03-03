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
using prod.Master.Welding.Dto;
using prod.Master.Welding.Exporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.Welding
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_Welding_Process)]
    public class MstWldProcessAppService : prodAppServiceBase, IMstWldProcessAppService
    {
        private readonly IDapperRepository<MstWldProcess, long> _dapperRepo;
        private readonly IRepository<MstWldProcess, long> _repo;
        private readonly IMstWldProcessExcelExporter _calendarListExcelExporter;

        public MstWldProcessAppService(IRepository<MstWldProcess, long> repo,
                                         IDapperRepository<MstWldProcess, long> dapperRepo,
                                        IMstWldProcessExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_Welding_Process_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstWldProcessDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstWldProcessDto input)
        {
            var mainObj = ObjectMapper.Map<MstWldProcess>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstWldProcessDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_Welding_Process_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstWldProcessDto>> GetAll(GetMstWldProcessInput input)
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
                         select new MstWldProcessDto
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

            return new PagedResultDto<MstWldProcessDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetProcessToExcel(MstWldProcessExportInput input)
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
                        select new MstWldProcessDto
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
        //    await _dapperRepo.ExecuteAsync(MstWldProcessConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
