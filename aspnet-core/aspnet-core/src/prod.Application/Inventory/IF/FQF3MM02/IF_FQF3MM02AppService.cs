using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.IF.FQF3MM01.Dto;
using prod.Inventory.IF.FQF3MM02.Dto;
using prod.Inventory.IF.FQF3MM02.Exporting;
using prod.Inventory.IF.FQF3MM06.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Inventory.IF.FQF3MM02
{
    [AbpAuthorize(AppPermissions.Pages_Interface_IF_FQF3MM02_View)]
    public class IF_FQF3MM02AppService : prodAppServiceBase, IIF_FQF3MM02AppService
    {
        private readonly IDapperRepository<IF_FQF3MM02, long> _dapperRepo;
        private readonly IRepository<IF_FQF3MM02, long> _repo;
        private readonly IIF_FQF3MM02ExcelExporter _calendarListExcelExporter;

        public IF_FQF3MM02AppService(IRepository<IF_FQF3MM02, long> repo,
                                         IDapperRepository<IF_FQF3MM02, long> dapperRepo,
                                        IIF_FQF3MM02ExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<IF_FQF3MM02Dto>> GetAll(GetIF_FQF3MM02Input input)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM02_SEARCH @p_postingdate_from, @p_postingdate_to, @p_PartCode, @p_MaterialCode";

            IEnumerable<IF_FQF3MM02Dto> result = await _dapperRepo.QueryAsync<IF_FQF3MM02Dto>(_sql, new
            {
                p_postingdate_from = input.PostingDateFrom,
                p_postingdate_to = input.PostingDateTo,
                p_PartCode = input.PartCode,
                p_MaterialCode = input.MaterialCode
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<IF_FQF3MM02Dto>(
                totalCount,
                 pagedAndFiltered
            );
        }

        public async Task<FileDto> GetFQF3MM02ToExcel(GetIF_FQF3MM02ExportInput input)
        {

            string _sql = "Exec INV_INTERFACE_FQF3MM02_SEARCH @p_postingdate_from, @p_postingdate_to, @p_PartCode, @p_MaterialCode";

            IEnumerable<IF_FQF3MM02Dto> result = await _dapperRepo.QueryAsync<IF_FQF3MM02Dto>(_sql, new
            {
                @p_postingdate_from = input.PostingDateFrom,
                @p_postingdate_to = input.PostingDateTo,
                @p_PartCode = input.PartCode,
                @p_MaterialCode = input.MaterialCode
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        [AbpAuthorize(AppPermissions.Pages_Interface_IF_FQF3MM02_ReCreate)]

        public async Task ReCreateDataFQF3MM02(DateTime date)
        {
            string _sql = "Exec JOB_INV_IF_FQF3MM02_RE_CREATE @p_date";

            await _dapperRepo.QueryAsync<IF_FQF3MM02Dto>(_sql, new
            {
                p_date = date
            });
        }


        public async Task<PagedResultDto<GetIF_FQF3MM02_VALIDATE>> GetInvInterfaceFQF3MM02Validate(GetIF_FQF3MM02_VALIDATEInput input)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM02_VALIDATE @p_postingdate_from,@p_postingdate_to";

            IEnumerable<GetIF_FQF3MM02_VALIDATE> result = await _dapperRepo.QueryAsync<GetIF_FQF3MM02_VALIDATE>(_sql, new
            {
                p_postingdate_from = input.PostingDateFrom,
                p_postingdate_to = input.PostingDateTo,

            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<GetIF_FQF3MM02_VALIDATE>(
                totalCount,
                 pagedAndFiltered
            );
        }


        public async Task<FileDto> GetValidate_FQF3MM02ToExcel(GetIF_FQF3MM02_VALIDATE_Input input)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM02_VALIDATE @p_postingdate_from,@p_postingdate_to";

            IEnumerable<GetIF_FQF3MM02_VALIDATE> result = await _dapperRepo.QueryAsync<GetIF_FQF3MM02_VALIDATE>(_sql, new
            {
                p_postingdate_from = input.PostingDateFrom,
                p_postingdate_to = input.PostingDateTo,

            });


            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportValidateToFile(exportToExcel);
        }

        //validate Result
        public async Task<PagedResultDto<GetIF_FQF3MM02_VALIDATE>> GetInvInterfaceFQF3MM02ValidateResult(GetIF_FQF3MM02_VALIDATEInput input)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM02_VALIDATE_RESULT @p_postingdate_from,@p_postingdate_to";

        IEnumerable<GetIF_FQF3MM02_VALIDATE> result = await _dapperRepo.QueryAsync<GetIF_FQF3MM02_VALIDATE>(_sql, new
        {
            p_postingdate_from = input.PostingDateFrom,
            p_postingdate_to = input.PostingDateTo,

        });

        var listResult = result.ToList();

        var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

        var totalCount = result.ToList().Count();

            return new PagedResultDto<GetIF_FQF3MM02_VALIDATE>(
                totalCount,
                 pagedAndFiltered
            );
        }


    public async Task<FileDto> GetValidateResult_FQF3MM02ToExcel(GetIF_FQF3MM02_VALIDATE_Input input)
    {
        string _sql = "Exec INV_INTERFACE_FQF3MM02_VALIDATE_RESULT @p_postingdate_from,@p_postingdate_to";

        IEnumerable<GetIF_FQF3MM02_VALIDATE> result = await _dapperRepo.QueryAsync<GetIF_FQF3MM02_VALIDATE>(_sql, new
        {
            p_postingdate_from = input.PostingDateFrom,
            p_postingdate_to = input.PostingDateTo,

        });


        var exportToExcel = result.ToList();
        return _calendarListExcelExporter.ExportValidateToFile(exportToExcel);
    }
}
}
