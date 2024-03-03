using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.Exporting;
using prod.Inventory.CKD.PhysicalStockIssuing.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prod.Inventory.CKD
{
    [AbpAuthorize(AppPermissions.Pages_Ckd_Physical_PhysicalStockIssuing_View)]
    public class InvCkdPhysicalStockIssuingAppService : prodAppServiceBase, IInvCkdPhysicalStockIssuingAppService
    {
        private readonly IDapperRepository<InvCkdPhysicalStockTransaction, long> _dapperRepo;
        private readonly IInvCkdPhysicalStockIssuingExcelExporter _calendarListExcelExporter;

        public InvCkdPhysicalStockIssuingAppService(
            IDapperRepository<InvCkdPhysicalStockTransaction, long> dapperRepo,
            IInvCkdPhysicalStockIssuingExcelExporter calendarListExcelExporter
            )
        {
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }
        public async Task<PagedResultDto<InvCkdPhysicalStockIssuingDto>> GetDataPhysicalStockIssuing(InvCkdPhysicalStockIssuingInputDto input)
        {
            string _sql = "Exec INV_CKD_PHYSICAL_STOCK_ISSUING_SEARCH @p_partNo, @p_workingDateFrom, @p_workingDateTo, @p_cfc, @p_supplier_no, " +
                "@p_color_sfx, @p_vin_no, @p_lot_no,@p_use_lot_no, @p_no_in_lot, @p_mode, @p_period_id";

            IEnumerable<InvCkdPhysicalStockIssuingDto> result = await _dapperRepo.QueryAsync<InvCkdPhysicalStockIssuingDto>(_sql, new
            {
                p_partNo = input.PartNo,
                p_workingDateFrom = input.WorkingDateFrom,
                p_workingDateTo = input.WorkingDateTo,
                p_cfc = input.Cfc,
                p_supplier_no = input.SupplierNo,
                p_color_sfx = input.ColorSfx,
                p_vin_no = input.VinNo,
                p_lot_no = input.LotNo,
                p_no_in_lot = input.NoInLot,
                p_use_lot_no = input.UseLot,
                p_mode = input.p_mode,
                p_period_id = input.PeriodId
            });

            var listResult = result.ToList();

            if (listResult.Count > 0) listResult[0].GrandTotal = listResult.Sum(e => e.Qty);

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCkdPhysicalStockIssuingDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetCkdPhysicalStockIssuingToExcel(InvCkdPhysicalStockIssuingExportInput input)
        {
            string _sql = "Exec INV_CKD_PHYSICAL_STOCK_ISSUING_SEARCH @p_partNo, @p_workingDateFrom, @p_workingDateTo, @p_cfc, @p_supplier_no, " +
                "@p_color_sfx, @p_vin_no, @p_lot_no,@p_use_lot_no, @p_no_in_lot, @p_mode, @p_period_id";

            IEnumerable<InvCkdPhysicalStockIssuingDto> result = await _dapperRepo.QueryAsync<InvCkdPhysicalStockIssuingDto>(_sql, new
            {
                p_partNo = input.PartNo,
                p_workingDateFrom = input.WorkingDateFrom,
                p_workingDateTo = input.WorkingDateTo,
                p_cfc = input.Cfc,
                p_supplier_no = input.SupplierNo,
                p_color_sfx = input.ColorSfx,
                p_vin_no = input.VinNo,
                p_lot_no = input.LotNo,
                p_no_in_lot = input.NoInLot,
                p_use_lot_no = input.UseLot,
                p_mode = input.p_mode,
                p_period_id = input.PeriodId
            });
            var list = result.ToList();
            return _calendarListExcelExporter.ExportToFile(list);

        }

        public async Task CalPhysicalStockIssuing()
        {
            Dapper.SqlMapper.Settings.CommandTimeout = 900;
            string _sql = "Exec JOB_INV_CKD_PHYSICAL_STOCK_TRANSACTION_ISSUE_CREATE";
            await _dapperRepo.ExecuteAsync(_sql);
        }

        public async Task<FileDto> GetCkdPhysicalStockIssuingDetailsToExcel(InvCkdPhysicalStockIssuingExportInput input)
        {
            Dapper.SqlMapper.Settings.CommandTimeout = 900;

            string _sql = "Exec INV_CKD_PHYSICAL_STOCK_ISSUING_EXPORT_DETAILS_DATA @p_partNo, @p_workingDateFrom, @p_workingDateTo, @p_cfc, @p_supplier_no, " +
                "@p_color_sfx, @p_vin_no, @p_lot_no,@p_use_lot_no, @p_no_in_lot, @p_mode, @p_period_id";

            IEnumerable<InvCkdPhysicalStockIssuingDetailsDataDto> result = await _dapperRepo.QueryAsync<InvCkdPhysicalStockIssuingDetailsDataDto>(_sql, new
            {
                p_partNo = input.PartNo,
                p_workingDateFrom = input.WorkingDateFrom,
                p_workingDateTo = input.WorkingDateTo,
                p_cfc = input.Cfc,
                p_supplier_no = input.SupplierNo,
                p_color_sfx = input.ColorSfx,
                p_vin_no = input.VinNo,
                p_lot_no = input.LotNo,
                p_no_in_lot = input.NoInLot,
                p_use_lot_no = input.UseLot,
                p_mode = input.p_mode,
                p_period_id = input.PeriodId
            });
            var list = result.ToList();
            return _calendarListExcelExporter.ExportToFileDetailsData(list);

        }

        public async Task<FileDto> GetCkdPhysicalStockIssuingReportSummaryToExcel(int? p_period)
        {
            Dapper.SqlMapper.Settings.CommandTimeout = 900;

            string _sql = "Exec INV_CKD_PHYSICAL_STOCK_ISSUING_EXPORT_SUMMARY @p_period_id";

            IEnumerable<InvCkdPhysicalStockIssuingReportSummartDto> result = await _dapperRepo.QueryAsync<InvCkdPhysicalStockIssuingReportSummartDto>(_sql, new
            {
                p_period_id = p_period
            });
            var list = result.ToList();
            return _calendarListExcelExporter.ExportReportSummaryToFile(list);

        }

    }
}
