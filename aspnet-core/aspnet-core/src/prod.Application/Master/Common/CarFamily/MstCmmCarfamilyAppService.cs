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
using prod.Master.Common.Dto;
using prod.Master.Common.Exporting;
using prod.Master.Common.CarSeries.Dto;

namespace prod.Master.Common
{
    [AbpAuthorize(AppPermissions.Pages_InvtSetup_Carfamily_View)]
    public class MstCmmCarfamilyAppService : prodAppServiceBase, IMstCmmCarfamilyAppService
    {
        private readonly IDapperRepository<MstCmmCarfamily, long> _dapperRepo;
        private readonly IRepository<MstCmmCarfamily, long> _repo;
        private readonly IMstCmmCarfamilyExcelExporter _calendarListExcelExporter;

        public MstCmmCarfamilyAppService(IRepository<MstCmmCarfamily, long> repo,
                                         IDapperRepository<MstCmmCarfamily, long> dapperRepo,
                                        IMstCmmCarfamilyExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        /*[AbpAuthorize(AppPermissions.Pages_Master_Common_Carfamily_CreateEdit)]
        public async Task CreateOrEdit(CreateOrEditMstCmmCarfamilyDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstCmmCarfamilyDto input)
        {
            var mainObj = ObjectMapper.Map<MstCmmCarfamily>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstCmmCarfamilyDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Master_Common_Carfamily_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }*/

        public async Task<PagedResultDto<MstCmmCarfamilyDto>> GetAll(GetMstCmmCarfamilyInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstCmmCarfamilyDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Name = o.Name,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstCmmCarfamilyDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetCarfamilyToExcel(MstCmmCarfamilyExportInput input)
        {
            var filtered = _repo.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);
            var system = from o in pageAndFiltered
                         select new MstCmmCarfamilyDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Name = o.Name,

                         };
            var exportToExcel = await system.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstCmmCarfamilyConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
