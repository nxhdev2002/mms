using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using prod.Master.LogA.Dto;

namespace prod.Master.LogA.Exporting
{

    public interface IMstLgaEkbProcessExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MstLgaEkbProcessDto> mstlgaekbprocess);

    }

}


