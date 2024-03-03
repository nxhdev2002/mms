using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using prod.Master.Inv.Exporting;
using prod.Master.Inv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prod.Authorization.Users;
using Abp.Application.Services.Dto;
using prod.Inventory.CPS.Dto;
using prod.Master.Inventory.MstInvHrEmployee.Dto;
using prod.Dto;
using prod.Inventory.CPS;
using Abp.Authorization;
using prod.Authorization;

namespace prod.Master.Inventory.MstInvHrEmployee
{
    [AbpAuthorize(AppPermissions.Pages_Master_Hr_HrEmployee_View)]
    public class MstInvHrEmployeeAppService : IMstInvHrEmployeeAppService
    {
        private readonly IDapperRepository<User, long> _userRepository;
        private readonly IMstInvHrEmployeeExcelExporter _calendarListExcelExporter;

        public MstInvHrEmployeeAppService(IDapperRepository<User, long> userRepository,
                                        IMstInvHrEmployeeExcelExporter calendarListExcelExporter
            )
        {
            _userRepository = userRepository;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<MstInvHrEmployeeDto>> GetAll(MstInvHrEmployeeInput input)
        {
            string _sql = "Exec MST_INV_HR_EMPLOYEE @p_employee_code, @p_name, @p_email_address";

            IEnumerable<MstInvHrEmployeeDto> result = await _userRepository.QueryAsync<MstInvHrEmployeeDto>(_sql,
                  new
                  {
                      p_employee_code = input.p_employee_code,
                      p_name = input.p_name,
                      p_email_address = input.p_email_address

                  });
            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<MstInvHrEmployeeDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetEmployeeToExcel(MstInvHrEmployeeInputExcel input)
        {

            string _sql = "Exec MST_INV_HR_EMPLOYEE @p_employee_code, @p_name, @p_email_address";

            IEnumerable<MstInvHrEmployeeDto> result = await _userRepository.QueryAsync<MstInvHrEmployeeDto>(_sql,
                  new
                  {
                      p_employee_code = input.p_employee_code,
                      p_name = input.p_name,
                      p_email_address = input.p_email_address

                  });
            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportEmployeeToFile(exportToExcel);
        }

    }
}
