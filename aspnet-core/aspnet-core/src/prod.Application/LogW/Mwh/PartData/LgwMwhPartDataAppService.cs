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
using prod.LogW.Mwh.Dto;
using prod.LogW.Mwh.Exporting;

namespace prod.LogW.Mwh
{
    //  [AbpAuthorize(AppPermissions.Pages_LogW_Mwh_PartData)]
    public class LgwMwhPartDataAppService : prodAppServiceBase, ILgwMwhPartDataAppService
    {
        private readonly IDapperRepository<LgwMwhPartData, long> _dapperRepo;
        private readonly IRepository<LgwMwhPartData, long> _repo;
        private readonly ILgwMwhPartDataExcelExporter _calendarListExcelExporter;

        public LgwMwhPartDataAppService(IRepository<LgwMwhPartData, long> repo,
                                         IDapperRepository<LgwMwhPartData, long> dapperRepo,
                                        ILgwMwhPartDataExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<LgwMwhPartDataDto>> GetAll(GetLgwMwhPartDataInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo), e => e.PartNo.Contains(input.PartNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo))             
                .WhereIf(!string.IsNullOrWhiteSpace(input.CaseNo), e => e.CaseNo.Contains(input.CaseNo))                
                .WhereIf(!string.IsNullOrWhiteSpace(input.ContainerNo), e => e.ContainerNo.Contains(input.ContainerNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierNo), e => e.SupplierNo.Contains(input.SupplierNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new LgwMwhPartDataDto
                         {
                             Id = o.Id,
                             PxppartId = o.PxppartId,
                             PartNo = o.PartNo,
                             LotNo = o.LotNo,
                             Fixlot = o.Fixlot,
                             CaseNo = o.CaseNo,
                             ModuleNo = o.ModuleNo,
                             ContainerNo = o.ContainerNo,
                             SupplierNo = o.SupplierNo,
                             UsageQty = o.UsageQty,
                             PartName = o.PartName,
                             CarfamilyCode = o.CarfamilyCode,
                             InvoiceParentId = o.InvoiceParentId,
                             PxpcaseId = o.PxpcaseId,
                             OrderType = o.OrderType,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<LgwMwhPartDataDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetPartDataToExcel(GetLgwMwhPartDataExportInput input)
        {
            var filtered = _repo.GetAll()
              .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo), e => e.PartNo.Contains(input.PartNo))
              .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo))
              .WhereIf(!string.IsNullOrWhiteSpace(input.CaseNo), e => e.CaseNo.Contains(input.CaseNo))
              .WhereIf(!string.IsNullOrWhiteSpace(input.ContainerNo), e => e.ContainerNo.Contains(input.ContainerNo))
              .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierNo), e => e.SupplierNo.Contains(input.SupplierNo))
              .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive))
              ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var query = from o in pageAndFiltered
                        select new LgwMwhPartDataDto
                        {
                            Id = o.Id,
                            PxppartId = o.PxppartId,
                            PartNo = o.PartNo,
                            LotNo = o.LotNo,
                            Fixlot = o.Fixlot,
                            CaseNo = o.CaseNo,
                            ModuleNo = o.ModuleNo,
                            ContainerNo = o.ContainerNo,
                            SupplierNo = o.SupplierNo,
                            UsageQty = o.UsageQty,
                            PartName = o.PartName,
                            CarfamilyCode = o.CarfamilyCode,
                            InvoiceParentId = o.InvoiceParentId,
                            PxpcaseId = o.PxpcaseId,
                            OrderType = o.OrderType,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(LgwMwhPartDataConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}

