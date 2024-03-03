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
using prod.Inventory.GPS.Dto;
using prod.Inventory.GPS.Exporting;
using prod.Inventory.GPS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.EntityFrameworkCore;

namespace prod.Inventory.GPS
{
    [AbpAuthorize(AppPermissions.Pages_Gps_Order_DailyOrder_View)]
    public class InvGpsDailyOrderAppService : prodAppServiceBase, IInvGpsDailyOrderAppService
    {
        private readonly IDapperRepository<InvGpsDailyOrder, long> _dapperRepo;
        private readonly IRepository<InvGpsDailyOrder, long> _repo;
        private readonly IInvGpsDailyOrderExcelExporter _calendarListExcelExporter;

        public InvGpsDailyOrderAppService(IRepository<InvGpsDailyOrder, long> repo,
                                         IDapperRepository<InvGpsDailyOrder, long> dapperRepo,
                                        IInvGpsDailyOrderExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_Gps_Order_DailyOrder_Edit)]
        public async Task CreateOrEdit(CreateOrEditInvGpsDailyOrderDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditInvGpsDailyOrderDto input)
        {
            var mainObj = ObjectMapper.Map<InvGpsDailyOrder>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditInvGpsDailyOrderDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

       [AbpAuthorize(AppPermissions.Pages_Gps_Order_DailyOrder_Edit)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<InvGpsDailyOrderDto>> GetAll(GetInvGpsDailyOrderInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom.Value.Date <= t.WorkingDate)
                .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo.Value.Date.AddDays(1) > t.WorkingDate)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Shift), e => e.Shift.Contains(input.Shift))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierCode), e => e.SupplierCode.Contains(input.SupplierCode))
                .WhereIf(!string.IsNullOrWhiteSpace(input.OrderNo), e => e.OrderNo.Contains(input.OrderNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.TruckNo), e => e.TruckNo.Contains(input.TruckNo));

            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate);

            var system = from o in pageAndFiltered
                         select new InvGpsDailyOrderDto
                         {
                             Id = o.Id,
                             WorkingDate = o.WorkingDate,
                             Shift = o.Shift,
                             SupplierName = o.SupplierName,
                             SupplierCode = o.SupplierCode,
                             OrderNo = o.OrderNo,
                             OrderDatetime = o.OrderDatetime,
                             TripNo = o.TripNo,
                             TruckNo = o.TruckNo,
                             EstArrivalDatetime = o.EstArrivalDatetime,                            
                             TruckUnloadingId = o.TruckUnloadingId,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<InvGpsDailyOrderDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetInvGpsDailyOrderToExcel(InvGpsDailyOrderExportInput input)
        {
            var filtered = _repo.GetAll()
               .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom.Value.Date <= t.WorkingDate)
                .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo.Value.Date.AddDays(1) > t.WorkingDate)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Shift), e => e.Shift.Contains(input.Shift))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierCode), e => e.SupplierCode.Contains(input.SupplierCode))
                .WhereIf(!string.IsNullOrWhiteSpace(input.OrderNo), e => e.OrderNo.Contains(input.OrderNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.TruckNo), e => e.TruckNo.Contains(input.TruckNo));

            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate);

            var query = from o in pageAndFiltered
                        select new InvGpsDailyOrderDto
                        {
                            Id = o.Id,
                            WorkingDate = o.WorkingDate,
                            Shift = o.Shift,
                            SupplierName = o.SupplierName,
                            SupplierCode = o.SupplierCode,
                            OrderNo = o.OrderNo,
                            OrderDatetime = o.OrderDatetime,
                            TripNo = o.TripNo,
                            TruckNo = o.TruckNo,
                            EstArrivalDatetime = o.EstArrivalDatetime,                         
                            TruckUnloadingId = o.TruckUnloadingId,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
