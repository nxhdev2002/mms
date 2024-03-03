using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.Gps.Issuings.Dto;
using prod.Inventory.IF.FQF3MM02.Dto;
using prod.Inventory.IF.FQF3MM04.Dto;
using prod.Inventory.IF.FQF3MM05.Exporting;
using prod.Inventory.IF.FQF3MM06.Dto;
using prod.Inventory.IF.FQF3MM07.Dto;
using prod.Inventory.IF.FQF3MM07.Dto;
using prod.Inventory.IF.FQF3MM07.Exporting;
using prod.SapIF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.IF.FQF3MM07
{
    [AbpAuthorize(AppPermissions.Pages_Interface_IF_FQF3MM07_View)]
    public class IF_FQF3MM07AppService : prodAppServiceBase, IIF_FQF3MM07AppService
    {
        private readonly IDapperRepository<IF_FQF3MM07, long> _dapperRepo;
        private readonly IRepository<IF_FQF3MM07, long> _repo;
        private readonly IIF_FQF3MM07ExcelExporter _calendarListExcelExporter;
        private readonly IDapperRepository<SapIFLogging, long> _sapIFLoggingRepo;


        public IF_FQF3MM07AppService(IRepository<IF_FQF3MM07, long> repo,
                                         IDapperRepository<IF_FQF3MM07, long> dapperRepo,
                                         IIF_FQF3MM07ExcelExporter calendarListExcelExporter,
                                         IDapperRepository<SapIFLogging, long> sapIFLoggingRepo
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
            _sapIFLoggingRepo = sapIFLoggingRepo;
        }

        public async Task<PagedResultDto<IF_FQF3MM07Dto>> GetAll(GetIF_FQF3MM07Input input)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM07_SEARCH @p_document_no, @p_documentdate_from, @p_documentdate_to, @p_postingdate_from, @p_postingdate_to";

            IEnumerable<IF_FQF3MM07Dto> result = await _dapperRepo.QueryAsync<IF_FQF3MM07Dto>(_sql, new
            {
                p_document_no = input.DocumentNo,
                p_documentdate_from = input.DocumentDateFrom,
                p_documentdate_to = input.DocumentDateTo,
                p_postingdate_from = input.PostingDateFrom,
                p_postingdate_to = input.PostingDateTo
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<IF_FQF3MM07Dto>(
                totalCount,
                 pagedAndFiltered
            );
        }
        public async Task<FileDto> GetFQF3MM07ToExcel(GetIF_FQF3MM07ExportInput input)
        {

            string _sql = "Exec INV_INTERFACE_FQF3MM07_SEARCH @p_document_no, @p_documentdate_from, @p_documentdate_to, @p_postingdate_from, @p_postingdate_to";

            IEnumerable<IF_FQF3MM07Dto> result = await _dapperRepo.QueryAsync<IF_FQF3MM07Dto>(_sql, new
            {
                p_document_no = input.DocumentNo,
                p_documentdate_from = input.DocumentDateFrom,
                p_documentdate_to = input.DocumentDateTo,
                p_postingdate_from = input.PostingDateFrom,
                p_postingdate_to = input.PostingDateTo
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        public async Task<FileDto> GetFundCommitmentItemDMToExcel()
        {

            string _sql = "Exec [INV_FUND_COMMITMENT_ITEM_DM_SEARCH] @p_id";

            IEnumerable<GetIF_FundCommitmentItemDMExportDto> result = await _dapperRepo.QueryAsync<GetIF_FundCommitmentItemDMExportDto>(_sql, new
            {
                p_id = 0
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportItemDMToFile(exportToExcel);
        }

        public async Task<PagedResultDto<GetIF_FundCommitmentItemDMExportDto>> GetViewFundCommmitmentItemDM(GetIF_FQF3MM07_Fundcmm_Input input)
        {
            string _sql = "Exec INV_FUND_COMMITMENT_ITEM_DM_SEARCH @p_id";

            IEnumerable<GetIF_FundCommitmentItemDMExportDto> result = await _dapperRepo.QueryAsync<GetIF_FundCommitmentItemDMExportDto>(_sql, new
            {
                p_id = input.IdFund
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<GetIF_FundCommitmentItemDMExportDto>(
                totalCount,
                 pagedAndFiltered
            );
        }

        public async Task<PagedResultDto<LoggingResponseDetailsOnlBudgetCheckDto>> GetViewLoggingResponseDetailsOnlBudgetCheck(GetIF_FQF3MM07_OnlBudgetCheck_Input input)
        {
            string _sql = "Exec INV_LOGGING_RESPONSE_ONL_BUDGET_CHECK_BY_DOCUMENTNO @p_DocumentNo";

            IEnumerable<LoggingResponseDetailsOnlBudgetCheckDto> result = await _dapperRepo.QueryAsync<LoggingResponseDetailsOnlBudgetCheckDto>(_sql, new {
                p_DocumentNo = input.p_DocumentNo
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<LoggingResponseDetailsOnlBudgetCheckDto>(
                totalCount,
                 pagedAndFiltered
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Interface_IF_FQF3MM07_ReCreate)]

        public async Task ReCreateDataFQF3MM07(DateTime date)
        {
            string _sql = "Exec JOB_INV_IF_FQF3MM07_RE_CREATE @p_date";

            await _dapperRepo.QueryAsync<IF_FQF3MM07Dto>(_sql, new
            {
                p_date = date
            });
        }

        public async Task<GetRequestBudgetCheck> GetViewRequestBudgetCheck(long logingId)
        {
            IEnumerable<GetRequestBudgetCheck> result = await _sapIFLoggingRepo.QueryAsync<GetRequestBudgetCheck>(@"
                exec [dbo].[INV_VIEW_REQUEST_BUDGET_CHECK] @id_Logging
            ", new
            {
                id_Logging = logingId,
            });

            return result.FirstOrDefault();
        }

        public async Task<PagedResultDto<GetIF_FQF3MM07_VALIDATE>> GetInvInterfaceFQF3MM07Validate(GetIF_FQF3MM07_VALIDATEInput input)
        {
            string _sql = "Exec [INV_INTERFACE_FQF3MM07_VALIDATE] @p_postingdate_from, @p_postingdate_to";

            IEnumerable<GetIF_FQF3MM07_VALIDATE> result = await _dapperRepo.QueryAsync<GetIF_FQF3MM07_VALIDATE>(_sql, new
            {
                p_postingdate_from = input.PostingDateFrom,
                p_postingdate_to = input.PostingDateTo,
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<GetIF_FQF3MM07_VALIDATE>(
                totalCount,
                 pagedAndFiltered
            );
        }


        public async Task<FileDto> GetFQF3MM07VALIDATEToExcel(GetIF_FQF3MM07_VALIDATEExcelInput input)
        {
            string _sql = "Exec [INV_INTERFACE_FQF3MM07_VALIDATE] @p_postingdate_from, @p_postingdate_to";

            IEnumerable<GetIF_FQF3MM07_VALIDATE> result = await _dapperRepo.QueryAsync<GetIF_FQF3MM07_VALIDATE>(_sql, new
            {
                p_postingdate_from = input.PostingDateFrom,
                p_postingdate_to = input.PostingDateTo,
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportValidateToFile(exportToExcel);
        }

        //validate Result
        public async Task<PagedResultDto<GetIF_FQF3MM07_VALIDATE>> GetInvInterfaceFQF3MM07ValidateResult(GetIF_FQF3MM07_VALIDATEInput input)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM07_VALIDATE_RESULT @p_postingdate_from, @p_postingdate_to";

            IEnumerable<GetIF_FQF3MM07_VALIDATE> result = await _dapperRepo.QueryAsync<GetIF_FQF3MM07_VALIDATE>(_sql, new
            {
                p_postingdate_from = input.PostingDateFrom,
                p_postingdate_to = input.PostingDateTo,
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<GetIF_FQF3MM07_VALIDATE>(
                totalCount,
                 pagedAndFiltered
            );
        }

        public async Task<FileDto> GetFQF3MM07VALIDATEResultToExcel(GetIF_FQF3MM07_VALIDATEExcelInput input)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM07_VALIDATE_RESULT @p_postingdate_from, @p_postingdate_to";

            IEnumerable<GetIF_FQF3MM07_VALIDATE> result = await _dapperRepo.QueryAsync<GetIF_FQF3MM07_VALIDATE>(_sql, new
            {
                p_postingdate_from = input.PostingDateFrom,
                p_postingdate_to = input.PostingDateTo,
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportValidateToFile(exportToExcel);
        }


        public async Task<GetRequestBudgetCheckFQF3MM07> GetViewRequestFuncommitment(long logingId)
        {
            IEnumerable<GetRequestBudgetCheckFQF3MM07> result = await _sapIFLoggingRepo.QueryAsync<GetRequestBudgetCheckFQF3MM07>(@"
                exec [dbo].[INV_VIEW_REQUEST_FUN_COMMITMENT] @id_Logging
            ", new
            {
                id_Logging = logingId,
            });

            return result.FirstOrDefault();
        }

    }
}
