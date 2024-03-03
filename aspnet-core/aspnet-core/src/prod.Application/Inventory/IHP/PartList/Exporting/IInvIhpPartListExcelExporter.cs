using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.IHP.Dto;
using prod.Inventory.IHP.PartGrade.Dto;
using System.Collections.Generic;

namespace prod.Inventory.IHP.Exporting
{
    public interface IInvIhpPartListExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<InvIhpPartListDto> invihppartlist);

        FileDto ExportPartGradeToFile(List<InvIhpPartGradeDto> invihppartgrade);

        FileDto ExportValidateToFile(List<ValidateIhpPartListDto> invIhppartlistvalidate);

        FileDto ExportToHistoricalFile(List<string> data, string tableName);
    }
}
