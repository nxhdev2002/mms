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
using prod;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Master.LogA;
using prod.Master.LogA.Dto;
using prod.Master.LogA.Exporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;


namespace prod.Master.LogA
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_BarUser)]
    public class MstLgaBarUserAppService : prodAppServiceBase, IMstLgaBarUserAppService
    {
        private readonly IDapperRepository<MstLgaBarUser, long> _dapperRepo;
        private readonly IRepository<MstLgaBarUser, long> _repo;
        private readonly IMstLgaBarUserExcelExporter _calendarListExcelExporter;

        public MstLgaBarUserAppService(IRepository<MstLgaBarUser, long> repo,
                                         IDapperRepository<MstLgaBarUser, long> dapperRepo,
                                        IMstLgaBarUserExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_BarUser_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstLgaBarUserDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstLgaBarUserDto input)
        {
            var mainObj = ObjectMapper.Map<MstLgaBarUser>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstLgaBarUserDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_BarUser_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstLgaBarUserDto>> GetAll(GetMstLgaBarUserInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.UserName), e => e.UserName.Contains(input.UserName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProcessCode), e => e.ProcessCode.Contains(input.ProcessCode))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstLgaBarUserDto
                         {
                             Id = o.Id,
                             UserId = o.UserId,
                             UserName = o.UserName,
                             UserDescription = o.UserDescription,
                             IsNeedPass = o.IsNeedPass,
                             Pwd = o.Pwd,
                             ProcessId = o.ProcessId,
                             ProcessCode = o.ProcessCode,
                             ProcessGroup = o.ProcessGroup,
                             ProcessSubgroup = o.ProcessSubgroup,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstLgaBarUserDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetBarUserToExcel(GetMstLgaBarUserExcelInput input)
        {
            var filtered = _repo.GetAll()
              .WhereIf(!string.IsNullOrWhiteSpace(input.UserName), e => e.UserName.Contains(input.UserName))
              .WhereIf(!string.IsNullOrWhiteSpace(input.ProcessCode), e => e.ProcessCode.Contains(input.ProcessCode))
              ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var query = from o in pageAndFiltered
                         select new MstLgaBarUserDto
                        {
                            Id = o.Id,
                            UserId = o.UserId,
                            UserName = o.UserName,
                            UserDescription = o.UserDescription,
                            IsNeedPass = o.IsNeedPass,
                            Pwd = o.Pwd,
                            ProcessId = o.ProcessId,
                            ProcessCode = o.ProcessCode,
                            ProcessGroup = o.ProcessGroup,
                            ProcessSubgroup = o.ProcessSubgroup,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }
    }
}
