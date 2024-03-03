using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Frame.Andon.Dto;

namespace prod.Frame.Andon.Exporting
{

    public interface IFrmAdoFramePlanBMPVExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<FrmAdoFramePlanBMPVDto> frmadoframeplanbmpv);
        FileDto ExportToFileErr(List<FrmAdoFramePlanBMPVDto> frameplanBMPV_Err);
    }

}


