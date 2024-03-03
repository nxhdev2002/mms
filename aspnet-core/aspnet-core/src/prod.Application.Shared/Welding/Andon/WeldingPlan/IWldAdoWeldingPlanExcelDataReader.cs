using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using prod.Welding.Andon.ImportDto;

namespace prod.Welding.Andon
{
    public interface IWldAdoWeldingPlanExcelDataReader : ITransientDependency
    {
        Task<List<ImportWldAdoWeldingPlanDto>> GetWldAdoWeldingPlanFromExcel(byte[] fileBytes, string fileName);

    }
}

