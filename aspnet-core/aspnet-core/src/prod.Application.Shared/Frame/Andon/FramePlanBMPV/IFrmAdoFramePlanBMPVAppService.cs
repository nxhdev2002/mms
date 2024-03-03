using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Frame.Andon.Dto;

namespace prod.Frame.Andon
{

    public interface IFrmAdoFramePlanBMPVAppService : IApplicationService
    {

        Task<PagedResultDto<FrmAdoFramePlanBMPVDto>> GetAll(GetFrmAdoFramePlanBMPVInput input);

        Task Delete(EntityDto input);

        Task<List<ImportFrmAdoFramePlanBMPVDto>> ImportFramePlanBMPV(List<ImportFrmAdoFramePlanBMPVDto> framePlanBMPVs);

        Task MergeDataFramePlanBMPV(string v_Guid);

        Task<PagedResultDto<FrmAdoFramePlanBMPVDto>> GetMessageErrorImport(string v_Guid);

        Task<FileDto> GetFramePlanBMPVToExcel(GetFrmAdoFramePlanBMPVExportInput input);
    }

}

