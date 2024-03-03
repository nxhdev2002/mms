using Abp.Application.Services;
using prod.Inventory.PIO.PartListOff.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.PIO.PartListOff
{
    public interface IInvPartListOffAppService : IApplicationService
    {
        Task<List<ImportPioPartListOffDto>> ImportDataInvPioPartListOffFromExcel(byte[] fileBytes, string fileName);
    }
}
