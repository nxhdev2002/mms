﻿using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.GPS.Dto;
using System.Collections.Generic;

namespace prod.Inventory.GPS.Exporting
{

    public interface IInvGpsContentListExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<InvGpsContentListDto> invgpscontentlist);

    }

}
