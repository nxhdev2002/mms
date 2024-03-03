using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Dto;
using prod.Master.Inventory.Forwarder.Dto;

namespace prod.Master.Inventory.Forwarder
{

    public interface IMstInvForwarderExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MstInvForwarderDto> mstinvforwarder);

    }

}


