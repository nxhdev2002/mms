using Abp.Application.Services;
using prod.Dto;
using prod.Master.Common.DriveTrain.Dto;
using prod.Master.Inventory.GpsCategory.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Inventory.GpsCategory.Exporting
{
    public interface IMstInvGpsCategoryExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<MstInvGpsCategoryDto> mstinvgpscategory);
    }
}
