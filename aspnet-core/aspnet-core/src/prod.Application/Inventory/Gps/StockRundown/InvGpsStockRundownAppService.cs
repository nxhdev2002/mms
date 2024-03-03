using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using prod.Authorization;
using prod.Inventory.GPS.Dto;
using prod.Inventory.GPS.Exporting;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Inventory.GPS
{
    [AbpAuthorize(AppPermissions.Pages_Gps_Rundown_StockRundown_View)]
    public class InvGpsStockRundownAppService : prodAppServiceBase, IInvGpsStockRundownAppService
    {
        private readonly IDapperRepository<InvGpsStockRundown, long> _dapperRepo;
        private readonly IRepository<InvGpsStockRundown, long> _repo;
        private readonly IInvGpsStockRundownExcelExporter _calendarListExcelExporter;

        public InvGpsStockRundownAppService(IRepository<InvGpsStockRundown, long> repo,
                                         IDapperRepository<InvGpsStockRundown, long> dapperRepo,
                                        IInvGpsStockRundownExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<InvGpsStockRundownDto>> GetAll(GetInvGpsStockRundownInput input)
        {
            string _sql = "Exec INV_GPS_STOCK_RUNDOWN_SEARCH @p_partno";

            IEnumerable<InvGpsStockRundownDto> result = await _dapperRepo.QueryAsync<InvGpsStockRundownDto>(_sql, new
            {
                p_partno = input.PartNo
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvGpsStockRundownDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task CalculatorRundown()
        {
            Dapper.SqlMapper.Settings.CommandTimeout = 900;
            string _calculator = "Exec JOB_INV_GPS_STOCK_RUNDOWN_CREATE";
            await _dapperRepo.QueryAsync<InvGpsStockRundownDto>(_calculator, new { });
        }

        //public async Task<FileDto> GetGpsStockRundownToExcel(InvGpsStockRundownExportInput input)
        //{
        //    var query = from o in _repo.GetAll()
        //                select new InvGpsStockRundownDto
        //                {
        //                    Id = o.Id,
        //                    PartNo = o.PartNo,
        //                    PartName = o.PartName,
        //                    PartId = o.PartId,
        //                    MaterialId = o.MaterialId,
        //                    Qty = o.Qty,
        //                    WorkingDate = o.WorkingDate,
        //                    TransactionId = o.TransactionId,
        //                };
        //    var exportToExcel = await query.ToListAsync();
        //    return _calendarListExcelExporter.ExportToFile(exportToExcel);
        //}

    }
}
