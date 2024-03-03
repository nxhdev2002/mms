using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.HistoricalData;
using prod.Inventory.GPS.Dto;
using prod.Master.Cmm;
using prod.Master.Cmm.Dto;
using prod.Master.Common.Dto;
using prod.Master.Common.MMValidationResult.Exporting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prod.Master.Common
{

    [AbpAuthorize(AppPermissions.Pages_Master_Cmm_MMValidationResult_View)]
    public class MstCommonMMValidationResultAppService : prodAppServiceBase, IMstCmmMMValidationResultAppService
    {
        private readonly IDapperRepository<MstCmmMMValidationResult, long> _dapperRepo;
        private readonly IRepository<MstCmmMMValidationResult, long> _repo;
        private readonly IMstCmmMMValidationResultExcelExporter _calendarListExcelExporter;
        private readonly IHistoricalDataAppService _historicalDataAppService;

        public MstCommonMMValidationResultAppService(IRepository<MstCmmMMValidationResult, long> repo,
                                         IDapperRepository<MstCmmMMValidationResult, long> dapperRepo,
                                        IMstCmmMMValidationResultExcelExporter calendarListExcelExporter,
                                        IHistoricalDataAppService historicalDataAppService
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
            _historicalDataAppService = historicalDataAppService;
        }

        public async Task<List<string>> GetMstCmmMMValidationResultHistory(GetMstCommonMMValidationResultHistoryInput input)
        {
            return await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
        }

        public async Task<FileDto> GetHistoricalDataToExcel(GetMstCommonMMValidationResultHistoryExcelInput input)
        {
            var data = await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
            return _calendarListExcelExporter.ExportToHistoricalFile(data);
        }

        public async Task<List<long?>> GetChangedRecords()
        {
            return await _historicalDataAppService.GetChangedRecordIds("MstCmmMMValidationResult");
        }
        public async Task<PagedResultDto<MstCommonMMValidationResultDto>> GetAll(GetMstCommonMMValidationResultInput input)
        {
            string _sql = "Exec MST_CMM_MM_VALIDATION_RESULT_SEARCH @p_MaterialCode, @p_MaterialGroup, @p_RuleCode, @p_RuleItem, @p_Resultfield";

            IEnumerable<MstCommonMMValidationResultDto> result = await _dapperRepo.QueryAsync<MstCommonMMValidationResultDto>(_sql, new
            {
                p_MaterialCode = input.MaterialCode,
                p_MaterialGroup = input.MaterialGroup,
                p_RuleCode = input.RuleCode,
                p_RuleItem = input.RuleItem,
                p_Resultfield = input.Resultfield
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<MstCommonMMValidationResultDto>(
                totalCount,
                listResult
               );
        }

        public async Task<FileDto> GetCmmMMValidationResultToExcel(GetMstCommonMMValidationResultExportInput input)
        {
            string _sql = "Exec MST_CMM_MM_VALIDATION_RESULT_SEARCH @p_MaterialCode, @p_MaterialGroup, @p_RuleCode, @p_RuleItem, @p_Resultfield";

            IEnumerable<MstCommonMMValidationResultDto> result = await _dapperRepo.QueryAsync<MstCommonMMValidationResultDto>(_sql, new
            {
                p_MaterialCode = input.MaterialCode,
                p_MaterialGroup = input.MaterialGroup,
                p_RuleCode = input.RuleCode,
                p_RuleItem = input.RuleItem,
                p_Resultfield = input.Resultfield
            });

            return _calendarListExcelExporter.ExportToFile(result.ToList());
        }
    }
}
