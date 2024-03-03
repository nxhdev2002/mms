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
using prod.Inv.D125.Dto;
using prod.Inv.D125.Exporting;

namespace prod.Inv.D125
{
    //  [AbpAuthorize(AppPermissions.Pages_Inv_D125_InvOutLineOff)]
    public class InvOutLineOffAppService : prodAppServiceBase, IInvOutLineOffAppService
    {
        private readonly IDapperRepository<InvOutLineOff, long> _dapperRepo;
        private readonly IRepository<InvOutLineOff, long> _repo;
        private readonly IInvOutLineOffExcelExporter _calendarListExcelExporter;

        public InvOutLineOffAppService(IRepository<InvOutLineOff, long> repo,
                                         IDapperRepository<InvOutLineOff, long> dapperRepo,
                                        IInvOutLineOffExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

      
        public async Task<PagedResultDto<InvOutLineOffDto>> GetAll(GetInvOutLineOffInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(input.PeriodId != 0, e => e.PeriodId == input.PeriodId)
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo), e => e.PartNo.Contains(input.PartNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.CarFamilyCode), e => e.CarFamilyCode.Contains(input.CarFamilyCode))
                .WhereIf(!string.IsNullOrWhiteSpace(input.InStockByLot), e => e.InStockByLot.Contains(input.InStockByLot))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new InvOutLineOffDto
                         {
                             Id = o.Id,
                             PeriodId = o.PeriodId,
                             PartNo = o.PartNo,
                             CarFamilyCode = o.CarFamilyCode,
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

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<InvOutLineOffDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetInvOutLineOffToExcel(GetInvOutLineOffExportInput input)
        {
            var filtered = _repo.GetAll()
          .WhereIf(input.PeriodId != 0, e => e.PeriodId == input.PeriodId)
          .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo), e => e.PartNo.Contains(input.PartNo))
          .WhereIf(!string.IsNullOrWhiteSpace(input.CarFamilyCode), e => e.CarFamilyCode.Contains(input.CarFamilyCode))
          .WhereIf(!string.IsNullOrWhiteSpace(input.InStockByLot), e => e.InStockByLot.Contains(input.InStockByLot))
    ;

            var query = from o in filtered
                        select new InvOutLineOffDto
                        {
                            Id = o.Id,
                            PeriodId = o.PeriodId,
                            PartNo = o.PartNo,
                            CarFamilyCode = o.CarFamilyCode,
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
