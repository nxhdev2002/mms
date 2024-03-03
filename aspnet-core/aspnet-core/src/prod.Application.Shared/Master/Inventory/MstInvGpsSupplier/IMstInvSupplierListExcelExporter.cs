using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;

using prod.Dto;
using prod.Master.Inventory.Dto;

namespace prod.Master.Inv.Exporting
{

    public interface IMstInvSupplierListExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MstInvSupplierListDto> mstinvgpssupplier);

    }

}
