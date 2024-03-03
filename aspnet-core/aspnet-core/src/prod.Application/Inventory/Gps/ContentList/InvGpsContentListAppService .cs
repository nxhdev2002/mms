using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Inventory.GPS;
using prod.Inventory.GPS.Dto;
using prod.Inventory.GPS.Exporting;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace vovina.Inventory.GPS
{
    [AbpAuthorize(AppPermissions.Pages_Gps_Receive_ContentList_View)]
    public class InvGpsContentListAppService : prodAppServiceBase, IInvGpsContentListAppService
    {
        private readonly IDapperRepository<InvGpsContentList, long> _dapperRepo;
        private readonly IRepository<InvGpsContentList, long> _repo;
        private readonly IInvGpsContentListExcelExporter _calendarListExcelExporter;

        public InvGpsContentListAppService(IRepository<InvGpsContentList, long> repo,
                                         IDapperRepository<InvGpsContentList, long> dapperRepo,
                                        IInvGpsContentListExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_Gps_Receive_ContentList_Edit)]
        public async Task CreateOrEdit(CreateOrEditInvGpsContentListDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditInvGpsContentListDto input)
        {
            var mainObj = ObjectMapper.Map<InvGpsContentList>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditInvGpsContentListDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll().FirstOrDefaultAsync(e => e.Id == input.Id);
                mainObj.WorkingDate = input.WorkingDate;
                mainObj.Shift = input.Shift;
                mainObj.SupplierName = input.SupplierName;
                mainObj.SupplierCode = input.SupplierCode;
                mainObj.OrderNo = input.OrderNo;
                mainObj.OrderDatetime = input.OrderDatetime;
                mainObj.TripNo = input.TripNo;
                mainObj.EstArrivalDatetime = input.EstArrivalDatetime;
                mainObj.Status = input.Status;
                mainObj.IsActive = input.IsActive;

                //var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Gps_Receive_ContentList_Edit)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<InvGpsContentListDto>> GetAll(GetInvGpsContentListInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;

            var filtered = _repo.GetAll()
                .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
                .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom.Value.Date <= t.WorkingDate)
                .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo.Value.Date.AddDays(1) > t.WorkingDate)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Shift), e => e.Shift.Contains(input.Shift))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierCode), e => e.SupplierCode.Contains(input.SupplierCode))
                .WhereIf(!string.IsNullOrWhiteSpace(input.DockNo), e => e.DockNo.Contains(input.DockNo));

            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate);

            var system = from o in pageAndFiltered
                         select new InvGpsContentListDto
                         {
                             Id = o.Id,
                             WorkingDate = o.WorkingDate,
                             Shift = o.Shift,
                             SupplierName = o.SupplierName,
                             SupplierCode = o.SupplierCode,
                             RenbanNo = o.RenbanNo,
                             PcAddress = o.PcAddress,
                             DockNo = o.DockNo,
                             OrderNo = o.OrderNo,
                             OrderDatetime = o.OrderDatetime,
                             TripNo = o.TripNo,
                             PalletBoxQty = o.PalletBoxQty,
                             EstPackingDatetime = o.EstPackingDatetime,
                             EstArrivalDatetime = o.EstArrivalDatetime,
                             ContentNo = o.ContentNo,
                             OrderId = o.OrderId,
                             PalletSize = o.PalletSize,
                             IsPalletOnly = o.IsPalletOnly,
                             PackagingType = o.PackagingType,
                             IsAdhocReceiving = o.IsAdhocReceiving,
                             GeneratedBy = o.GeneratedBy,
                             UnpackStatus = o.UnpackStatus,
                             ModuleCd = o.ModuleCd,
                             ModuleRunNo = o.ModuleRunNo,
                             UpStartAct = o.UpStartAct,
                             UpFinishAct = o.UpFinishAct,
                             UpScanUserId = o.UpScanUserId,
                             Status = o.Status,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<InvGpsContentListDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetInvGpsContentListToExcel(InvGpsContentListExportInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;

            var filtered = _repo.GetAll()
                .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
                .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom.Value.Date <= t.WorkingDate)
                .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo.Value.Date.AddDays(1) > t.WorkingDate)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Shift), e => e.Shift.Contains(input.Shift))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierCode), e => e.SupplierCode.Contains(input.SupplierCode))
                .WhereIf(!string.IsNullOrWhiteSpace(input.DockNo), e => e.DockNo.Contains(input.DockNo));

            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate);

            var query = from o in pageAndFiltered
                        select new InvGpsContentListDto
                        {
                            Id = o.Id,
                            WorkingDate = o.WorkingDate,
                            Shift = o.Shift,
                            SupplierName = o.SupplierName,
                            SupplierCode = o.SupplierCode,
                            RenbanNo = o.RenbanNo,
                            PcAddress = o.PcAddress,
                            DockNo = o.DockNo,
                            OrderNo = o.OrderNo,
                            OrderDatetime = o.OrderDatetime,
                            TripNo = o.TripNo,
                            PalletBoxQty = o.PalletBoxQty,
                            EstPackingDatetime = o.EstPackingDatetime,
                            EstArrivalDatetime = o.EstArrivalDatetime,
                            ContentNo = o.ContentNo,
                            OrderId = o.OrderId,
                            PalletSize = o.PalletSize,
                            IsPalletOnly = o.IsPalletOnly,
                            PackagingType = o.PackagingType,
                            IsAdhocReceiving = o.IsAdhocReceiving,
                            GeneratedBy = o.GeneratedBy,
                            UnpackStatus = o.UnpackStatus,
                            ModuleCd = o.ModuleCd,
                            ModuleRunNo = o.ModuleRunNo,
                            UpStartAct = o.UpStartAct,
                            UpFinishAct = o.UpFinishAct,
                            UpScanUserId = o.UpScanUserId,
                            Status = o.Status,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}

