using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using System.Collections.Generic;


namespace prod.Inventory.CKD.SmqdOrder.Exporting
{
    public interface IInvCkdSmqdOrdergExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<InvCkdSmqdOrderDto> invckdmodulecase);

        FileDto ExportToFileErr(List<InvCkdSmqdOrderImportDto> invckdpartrobbing);
    }
}
