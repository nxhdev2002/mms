using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.PIO.PartList.Dto;
using prod.Inventory.PIO.PartListOff.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Inventory.PIO.PartList
{

    public interface IInvPioPartListExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<InvPioPartListDto> invpiopartlist);
        FileDto ExportToFileErr(List<InvPioPartListImportDto> invckdpartlist_err);
    }
}
