using Abp.Application.Services.Dto;
using Abp.Application.Services;
using System.Threading.Tasks;
using prod.Inventory.GPS.Dto;
using System.Collections.Generic;

namespace prod.Inventory.GPS
{

    public interface IInvGpsIssuingAppService : IApplicationService
    {

        Task<PagedResultDto<InvGpsIssuingDto>> GetAll(GetInvGpsIssuingInput input);


       Task CreateOrEdit(CreateOrEditInvGpsIssuingDto input);

        Task Delete(EntityDto input);

        Task<List<GetInvGesIssuingImport>> ImportDataInvGpsIssuingListFromExcel(byte[] fileBytes, string fileName);

        //Task<List<GetInvGesIssuingImport>> ImportDataInvGpsIssuingGentaniListFromExcel(byte[] fileBytes, string fileName);
    }

}

