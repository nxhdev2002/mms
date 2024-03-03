using Abp.Application.Services;
using prod.Dto;
using prod.Master.Inventory.Dto;
using System.Collections.Generic;

namespace prod.Master.Inventory.Exporting
{

    public interface IMstInvCkdRentalWarehouseExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MstInvCkdRentalWarehouseDto> mstInvCkdRentalWarehouses);

    }

}

