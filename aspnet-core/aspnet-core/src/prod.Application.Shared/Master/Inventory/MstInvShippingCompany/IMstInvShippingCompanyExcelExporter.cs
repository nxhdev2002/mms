﻿using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.Inventory.Dto;

namespace prod.Master.Inventory.Exporting
{

    public interface IMstInvShippingCompanyExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MstInvShippingCompanyDto> mstinvshippingcompany);

    }

}

