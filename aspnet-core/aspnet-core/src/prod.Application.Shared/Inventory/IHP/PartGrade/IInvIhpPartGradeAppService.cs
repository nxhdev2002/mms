using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.IHP.PartGrade.Dto;
using System.Threading.Tasks;

namespace prod.Inventory.IHP
{
    public interface IInvIhpPartGradeAppService : IApplicationService
    {
        Task<PagedResultDto<InvIhpPartGradeDto>> GetAll(GetInvIhpPartGradeInput input);
    }
}
