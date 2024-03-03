using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.SmqdOrderLeadTime.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.CKD.SmqdOrderLeadTime.Exporting
{
    public interface IInvCkdSmqdOrderLeadTimeExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<InvCkdSmqdOrderLeadTimeDto> invckdsmqdorderleadtime);

        FileDto ExportToFileErrOrderLeadTime(List<InvCkdSmqdOrderLeadImportDto> invckdsmqdorderleadtimeerr);

    }
}
