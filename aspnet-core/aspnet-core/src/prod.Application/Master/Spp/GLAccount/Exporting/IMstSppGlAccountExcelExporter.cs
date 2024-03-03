using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Spp.Dto;
using prod.Dto;

namespace prod.Master.Spp.Exporting
{

    public interface IMstSppGlAccountExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MstSppGlAccountDto> mstsppglaccount);

    }

}


