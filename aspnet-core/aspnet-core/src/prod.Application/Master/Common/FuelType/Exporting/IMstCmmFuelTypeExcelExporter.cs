using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using prod.Dto;
using prod.Master.Common.Dto;

namespace prod.Master.Common.Exporting
{

    public interface IMstCmmFuelTypeExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MstCmmFuelTypeDto> mstcmmfueltype);

    }

}
