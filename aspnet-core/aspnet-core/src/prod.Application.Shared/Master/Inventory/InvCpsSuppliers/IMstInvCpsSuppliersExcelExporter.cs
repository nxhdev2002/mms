using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Inventory.Dto;

namespace prod.Master.Inventory.Exporting
{

    public interface IMstInvCpsSuppliersExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MstInvCpsSuppliersDto> mstinvcpssuppliers);

    }

}


