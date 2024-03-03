using Abp.Application.Services;
using System.Collections.Generic;
using prod.Dto;
using prod.Inventory.CKD.Dto;

namespace prod.Inventory.CKD.Exporting
{

    public interface IInvCkdPartListExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<InvCkdPartListDto> invckdpartlist);

        FileDto ExportToFileErr(List<ImportCkdPartListDto> invckdpartlist_err);

        FileDto ExportToFileLotErr(List<ImportCkdPartListDto> invckdpartlistlot_err);
        FileDto ExportToHistoricalFile(List<string> data);
        FileDto ExportValidateToFile(List<ValidatePartListDto> invckdpartlistvalidate);

        FileDto ExportToFileErrGrade(List<ImportInvCkdPartGradeDto> listerror);

    }

}


