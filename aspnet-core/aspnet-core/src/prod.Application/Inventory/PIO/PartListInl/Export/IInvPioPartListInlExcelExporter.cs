using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.PIO.PartListInl.Dto;
using System.Collections.Generic;

namespace prod.Inventory.PIO.PartListInl.Export
{
    public interface IInvPioPartListInlExcelExporter : IApplicationService
    {
        FileDto ExportValidateToFile(List<ValidatePartListDto> invckdpartlistvalidate);

        FileDto ExportToFileErr(List<ImportPioPartListDto> invckdpartlist_err);

        FileDto ExportToFileLotErr(List<ImportPioPartListDto> invckdpartlistlot_err);

        FileDto ExportToHistoricalFile(List<string> data, string tableName);

    }
}
