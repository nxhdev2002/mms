using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;

using prod.Dto;
using prod.Master.Inv.Dto;

namespace prod.Master.Inv.Exporting
{

    public interface IMstInvGpsSupplierPicExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MstInvGpsSupplierPicDto> mstinvgpssupplierpic);

    }

}


