using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Inventory.GPS.Dto;
using prod.Inventory.GPS.Dto;

namespace prod.Inventory.GPS
{

    public interface IInvGpsStockConceptAppService : IApplicationService
    {

        Task<PagedResultDto<InvGpsStockConceptDto>> GetAll(GetInvGpsStockConceptInput input);

        Task CreateOrEdit(CreateOrEditInvGpsStockConceptDto input);

        Task Delete(EntityDto input);

        Task<List<InvGpsStockConceptDto>> ImportData_InvGpsStockConcept_FromExcel(byte[] fileBytes, string fileName);

    }

}


