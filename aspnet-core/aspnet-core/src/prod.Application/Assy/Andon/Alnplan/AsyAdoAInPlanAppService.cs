using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using prod.Assy.Andon.Dto;
using prod.Assy.Andon.Exporting;
using prod.Authorization;
using prod.Dto;
using prod.HistoricalData;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Assy.Andon
{
    [AbpAuthorize(AppPermissions.Pages_ProdPlan_AInPlan_View)]
    public class AsyAdoAInPlanAppService : prodAppServiceBase, IAsyAdoAInPlanAppService
    {
        private readonly IDapperRepository<AsyAdoAInPlan, long> _dapperRepo;
        private readonly IRepository<AsyAdoAInPlan, long> _repo;
        private readonly IAsyAdoAInPlanExcelExporter _calendarListExcelExporter;
        private readonly IHistoricalDataAppService _historicalDataAppService;

        public AsyAdoAInPlanAppService(IRepository<AsyAdoAInPlan, long> repo,
                                         IDapperRepository<AsyAdoAInPlan, long> dapperRepo,
                                        IAsyAdoAInPlanExcelExporter calendarListExcelExporter,
                                        IHistoricalDataAppService historicalDataAppService
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
            _historicalDataAppService = historicalDataAppService;
        }
        public async Task<List<string>> GetAsyAdoAInPlanHistory(GetAsyAdoVehicleDetailsHistoryInput input)
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
            return await _historicalDataAppService.GetChangedRecordIds("AsyAdoAInPlan");
        }
        public async Task<PagedResultDto<AsyAdoAInPlanDto>> GetAll(GetAsyAdoAInPlanInput input)
        {
            string _sql = "Exec ASY_ADO_AIN_PLAN_SEARCH_VEHICLE_SEARCH_VEHICLE @p_Model, @p_LotNo, @p_Grade, @p_BodyNo, @p_SequenceNo,  @p_VinNo";

            IEnumerable<AsyAdoAInPlanDto> result = await _dapperRepo.QueryAsync<AsyAdoAInPlanDto>(_sql, new
            {
     


                p_Model = input.Model,
                p_LotNo = input.LotNo,
                p_Grade = input.Grade,
                p_BodyNo = input.BodyNo,
                p_SequenceNo = input.SequenceNo,
                p_VinNo = input.VinNo
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();
            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            return new PagedResultDto<AsyAdoAInPlanDto>(
               totalCount,
               pagedAndFiltered
            );
        }


        public async Task<FileDto> GetAInPlanToExcel(GetAsyAdoAInPlanExportInput input)
        {
            string _sql = "Exec ASY_ADO_AIN_PLAN_SEARCH_VEHICLE_SEARCH_VEHICLE @p_Model, @p_LotNo, @p_Grade, @p_BodyNo, @p_SequenceNo,  @p_VinNo";

            IEnumerable<AsyAdoAInPlanDto> result = await _dapperRepo.QueryAsync<AsyAdoAInPlanDto>(_sql, new
            {
                p_Model = input.Model,
                p_LotNo = input.LotNo,
                p_Grade = input.Grade,
                p_BodyNo = input.BodyNo,
                p_SequenceNo = input.SequenceNo,
                p_VinNo = input.VinNo
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
