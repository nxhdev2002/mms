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
using prod.Master.Inventory;
using prod.Master.Inv.Dto;

namespace prod.Master.Inv
{
    [AbpAuthorize(AppPermissions.Pages_Master_Gps_GpsSupplierInfo_View)]
    public class MstInvGpsSupplierInfoAppService : prodAppServiceBase, IMstInvGpsSupplierInfoAppService
    {
        private readonly IDapperRepository<MstInvGpsSupplierInfo, long> _dapperRepo;
        private readonly IRepository<MstInvGpsSupplierInfo, long> _repo;
        private readonly IRepository<MstInvGpsSupplierOrderTime, long> _repoOrder;
        private readonly IMstInvGpsSupplierInfoExcelExporter _calendarListExcelExporter;

        public MstInvGpsSupplierInfoAppService(IRepository<MstInvGpsSupplierInfo, long> repo,
                                        IRepository<MstInvGpsSupplierOrderTime, long> repoOrder,
                                        IDapperRepository<MstInvGpsSupplierInfo, long> dapperRepo,
                                        IMstInvGpsSupplierInfoExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _repoOrder = repoOrder;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_Master_Gps_GpsSupplierInfo_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstInvGpsSupplierInfoDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstInvGpsSupplierInfoDto input)
        {
            var mainObj = ObjectMapper.Map<MstInvGpsSupplierInfo>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstInvGpsSupplierInfoDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Master_Gps_GpsSupplierInfo_Edit)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstInvGpsSupplierInfoDto>> GetAllSupplierInfo(GetMstInvGpsSupplierInfoInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierCode), e => e.SupplierCode.Contains(input.SupplierCode))
                .WhereIf(!string.IsNullOrWhiteSpace(input.DeliveryMethod), e => e.DeliveryMethod.Contains(input.DeliveryMethod))
                .WhereIf(!string.IsNullOrWhiteSpace(input.DeliveryFrequency), e => e.DeliveryFrequency.Contains(input.DeliveryFrequency))
                .WhereIf(!string.IsNullOrWhiteSpace(input.OrderDateType), e => e.OrderDateType.Contains(input.OrderDateType))
                .WhereIf(!string.IsNullOrWhiteSpace(input.KeihenType), e => e.KeihenType.Contains(input.KeihenType))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstInvGpsSupplierInfoDto
                         {
                             Id = o.Id,
                             SupplierCode = o.SupplierCode,
                             SupplierPlantCode = o.SupplierPlantCode,
                             SupplierName = o.SupplierName,
                             Address = o.Address,
                             DockX = o.DockX,
                             DockXAddress = o.DockXAddress,
                             DeliveryMethod = o.DeliveryMethod,
                             DeliveryFrequency = o.DeliveryFrequency,
                             Cd = o.Cd,
                             OrderDateType = o.OrderDateType,
                             KeihenType = o.KeihenType,
                             StkConceptTmvMin = o.StkConceptTmvMin,
                             StkConceptTmvMax = o.StkConceptTmvMax,
                             StkConceptSupMMin = o.StkConceptSupMMin,
                             StkConceptSupMMax = o.StkConceptSupMMax,
                             StkConceptSupPMin = o.StkConceptSupPMin,
                             StkConceptSupPMax = o.StkConceptSupPMax,
                             TmvProductPercentage = o.TmvProductPercentage,
                             PicMainId = o.PicMainId,
                             DeliveryLt = o.DeliveryLt,
                             ProductionShift = o.ProductionShift,
                             TcFrom = o.TcFrom,
                             TcTo = o.TcTo,
                             OrderTrip = o.OrderTrip,
                             SupplierNameEn = o.SupplierNameEn,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstInvGpsSupplierInfoDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetGpsSupplierInfoToExcel(MstInvGpsSupplierInfoExportInput input)
        {
            var query = from o in _repo.GetAll()
                        select new MstInvGpsSupplierInfoDto
                        {
                            Id = o.Id,
                            SupplierCode = o.SupplierCode,
                            SupplierPlantCode = o.SupplierPlantCode,
                            SupplierName = o.SupplierName,
                            Address = o.Address,
                            DockX = o.DockX,
                            DockXAddress = o.DockXAddress,
                            DeliveryMethod = o.DeliveryMethod,
                            DeliveryFrequency = o.DeliveryFrequency,
                            Cd = o.Cd,
                            OrderDateType = o.OrderDateType,
                            KeihenType = o.KeihenType,
                            StkConceptTmvMin = o.StkConceptTmvMin,
                            StkConceptTmvMax = o.StkConceptTmvMax,
                            StkConceptSupMMin = o.StkConceptSupMMin,
                            StkConceptSupMMax = o.StkConceptSupMMax,
                            StkConceptSupPMin = o.StkConceptSupPMin,
                            StkConceptSupPMax = o.StkConceptSupPMax,
                            TmvProductPercentage = o.TmvProductPercentage,
                            PicMainId = o.PicMainId,
                            DeliveryLt = o.DeliveryLt,
                            ProductionShift = o.ProductionShift,
                            TcFrom = o.TcFrom,
                            TcTo = o.TcTo,
                            OrderTrip = o.OrderTrip,
                            SupplierNameEn = o.SupplierNameEn,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        public async Task<PagedResultDto<MstInvGpsSupplierOrderTimeDto>> GetSupplierOrderTimeBySupplierId(GetMstInvGpsSupplierOrderTimeInput input)
        {
            var filtered = _repoOrder.GetAll()
                .Where(e => e.SupplierId == input.SupplierId)
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstInvGpsSupplierOrderTimeDto
                         {
                             Id = o.Id,
                             SupplierId = o.SupplierId,
                             OrderSeq = o.OrderSeq,
                             OrderType = o.OrderType,
                             OrderTime = o.OrderTime,
                             ReceivingDay = o.ReceivingDay,
                             ReceiveTime = o.ReceiveTime,
                             KeihenTime = o.KeihenTime,
                             KeihenDay = o.KeihenDay,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstInvGpsSupplierOrderTimeDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }
    }
}
