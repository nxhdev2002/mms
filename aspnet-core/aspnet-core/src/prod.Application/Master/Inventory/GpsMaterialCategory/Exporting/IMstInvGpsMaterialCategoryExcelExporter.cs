using prod.Dto;
using prod.Master.Inventory.GpsCategory.Dto;
using prod.Master.Inventory.GpsMaterialCategory.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Inventory.GpsMaterialCategory.Exporting
{
    public interface IMstInvGpsMaterialCategoryExcelExporter
    {
        FileDto ExportToFile(List<MstInvGpsMaterialCategoryDto> mstinvgpsmaterialcategory);
    }
}
