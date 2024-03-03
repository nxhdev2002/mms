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
using prod.LogA.Bar.Dto;
using prod.LogA.Bar.Exporting;

namespace prod.LogA.Bar
{
    //  [AbpAuthorize(AppPermissions.Pages_LogA_Bar_BarScanInfo)]
    public class LgaBarScanInfoAppService : prodAppServiceBase, ILgaBarScanInfoAppService
    {
        private readonly IDapperRepository<LgaBarScanInfo, long> _dapperRepo;
        private readonly IRepository<LgaBarScanInfo, long> _repo;
        private readonly ILgaBarScanInfoExcelExporter _calendarListExcelExporter;

        public LgaBarScanInfoAppService(IRepository<LgaBarScanInfo, long> repo,
                                         IDapperRepository<LgaBarScanInfo, long> dapperRepo,
                                        ILgaBarScanInfoExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_LogA_Bar_BarScanInfo_Edit)]
        public async Task CreateOrEdit(CreateOrEditLgaBarScanInfoDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditLgaBarScanInfoDto input)
        {
            var mainObj = ObjectMapper.Map<LgaBarScanInfo>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditLgaBarScanInfoDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_LogA_Bar_BarScanInfo_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<LgaBarScanInfoDto>> GetAll(GetLgaBarScanInfoInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.UserName), e => e.UserName.Contains(input.UserName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ScanPartNo), e => e.ScanPartNo.Contains(input.ScanPartNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var system = from o in pageAndFiltered
                         select new LgaBarScanInfoDto
                         {
                             Id = o.Id,
                             UserId = o.UserId,
                             UserName = o.UserName,
                             ScanValue = o.ScanValue,
                             ScanPartNo = o.ScanPartNo,
                             ScanBackNo = o.ScanBackNo,
                             ScanType = o.ScanType,
                             ScanDatetime = o.ScanDatetime,
                             ProdLine = o.ProdLine,
                             RefId = o.RefId,
                             Status = o.Status,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<LgaBarScanInfoDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }

        public async Task<FileDto> GetBarScanInfoToExcel(GetLgaBarScanInfoExportInput input)
        {
            var filtered = _repo.GetAll()
               .WhereIf(!string.IsNullOrWhiteSpace(input.UserName), e => e.UserName.Contains(input.UserName))
               .WhereIf(!string.IsNullOrWhiteSpace(input.ScanPartNo), e => e.ScanPartNo.Contains(input.ScanPartNo))
               .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
               ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var query = from o in pageAndFiltered
                        select new LgaBarScanInfoDto
                        {
                            Id = o.Id,
                            UserId = o.UserId,
                            UserName = o.UserName,
                            ScanValue = o.ScanValue,
                            ScanPartNo = o.ScanPartNo,
                            ScanBackNo = o.ScanBackNo,
                            ScanType = o.ScanType,
                            ScanDatetime = o.ScanDatetime,
                            ProdLine = o.ProdLine,
                            RefId = o.RefId,
                            Status = o.Status,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
