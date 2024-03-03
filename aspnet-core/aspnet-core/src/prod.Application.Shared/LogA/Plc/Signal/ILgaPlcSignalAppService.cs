using Abp.Application.Services.Dto;
using Abp.Application.Services;
using prod.LogW.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using prod.LogA.Plc.Signal.Dto;

namespace prod.LogA.Plc.Signal
{
    public interface ILgaPlcSignalAppService : IApplicationService
    {

        Task<PagedResultDto<LgaPlcSignalDto>> GetAll(GetLgaPlcSignalInput input);

    }
}
