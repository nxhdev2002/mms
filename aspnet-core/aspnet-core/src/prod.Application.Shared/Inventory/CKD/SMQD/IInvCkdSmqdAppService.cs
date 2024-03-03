using Abp.Application.Services.Dto;
using Abp.Application.Services;
using prod.Inventory.CKD.SMQD.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.CKD.SMQD
{
    public interface IInvCkdSmqdAppService : IApplicationService
    {

        Task<PagedResultDto<InvCkdSmqdDto>> GetAll(GetInvCkdSmqdInput input);

        Task<List<InvCkdSmqdImportDto>> ImportInvCkdSmqdFromExcel(byte[] fileBytes, string fileName);

/*        Task<List<InvPxpReturnImportDto>> ImportPXPReturnFromExcel(byte[] fileBytes, string fileName);
        Task<List<InvPxpReturnImportDto>> ImportPXPINFromExcel(byte[] fileBytes, string fileName);*/

        //New 
        Task<List<InvPxpOutImportDto>> ImportPXPOtherOutFromExcel(byte[] fileBytes, string fileName);
        Task<List<InvPxpReturnImportDto>> ImportPXPInExcel(byte[] fileBytes, string fileName);
        Task<List<InvPxpReturnImportDto>> ImportPXPReturnExcel(byte[] fileBytes, string fileName);
    }


}
