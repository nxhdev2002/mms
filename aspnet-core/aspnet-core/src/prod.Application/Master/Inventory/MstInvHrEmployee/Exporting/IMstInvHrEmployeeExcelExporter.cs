using Abp.Application.Services;
using prod.Dto;
using prod.Master.Inventory.MstInvHrEmployee.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Master.Inventory.MstInvHrEmployee
{
    public interface IMstInvHrEmployeeExcelExporter : IApplicationService
    {
        FileDto ExportEmployeeToFile(List<MstInvHrEmployeeDto> mstInvHrEmployee);
    }
}
