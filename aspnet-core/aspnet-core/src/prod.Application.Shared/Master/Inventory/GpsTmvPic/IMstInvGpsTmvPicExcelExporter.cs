using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using prod.Dto;
using prod.Master.Inv.Dto;

namespace prod.Master.Inv.Exporting
{

    public interface IMstInvGpsTmvPicExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MstInvGpsTmvPicDto> mstinvgpstmvpic);

    }

}