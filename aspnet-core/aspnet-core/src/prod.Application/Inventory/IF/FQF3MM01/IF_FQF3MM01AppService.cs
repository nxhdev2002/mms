using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.Exporting;
using prod.Inventory.IF.FQF3MM01;
using prod.Inventory.IF.FQF3MM01.Dto;
using prod.Inventory.IF.FQF3MM01.Exporting;
using prod.Inventory.IF.FQF3MM02.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.IF.FQF3MM01
{
    [AbpAuthorize(AppPermissions.Pages_Interface_IF_FQF3MM01_View)]

    public class IF_FQF3MM01AppService : prodAppServiceBase, IIF_FQF3MM01AppService
    {
        private readonly IDapperRepository<IF_FQF3MM01, long> _dapperRepo;
        private readonly IRepository<IF_FQF3MM01, long> _repo;
        private readonly IIF_FQF3MM01ExcelExporter _ifFQF3MM0ExcelExporter;

        public IF_FQF3MM01AppService(IRepository<IF_FQF3MM01, long> repo,
                                         IDapperRepository<IF_FQF3MM01, long> dapperRepo,
                                         IIF_FQF3MM01ExcelExporter ifFQF3MM0ExcelExporter)
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _ifFQF3MM0ExcelExporter = ifFQF3MM0ExcelExporter;
        }

        public async Task<PagedResultDto<IF_FQF3MM01Dto>> GetAll(GetIF_FQF3MM01Input input)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM01_SEARCH @p_lotcode, @p_vin, @p_lineoffdatetime_from, @p_lineoffdatetime_to,  @p_smscarfamilycode";

            IEnumerable<IF_FQF3MM01Dto> result = await _dapperRepo.QueryAsync<IF_FQF3MM01Dto>(_sql, new
            {
                p_lotcode = input.LotCode,
                p_vin = input.Vin,
                p_lineoffdatetime_from = input.LineOffDatetimeFrom,
                p_lineoffdatetime_to = input.LineOffDatetimeTo,
                p_smscarfamilycode = input.SmsCarFamilyCode
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<IF_FQF3MM01Dto>(
                totalCount,
                 pagedAndFiltered
            );
        }

        public async Task<FileDto> GetIF_FQF3MM01ToExcel(GetIF_FQF3MM01Input input)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM01_SEARCH @p_lotcode, @p_vin, @p_lineoffdatetime_from, @p_lineoffdatetime_to, @p_smscarfamilycode";

            IEnumerable<IF_FQF3MM01Dto> result = await _dapperRepo.QueryAsync<IF_FQF3MM01Dto>(_sql, new
            {
                p_lotcode = input.LotCode,
                p_vin = input.Vin,
                p_lineoffdatetime_from = input.LineOffDatetimeFrom,
                p_lineoffdatetime_to = input.LineOffDatetimeTo,
                p_smscarfamilycode = input.SmsCarFamilyCode

            });

            var exportToExcel = result.ToList();
            return _ifFQF3MM0ExcelExporter.ExportToFile(exportToExcel);
        }
        public async Task ReCreateDataFQF3MM01(DateTime date)
        {
            string _sql = "Exec JOB_INV_IF_FQF3MM01_RE_CREATE @p_date";

            await _dapperRepo.QueryAsync<IF_FQF3MM01Dto>(_sql, new
            {
                p_date = date
            });
        }


        public async Task<PagedResultDto<GetIF_FQF3MM01_VALIDATE>> GetInvInterfaceFQF3MM01Validate(GetIF_FQF3MM01_VALIDATEInput input)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM01_VALIDATE  @p_lineoffdatetime_from, @p_lineoffdatetime_to";

            IEnumerable<GetIF_FQF3MM01_VALIDATE> result = await _dapperRepo.QueryAsync<GetIF_FQF3MM01_VALIDATE>(_sql, new
            {
                p_lineoffdatetime_from = input.LineOffDatetimeFrom,
                p_lineoffdatetime_to = input.LineOffDatetimeTo
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<GetIF_FQF3MM01_VALIDATE>(
                totalCount,
                 pagedAndFiltered
            );
        }



        public async Task<FileDto> GetValidate_FQF3MM01ToExcel(DateTime? LineOffDatetimeFrom, DateTime? LineOffDatetimeTo)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM01_VALIDATE @p_lineoffdatetime_from, @p_lineoffdatetime_to";

            IEnumerable<GetIF_FQF3MM01_VALIDATE> result = await _dapperRepo.QueryAsync<GetIF_FQF3MM01_VALIDATE>(_sql, new
            {
                p_lineoffdatetime_from = LineOffDatetimeFrom,
                p_lineoffdatetime_to = LineOffDatetimeTo

            });

            var exportToExcel = result.ToList();
            return _ifFQF3MM0ExcelExporter.ExportValidateToFile(exportToExcel);
        }

        //validate Result
        public async Task<PagedResultDto<GetIF_FQF3MM01_VALIDATE>> GetInvInterfaceFQF3MM01ValidateResult(GetIF_FQF3MM01_VALIDATEInput input)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM01_VALIDATE_RESULT  @p_lineoffdatetime_from, @p_lineoffdatetime_to";

            IEnumerable<GetIF_FQF3MM01_VALIDATE> result = await _dapperRepo.QueryAsync<GetIF_FQF3MM01_VALIDATE>(_sql, new
            {
                p_lineoffdatetime_from = input.LineOffDatetimeFrom,
                p_lineoffdatetime_to = input.LineOffDatetimeTo
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<GetIF_FQF3MM01_VALIDATE>(
                totalCount,
                 pagedAndFiltered
            );
        }

        public async Task<FileDto> GetValidateResult_FQF3MM01ToExcel(DateTime? LineOffDatetimeFrom, DateTime? LineOffDatetimeTo)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM01_VALIDATE_RESULT @p_lineoffdatetime_from, @p_lineoffdatetime_to";

            IEnumerable<GetIF_FQF3MM01_VALIDATE> result = await _dapperRepo.QueryAsync<GetIF_FQF3MM01_VALIDATE>(_sql, new
            {
                p_lineoffdatetime_from = LineOffDatetimeFrom,
                p_lineoffdatetime_to = LineOffDatetimeTo

            });

            var exportToExcel = result.ToList();
            return _ifFQF3MM0ExcelExporter.ExportValidateToFile(exportToExcel);
        }

    }
}
