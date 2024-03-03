using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using prod.Plan.Ccr.ProductionPlan.ImportDto;

namespace prod.Plan.Ccr.ProductionPlan
{
    public interface IPlnCcrProductionPlanExcelDataReader : ITransientDependency
    {
        Task<List<ImportPlnCcrProductionPlanDto>> GetPlnCcrProductionPlanFromExcel(byte[] fileBytes, string fileName);

    }
}
