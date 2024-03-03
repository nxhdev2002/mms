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
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.Exporting;

namespace prod.Inventory.CKD
{
    //  [AbpAuthorize(AppPermissions.Pages_Inventory_CKD_PhysicalStockPartPeriod)]
    public class InvCkdPhysicalStockPartPeriodAppService : prodAppServiceBase, IInvCkdPhysicalStockPartPeriodAppService
    {
        private readonly IDapperRepository<InvCkdPhysicalStockPartPeriod, long> _dapperRepo;
        private readonly IRepository<InvCkdPhysicalStockPartPeriod, long> _repo;
        private readonly IInvCkdPhysicalStockPartPeriodExcelExporter _calendarListExcelExporter;

        public InvCkdPhysicalStockPartPeriodAppService(IRepository<InvCkdPhysicalStockPartPeriod, long> repo,
                                         IDapperRepository<InvCkdPhysicalStockPartPeriod, long> dapperRepo,
                                        IInvCkdPhysicalStockPartPeriodExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Inventory_CKD_PhysicalStockPartPeriod_Edit)]
        public async Task CreateOrEdit(CreateOrEditInvCkdPhysicalStockPartPeriodDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditInvCkdPhysicalStockPartPeriodDto input)
        {
            var mainObj = ObjectMapper.Map<InvCkdPhysicalStockPartPeriod>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditInvCkdPhysicalStockPartPeriodDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Inventory_CKD_PhysicalStockPartPeriod_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<InvCkdPhysicalStockPartPeriodDto>> GetAll(GetInvCkdPhysicalStockPartPeriodInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo), e => e.PartNo.Contains(input.PartNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartNoNormalized), e => e.PartNoNormalized.Contains(input.PartNoNormalized))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartName), e => e.PartName.Contains(input.PartName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartNoNormalizedS4), e => e.PartNoNormalizedS4.Contains(input.PartNoNormalizedS4))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ColorSfx), e => e.ColorSfx.Contains(input.ColorSfx))
                .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Remark), e => e.Remark.Contains(input.Remark))
                .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new InvCkdPhysicalStockPartPeriodDto
                         {
                             Id = o.Id,
                             PartNo = o.PartNo,
                             PartNoNormalized = o.PartNoNormalized,
                             PartName = o.PartName,
                             PartNoNormalizedS4 = o.PartNoNormalizedS4,
                             ColorSfx = o.ColorSfx,
                             LotNo = o.LotNo,
                             PartListId = o.PartListId,
                             MaterialId = o.MaterialId,
                             BeginQty = o.BeginQty,
                             ReceiveQty = o.ReceiveQty,
                             IssueQty = o.IssueQty,
                             CalculatorQty = o.CalculatorQty,
                             ActualQty = o.ActualQty,
                             PeriodId = o.PeriodId,
                             LastCalDatetime = o.LastCalDatetime,
                             Transtype = o.Transtype,
                             Remark = o.Remark,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<InvCkdPhysicalStockPartPeriodDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetPhysicalStockPartPeriodToExcel(InvCkdPhysicalStockPartPeriodExportInput input)
        {
            var query = from o in _repo.GetAll()
                        select new InvCkdPhysicalStockPartPeriodDto
                        {
                            Id = o.Id,
                            PartNo = o.PartNo,
                            PartNoNormalized = o.PartNoNormalized,
                            PartName = o.PartName,
                            PartNoNormalizedS4 = o.PartNoNormalizedS4,
                            ColorSfx = o.ColorSfx,
                            LotNo = o.LotNo,
                            PartListId = o.PartListId,
                            MaterialId = o.MaterialId,
                            BeginQty = o.BeginQty,
                            ReceiveQty = o.ReceiveQty,
                            IssueQty = o.IssueQty,
                            CalculatorQty = o.CalculatorQty,
                            ActualQty = o.ActualQty,
                            PeriodId = o.PeriodId,
                            LastCalDatetime = o.LastCalDatetime,
                            Transtype = o.Transtype,
                            Remark = o.Remark,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(InvCkdPhysicalStockPartPeriodConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
