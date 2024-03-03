using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.DRM.Dto;
using System.Collections.Generic;

namespace prod.Inventory.DRM.Exporting
{
    public interface IInvDrmPartListExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<InvDrmPartListDto> invdrmpartlist);

        FileDto ExportToFileErr(List<InvDrmIhpPartImportDto> list_drm_import_err,
                                List<InvDrmIhpPartImportDto> list_ihp_import_err);
        FileDto ExportToHistoricalFile(List<string> data);

    }
}
