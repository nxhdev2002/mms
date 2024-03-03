using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.IF.FQF3MM05.Dto;
using prod.Inventory.IF.FQF3MM05.Exporting;
using prod.Inventory.IF.FQF3MM06.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prod.Inventory.IF.FQF3MM06
{
    [AbpAuthorize(AppPermissions.Pages_Interface_IF_FQF3MM06_View)]
    public class IF_FQF3MM06AppService : prodAppServiceBase, IIF_FQF3MM06AppService
    {
        private readonly IDapperRepository<IF_FQF3MM06, long> _dapperRepo;
        private readonly IIF_FQF3MM06ExcelExporter _calendarListExcelExporter;


        public IF_FQF3MM06AppService(IDapperRepository<IF_FQF3MM06, long> dapperRepo, IIF_FQF3MM06ExcelExporter calendarListExcelExporter)
        {
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<IF_FQF3MM06Dto>> GetAll(GetIF_FQF3MM06Input input)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM06_SEARCH @p_MaterialCode, @p_MaterialDescription,@p_createDate_from, @p_createDate_to";

            IEnumerable<IF_FQF3MM06Dto> result = await _dapperRepo.QueryAsync<IF_FQF3MM06Dto>(_sql, new
            {
                p_MaterialCode = input.MaterialCode,
                p_MaterialDescription = input.MaterialDescription,
                p_createDate_from = input.CreateDateFrom,
                p_createDate_to = input.CreateDateTo
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<IF_FQF3MM06Dto>(
                totalCount,
                 pagedAndFiltered
            );
        }
        public async Task<FileDto> GetFQF3MM06ToExcel(GetIF_FQF3MM06ExportInput input)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM06_SEARCH @p_MaterialCode, @p_MaterialDescription,@p_createDate_from, @p_createDate_to";

            IEnumerable<IF_FQF3MM06Dto> result = await _dapperRepo.QueryAsync<IF_FQF3MM06Dto>(_sql, new
            {
                p_MaterialCode = input.MaterialCode,
                p_MaterialDescription = input.MaterialDescription,
                p_createDate_from = input.CreateDateFrom,
                p_createDate_to = input.CreateDateTo
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }
        public async Task ReCreateDataFQF3MM06(DateTime date)
        {
            string _sql = "Exec JOB_INV_IF_FQF3MM06_RE_CREATE @p_date";

            await _dapperRepo.QueryAsync<IF_FQF3MM06Dto>(_sql, new
            {
                p_date = date
            });
        }

        public async Task<PagedResultDto<GetIF_FQF3MM06_VALIDATE>> GetInvInterfaceFQF3MM06Validate(GetIF_FQF3MM06_VALIDATEInput input)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM06_VALIDATE @p_createDate_from, @p_createDate_to";

            IEnumerable<GetIF_FQF3MM06_VALIDATE> result = await _dapperRepo.QueryAsync<GetIF_FQF3MM06_VALIDATE>(_sql, new
            {
                @p_createDate_from = input.CreateDateFrom,
                @p_createDate_to = input.CreateDateTo
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<GetIF_FQF3MM06_VALIDATE>(
                totalCount,
                pagedAndFiltered
            );
        }

        public async Task<FileDto> GetFQF3MM06VALIDATEToExcel(GetIF_FQF3MM06VALIDATEExportInput input)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM06_VALIDATE @p_createDate_from, @p_createDate_to";

            IEnumerable<GetIF_FQF3MM06_VALIDATE> result = await _dapperRepo.QueryAsync<GetIF_FQF3MM06_VALIDATE>(_sql, new
            {
                p_createDate_from = input.CreateDateFrom,
                p_createDate_to = input.CreateDateTo,
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportValidateToFile(exportToExcel);
        }

        //validate Result
        public async Task<PagedResultDto<GetIF_FQF3MM06_VALIDATE>> GetInvInterfaceFQF3MM06ValidateResult(GetIF_FQF3MM06_VALIDATEInput input)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM06_VALIDATE_RESULT @p_createDate_from, @p_createDate_to";

            IEnumerable<GetIF_FQF3MM06_VALIDATE> result = await _dapperRepo.QueryAsync<GetIF_FQF3MM06_VALIDATE>(_sql, new
            {
                @p_createDate_from = input.CreateDateFrom,
                @p_createDate_to = input.CreateDateTo
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<GetIF_FQF3MM06_VALIDATE>(
                totalCount,
                pagedAndFiltered
            );
        }

        public async Task<FileDto> GetFQF3MM06VALIDATEResultToExcel(GetIF_FQF3MM06VALIDATEExportInput input)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM06_VALIDATE_RESULT @p_createDate_from, @p_createDate_to";

            IEnumerable<GetIF_FQF3MM06_VALIDATE> result = await _dapperRepo.QueryAsync<GetIF_FQF3MM06_VALIDATE>(_sql, new
            {
                p_createDate_from = input.CreateDateFrom,
                p_createDate_to = input.CreateDateTo,
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportValidateToFile(exportToExcel);
        }
    }
}
