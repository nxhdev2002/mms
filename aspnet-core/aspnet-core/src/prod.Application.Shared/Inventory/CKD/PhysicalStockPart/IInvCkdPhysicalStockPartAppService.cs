using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Inventory.CKD.Dto;


namespace prod.Inventory.CKD
{

    public interface IInvCkdPhysicalStockPartAppService : IApplicationService
    {
        Task<PagedResultDto<InvCkdPhysicalStockPartDto>> GetAll(GetInvCkdPhysicalStockPartInput input);

        Task<string> CreateOrEdit(CreateOrEditInvCkdPhysicalStockPartDto input);

        Task<List<InvCkdPhysicalStockPartDto_T>> ImportDataInvCkdPhysicalStockPartFromExcel(byte[] fileBytes, string fileName);

        Task<List<InvCkdPhysicalStockLotDto_T>> ImportDataInvCkdPhysicalStockLotFromExcel(byte[] fileBytes, string fileName);

        Task<PagedResultDto<InvCkdPhysicalStockErrDto>> GetMessageErrorImport(string v_Guid, string v_Screen);
    }

}
