using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Authorization;
using prod.Dto;
using prod.Inv.D125.Dto;
using prod.Inv.D125.Exporting;

namespace prod.Inv.D125.InDetails
{
    //  [AbpAuthorize(AppPermissions.Pages_Inv_D125_InDetails)]
    public class InvInDetailsAppService : prodAppServiceBase, IInvInDetailsAppService
    {
        private readonly IDapperRepository<InvInDetails, long> _dapperRepo;
        private readonly IRepository<InvInDetails, long> _repo;
        private readonly IInvInDetailsExcelExporter _calendarListExcelExporter;

        public InvInDetailsAppService(IRepository<InvInDetails, long> repo,
                                         IDapperRepository<InvInDetails, long> dapperRepo,
                                        IInvInDetailsExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

       

        public async Task<PagedResultDto<InvInDetailsDto>> GetAll(GetInvInDetailsInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(input.PeriodId != 0, e => e.PeriodId == input.PeriodId)
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo), e => e.PartNo.Contains(input.PartNo))    
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierNo), e => e.SupplierNo.Contains(input.SupplierNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.FixLot), e => e.FixLot.Contains(input.FixLot))
                .WhereIf(!string.IsNullOrWhiteSpace(input.CarfamilyCode), e => e.CarfamilyCode.Contains(input.CarfamilyCode))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new InvInDetailsDto
                         {
                             Id = o.Id,
                             PeriodId = o.PeriodId,
                             InvoiceNo = o.InvoiceNo,
                             PartNo = o.PartNo,
                             UsageQty = o.UsageQty,
                             InvoiceDate = o.InvoiceDate,
                             ReceiveDate = o.ReceiveDate,
                             SupplierNo = o.SupplierNo,
                             FixLot = o.FixLot,
                             CarfamilyCode = o.CarfamilyCode,
                             CustomsDeclareNo = o.CustomsDeclareNo,
                             DeclareDate = o.DeclareDate
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<InvInDetailsDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }

        public async Task<FileDto> GetInDetailsToExcel(GetInvInDetailsExportInput input)
        {
            var filtered = _repo.GetAll()
               .WhereIf(input.PeriodId != 0, e => e.PeriodId == input.PeriodId)
               .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo), e => e.PartNo.Contains(input.PartNo))
               .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierNo), e => e.SupplierNo.Contains(input.SupplierNo))
               .WhereIf(!string.IsNullOrWhiteSpace(input.FixLot), e => e.FixLot.Contains(input.FixLot))
               .WhereIf(!string.IsNullOrWhiteSpace(input.CarfamilyCode), e => e.CarfamilyCode.Contains(input.CarfamilyCode));

            var query = from o in filtered
                        select new InvInDetailsDto
                        {
                            Id = o.Id,
                            PeriodId = o.PeriodId,
                            InvoiceNo = o.InvoiceNo,
                            PartNo = o.PartNo,
                            UsageQty = o.UsageQty,
                            InvoiceDate = o.InvoiceDate,
                            ReceiveDate = o.ReceiveDate,
                            SupplierNo = o.SupplierNo,
                            FixLot = o.FixLot,
                            CarfamilyCode = o.CarfamilyCode,
                            CustomsDeclareNo = o.CustomsDeclareNo,
                            DeclareDate = o.DeclareDate
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

       

    }
}

