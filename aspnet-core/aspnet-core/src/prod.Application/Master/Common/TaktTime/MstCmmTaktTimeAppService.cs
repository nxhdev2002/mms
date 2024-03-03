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

namespace prod.Master.Cmm
{
    [AbpAuthorize(AppPermissions.Pages_Master_Common_TaktTime_View)]
    public class MstCmmTaktTimeAppService : prodAppServiceBase, IMstCmmTaktTimeAppService
    {
        private readonly IDapperRepository<MstCmmTaktTime, long> _dapperRepo;
        private readonly IRepository<MstCmmTaktTime, long> _repo;
        private readonly IMstCmmTaktTimeExcelExporter _calendarListExcelExporter;

        public MstCmmTaktTimeAppService(IRepository<MstCmmTaktTime, long> repo,
                                         IDapperRepository<MstCmmTaktTime, long> dapperRepo,
                                        IMstCmmTaktTimeExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_Master_Common_TaktTime_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstCmmTaktTimeDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstCmmTaktTimeDto input)
        {
            var mainObj = ObjectMapper.Map<MstCmmTaktTime>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstCmmTaktTimeDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Master_Common_TaktTime_Edit)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstCmmTaktTimeDto>> GetAll(GetMstCmmTaktTimeInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                .WhereIf(!string.IsNullOrWhiteSpace(input.GroupCd), e => e.GroupCd.Contains(input.GroupCd))
                .WhereIf(!string.IsNullOrWhiteSpace(input.TaktTime), e => e.TaktTime.Contains(input.TaktTime))
                .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstCmmTaktTimeDto
                         {
                             Id = o.Id,
                             ProdLine = o.ProdLine,
                             GroupCd = o.GroupCd,
                             TaktTimeSecond = o.TaktTimeSecond,
                             TaktTime = o.TaktTime,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstCmmTaktTimeDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetTaktTimeToExcel(MstCmmTaktTimeExportInput input)
        {
            var filtered = _repo.GetAll()
               .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
               .WhereIf(!string.IsNullOrWhiteSpace(input.GroupCd), e => e.GroupCd.Contains(input.GroupCd))
               .WhereIf(!string.IsNullOrWhiteSpace(input.TaktTime), e => e.TaktTime.Contains(input.TaktTime))
               .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive))
               ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var query = from o in pageAndFiltered
                         select new MstCmmTaktTimeDto
                        {
                            Id = o.Id,
                            ProdLine = o.ProdLine,
                            GroupCd = o.GroupCd,
                            TaktTimeSecond = o.TaktTimeSecond,
                            TaktTime = o.TaktTime,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstCmmTaktTimeConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
