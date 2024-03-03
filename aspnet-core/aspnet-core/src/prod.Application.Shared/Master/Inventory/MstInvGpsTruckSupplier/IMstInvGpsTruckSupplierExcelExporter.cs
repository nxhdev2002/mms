using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;

using System.Threading.Tasks;

using prod.Dto;
using prod.Master.Inv.Dto;

namespace prod.Master.Inv.Exporting
{

    public interface IMstInvGpsTruckSupplierExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MstInvGpsTruckSupplierDto> mstinvgpstrucksupplier);

    }

}


