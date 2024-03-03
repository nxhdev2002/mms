using prod.Dto;
using prod.Inventory.Gps.PartListByCategory.Dto;
using prod.Master.Inventory.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.Gps.PartListByCategory.Exporting
{
    public interface IInvGpsPartListByCategoryExcelExporter
    {
        FileDto ExportToFile(List<InvGpsPartListByCategoryDto> invGpsMaster);
    }
}
