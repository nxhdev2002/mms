using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.DRM.StockPart.Dto;
using prod.Inventory.IF.FQF3MM06.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.IF.FQF3MM05.Exporting
{
    public interface IIF_FQF3MM06ExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<IF_FQF3MM06Dto> fqf3mm06);
        FileDto ExportValidateToFile(List<GetIF_FQF3MM06_VALIDATE> fqf3mm06);
    }
}
