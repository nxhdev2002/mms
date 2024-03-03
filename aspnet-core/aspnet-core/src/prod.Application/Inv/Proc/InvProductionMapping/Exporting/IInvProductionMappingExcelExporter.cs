using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prod.Dto;
using prod.Inv.Proc.Dto;

namespace prod.Inv.Proc.Exporting
{
    public interface IInvProductionMappingExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<InvProductionMappingDto> invimportprocductionmapping);
        FileDto ExportToHistoricalFile(List<string> data);

    }
}
