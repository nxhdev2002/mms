using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using AutoMapper.Internal.Mappers;
using Dapper;
using Microsoft.EntityFrameworkCore;
using NUglify.Helpers;
using prod.Authorization;
using prod.Dto;
using prod.Master.Common.Dto;
using prod.Master.Common.Exporting;
using prod.Master.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.EntityFrameworkCore;


namespace prod.Master.Common
{
    [AbpAuthorize(AppPermissions.Pages_InvtSetup_FuelType_View)]
    public class MstCmmFuelTypeAppService : prodAppServiceBase, IMstCmmFuelTypeAppService
    {
        private readonly IDapperRepository<MstCmmFuelType, long> _dapperRepo;
        private readonly IRepository<MstCmmFuelType, long> _repo;
        private readonly IMstCmmFuelTypeExcelExporter _calendarListExcelExporter;

        public MstCmmFuelTypeAppService(IRepository<MstCmmFuelType, long> repo,
                                         IDapperRepository<MstCmmFuelType, long> dapperRepo,
                                        IMstCmmFuelTypeExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //[AbpAuthorize(AppPermissions.Pages_InvtSetup_FuelType_Edit)]
        //public async Task CreateOrEdit(CreateOrEditMstCmmFuelTypeDto input)
        //{
        //    if (input.Id == null) await Create(input);
        //    else await Update(input);
        //}

        ////CREATE
        //private async Task Create(CreateOrEditMstCmmFuelTypeDto input)
        //{
        //    var mainObj = ObjectMapper.Map<MstCmmFuelType>(input);

        //    await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        //}

        //// EDIT
        //private async Task Update(CreateOrEditMstCmmFuelTypeDto input)
        //{
        //    using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
        //    {
        //        var mainObj = await _repo.GetAll()
        //        .FirstOrDefaultAsync(e => e.Id == input.Id);

        //        var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
        //    }
        //}

        //[AbpAuthorize(AppPermissions.Pages_InvtSetup_FuelType_Delete)]
        //public async Task Delete(EntityDto input)
        //{
        //    var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
        //    _repo.HardDelete(mainObj);
        //    CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        //}

        public async Task<PagedResultDto<MstCmmFuelTypeDto>> GetAll(GetMstCmmFuelTypeInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name))
                .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstCmmFuelTypeDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Name = o.Name,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstCmmFuelTypeDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetFuelTypeToExcel(MstCmmFuelTypeExportInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name))
                .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);
            var query = from o in pageAndFiltered
                         select new MstCmmFuelTypeDto
                        {
                            Id = o.Id,
                            Code = o.Code,
                            Name = o.Name,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstCmmFuelTypeConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
