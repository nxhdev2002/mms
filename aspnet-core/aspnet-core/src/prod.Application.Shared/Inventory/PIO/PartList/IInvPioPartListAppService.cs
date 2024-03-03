using Abp.Application.Services.Dto;
using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using prod.Inventory.PIO.PartList.Dto;

namespace prod.Inventory.PIO.PartList
{

    public interface IInvPioPartListAppService : IApplicationService
    {
        Task<PagedResultDto<InvPioPartListDto>> GetAll(GetInvPioPartListInput input);

        Task<List<InvPioPartListImportDto>> ImportPartListFromExcel(byte[] fileBytes, string fileName);
    }
}
