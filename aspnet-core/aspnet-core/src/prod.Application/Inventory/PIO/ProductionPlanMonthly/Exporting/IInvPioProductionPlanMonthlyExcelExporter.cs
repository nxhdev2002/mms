using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.PIO.InvPioProductionPlanMonthly.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.PIO.InvPioProductionPlanMonthly.Exporting
{
    public interface IInvPioProductionPlanMonthlyExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<InvPioProductionPlanMonthlyDto> invckdproductionplanmonthly);

        FileDto ExportToFileErr(List<InvPioProductionPlanMonthlyImportDto> listimporterr);
    }
}
