using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.StockBalance.Exporting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prod.Inventory.CKD
{
    [AbpAuthorize(AppPermissions.Pages_Ckd_Warehouse_StockBalance_View)]
    public class InvCkdStockBalanceAppService : prodAppServiceBase, IInvCkdStockBalanceAppService
    {
        private readonly IDapperRepository<InvCkdContainerList, long> _dapperRepo;
        private readonly IInvCkdStockBalanceExcelExporter _calendarListExcelExporter;


        public InvCkdStockBalanceAppService(IDapperRepository<InvCkdContainerList, long> dapperRepo,
            IInvCkdStockBalanceExcelExporter calendarListExcelExporter)
        {
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<InvCkdStockBalanceDto>> GetDataBalance(InvCkdStockBalanceInputDto input)
        {
            Dapper.SqlMapper.Settings.CommandTimeout = 900;
            string _sql = "Exec INV_CKD_STOCK_BALANCE_GET @p_PartNo, @p_ColorSfx, @p_Cfc, @p_SupplierNo, @p_WorkingDateFrom, @p_WorkingDateTo, @p_Diff";

            IEnumerable<InvCkdStockBalanceDto> result = await _dapperRepo.QueryAsync<InvCkdStockBalanceDto>(_sql, new
            {
                p_PartNo = input.PartNo,
                p_ColorSfx = input.ColorSfx,
                p_Cfc = input.Cfc,
                p_SupplierNo = input.SupplierNo,
                p_WorkingDateFrom = input.WorkingDateFrom,
                p_WorkingDateTo = input.WorkingDateTo,
                p_Diff = input.Diff
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            if (listResult.Count > 0)
            {
                listResult[0].GrandBegining = listResult.Sum(e => e.Begining);
                listResult[0].GrandReceiving = listResult.Sum(e => e.Receiving);
                listResult[0].GrandIssuing = listResult.Sum(e => e.Issuing);
                listResult[0].GrandClosing = listResult.Sum(e => e.Closing);
                listResult[0].GrandConcept = listResult.Sum(e => e.Concept);
                listResult[0].GrandDiff = listResult.Sum(e => e.Diff);
            }

            return new PagedResultDto<InvCkdStockBalanceDto>(
                totalCount,
                pagedAndFiltered);
        }

        //public async Task<PagedResultDto<InvPeriodDto>> GetIdInvPeriod()
        //{
        //    string _sql = "Exec INV_PERIOD_GETID";

        //    var data = await _dapperRepo.QueryAsync<InvPeriodDto>(_sql, new { });

        //    var results = from d in data
        //                  select new InvPeriodDto
        //                  {
        //                      Id = d.Id,
        //                      Description = d.Description,
        //                      From_Date = d.From_Date,
        //                      To_Date = d.To_Date,
        //                      Status = d.Status
        //                  };

        //    var totalCount = data.ToList().Count;
        //    return new PagedResultDto<InvPeriodDto>(
        //        totalCount,
        //        results.ToList()
        //    );
        //}

        public async Task<FileDto> GetStockBalanceToExcel(GetStockBalanceExportInput input)
        {
            string _sql = "Exec INV_CKD_STOCK_BALANCE_GET @p_PartNo, @p_ColorSfx, @p_Cfc, @p_SupplierNo, @p_WorkingDateFrom, @p_WorkingDateTo, @p_Diff";

            IEnumerable<InvCkdStockBalanceDto> result = await _dapperRepo.QueryAsync<InvCkdStockBalanceDto>(_sql, new
            {
                p_PartNo = input.PartNo,
                p_ColorSfx = input.ColorSfx,
                p_Cfc = input.Cfc,
                p_SupplierNo = input.SupplierNo,
                p_WorkingDateFrom = input.WorkingDateFrom,
                p_WorkingDateTo = input.WorkingDateTo,
                p_Diff = input.Diff
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFileStockBalance(exportToExcel);
        }
    }
}
