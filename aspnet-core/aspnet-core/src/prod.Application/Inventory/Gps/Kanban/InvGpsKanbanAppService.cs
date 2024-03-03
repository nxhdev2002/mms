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
using prod.Inventory.GPS.Dto;
using prod.Inventory.GPS.Exporting;

namespace prod.Inventory.GPS
{
    [AbpAuthorize(AppPermissions.Pages_Gps_Receive_Kanban_View)]
    public class InvGpsKanbanAppService : prodAppServiceBase, IInvGpsKanbanAppService
    {
        private readonly IDapperRepository<InvGpsKanban, long> _dapperRepo;
        private readonly IRepository<InvGpsKanban, long> _repo;
        private readonly IInvGpsKanbanExcelExporter _calendarListExcelExporter;

        public InvGpsKanbanAppService(IRepository<InvGpsKanban, long> repo,
                                         IDapperRepository<InvGpsKanban, long> dapperRepo,
                                        IInvGpsKanbanExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_Gps_Receive_Kanban_Edit)]
        public async Task CreateOrEdit(CreateOrEditInvGpsKanbanDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditInvGpsKanbanDto input)
        {
            var mainObj = ObjectMapper.Map<InvGpsKanban>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditInvGpsKanbanDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Gps_Receive_Kanban_Edit)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<InvGpsKanbanDto>> GetAll(GetInvGpsKanbanInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.BackNo), e => e.BackNo.Contains(input.BackNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo), e => e.PartNo.Contains(input.PartNo))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new InvGpsKanbanDto
                         {
                             Id = o.Id,
                             ContentListId = o.ContentListId,
                             BackNo = o.BackNo,
                             PartNo = o.PartNo,
                             ColorSfx = o.ColorSfx,
                             PartName = o.PartName,
                             BoxSize = o.BoxSize,
                             BoxQty = o.BoxQty,
                             PcAddress = o.PcAddress,
                             WhSpsPicking = o.WhSpsPicking,
                             ActualBoxQty = o.ActualBoxQty,
                             RenbanNo = o.RenbanNo,
                             NoInRenban = o.NoInRenban,
                             PackagingType = o.PackagingType,
                             ActualBoxSize = o.ActualBoxSize,
                             GeneratedBy = o.GeneratedBy,
                             Status = o.Status,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<InvGpsKanbanDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetInvGpsKanbanToExcel(InvGpsKanbanExportInput input)
        {
            var filtered = _repo.GetAll()
              .WhereIf(!string.IsNullOrWhiteSpace(input.BackNo), e => e.BackNo.Contains(input.BackNo))
              .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo), e => e.PartNo.Contains(input.PartNo))
              ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var query = from o in pageAndFiltered
                        select new InvGpsKanbanDto
                        {
                            Id = o.Id,
                            ContentListId = o.ContentListId,
                            BackNo = o.BackNo,
                            PartNo = o.PartNo,
                            ColorSfx = o.ColorSfx,
                            PartName = o.PartName,
                            BoxSize = o.BoxSize,
                            BoxQty = o.BoxQty,
                            PcAddress = o.PcAddress,
                            WhSpsPicking = o.WhSpsPicking,
                            ActualBoxQty = o.ActualBoxQty,
                            RenbanNo = o.RenbanNo,
                            NoInRenban = o.NoInRenban,
                            PackagingType = o.PackagingType,
                            ActualBoxSize = o.ActualBoxSize,
                            GeneratedBy = o.GeneratedBy,
                            Status = o.Status,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(InvGpsKanbanConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
