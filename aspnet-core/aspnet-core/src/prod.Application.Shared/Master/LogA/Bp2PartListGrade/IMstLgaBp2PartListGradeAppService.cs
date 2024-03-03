using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.LogA.Bp2PartListGrade.Dto;
using prod.Master.LogA.Dto;
using prod.Master.LogA.ImportDto;

namespace prod.Master.LogA.Bp2PartListGrade
{
    public interface IMstLgaBp2PartListGradeAppService : IApplicationService
    {

        Task<PagedResultDto<MstLgaBp2PartListGradeDto>> GetAll(GetMstLgaBp2PartListGradeInput input);

        Task CreateOrEdit(CreateOrEditMstLgaBp2PartListGradeDto input);

        Task Delete(EntityDto input);

        Task<List<ImportMstLgaBp2PartListGradeDto>> ImportMstLgaBp2PartListGradeFromExcel(List<ImportMstLgaBp2PartListGradeDto> LotUpPlans);

        Task<FileDto> ExportPartListGrade(string model);
    }
}
