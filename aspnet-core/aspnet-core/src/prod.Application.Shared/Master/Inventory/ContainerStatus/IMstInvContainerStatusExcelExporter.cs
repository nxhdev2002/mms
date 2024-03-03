using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Dto;
using prod.Master.Inventory.ContainerStatus.Dto;

namespace prod.Master.Inventory.Exporting
{

    public interface IMstInvContainerStatusExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MstInvContainerStatusDto> mstinvcontainerstatus);

    }

}


