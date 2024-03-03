using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using prod.LogA.Lds.LotPlan.ImportDto;

namespace prod.LogA.Lds.LotPlan
{
    public interface ILgaLdsLotPlanExcelDataReader : ITransientDependency
    {
        Task<List<ImportLgaLdsLotPlanDto>> GetLgaLdsLotPlanFromExcel(byte[] fileBytes, string fileName);
    }
}
