using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.LogW.Mwh.Dto;

namespace prod.LogW.Mwh.ContList.Exporting
{

    public interface ILgwMwhContListExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<LgwMwhContListDto> lgwmwhcontlist);

    }

}


