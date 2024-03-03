using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.IF;
using prod.Inventory.IF.Dto;
using prod.Inventory.IF.Exporting;
using prod.Inventory.IF.FQF3MM01.Dto;
using prod.Inventory.IF.FQF3MM03.Dto;
using prod.Inventory.IF.FQF3MM04.Dto;
using prod.Inventory.IF.FQF3MM05.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.IF
{
    [AbpAuthorize(AppPermissions.Pages_Interface_IF_FQF3MM04_View)]
    public class IF_FQF3MM04AppService : prodAppServiceBase, IIF_FQF3MM04AppService
    {
        private readonly IDapperRepository<IF_FQF3MM04, long> _dapperRepo;
        private readonly IIF_FQF3MM04ExcelExporter _calendarListExcelExporter;

        public IF_FQF3MM04AppService(IDapperRepository<IF_FQF3MM04, long> dapperRepo,
                                         IIF_FQF3MM04ExcelExporter calendarListExcelExporter
            )
        {
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<IF_FQF3MM04Dto>> GetAll(GetIF_FQF3MM04Input input)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM04_SEARCH @p_DevaningDateFrom, @p_DevaningDateTo, @p_InvoiceNo, @p_Renban, @p_ContainerNo, @p_ModuleNo";

            IEnumerable<IF_FQF3MM04Dto> result = await _dapperRepo.QueryAsync<IF_FQF3MM04Dto>(_sql, new
            {
                p_DevaningDateFrom = input.DevaningDateFrom,
                p_DevaningDateTo = input.DevaningDateTo,
                p_InvoiceNo = input.InvoiceNo,
                p_Renban = input.Renban,
                p_ContainerNo = input.ContainerNo,
                p_ModuleNo = input.ModuleNo
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<IF_FQF3MM04Dto>(
                totalCount,
                 pagedAndFiltered
            );
        }

        public async Task<FileDto> GetFQF3MM04ToExcel(IF_FQF3MM04ExportInput input)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM04_SEARCH @p_DevaningDateFrom, @p_DevaningDateTo, @p_InvoiceNo, @p_Renban, @p_ContainerNo, @p_ModuleNo";

            IEnumerable<IF_FQF3MM04Dto> result = await _dapperRepo.QueryAsync<IF_FQF3MM04Dto>(_sql, new
            {
                p_DevaningDateFrom = input.DevaningDateFrom,
                p_DevaningDateTo = input.DevaningDateTo,
                p_InvoiceNo = input.InvoiceNo,
                p_Renban = input.Renban,
                p_ContainerNo = input.ContainerNo,
                p_ModuleNo = input.ModuleNo
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }
        public async Task ReCreateDataFQF3MM04(DateTime date)
        {
            string _sql = "Exec JOB_INV_IF_FQF3MM04_RE_CREATE @p_date";

            await _dapperRepo.QueryAsync<IF_FQF3MM04Dto>(_sql, new
            {
                p_date = date
            });
        }



        public async Task<PagedResultDto<GetIF_FQF3MM04_VALIDATE>> GetInvInterfaceFQF3MM04Validate(GetIF_FQF3MM04_VALIDATEInput input)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM04_VALIDATE @p_DevaningDateFrom, @p_DevaningDateTo";

            IEnumerable<GetIF_FQF3MM04_VALIDATE> result = await _dapperRepo.QueryAsync<GetIF_FQF3MM04_VALIDATE>(_sql, new
            {
                p_DevaningDateFrom = input.DevaningDateFrom,
                p_DevaningDateTo = input.DevaningDateTo

            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<GetIF_FQF3MM04_VALIDATE>(
                totalCount,
                 pagedAndFiltered
            );
        }


        public async Task<FileDto> GetValidate_FQF3MM04ToExcel(DateTime? DevaningDateFrom, DateTime? DevaningDateTo)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM04_VALIDATE @p_DevaningDateFrom, @p_DevaningDateTo";

            IEnumerable<GetIF_FQF3MM04_VALIDATE> result = await _dapperRepo.QueryAsync<GetIF_FQF3MM04_VALIDATE>(_sql, new
            {
                p_DevaningDateFrom = DevaningDateFrom,
                p_DevaningDateTo = DevaningDateTo

            });


            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportValidateToFile(exportToExcel);
        }

        //validate Result
        public async Task<PagedResultDto<GetIF_FQF3MM04_VALIDATE>> GetInvInterfaceFQF3MM04ValidateResult(GetIF_FQF3MM04_VALIDATEInput input)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM04_VALIDATE_RESULT @p_DevaningDateFrom, @p_DevaningDateTo";

            IEnumerable<GetIF_FQF3MM04_VALIDATE> result = await _dapperRepo.QueryAsync<GetIF_FQF3MM04_VALIDATE>(_sql, new
            {
                p_DevaningDateFrom = input.DevaningDateFrom,
                p_DevaningDateTo = input.DevaningDateTo

            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<GetIF_FQF3MM04_VALIDATE>(
                totalCount,
                 pagedAndFiltered
            );
        }


        public async Task<FileDto> GetValidateResult_FQF3MM04ToExcel(DateTime? DevaningDateFrom, DateTime? DevaningDateTo)
        {
            string _sql = "Exec INV_INTERFACE_FQF3MM04_VALIDATE_RESULT @p_DevaningDateFrom, @p_DevaningDateTo";

            IEnumerable<GetIF_FQF3MM04_VALIDATE> result = await _dapperRepo.QueryAsync<GetIF_FQF3MM04_VALIDATE>(_sql, new
            {
                p_DevaningDateFrom = DevaningDateFrom,
                p_DevaningDateTo = DevaningDateTo

            });


            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportValidateToFile(exportToExcel);
        }
    }
}

