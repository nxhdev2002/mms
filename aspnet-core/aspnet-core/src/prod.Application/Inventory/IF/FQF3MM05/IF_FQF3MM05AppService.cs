using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.IF.FQF3MM02.Dto;
using prod.Inventory.IF.FQF3MM05.Dto;
using prod.Inventory.IF.FQF3MM05.Exporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prod.Inventory.IF.FQF3MM05
{
    [AbpAuthorize(AppPermissions.Pages_Interface_IF_FQF3MM05_View)]
    public class IF_FQF3MM05AppService : prodAppServiceBase, IIF_FQF3MM05AppService
    {
        private readonly IDapperRepository<IF_FQF3MM05, long> _dapperRepo;
        private readonly IIF_FQF3MM05ExcelExporter _calendarListExcelExporter;

        public IF_FQF3MM05AppService(IDapperRepository<IF_FQF3MM05, long> dapperRepo,
                                     IIF_FQF3MM05ExcelExporter calendarListExcelExporter)
        {
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<IF_FQF3MM05Dto>> GetAll(GetIF_FQF3MM05Input input)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM05_SEARCH @p_postingdate_from, @p_postingdate_to, @p_materialcodefrom, @p_valuationtypefrom";

            IEnumerable<IF_FQF3MM05Dto> result = await _dapperRepo.QueryAsync<IF_FQF3MM05Dto>(_sql, new
            {
                p_postingdate_from = input.PostingDateFrom,
                p_postingdate_to = input.PostingDateTo,
                p_materialcodefrom = input.MaterialCodeFrom,
                p_valuationtypefrom = input.ValuationTypeFrom
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<IF_FQF3MM05Dto>(
                totalCount,
                 pagedAndFiltered
            );
        }

        public async Task<FileDto> GetFQF3MM05ToExcel(GetIF_FQF3MM05ExportInput input)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM05_SEARCH @p_postingdate_from, @p_postingdate_to, @p_materialcodefrom, @p_valuationtypefrom";

            IEnumerable<IF_FQF3MM05Dto> result = await _dapperRepo.QueryAsync<IF_FQF3MM05Dto>(_sql, new
            {
                p_postingdate_from = input.PostingDateFrom,
                p_postingdate_to = input.PostingDateTo,
                p_materialcodefrom = input.MaterialCodeFrom,
                p_valuationtypefrom = input.ValuationTypeFrom
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        [AbpAuthorize(AppPermissions.Pages_Interface_IF_FQF3MM05_ReCreate)]

        public async Task ReCreateDataFQF3MM05(DateTime date)
        {
            string _sql = "Exec JOB_INV_IF_FQF3MM05_RE_CREATE @p_date";

            await _dapperRepo.QueryAsync<IF_FQF3MM05Dto>(_sql, new
            {
                p_date = date
            });
        }

        public async Task<PagedResultDto<GetIF_FQF3MM05_VALIDATE>> GetInvInterfaceFQF3MM05Validate(GetIF_FQF3MM05_VALIDATEInput input)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM05_VALIDATE @p_postingdate_from, @p_postingdate_to";

            IEnumerable<GetIF_FQF3MM05_VALIDATE> result = await _dapperRepo.QueryAsync<GetIF_FQF3MM05_VALIDATE>(_sql, new
            {
                p_postingdate_from = input.PostingDateFrom,
                p_postingdate_to = input.PostingDateTo
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<GetIF_FQF3MM05_VALIDATE>(
                totalCount,
                 pagedAndFiltered
            );
        }


        public async Task<FileDto> GetFQF3MM05VALIDATEToExcel(GetIF_FQF3MM05VALIDATEExportInput input)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM05_VALIDATE @p_postingdate_from, @p_postingdate_to";

            IEnumerable<GetIF_FQF3MM05_VALIDATE> result = await _dapperRepo.QueryAsync<GetIF_FQF3MM05_VALIDATE>(_sql, new
            {
                p_postingdate_from = input.PostingDateFrom,
                p_postingdate_to = input.PostingDateTo,
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportValidateToFile(exportToExcel);
        }

        //validate Result
        public async Task<PagedResultDto<GetIF_FQF3MM05_VALIDATE>> GetInvInterfaceFQF3MM05ValidateResult(GetIF_FQF3MM05_VALIDATEInput input)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM05_VALIDATE_RESULT @p_postingdate_from, @p_postingdate_to";

            IEnumerable<GetIF_FQF3MM05_VALIDATE> result = await _dapperRepo.QueryAsync<GetIF_FQF3MM05_VALIDATE>(_sql, new
            {
                p_postingdate_from = input.PostingDateFrom,
                p_postingdate_to = input.PostingDateTo
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<GetIF_FQF3MM05_VALIDATE>(
                totalCount,
                 pagedAndFiltered
            );
        }


        public async Task<FileDto> GetFQF3MM05VALIDATEResultToExcel(GetIF_FQF3MM05VALIDATEExportInput input)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM05_VALIDATE_RESULT @p_postingdate_from, @p_postingdate_to";

            IEnumerable<GetIF_FQF3MM05_VALIDATE> result = await _dapperRepo.QueryAsync<GetIF_FQF3MM05_VALIDATE>(_sql, new
            {
                p_postingdate_from = input.PostingDateFrom,
                p_postingdate_to = input.PostingDateTo,
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportValidateToFile(exportToExcel);
        }
    }
}
