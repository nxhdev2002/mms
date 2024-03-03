using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using prod.LogA.Bp2.Dto;

namespace prod.LogA.Bp2
{
    public interface ILgaBp2BigPartDSMAppServices
    {
        Task<PagedResultDto<LgaBp2BigPartDSMDto>> GetDataBigPartDSM(string prod_line);
    }
}
