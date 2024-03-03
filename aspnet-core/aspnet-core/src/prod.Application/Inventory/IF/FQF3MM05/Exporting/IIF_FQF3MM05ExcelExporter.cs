using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.DRM.StockPart.Dto;
using prod.Inventory.IF.FQF3MM05.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.IF.FQF3MM05.Exporting
{
    public interface IIF_FQF3MM05ExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<IF_FQF3MM05Dto> fqf3mm05);
        FileDto ExportValidateToFile(List<GetIF_FQF3MM05_VALIDATE> fqf3mm05);
    }
}
