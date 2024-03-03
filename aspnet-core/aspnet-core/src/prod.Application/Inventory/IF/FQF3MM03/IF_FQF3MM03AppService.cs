using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.IF.Dto;
using prod.Inventory.IF.Exporting;
using prod.Inventory.IF.FQF3MM02.Dto;
using prod.Inventory.IF.FQF3MM03.Dto;
using prod.Inventory.IF.FQF3MM04.Dto;
using prod.Inventory.IF.FQF3MM07.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prod.Inventory.IF.FQF3MM03
{
    [AbpAuthorize(AppPermissions.Pages_Interface_IF_FQF3MM03_View)]
    public class IF_FQF3MM03AppService : prodAppServiceBase, IIF_FQF3MM03AppService
    {
        private readonly IDapperRepository<IF_FQF3MM03, long> _dapperRepo;
        private readonly IIF_FQF3MM03ExcelExporter _calendarListExcelExporter;

        public IF_FQF3MM03AppService(IDapperRepository<IF_FQF3MM03, long> dapperRepo,
                                      IIF_FQF3MM03ExcelExporter calendarListExcelExporter
            )
        {
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<IF_FQF3MM03Dto>> GetAll(GetIF_FQF3MM03Input input)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM03_SEARCH @p_partno, @p_suppliercode, @p_postingdate_from, @p_postingdate_to, @p_pdsNo, @p_sequence_from, @p_sequence_to";

            IEnumerable<IF_FQF3MM03Dto> result = await _dapperRepo.QueryAsync<IF_FQF3MM03Dto>(_sql, new
            {
                p_partno = input.PartNo,
                p_postingdate_from = input.PostingDateFrom,
                p_postingdate_to = input.PostingDateTo,
                p_suppliercode = input.SupplierCode,
                p_pdsNo = input.PdsNo,
                p_sequence_from = input.SequenceDateFrom,
                p_sequence_to = input.SequenceDateTo
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<IF_FQF3MM03Dto>(
                totalCount,
                 pagedAndFiltered
            );
        }

        public async Task<FileDto> GetFQF3MM03ToExcel(IF_FQF3MM03ExportInput input)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM03_SEARCH @p_partno, @p_suppliercode, @p_postingdate_from, @p_postingdate_to,  @p_pdsNo, @p_sequence_from, @p_sequence_to";

            IEnumerable<IF_FQF3MM03Dto> result = await _dapperRepo.QueryAsync<IF_FQF3MM03Dto>(_sql, new
            {
                p_partno = input.PartNo,
                p_postingdate_from = input.PostingDateFrom,
                p_postingdate_to = input.PostingDateTo,
                p_suppliercode = input.SupplierCode,
                p_pdsNo = input.PdsNo,
                p_sequence_from = input.SequenceDateFrom,
                p_sequence_to = input.SequenceDateTo
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }
        public async Task ReCreateDataFQF3MM03(DateTime date)
        {
            string _sql = "Exec JOB_INV_IF_FQF3MM03_RE_CREATE @p_date";

            await _dapperRepo.QueryAsync<IF_FQF3MM03Dto>(_sql, new
            {
                p_date = date
            });
        }

        public async Task<PagedResultDto<BusinessDataDto>> GetViewBusinessData(BusinessDataInput input)
        {
            string _sql = "Exec INV_DAILY_BUSINESS_DATA @p_PartNo, @p_Supllier, @p_WorkingDateFrom, @p_WorkingDateTo";

            IEnumerable<BusinessDataDto> result = await _dapperRepo.QueryAsync<BusinessDataDto>(_sql, new 
            {
                p_PartNo = input.PartNo,
                p_Supllier = input.SupplierCode,
                p_WorkingDateFrom = input.WorkingDateFrom,
                p_WorkingDateTo = input.WorkingDateTo
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<BusinessDataDto>(
                totalCount,
                 pagedAndFiltered
            );
        }


        public async Task<PagedResultDto<GetIF_FQF3MM03_VALIDATE>> GetInvInterfaceFQF3MM03Validate(GetIF_FQF3MM03_VALIDATEInput input)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM03_VALIDATE @p_postingdate_from, @p_postingdate_to";

            IEnumerable<GetIF_FQF3MM03_VALIDATE> result = await _dapperRepo.QueryAsync<GetIF_FQF3MM03_VALIDATE>(_sql, new
            {
                p_postingdate_from = input.PostingDateFrom,
                p_postingdate_to = input.PostingDateTo
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<GetIF_FQF3MM03_VALIDATE>(
                totalCount,
                 pagedAndFiltered
            );
        }


        public async Task<FileDto> GetValidate_FQF3MM03ToExcel(GetIF_FQF3MM03_VALIDATE_Input input)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM03_VALIDATE @p_postingdate_from, @p_postingdate_to";

            IEnumerable<GetIF_FQF3MM03_VALIDATE> result = await _dapperRepo.QueryAsync<GetIF_FQF3MM03_VALIDATE>(_sql, new
            {
                p_postingdate_from = input.PostingDateFrom,
                p_postingdate_to = input.PostingDateTo
            });


            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportValidateToFile(exportToExcel);
        }

        //validate Result
        public async Task<PagedResultDto<GetIF_FQF3MM03_VALIDATE>> GetInvInterfaceFQF3MM03ValidateResult(GetIF_FQF3MM03_VALIDATEInput input)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM03_VALIDATE_RESULT @p_postingdate_from, @p_postingdate_to";

            IEnumerable<GetIF_FQF3MM03_VALIDATE> result = await _dapperRepo.QueryAsync<GetIF_FQF3MM03_VALIDATE>(_sql, new
            {
                p_postingdate_from = input.PostingDateFrom,
                p_postingdate_to = input.PostingDateTo
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<GetIF_FQF3MM03_VALIDATE>(
                totalCount,
                 pagedAndFiltered
            );
        }


        public async Task<FileDto> GetValidateResult_FQF3MM03ToExcel(GetIF_FQF3MM03_VALIDATE_Input input)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM03_VALIDATE_RESULT @p_postingdate_from, @p_postingdate_to";

            IEnumerable<GetIF_FQF3MM03_VALIDATE> result = await _dapperRepo.QueryAsync<GetIF_FQF3MM03_VALIDATE>(_sql, new
            {
                p_postingdate_from = input.PostingDateFrom,
                p_postingdate_to = input.PostingDateTo
            });


            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportValidateToFile(exportToExcel);
        }
    }
}
