﻿using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Common.Dto;

namespace prod.Master.Common.Exporting
{

    public interface IMstCmmBrandExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MstCmmBrandDto> mstcmmbrand);

    }

}

