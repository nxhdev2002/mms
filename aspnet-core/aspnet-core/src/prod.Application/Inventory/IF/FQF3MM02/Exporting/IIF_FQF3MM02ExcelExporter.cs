using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.IF.FQF3MM02.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.IF.FQF3MM02.Exporting
{
    public interface IIF_FQF3MM02ExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<IF_FQF3MM02Dto> if_fqf3mm02);
        FileDto ExportValidateToFile(List<GetIF_FQF3MM02_VALIDATE> fqf3mm02);

    }
}
