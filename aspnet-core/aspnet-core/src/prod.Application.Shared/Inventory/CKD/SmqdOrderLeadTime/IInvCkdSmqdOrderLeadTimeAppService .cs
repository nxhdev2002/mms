using Abp.Application.Services.Dto;
using Abp.Application.Services;
using prod.Inventory.CKD.SmqdOrderLeadTime.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using prod.Inventory.CKD.Dto;

namespace prod.Inventory.CKD.SmqdOrderLeadTime
{
    public interface IInvCkdSmqdOrderLeadTimeAppService : IApplicationService
    {
        Task<PagedResultDto<InvCkdSmqdOrderLeadTimeDto>> GetAll(GetInvCkdSmqdOrderLeadTimeInput input);

        Task<List<InvCkdSmqdOrderLeadImportDto>> ImportSmqdOrderLeadTimeFromExcel(byte[] fileBytes, string fileName);


        Task MergeDataInvCkdSmqdOrderLeadTime(string v_Guid);

        Task<PagedResultDto<InvCkdSmqdOrderLeadImportDto>> GetMessageErrorImportOrderLeadTime(string v_Guid);


    }

}
