using Abp.Application.Services.Dto;
using Abp.Application.Services;
using prod.Inventory.Gps.StockReceivingTransDetails.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using prod.Inventory.Gps.User.Dto;

namespace prod.Inventory.Gps.User
{
    public interface IInvGpsUserAppService : IApplicationService
    {
        Task<PagedResultDto<InvGpsUserDto>> GetAll(GetInvGpsUserInput input);

    }
}



