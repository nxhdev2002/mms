using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.LogA.Bp2.PxPUpPlan.Dto;

namespace prod.LogA.Bp2.PxPUpPlan.Exporting
{
    public interface ILgaBp2PxPUpPlanExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<LgaBp2PxPUpPlanDto> lgabp2pxpupplan);
    }
}
