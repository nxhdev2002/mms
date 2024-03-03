using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.Gps.PartList.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.Gps.PartList.Exporting
{
    public interface IInvGpsPartListExcelExporter : IApplicationService
    {

        FileDto ExportToFileErr(List<InvGpsPartListImportDto> invgpspartlistcolor_err);

        FileDto ExportToFileNoColorErr(List<InvGpsPartListImportDto> invgpspartlistnocolor_err);

        FileDto ExportValidateToFile(List<ValidateGpsPartListDto> invgpspartlistvalidate);

    }
}
