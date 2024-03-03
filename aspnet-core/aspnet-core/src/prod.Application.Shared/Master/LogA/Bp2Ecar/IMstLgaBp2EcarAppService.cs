using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using prod.Dto;
using prod.Master.LogA.Dto;

namespace prod.Master.LogA
{
    public interface IMstLgaBp2EcarAppService : IApplicationService
    {
        Task<PagedResultDto<MstLgaBp2EcarDto>> GetAll(GetMstLgaBp2EcarInput input);

        Task CreateOrEdit(CreateOrEditMstLgaBp2EcarDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetBp2EcarToExcel(GetMstLgaBp2EcarExcelInput input);

    }
}
