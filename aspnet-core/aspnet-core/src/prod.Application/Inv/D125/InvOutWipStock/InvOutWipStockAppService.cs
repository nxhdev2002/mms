using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Authorization;
using prod.Dto;
using prod.Inv.D125;
using prod.Inv.D125.Dto;
using prod.Inv.D125.Exporting;

namespace prod.Inv
{
    //  [AbpAuthorize(AppPermissions.Pages_Inv_D125_OutWipStock)]
    public class InvOutWipStockAppService : prodAppServiceBase, IInvOutWipStockAppService
    {
        private readonly IDapperRepository<InvOutWipStock, long> _dapperRepo;
        private readonly IRepository<InvOutWipStock, long> _repo;
        private readonly IInvOutWipStockExcelExporter _calendarListExcelExporter;

        public InvOutWipStockAppService(IRepository<InvOutWipStock, long> repo,
                                         IDapperRepository<InvOutWipStock, long> dapperRepo,
                                        IInvOutWipStockExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<InvOutWipStockDto>> GetAll(GetInvOutWipStockInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(input.PeriodId != 0, e => e.PeriodId == input.PeriodId)
                .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo), e => e.PartNo.Contains(input.PartNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.CarfamilyCode), e => e.CarfamilyCode.Contains(input.CarfamilyCode))             
                .WhereIf(!string.IsNullOrWhiteSpace(input.InStockByLot), e => e.InStockByLot.Contains(input.InStockByLot))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new InvOutWipStockDto
                         {
                             Id = o.Id,
                             PeriodId = o.PeriodId,
                             LotNo = o.LotNo,
                             PartNo = o.PartNo,
                             CarfamilyCode = o.CarfamilyCode,
                             UsageQty = o.UsageQty,
                             SumCif = o.SumCif,
                             SumTax = o.SumTax,
                             SumInland = o.SumInland,
                             Amount = o.Amount,
                             SumCifVn = o.SumCifVn,
                             SumTaxVn = o.SumTaxVn,
                             SumInlandVn = o.SumInlandVn,
                             AmountVn = o.AmountVn,
                             CustomsDeclareNo = o.CustomsDeclareNo,
                             DeclareDate = o.DeclareDate,
                             DcType = o.DcType,
                             InStockByLot = o.InStockByLot,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<InvOutWipStockDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetOutWipStockToExcel(GetInvOutWipStockExportInput input)
        {
            var filtered = _repo.GetAll()
              .WhereIf(input.PeriodId != 0, e => e.PeriodId == input.PeriodId)
              .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo))
              .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo), e => e.PartNo.Contains(input.PartNo))
              .WhereIf(!string.IsNullOrWhiteSpace(input.CarfamilyCode), e => e.CarfamilyCode.Contains(input.CarfamilyCode))
              .WhereIf(!string.IsNullOrWhiteSpace(input.InStockByLot), e => e.InStockByLot.Contains(input.InStockByLot));

            var query = from o in filtered
                        select new InvOutWipStockDto
                        {
                            Id = o.Id,
                            PeriodId = o.PeriodId,
                            LotNo = o.LotNo,
                            PartNo = o.PartNo,
                            CarfamilyCode = o.CarfamilyCode,
                            UsageQty = o.UsageQty,
                            SumCif = o.SumCif,
                            SumTax = o.SumTax,
                            SumInland = o.SumInland,
                            Amount = o.Amount,
                            SumCifVn = o.SumCifVn,
                            SumTaxVn = o.SumTaxVn,
                            SumInlandVn = o.SumInlandVn,
                            AmountVn = o.AmountVn,
                            CustomsDeclareNo = o.CustomsDeclareNo,
                            DeclareDate = o.DeclareDate,
                            DcType = o.DcType,
                            InStockByLot = o.InStockByLot
                           
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

     
    }
}
