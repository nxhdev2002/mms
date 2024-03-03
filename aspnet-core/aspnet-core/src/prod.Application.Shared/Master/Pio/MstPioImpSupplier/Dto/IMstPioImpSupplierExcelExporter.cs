using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Dto;
using prod.Master.Pio.Dto;

namespace prod.Master.Inventory.Exporting
{

    public interface IMstPioImpSupplierExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MstPioImpSupplierDto> mstinvpioparttype);

    }

}


