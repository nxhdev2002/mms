using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.GPS.Dto;
using prod.Inventory.GPS.Exporting;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Inventory.GPS
{
    [AbpAuthorize(AppPermissions.Pages_Gps_Rundown_StockRundownTransaction_View)]
    public class InvGpsStockRundownTransactionAppService : prodAppServiceBase, IInvGpsStockRundownTransactionAppService
    {
        private readonly IDapperRepository<InvGpsStockRundownTransaction, long> _dapperRepo;
        private readonly IRepository<InvGpsStockRundownTransaction, long> _repo;
        private readonly IInvGpsStockRundownTransactionExcelExporter _calendarListExcelExporter;

        public InvGpsStockRundownTransactionAppService(IRepository<InvGpsStockRundownTransaction, long> repo,
                                         IDapperRepository<InvGpsStockRundownTransaction, long> dapperRepo,
                                        IInvGpsStockRundownTransactionExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<InvGpsStockRundownTransactionDto>> GetAll(GetInvGpsStockRundownTransactionInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo), e => e.PartNo.Contains(input.PartNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartName), e => e.PartName.Contains(input.PartName))

                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new InvGpsStockRundownTransactionDto
                         {
                             Id = o.Id,
                             PartNo = o.PartNo,
                             PartName = o.PartName,
                             PartId = o.PartId,
                             MaterialId = o.MaterialId,
                             Qty = o.Qty,
                             WorkingDate = o.WorkingDate,
                             TransactionDate = o.TransactionDate,
                             TransactionId = o.TransactionId,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<InvGpsStockRundownTransactionDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetGpsStockRundownTransactionToExcel(InvGpsStockRundownTransactionExportInput input)
        {
            var query = from o in _repo.GetAll()
                        select new InvGpsStockRundownTransactionDto
                        {
                            Id = o.Id,
                            PartNo = o.PartNo,
                            PartName = o.PartName,
                            PartId = o.PartId,
                            MaterialId = o.MaterialId,
                            Qty = o.Qty,
                            WorkingDate = o.WorkingDate,
                            TransactionDate = o.TransactionDate,
                            TransactionId = o.TransactionId,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(InvGpsStockRundownTransactionConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
