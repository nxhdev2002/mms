﻿using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.CKD.PhysicalConfirmLot.ExportingErr
{
    public interface IInvCkdPhysicalConfirmLotExcelExporter : IApplicationService
    {
        FileDto ExportToFileErr(List<InvCkdPhysicalConfirmLot_TDto> listimporterr);

    }
}