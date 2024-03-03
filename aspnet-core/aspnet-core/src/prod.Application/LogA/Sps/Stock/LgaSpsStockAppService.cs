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
using prod.LogA.Sps.Dto;
using prod.LogA.Sps.Exporting;

namespace prod.LogA.Sps
{
    //  [AbpAuthorize(AppPermissions.Pages_LogA_Sps_Stock)]
    public class LgaSpsStockAppService : prodAppServiceBase, ILgaSpsStockAppService
    {
        private readonly IDapperRepository<LgaSpsStock, long> _dapperRepo;
        private readonly IRepository<LgaSpsStock, long> _repo;
        private readonly ILgaSpsStockExcelExporter _calendarListExcelExporter;

        public LgaSpsStockAppService(IRepository<LgaSpsStock, long> repo,
                                         IDapperRepository<LgaSpsStock, long> dapperRepo,
                                        ILgaSpsStockExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_LogA_Sps_Stock_Edit)]
        public async Task CreateOrEdit(CreateOrEditLgaSpsStockDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditLgaSpsStockDto input)
        {
            var mainObj = ObjectMapper.Map<LgaSpsStock>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditLgaSpsStockDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_LogA_Sps_Stock_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<LgaSpsStockDto>> GetAll(GetLgaSpsStockInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo), e => e.PartNo.Contains(input.PartNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartName), e => e.PartName.Contains(input.PartName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierNo), e => e.SupplierNo.Contains(input.SupplierNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.BackNo), e => e.BackNo.Contains(input.BackNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SpsRackAddress), e => e.SpsRackAddress.Contains(input.SpsRackAddress))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PcRackAddress), e => e.PcRackAddress.Contains(input.PcRackAddress))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Process), e => e.Process.Contains(input.Process))
                .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new LgaSpsStockDto
                         {
                             Id = o.Id,
                             PartNo = o.PartNo,
                             PartName = o.PartName,
                             SupplierNo = o.SupplierNo,
                             BackNo = o.BackNo,
                             SpsRackAddress = o.SpsRackAddress,
                             PcRackAddress = o.PcRackAddress,
                             RackCapBox = o.RackCapBox,
                             PcPicKingMember = o.PcPicKingMember,
                             EkbQty = o.EkbQty,
                             StockQty = o.StockQty,
                             BoxQty = o.BoxQty,
                             Process = o.Process,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<LgaSpsStockDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetStockToExcel(LgaSpsStockExportInput input)
        {
            var query = from o in _repo.GetAll()
                        select new LgaSpsStockDto
                        {
                            Id = o.Id,
                            PartNo = o.PartNo,
                            PartName = o.PartName,
                            SupplierNo = o.SupplierNo,
                            BackNo = o.BackNo,
                            SpsRackAddress = o.SpsRackAddress,
                            PcRackAddress = o.PcRackAddress,
                            RackCapBox = o.RackCapBox,
                            PcPicKingMember = o.PcPicKingMember,
                            EkbQty = o.EkbQty,
                            StockQty = o.StockQty,
                            BoxQty = o.BoxQty,
                            Process = o.Process,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(LgaSpsStockConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
