using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.PartRobbing.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.CKD.PartRobbing.Exporting
{
    public interface IInvCkdPartRobbingExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<InvCkdPartRobbingDto> invckdmodulecase);

        FileDto ExportToFileErr(List<InvCkdPartRobbingImportDto> invckdpartrobbing);
    }
}
