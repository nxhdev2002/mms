using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using System.Collections.Generic;

namespace prod.Inventory.CKD
{
    public interface IInvCkdShipmentExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<InvCkdShipmentDto> invckdshipment);

    }
}
