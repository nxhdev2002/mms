using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;

using prod.Inv.Dmr.Dto;

namespace prod.Inv.Dmr
{

    public interface IInvImportByContAppService : IApplicationService
    {

        Task<PagedResultDto<InvImportByContDto>> GetAll(GetInvImportByContInput input);

        Task<FileDto> GetImportByContToExcel(GetInvImportByContExportInput input);

    }

}


