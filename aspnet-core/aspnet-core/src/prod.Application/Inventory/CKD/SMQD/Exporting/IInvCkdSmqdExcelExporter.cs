using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.CKD.SMQD.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.CKD.SMQD.Exporting
{
    public interface IInvCkdSmqdExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<InvCkdSmqdDto> invckdsmqd);

        FileDto ExportToFileErr(List<InvCkdSmqdImportDto> listimporterr);

    }

}
