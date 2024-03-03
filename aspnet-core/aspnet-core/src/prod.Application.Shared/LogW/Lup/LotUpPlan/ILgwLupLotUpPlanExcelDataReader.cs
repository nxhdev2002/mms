using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using prod.LogW.Lup.Dto;

namespace prod.LogW.Lup.LotUpPlan
{

    public interface ILgwLupLotUpPlanExcelDataReader : ITransientDependency
    {
        Task<List<ImportLotUpPlanDto>> GetLotUpPlanFromExcel(byte[] fileBytes, string fileName);
    }
}
