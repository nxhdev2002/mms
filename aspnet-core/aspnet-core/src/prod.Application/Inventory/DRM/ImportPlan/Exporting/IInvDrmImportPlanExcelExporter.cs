using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Inventory.DRM.Dto;
using prod.Dto;
using prod.Inventory.DRM.Dto;

namespace prod.Inventory.DRM.Exporting
{

    public interface IInvDrmImportPlanExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<InvDrmImportPlanDto> invdrmimportplan);

        FileDto ExportToFileErr(List<InvDrmImportPlanImportDto> indrmimportplan_err);


    }

}


