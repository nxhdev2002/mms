using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using prod.Assy.Andon.Dto;
using prod.Assy.Andon.Exporting;
using prod.Authorization;
using prod.Dto;
using prod.HistoricalData;
using prod.Inventory.CKD.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Assy.Andon
{
    [AbpAuthorize(AppPermissions.Pages_ProdPlan_VehicleDetails_View)]
    public class AsyAdoVehicleDetailsAppService : prodAppServiceBase, IAsyAdoVehicleDetailsAppService
    {
        private readonly IDapperRepository<AsyAdoVehicleDetails, long> _dapperRepo;
        private readonly IRepository<AsyAdoVehicleDetails, long> _repo;
        private readonly IAsyAdoVehicleDetailsExcelExporter _calendarListExcelExporter;
        private readonly IHistoricalDataAppService _historicalDataAppService;


        public AsyAdoVehicleDetailsAppService(IRepository<AsyAdoVehicleDetails, long> repo,
                                         IDapperRepository<AsyAdoVehicleDetails, long> dapperRepo,
                                        IAsyAdoVehicleDetailsExcelExporter calendarListExcelExporter,
                                        IHistoricalDataAppService historicalDataAppService
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
            _historicalDataAppService = historicalDataAppService;
        }

        public async Task<List<string>> GetAsyAdoVehicleDetailsHistory(GetAsyAdoVehicleDetailsHistoryInput input)
        {
            return await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
        }

        public async Task<FileDto> GetHistoricalDataToExcel(GetAsyAdoVehicleDetailsHistoryExcelInput input)
        {
            var data = await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
            return _calendarListExcelExporter.ExportToHistoricalFile(data);
        }

        public async Task<List<long?>> GetChangedRecords()
        {
            return await _historicalDataAppService.GetChangedRecordIds("AsyAdoVehicleDetails");
        }


        public async Task<PagedResultDto<AsyAdoVehicleDetailsDto>> GetAll(GetAsyAdoVehicleDetailsInput input)
        {
            string _sql = "Exec ASY_ADO_VEHICLE_DETAILS_SEARCH_VEHICLE @p_Cfc, @p_BodyNo, @p_LotNo, @p_Vin, @p_SequenceNo";

            IEnumerable<AsyAdoVehicleDetailsDto> result = await _dapperRepo.QueryAsync<AsyAdoVehicleDetailsDto>(_sql, new
            {
                p_Cfc = input.Cfc,
                p_BodyNo = input.BodyNo,
                p_LotNo = input.LotNo,
                p_Vin = input.Vin,
                p_SequenceNo = input.SequenceNo
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();
            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            return new PagedResultDto<AsyAdoVehicleDetailsDto>(
               totalCount,
               pagedAndFiltered
            );
        }


        public async Task<FileDto> GetVehicleDetailsToExcel(GetAsyAdoVehicleDetailsExportInput input)
        {
            string _sql = "Exec ASY_ADO_VEHICLE_DETAILS_SEARCH_VEHICLE @p_Cfc,@p_BodyNo, @p_LotNo, @p_Vin, @p_SequenceNo";

            IEnumerable<AsyAdoVehicleDetailsExcelDto> result = await _dapperRepo.QueryAsync<AsyAdoVehicleDetailsExcelDto>(_sql, new
            {
                p_Cfc = input.Cfc,
                p_BodyNo = input.BodyNo,
                p_LotNo = input.LotNo,
                p_Vin = input.Vin,
                p_SequenceNo = input.SequenceNo
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
