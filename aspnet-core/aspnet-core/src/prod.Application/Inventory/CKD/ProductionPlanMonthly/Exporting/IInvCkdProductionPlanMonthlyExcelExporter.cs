using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.ProductionPlanMonthly.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.CKD.ProductionPlanMonthly.Exporting
{
    public interface IInvCkdProductionPlanMonthlyExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<InvCkdProductionPlanMonthlyDto> invckdproductionplanmonthly);

        FileDto ExportToFileErr(List<InvCkdProductionPlanMonthlyImportDto> listimporterr);

       

    }
}
