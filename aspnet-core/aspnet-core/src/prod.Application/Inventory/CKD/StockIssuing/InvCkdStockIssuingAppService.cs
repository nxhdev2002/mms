using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using prod.Authorization;
using prod.Dto;
using prod.HistoricalData;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.Exporting;
using prod.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Inventory.CKD
{
    [AbpAuthorize(AppPermissions.Pages_Ckd_Warehouse_StockIssuing_View)]
    public class InvCkdStockIssuingAppService : prodAppServiceBase, IInvCkdStockIssuingAppService
    {
        private readonly IDapperRepository<InvCkdStockIssuing, long> _dapperRepo;
        private readonly IRepository<InvCkdStockIssuing, long> _repo;
        private readonly IInvCkdStockIssuingExcelExporter _stockIssuingListExcelExporter;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IHistoricalDataAppService _historicalDataAppService;

        public InvCkdStockIssuingAppService(IRepository<InvCkdStockIssuing, long> repo,
                                         IDapperRepository<InvCkdStockIssuing, long> dapperRepo,
                                        IInvCkdStockIssuingExcelExporter stockIssuingListExcelExporter,
                                        ITempFileCacheManager tempFileCacheManager,
                                        IHistoricalDataAppService historicalDataAppService
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _stockIssuingListExcelExporter = stockIssuingListExcelExporter;
            _tempFileCacheManager = tempFileCacheManager;
            _historicalDataAppService = historicalDataAppService;
        }


        public async Task<List<string>> GetStockIssuingHistory(GetInvCkdStockIssuingHistoryInput input)
        {
            return await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
        }

        public async Task<FileDto> GetHistoricalDataToExcel(GetInvCkdStockIssuingHistoryExcelInput input)
        {
            var data = await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
            return _stockIssuingListExcelExporter.ExportToHistoricalFile(data);
        }

        public async Task<List<long?>> GetChangedRecords()
        {
            var listStockIssuing = await _historicalDataAppService.GetChangedRecordIds("InvCkdStockIssuing");
            return listStockIssuing;
        }
        public async Task<PagedResultDto<InvCkdStockIssuingDto>> GetAll(GetInvCkdStockIssuingInput input)
        {
            Dapper.SqlMapper.Settings.CommandTimeout = 900;
            string _sql = "Exec INV_CKD_STOCK_ISSUING_SEARCH @p_PartNo, @p_WorkingDateFrom, @p_WorkingDateTo, @p_Cfc, @p_SupplierNo, @p_color_sfx, @p_vin_no, @p_lot_no, @p_no_in_lot,@p_part_type";

            IEnumerable<InvCkdStockIssuingDto> result = await _dapperRepo.QueryAsync<InvCkdStockIssuingDto>(_sql, new
            {
                p_PartNo = input.PartNo,
                p_color_sfx = input.ColorSfx,
                p_WorkingDateFrom = input.WorkingDateFrom,
                p_WorkingDateTo = input.WorkingDateTo,
                p_SupplierNo = input.SupplierNo,
                p_vin_no = input.VinNo,
                p_Cfc = input.Cfc,
                p_lot_no = input.LotNo,
                p_no_in_lot = input.NoInLot,
                p_part_type = input.PartType
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            if (listResult.Count > 0) listResult[0].GrandTotal = listResult.Sum(e => e.Qty);

            return new PagedResultDto<InvCkdStockIssuingDto>(
               totalCount,
               pagedAndFiltered);
        }


        public async Task<FileDto> GetStockIssuingToExcel(InvCkdStockIssuingExportInput input)
        {
            Dapper.SqlMapper.Settings.CommandTimeout = 900;
            string _sql = "Exec INV_CKD_STOCK_ISSUING_SEARCH @p_PartNo, @p_WorkingDateFrom, @p_WorkingDateTo, @p_Cfc, @p_SupplierNo, @p_color_sfx, @p_vin_no, @p_lot_no, @p_no_in_lot,@p_part_type";

            IEnumerable<InvCkdStockIssuingDto> result = await _dapperRepo.QueryAsync<InvCkdStockIssuingDto>(_sql, new
            {
                p_PartNo = input.PartNo,
                p_color_sfx = input.ColorSfx,
                p_WorkingDateFrom = input.WorkingDateFrom,
                p_WorkingDateTo = input.WorkingDateTo,
                p_SupplierNo = input.SupplierNo,
                p_vin_no = input.VinNo,
                p_Cfc = input.Cfc,
                p_lot_no = input.LotNo,
                p_no_in_lot = input.NoInLot,
                p_part_type = input.PartType
            });

            var exportToExcel = result.ToList();
            return _stockIssuingListExcelExporter.ExportToFile(exportToExcel);
        }

        public async Task<FileDto> GetStockIssuingByMaterialToExcel(InvCkdStockIssuingExportInput input)
        {
            Dapper.SqlMapper.Settings.CommandTimeout = 900;
            string _sql = "Exec INV_CKD_STOCK_ISSUING_BY_MATERIAL_GETSS @p_PartNo, @p_WorkingDateFrom, @p_WorkingDateTo, @p_Cfc, @p_SupplierNo, @p_color_sfx, @p_vin_no, @p_lot_no, @p_no_in_lot, @p_part_type";

            IEnumerable<InvCkdStockPartByMaterialDto> result = await _dapperRepo.QueryAsync<InvCkdStockPartByMaterialDto>(_sql, new
            {
                p_PartNo = input.PartNo,
                p_color_sfx = input.ColorSfx,
                p_WorkingDateFrom = input.WorkingDateFrom,
                p_WorkingDateTo = input.WorkingDateTo,
                p_SupplierNo = input.SupplierNo,
                p_vin_no = input.VinNo,
                p_Cfc = input.Cfc,
                p_lot_no = input.LotNo,
                p_no_in_lot = input.NoInLot,
                p_part_type = input.PartType
            });

            var exportToExcel = result.ToList();
            return _stockIssuingListExcelExporter.ExportByMaterialToFile(exportToExcel);
        }

        public async Task<PagedResultDto<InvCkdStockIssuingTranslocDto>> GetDataStockIssuingView(GetInvCkdStockIssuingViewInput input)
        {
            string _sql = "Exec INV_CKD_STOCK_ISSUING_TRANSLOC @p_WorkingDateFrom, @p_WorkingDateTo , @p_PartType";

            IEnumerable<InvCkdStockIssuingTranslocDto> result = await _dapperRepo.QueryAsync<InvCkdStockIssuingTranslocDto>(_sql, new
            {
                p_WorkingDateFrom = input.WorkingDateFrom,
                p_WorkingDateTo = input.WorkingDateTo,
                p_PartType = input.PartType
            });

            var listResult = result.OrderBy(x => x.RunningNo).ToList();

            if (listResult.Count > 0) listResult[0].GrandTotal = listResult.Sum(e => e.Quantity);

            var totalCount = result.ToList().Count();
            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            return new PagedResultDto<InvCkdStockIssuingTranslocDto>(
                totalCount,
                pagedAndFiltered);

        }

        public async Task<FileDto> GetDataStockIssuingViewToExcel(GetInvCkdStockIssuingViewInput input)
        {
            string _sql = "Exec INV_CKD_STOCK_ISSUING_TRANSLOC @p_WorkingDateFrom, @p_WorkingDateTo , @p_PartType";

            IEnumerable<InvCkdStockIssuingTranslocDto> result = await _dapperRepo.QueryAsync<InvCkdStockIssuingTranslocDto>(_sql, new
            {
                p_WorkingDateFrom = input.WorkingDateFrom,
                p_WorkingDateTo = input.WorkingDateTo,
                p_PartType = input.PartType
            });

            var exportToExcel = result.ToList();
            return _stockIssuingListExcelExporter.ExportStockIssuingViewToFile(exportToExcel);
        }
        public async Task<PagedResultDto<InvCkdStockIssuingValidateDto>> GetValidateStockIssuing(GetInvCkdStockIssuingValidateInput input)
        {
            var data = await _dapperRepo.QueryAsync<InvCkdStockIssuingValidateDto>("Exec INV_CKD_STOCK_ISSUING_VALIDATE @p_WorkingDateFrom, @p_WorkingDateTo", new {
                p_WorkingDateFrom =  input.WorkingDateFrom,
                p_WorkingDateTo = input.WorkingDateTo

            });
            var rsData = from o in data
                         select new InvCkdStockIssuingValidateDto
                         {
                             Id = o.Id,
                             PartNo = o.PartNo,
                             SupplierNo = o.SupplierNo,
                             Cfc = o.Cfc,
                             LotNo = o.LotNo,
                             NoInLot = o.NoInLot,
                             VinNo = o.VinNo,
                             MessagesError = o.MessagesError
                         };
            var listResult = data.ToList();
            var totalCount = rsData.Count();
            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            return new PagedResultDto<InvCkdStockIssuingValidateDto>(
                totalCount,
                pagedAndFiltered
            );
        }

        public async Task<FileDto> ExcelCheckValidateStockIssuing()
        {
            string _sql = "Exec INV_CKD_STOCK_ISSUING_VALIDATE";

            IEnumerable<InvCkdStockIssuingValidateDto> result = await _dapperRepo.QueryAsync<InvCkdStockIssuingValidateDto>(_sql, new { });

            var exportToExcel = result.ToList();
            return _stockIssuingListExcelExporter.ExportStockIssuingValidate(exportToExcel);
        }
    }
}
