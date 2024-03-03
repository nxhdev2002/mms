using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.IHP.Dto;
using prod.Inventory.IHP.PartGrade.Dto;
using System.Threading.Tasks;

namespace prod.Inventory.IHP
{
    public interface IInvIhpPartListAppService : IApplicationService
    {
        Task<PagedResultDto<InvIhpPartListDto>> GetDataPartList(GetInvIhpPartListInput input);

        Task<PagedResultDto<InvIhpPartGradeDto>> GetDataPartGradebyId(GetInvIhpPartGradeInput input);
    }
}
