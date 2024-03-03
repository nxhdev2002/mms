using Abp.Application.Services;
using System.Collections.Generic;
using prod.Dto;
using prod.Master.LogA.Dto;

namespace prod.Master.LogA.Exporting
{

    public interface IMstLgaEkbUserExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MstLgaEkbUserDto> mstlgaekbuser);

    }

}


