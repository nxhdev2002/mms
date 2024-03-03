using Abp.Dependency;
using System.Collections.Generic;
using System.Threading.Tasks;
using prod.Frame.Andon.Dto;
using prod.LogW.Pup.ImportDto;

namespace prod.LogW.Pup
{
    public interface IPxPUpPlanExcelDataReader : ITransientDependency
    {
        Task<List<ImportPxPUpPlanDto>> GetPxPUpPlanFromExcel(byte[] fileBytes, string fileName);
    }
}
