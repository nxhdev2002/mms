using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using NPOI.SS.Formula.Functions;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.Gps.Mapping.Exporting;
using prod.Inventory.Gps.User.Dto;
using prod.Inventory.Gps.User.Exporting;
using prod.Inventory.GPS;
using prod.Inventory.GPS.Dto;
using prod.Inventory.GPS.Exporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.Gps.User
{
    [AbpAuthorize(AppPermissions.Pages_Gps_User)]
    public class InvGpsUserAppService: prodAppServiceBase, IInvGpsUserAppService
    {
        private readonly IDapperRepository<InvGpsUser, long> _dapperRepo;
        private readonly IInvGpsUserExcelExporter _excelExporter;



        public InvGpsUserAppService(IRepository<InvGpsUser, long> repo,
                                         IDapperRepository<InvGpsUser, long> dapperRepo,
                                         IInvGpsUserExcelExporter excelExporter
            )
        {
            _dapperRepo = dapperRepo;
            _excelExporter = excelExporter;
    }

        public async Task<PagedResultDto<InvGpsUserDto>> GetAll(GetInvGpsUserInput input)
        {
            string _sql = "Exec INV_GPS_USER_SEARCH @p_EmployeeCode, @p_Name";

            IEnumerable<InvGpsUserDto> result = await _dapperRepo.QueryAsync<InvGpsUserDto>(_sql, new
            {
                p_EmployeeCode = input.EmployeeCode,
                p_Name = input.Name

            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvGpsUserDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetExportToFile(GetInvGpsUserExportInput input)
        {
            string _sql = "Exec INV_GPS_USER_SEARCH @p_EmployeeCode, @p_Name";

            IEnumerable<InvGpsUserDto> result = await _dapperRepo.QueryAsync<InvGpsUserDto>(_sql, new
            {
                p_EmployeeCode = input.EmployeeCode,
                p_Name = input.Name

            });

            var exportToExcel = result.ToList();
            return _excelExporter.ExportToFile(exportToExcel);
        }
    }
}




