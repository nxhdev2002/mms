using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Dto;
using prod.Inventory.Gps.FinStock.Dto;
using prod.Inventory.GPS.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.Gps.FinStock
{
    public interface IInvGpsFinStock : IApplicationService
    {
        Task<PagedResultDto<InvGpsFinStockDto>> GetAll(InvGpsFinStockImput input);
        Task<FileDto> GetInvGpsFinStockToExcel(InvGpsFinStockImput input);
        Task<List<InvGpsFinStockImportDto>> ImportDataInvGpsFinStockFromExcel(byte[] fileBytes, string fileName);
    }
}
