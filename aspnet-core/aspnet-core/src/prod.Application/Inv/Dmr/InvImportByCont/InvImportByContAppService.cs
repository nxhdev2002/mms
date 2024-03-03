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
using NPOI.SS.Formula.Functions;
using NUglify.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Inv.Dmr.Dto;
using prod.Inv.Dmr.Exporting;

namespace prod.Inv.Dmr
{
    //  [AbpAuthorize(AppPermissions.Pages_Inv_Dmr_ImportByCont)]
    public class InvImportByContAppService : prodAppServiceBase, IInvImportByContAppService
    {
        private readonly IDapperRepository<InvImportByCont, long> _dapperRepo;
        private readonly IRepository<InvImportByCont, long> _repo;
        private readonly IInvImportByContExcelExporter _calendarListExcelExporter;

        public InvImportByContAppService(IRepository<InvImportByCont, long> repo,
                                         IDapperRepository<InvImportByCont, long> dapperRepo,
                                        IInvImportByContExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }


        public async Task<PagedResultDto<InvImportByContDto>> GetAll(GetInvImportByContInput input)
        {
            
            var filtered = _repo.GetAll()
                 .WhereIf(input.PeriodId != 0, e => e.PeriodId == input.PeriodId)
                 .WhereIf(!string.IsNullOrWhiteSpace(input.ContainerNo), e => e.ContainerNo.Contains(input.ContainerNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.CaseNo), e => e.CaseNo.Contains(input.CaseNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo), e => e.PartNo.Contains(input.PartNo))       
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierNo), e => e.SupplierNo.Contains(input.SupplierNo))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new InvImportByContDto
                         {
                             Id = o.Id,
                             PeriodId = o.PeriodId,
                             ContainerNo = o.ContainerNo,
                             InvoiceNo = o.InvoiceNo,
                             CaseNo = o.CaseNo,
                             LotNo = o.LotNo,
                             PartNo = o.PartNo,
                             DateIn = o.DateIn,
                             Fob = o.Fob,
                             Cif = o.Cif,
                             ImportTax = o.ImportTax,
                             InlandCharge = o.InlandCharge,
                             Amount = o.Amount,
                             Qty = o.Qty,
                             Price = o.Price,
                             FobVn = o.FobVn,
                             CifVn = o.CifVn,
                             ImportTaxVn = o.ImportTaxVn,
                             InlandChargeVn = o.InlandChargeVn,
                             AmountVn = o.AmountVn,
                             PriceVn = o.PriceVn,
                             InvoiceDate = o.InvoiceDate,
                             ReceiveDate = o.ReceiveDate,
                             ContSize = o.ContSize,
                             Eta = o.Eta,
                             SupplierNo = o.SupplierNo,
                             TotalFob = pageAndFiltered.Sum(x => x.Fob),
                             TotalCif = pageAndFiltered.Sum(x => x.Cif),
                             TotalImportTax = pageAndFiltered.Sum(x => x.ImportTax),
                             TotalInlandCharge = pageAndFiltered.Sum(x => x.InlandCharge),
                             TotalAmount = pageAndFiltered.Sum(x => x.Amount)
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<InvImportByContDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetImportByContToExcel(GetInvImportByContExportInput input)
        {
            var filtered = _repo.GetAll()
                     .WhereIf(input.PeriodId != 0, e => e.PeriodId == input.PeriodId)
                   .WhereIf(!string.IsNullOrWhiteSpace(input.ContainerNo), e => e.ContainerNo.Contains(input.ContainerNo))
                  .WhereIf(!string.IsNullOrWhiteSpace(input.CaseNo), e => e.CaseNo.Contains(input.CaseNo))
                  .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo))
                  .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo), e => e.PartNo.Contains(input.PartNo))
                  .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierNo), e => e.SupplierNo.Contains(input.SupplierNo));

            var query = from o in filtered
                        select new InvImportByContDto
                        {
                            Id = o.Id,
                            PeriodId = o.PeriodId,
                            ContainerNo = o.ContainerNo,
                            InvoiceNo = o.InvoiceNo,
                            CaseNo = o.CaseNo,
                            LotNo = o.LotNo,
                            PartNo = o.PartNo,
                            DateIn = o.DateIn,
                            Fob = o.Fob,
                            Cif = o.Cif,
                            ImportTax = o.ImportTax,
                            InlandCharge = o.InlandCharge,
                            Amount = o.Amount,
                            Qty = o.Qty,
                            Price = o.Price,
                            FobVn = o.FobVn,
                            CifVn = o.CifVn,
                            ImportTaxVn = o.ImportTaxVn,
                            InlandChargeVn = o.InlandChargeVn,
                            AmountVn = o.AmountVn,
                            PriceVn = o.PriceVn,
                            InvoiceDate = o.InvoiceDate,
                            ReceiveDate = o.ReceiveDate,
                            ContSize = o.ContSize,
                            Eta = o.Eta,
                            SupplierNo = o.SupplierNo,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }


    }
}
