using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Master.Cmm.Dto;
using prod.Master.Common.GradeColor.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace prod.Master.Common.GradeColor
{
    public interface IMstCommGradeColorAppservice : IApplicationService
    {
        Task<PagedResultDto<MstCmmLotCodeGradeTDto>> GetAllGradeColor(MstCmmLotCodeGradeInput input);
        Task<PagedResultDto<MstCmmGradeColorDetailDto>> GetAllGradeColorDetail(MstCmmGradeColorDetailInput input);
        Task<List<MstColorDto>> GetColorDetailColorList(int gradeId);
        Task CreateOrEditGradeColor(string ListColorId, int GradeId);

    }
}
