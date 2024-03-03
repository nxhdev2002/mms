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
    //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_Trip)]
    public class MstLgaLdsTripAppService : prodAppServiceBase, IMstLgaLdsTripAppService
    {
        private readonly IDapperRepository<MstLgaLdsTrip, long> _dapperRepo;
        private readonly IRepository<MstLgaLdsTrip, long> _repo;
        private readonly IMstLgaLdsTripExcelExporter _calendarListExcelExporter;

        public MstLgaLdsTripAppService(IRepository<MstLgaLdsTrip, long> repo,
                                         IDapperRepository<MstLgaLdsTrip, long> dapperRepo,
                                        IMstLgaLdsTripExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_Trip_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstLgaLdsTripDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstLgaLdsTripDto input)
        {
            var mainObj = ObjectMapper.Map<MstLgaLdsTrip>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstLgaLdsTripDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_Trip_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstLgaLdsTripDto>> GetAll(GetMstLgaLdsTripInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                .WhereIf(!string.IsNullOrWhiteSpace(input.DeliveryName), e => e.DeliveryName.Contains(input.DeliveryName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.DollyName), e => e.DollyName.Contains(input.DollyName));

            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var system = from o in pageAndFiltered
                         select new MstLgaLdsTripDto
                         {
                             Id = o.Id,
                             ProdLine = o.ProdLine,
                             DeliveryNo = o.DeliveryNo,
                             DeliveryName = o.DeliveryName,
                             Model = o.Model,
                             TripNumber = o.TripNumber,
                             DollyName = o.DollyName,
                             TaktTime = o.TaktTime,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstLgaLdsTripDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }

        public async Task<FileDto> GetTripToExcel(GetMstLgaLdsTripExcelInput input)
        {
            var filtered = _repo.GetAll()
               .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
               .WhereIf(!string.IsNullOrWhiteSpace(input.DeliveryName), e => e.DeliveryName.Contains(input.DeliveryName))
               .WhereIf(!string.IsNullOrWhiteSpace(input.DollyName), e => e.DollyName.Contains(input.DollyName));

            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var query = from o in pageAndFiltered
                         select new MstLgaLdsTripDto
                        {
                            Id = o.Id,
                            ProdLine = o.ProdLine,
                            DeliveryNo = o.DeliveryNo,
                            DeliveryName = o.DeliveryName,
                            Model = o.Model,
                            TripNumber = o.TripNumber,
                            DollyName = o.DollyName,
                            TaktTime = o.TaktTime,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
