using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Dto;
using prod.Master.Inventory.ContainerDeliveryType.Dto;

namespace prod.Master.Inventory.ContainerDeliveryType
{

    public interface IMstInvContainerDeliveryTypeExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MstInvContainerDeliveryTypeDto> mstinvcontainerdeliverytype);

    }

}


