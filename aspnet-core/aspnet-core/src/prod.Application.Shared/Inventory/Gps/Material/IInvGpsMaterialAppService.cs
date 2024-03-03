using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Inventory.GPS.Dto;
using prod.Inventory.Gps.StockIssuingTransDetails.Dto;

namespace prod.Inventory.GPS
{

    public interface IInvGpsMaterialAppService : IApplicationService
    {

        Task<PagedResultDto<InvGpsMaterialDto>> GetAll(GetInvGpsMaterialInput input);

        Task CreateOrEdit(CreateOrEditInvGpsMaterialDto input);


        Task<List<ImportInvGpsMaterialDto>> ImportInvGpsMaterialExcel(byte[] fileBytes, string fileName);

    }

}


