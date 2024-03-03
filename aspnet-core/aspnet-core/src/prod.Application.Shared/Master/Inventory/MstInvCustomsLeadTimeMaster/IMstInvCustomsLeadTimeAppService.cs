using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.Gps.StockIssuingTransDetails.Dto;
using prod.Master.Inv.Dto;
using prod.Master.Inventory.Dto;
using prod.Master.Inventory.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Inventory
{
    public interface IMstInvCustomsLeadTimeAppService : IApplicationService
    {
        Task<PagedResultDto<MstInvCustomsLeadTimeDto>> GetAll(GetMstInvCustomsLeadTimeInput input);
        Task CreateOrEdit(CreateOrEditMstInvCustomsLeadTimeDto input);
        Task Delete(EntityDto input);
        Task<List<MstInvCustomsLeadTimeImportDto>> ImportCustomsLeadTimeMasterFromExcel(byte[] fileBytes, string fileName);
    }
}
