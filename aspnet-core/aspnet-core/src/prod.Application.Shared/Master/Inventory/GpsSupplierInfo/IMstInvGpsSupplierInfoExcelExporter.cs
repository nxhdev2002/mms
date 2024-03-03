using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using prod.Master.Inv.Dto;

namespace prod.Master.Inventory
{

    public interface IMstInvGpsSupplierInfoExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MstInvGpsSupplierInfoDto> mstinvgpssupplierinfo);

    }

}