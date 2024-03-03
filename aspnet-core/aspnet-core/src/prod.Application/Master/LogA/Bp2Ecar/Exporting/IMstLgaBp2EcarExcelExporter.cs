using Abp.Application.Services;
using prod.Dto;
using prod.Master.LogA.Dto;
using System;
using System.Collections.Generic;

namespace prod.Master.LogA.Exporting
{
    public interface IMstLgaBp2EcarExcelExporter : IApplicationService
    {
       FileDto ExportToFile(List<MstLgaBp2EcarDto> mstlgabp2ecars);
    }
}
