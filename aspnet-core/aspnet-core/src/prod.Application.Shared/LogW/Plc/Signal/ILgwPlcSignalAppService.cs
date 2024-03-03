using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using prod.Dto;
using prod.LogW.Ado.Dto;
using prod.LogW.Dto;
using prod.LogW.Plc.Dto;

namespace prod.LogW.Plc
{
    public interface ILgwPlcSignalAppService : IApplicationService
    {
        Task<PagedResultDto<LgwPlcSignalDto>> GetAll(GetLgwPlcSignalInput input);
        Task<FileDto> GetSignalToExcel(GetLgwPlcSignalExportInput input);
    }
}
