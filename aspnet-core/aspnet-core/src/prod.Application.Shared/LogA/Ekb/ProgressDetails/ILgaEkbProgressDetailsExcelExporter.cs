using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.LogA.Ekb.Dto;

namespace prod.LogA.Ekb.Exporting
{
    public interface ILgaEkbProgressDetailsExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<LgaEkbProgressDetailsDto> lgaekbprogressdetails);
    }
}