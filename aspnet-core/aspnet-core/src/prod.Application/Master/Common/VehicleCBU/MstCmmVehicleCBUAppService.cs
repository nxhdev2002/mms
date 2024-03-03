using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.HistoricalData;
using prod.Inventory.CKD.Dto;
using prod.Master.Cmm;
using prod.Master.Cmm.Dto;
using prod.Master.Common.Dto;
using prod.Master.Common.VehicleCBU.Dto;
using prod.Master.Common.VehicleCBU.Exporting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prod.Master.Common.VehicleCBU
{
    [AbpAuthorize(AppPermissions.Pages_Master_Cmm_VehicleCBU_View)]
    public class MstCmmVehicleCBUAppService : prodAppServiceBase, IMstCmmVehicleCBUAppService
    {
        private readonly IDapperRepository<MstCmmVehicleCBU, long> _dapperRepo;
        private readonly IDapperRepository<MstCmmVehicleCBUColor, long> _dapperRepoColor;
        private readonly IRepository<MstCmmVehicleCBU, long> _repo;
        private readonly IMstCmmVehicleCBUExcelExporter _calendarListExcelExporter;
        private readonly IHistoricalDataAppService _historicalDataAppService;

        public MstCmmVehicleCBUAppService(IRepository<MstCmmVehicleCBU, long> repo,
                                         IDapperRepository<MstCmmVehicleCBU, long> dapperRepo,
                                         IDapperRepository<MstCmmVehicleCBUColor, long> dapperRepoColor,
                                         IMstCmmVehicleCBUExcelExporter calendarListExcelExporter,
                                         IHistoricalDataAppService historicalDataAppService
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _dapperRepoColor = dapperRepoColor;
            _calendarListExcelExporter = calendarListExcelExporter;
            _historicalDataAppService = historicalDataAppService;
        }

        public async Task<List<string>> GetMstCmmVehicleCBUHistory(GetMstCmmVehicleCBUHistoryInput input)
        {
            return await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
        }

        public async Task<FileDto> GetHistoricalDataToExcel(GetMstCmmVehicleCBUHistoryExcelInput input)
        {
            var data = await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
            return _calendarListExcelExporter.ExportToHistoricalFile(data);
        }

        public async Task<List<long?>> GetChangedRecords()
        {
            return await _historicalDataAppService.GetChangedRecordIds("MstCmmVehicleCBU");
        }

            public async Task<PagedResultDto<MstCmmVehicleCBUColorDto>> GetVehicleCBUColorById(GetMstCmmVehicleCBUColorInput input)
        {
            string _sql = "Exec INV_MST_CMM_VEHICLE_CBU_COLOR_BY_ID @p_id";

            IEnumerable<MstCmmVehicleCBUColorDto> result = await _dapperRepoColor.QueryAsync<MstCmmVehicleCBUColorDto>(_sql, new
            {
                p_id = input.VehicleCBUId
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<MstCmmVehicleCBUColorDto>(
                totalCount,
                pagedAndFiltered);
        }
        public async Task<PagedResultDto<MstCmmMaterialMasterDto>> GetDataMaterialMasterbyId(long? Idcar)
        {
            string _sql = "Exec VIEW_DATA_MATERIAL_MASTER_CBU_ID @p_id_car";


            IEnumerable<MstCmmMaterialMasterDto> result = await _dapperRepo.QueryAsync<MstCmmMaterialMasterDto>(_sql, new
            {
                p_id_car = Idcar
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<MstCmmMaterialMasterDto>(
                totalCount,
                listResult);
        }
        public async Task<PagedResultDto<MstCmmVehicleCBUDto>> GetVehicleCBUSearch(GetMstCmmVehicleCBUInput input)
        {
            string _sql = "Exec INV_MST_CMM_VEHICLE_CBU_SEARCH @p_vehicle_type, @p_model, @p_marketing_code, @p_katashiki";

            IEnumerable<MstCmmVehicleCBUDto> result = await _dapperRepo.QueryAsync<MstCmmVehicleCBUDto>(_sql, new
            {
                p_vehicle_type = input.VehicleType,
                p_model = input.Model,
                p_marketing_code = input.MarketingCode,
                p_katashiki = input.Katashiki
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<MstCmmVehicleCBUDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetVehicleCBUToExcel(GetMstCmmVehicleCBUExportInput input)
        {
            string _sql = "Exec INV_MST_CMM_VEHICLE_CBU_SEARCH @p_vehicle_type, @p_model, @p_marketing_code, @p_katashiki";

            IEnumerable<MstCmmVehicleCBUDto> result = await _dapperRepo.QueryAsync<MstCmmVehicleCBUDto>(_sql, new
            {
                p_vehicle_type = input.VehicleType,
                p_model = input.Model,
                p_marketing_code = input.MarketingCode,
                p_katashiki = input.Katashiki
            });
            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        public async Task<FileDto> GetVehicleCBUColorToExcel(GetMstCmmVehicleCBUColorExportInput input)
        {
            string _sql = "Exec INV_MST_CMM_VEHICLE_CBU_COLOR_BY_ID @p_id";

            IEnumerable<MstCmmVehicleCBUColorDto> result = await _dapperRepoColor.QueryAsync<MstCmmVehicleCBUColorDto>(_sql, new
            {
                p_id = input.VehicleCBUId
            });
            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFileColor(exportToExcel);
        }

        [AbpAuthorize(AppPermissions.Pages_Master_Cmm_VehicleCBU_View)]
        public async Task ValidateCheckingRule()
        {
            Dapper.SqlMapper.Settings.CommandTimeout = 900;

            string _calculator = "Exec MST_CMM_MM_CHECKING_RULE_RUNALL";

            await _dapperRepo.QueryAsync<MstCmmVehicleCBUDto>(_calculator, new { });
        }



        public async Task<FileDto> GetValidateVehicleCBUToExcel(GetVehicleCBUValidationResultInput input)
        {
            string _sql = "Exec MST_CMM_MM_VALIDATION_RESULT_SEARCH @p_MaterialCode, @p_MaterialGroup, @p_RuleCode, @p_RuleItem, @p_Resultfield";

            IEnumerable<MstCmmVehicleCBUColorValidationResultDto> result = await _dapperRepo.QueryAsync<MstCmmVehicleCBUColorValidationResultDto>(_sql, new
            {
                p_MaterialCode = input.MaterialCode,
                p_MaterialGroup = input.MaterialGroup,
                p_RuleCode = input.RuleCode,
                p_RuleItem = input.RuleItem,
                p_Resultfield = input.Resultfield
            });

            return _calendarListExcelExporter.ExportToFileValitate(result.ToList());

        }

        public async Task<List<MstCmmVehicleCBUColorValidationResultDto>> GetDataValidationResult(GetVehicleCBUValidationResultInput input)
        {
            string _sql = "Exec MST_CMM_MM_VALIDATION_RESULT_SEARCH @p_MaterialCode, @p_MaterialGroup, @p_RuleCode, @p_RuleItem, @p_Resultfield";

            IEnumerable<MstCmmVehicleCBUColorValidationResultDto> result = await _dapperRepo.QueryAsync<MstCmmVehicleCBUColorValidationResultDto>(_sql, new
            {
                p_MaterialCode = input.MaterialCode,
                p_MaterialGroup = input.MaterialGroup,
                p_RuleCode = input.RuleCode,
                p_RuleItem = input.RuleItem,
                p_Resultfield = input.Resultfield

            });

            return result.ToList();
        }


        // EDIT
        [AbpAuthorize(AppPermissions.Pages_Master_Cmm_VehicleCBU_View)]
        public async Task UpdateCreateMaterial(UpdateCmmVehicleCBUCreateMaterial input)
        {
            string _sql = "Exec MST_CMM_VEHICLE_CBU_CREATE_MATERIAL @p_id";

            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_id = input.Id
            });
        }


    }
}