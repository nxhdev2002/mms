using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.CPS.SapAssetMaster.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.CPS.SapAssetMaster.Exporting
{
    public interface IInvCpsSapAssetMasterExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<InvCpsSapAssetMasterDto> InvCpsSapAssetMaster);
    }
}
