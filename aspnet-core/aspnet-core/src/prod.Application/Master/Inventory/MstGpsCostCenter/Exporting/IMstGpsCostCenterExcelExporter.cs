using prod.Dto;
using prod.Master.Inventory.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Inventory.Exporting
{
    public interface IMstGpsCostCenterExcelExporter
    {
        FileDto ExportToFile(List<MstGpsCostCenterDto> mstGpsCostCenters);
    }
}
