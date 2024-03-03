using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using prod.Authorization;
using prod.Dto;
using prod.HistoricalData;
using prod.Inventory.CKD.Dto;
using prod.Inventory.GPS.Dto;
using prod.Master.Cmm;
using prod.Master.Common.Dto;
using prod.Master.Common.Exporting;
using prod.Master.Common.MaterialMaster;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.Common
{
    [AbpAuthorize(AppPermissions.Pages_MaterialMaster_MaterialMaster_View)]
    public class MstCmmMaterialMasterAppService : prodAppServiceBase, IMstCmmMaterialMasterAppService
    {
        private readonly IDapperRepository<MstCmmMaterialMaster, long> _dapperRepo;
        private readonly IDapperRepository<MstCmmMMValidationResult, long> _dapperRepoValidate;
        private readonly IRepository<MstCmmMaterialMaster, long> _repo;
        private readonly IDapperRepository<MstCmmLotCodeGrade, long> _mstCmmLotCodeGradeRepo;
        private readonly IMstCmmMaterialMasterExcelExporter _calendarListExcelExporter;
        private readonly IHistoricalDataAppService _historicalDataAppService;

        public MstCmmMaterialMasterAppService(IRepository<MstCmmMaterialMaster, long> repo,
                                             IDapperRepository<MstCmmMMValidationResult, long> dapperRepoValidate,
                                             IDapperRepository<MstCmmLotCodeGrade, long> mstCmmLotCodeGradeRepo,
                                             IDapperRepository<MstCmmMaterialMaster, long> dapperRepo,
                                             IMstCmmMaterialMasterExcelExporter calendarListExcelExporter,
                                             IHistoricalDataAppService historicalDataAppService
            )
        {
            _repo = repo;
            _dapperRepoValidate = dapperRepoValidate;
            _mstCmmLotCodeGradeRepo = mstCmmLotCodeGradeRepo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
            _historicalDataAppService = historicalDataAppService;
        }

        public async Task<List<string>> GetMstCmmMaterialMasterHistory(GetMstCmmMaterialMasterHistoryInput input)
        {
            return await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
        }

        public async Task<FileDto> GetHistoricalDataToExcel(GetMstCmmMaterialMasterHistoryExcelInput input)
        {
            var data = await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
            return _calendarListExcelExporter.ExportToHistoricalFile(data);
        }

        public async Task<List<long?>> GetChangedRecords()
        {
            return await _historicalDataAppService.GetChangedRecordIds("MstCmmMaterialMaster");
        }

        public async Task<PagedResultDto<MstCmmMaterialMasterDto>> GetAll(GetMstCmmMaterialMasterInput input)
        {
            string _sql = "Exec MST_CMM_MATERIAL_MASTER_SEARCH @p_materialcode, @p_materialgroup, @p_valuationtype";


            IEnumerable<MstCmmMaterialMasterDto> result = await _dapperRepo.QueryAsync<MstCmmMaterialMasterDto>(_sql, new
            {
                p_materialcode = input.MaterialCode,
                p_materialgroup = input.MaterialGroup,
                p_valuationtype = input.ValuationType
            });

            var listResult = result.ToList();
            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<MstCmmMaterialMasterDto>(
                totalCount,
                pagedAndFiltered);
        }


        public async Task<FileDto> GetMaterialMasterToExcel(MstCmmMaterialMasterExportInput input)
        {
            string _sql = "Exec MST_CMM_MATERIAL_MASTER_SEARCH @p_materialcode, @p_materialgroup, @p_valuationtype";


            IEnumerable<MstCmmMaterialMasterDto> result = await _dapperRepo.QueryAsync<MstCmmMaterialMasterDto>(_sql, new
            {
                p_materialcode = input.MaterialCode,
                p_materialgroup = input.MaterialGroup,
                p_valuationtype = input.ValuationType
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }


        public async Task<PagedResultDto<MstCmmMaterialMasterDto>> GetDataMaterialbyId(long? IdMaterial)
        {
            string _sql = "Exec VIEW_DATA_MATERIAL_BY_ID @p_id_material";


            IEnumerable<MstCmmMaterialMasterDto> result = await _dapperRepo.QueryAsync<MstCmmMaterialMasterDto>(_sql, new
            {
                p_id_material = IdMaterial
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<MstCmmMaterialMasterDto>(
                totalCount,
                listResult);
        }

        public async Task<PagedResultDto<MstCmmMaterialMasterDto>> GetDataLotCodeGradebyId(long? Idcar)
        {
            string _sql = "Exec VIEW_DATA_LOT_CODE_GRADE_BY_ID @p_id_car";


            IEnumerable<MstCmmMaterialMasterDto> result = await _mstCmmLotCodeGradeRepo.QueryAsync<MstCmmMaterialMasterDto>(_sql, new
            {
                p_id_car = Idcar
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<MstCmmMaterialMasterDto>(
                totalCount,
                listResult);
        }

        public async Task ValidateCheckingRule()
        {
            Dapper.SqlMapper.Settings.CommandTimeout = 900;

            string _calculator = "Exec MST_CMM_MM_CHECKING_RULE_RUNALL";

            await _dapperRepo.QueryAsync<MstCmmMMValidationResultDto>(_calculator, new { });      
        }

        public async Task<FileDto> GetValidateCmmMMToExcel(GetMasterialValidationResultInput input)
        {
            string _sql = "Exec MST_CMM_MM_VALIDATION_RESULT_SEARCH @p_MaterialCode, @p_MaterialGroup, @p_RuleCode, @p_RuleItem, @p_Resultfield";

            IEnumerable<MstCmmMMValidationResultDto> result = await _dapperRepo.QueryAsync<MstCmmMMValidationResultDto>(_sql, new
            {
                p_MaterialCode = input.MaterialCode,
                p_MaterialGroup = input.MaterialGroup,
                p_RuleCode = input.RuleCode,
                p_RuleItem = input.RuleItem,
                p_Resultfield = input.Resultfield
            });

            return _calendarListExcelExporter.ExportToFileValitate(result.ToList());

        }

        public async Task<List<MstCmmMMValidationResultDto>> GetDataValidationResult(GetMasterialValidationResultInput input)
        {
            string _sql = "Exec MST_CMM_MM_VALIDATION_RESULT_SEARCH @p_MaterialCode, @p_MaterialGroup, @p_RuleCode, @p_RuleItem, @p_Resultfield";

            IEnumerable<MstCmmMMValidationResultDto> result = await _dapperRepo.QueryAsync<MstCmmMMValidationResultDto>(_sql, new
            {
                p_MaterialCode = input.MaterialCode,
                p_MaterialGroup = input.MaterialGroup,
                p_RuleCode = input.RuleCode,
                p_RuleItem = input.RuleItem,
                p_Resultfield = input.Resultfield

            });

            return result.ToList();
        }

    }
}
