using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Linq.Extensions;
using Abp.Collections.Extensions;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.Master.Cmm;
using prod.Master.Common.CarSeries.Dto;
using prod.Master.Common.CarSeries.Exporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prod.Master.Common.CarSeries
{
    [AbpAuthorize(AppPermissions.Pages_InvtSetup_ProductType_View)]
    public class MstCmmCarSeriesAppService : prodAppServiceBase, IMstCmmCarSeriesAppservice
    {
        private readonly IDapperRepository<MstCmmCarSeries, long> _dapperRepo;
        private readonly IRepository<MstCmmCarSeries, long> _repo;
        private readonly IMstCmmCarSeriesExcelExporter _calendarListExcelExporter;

        public MstCmmCarSeriesAppService(IRepository<MstCmmCarSeries, long> repo,
                                         IDapperRepository<MstCmmCarSeries, long> dapperRepo,
                                       IMstCmmCarSeriesExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

 

        //[AbpAuthorize(AppPermissions.Pages_InvtSetup_ProductType_Edit)]
        //public async Task CreateOrEdit(CreateOrEditMstCmmCarSeriesDto input)
        //{
        //	if (input.Id == null) await Create(input);
        //	else await Update(input);
        //}

        ////CREATE
        //private async Task Create(CreateOrEditMstCmmCarSeriesDto input)
        //{
        //	var mainObj = ObjectMapper.Map<MstCmmCarSeries>(input);

        //	await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        //}

        //// EDIT
        //private async Task Update(CreateOrEditMstCmmCarSeriesDto input)
        //{
        //	using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
        //	{
        //		var mainObj = await _repo.GetAll()
        //		.FirstOrDefaultAsync(e => e.Id == input.Id);

        //		var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
        //	}
        //}

        //[AbpAuthorize(AppPermissions.Pages_InvtSetup_ProductType_Delete)]
        //public async Task Delete(EntityDto input)
        //{
        //          var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
        //          _repo.HardDelete(mainObj);
        //          CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        //      }

        public async Task<PagedResultDto<MstCmmCarSeriesDto>> GetAll(GetMstCmmCarSeriesInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstCmmCarSeriesDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Name = o.Name,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstCmmCarSeriesDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetCarSeriesToExcel(MstCmmCarSeriesExportInput input)
        {
            var filtered = _repo.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);
            var system = from o in pageAndFiltered
                         select new MstCmmCarSeriesDto
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
        //    await _dapperRepo.ExecuteAsync(MstCmmCarSeriesConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
