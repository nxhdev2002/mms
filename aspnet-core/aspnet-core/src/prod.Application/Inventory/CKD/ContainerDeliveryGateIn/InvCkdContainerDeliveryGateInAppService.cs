using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.Exporting;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Inventory.CKD
{
    [AbpAuthorize(AppPermissions.Pages_Ckd_Warehouse_ContainerDeliveryGateIn_View)]
    public class InvCkdContainerDeliveryGateInAppService : prodAppServiceBase, IInvCkdContainerDeliveryGateInAppService
    {
        private readonly IDapperRepository<InvCkdContainerDeliveryGateIn, long> _dapperRepo;
        private readonly IRepository<InvCkdContainerDeliveryGateIn, long> _repo;
        private readonly IInvCkdContainerDeliveryGateInExcelExporter _calendarListExcelExporter;

        public InvCkdContainerDeliveryGateInAppService(IRepository<InvCkdContainerDeliveryGateIn, long> repo,
                                         IDapperRepository<InvCkdContainerDeliveryGateIn, long> dapperRepo,
                                        IInvCkdContainerDeliveryGateInExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }




        public async Task<PagedResultDto<InvCkdContainerDeliveryGateInDto>> GetAll(GetInvCkdContainerDeliveryGateInInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.PlateNo), e => e.PlateNo.Contains(input.PlateNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ContainerNo), e => e.ContainerNo.Contains(input.ContainerNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Renban), e => e.Renban.Contains(input.Renban))
                .WhereIf(input.DateFrom.HasValue, t => input.DateFrom.Value.Date <= t.DateIn)
                .WhereIf(input.DateTo.HasValue, t => input.DateTo.Value.Date.AddDays(1) > t.DateIn)
                ;
            var pageAndFiltered = filtered.OrderByDescending(s => s.DateIn)
                                        .ThenByDescending(s => s.StartTime)
                                        .ThenByDescending(s => s.FinishTime);


            var system = from o in pageAndFiltered
                         select new InvCkdContainerDeliveryGateInDto
                         {
                             Id = o.Id,
                             PlateNo = o.PlateNo,
                             ContainerNo = o.ContainerNo,
                             Renban = o.Renban,
                             DateIn = o.DateIn,
                             DriverName = o.DriverName,
                             Forwarder = o.Forwarder,
                             TimeIn = o.TimeIn,
                             TimeOut = o.TimeOut,
                             CkdReqId = o.CkdReqId,
                             CardNo = o.CardNo,
                             Mobile = o.Mobile,
                             CallTime = o.CallTime,
                             CancelTime = o.CancelTime,
                             StartTime = o.StartTime,
                             FinishTime = o.FinishTime,
                             IdNo = o.IdNo,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<InvCkdContainerDeliveryGateInDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetContainerDeliveryGateInToExcel(GetInvCkdContainerDeliveryGateInInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.PlateNo), e => e.PlateNo.Contains(input.PlateNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ContainerNo), e => e.ContainerNo.Contains(input.ContainerNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Renban), e => e.Renban.Contains(input.Renban))
                .WhereIf(input.DateFrom.HasValue, t => input.DateFrom.Value.Date <= t.DateIn)
                .WhereIf(input.DateTo.HasValue, t => input.DateTo.Value.Date.AddDays(1) > t.DateIn)
                ;
            var pageAndFiltered = filtered.OrderByDescending(s => s.DateIn)
                                        .ThenByDescending(s => s.StartTime)
                                        .ThenByDescending(s => s.FinishTime);
            var query = from o in pageAndFiltered
                        select new InvCkdContainerDeliveryGateInDto
                        {
                            Id = o.Id,
                            PlateNo = o.PlateNo,
                            ContainerNo = o.ContainerNo,
                            Renban = o.Renban,
                            DateIn = o.DateIn,
                            DriverName = o.DriverName,
                            Forwarder = o.Forwarder,
                            TimeIn = o.TimeIn,
                            TimeOut = o.TimeOut,
                            CkdReqId = o.CkdReqId,
                            CardNo = o.CardNo,
                            Mobile = o.Mobile,
                            CallTime = o.CallTime,
                            CancelTime = o.CancelTime,
                            StartTime = o.StartTime,
                            FinishTime = o.FinishTime,
                            IdNo = o.IdNo,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(InvCkdContainerDeliveryGateInConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
