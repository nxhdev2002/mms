using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using prod.Master.Cmm.Dto;

namespace prod.Master.Common.Tax.Exporting
{

    public interface IMstCmmTaxExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MstCmmTaxDto> mstcmmtax);

    }

}

