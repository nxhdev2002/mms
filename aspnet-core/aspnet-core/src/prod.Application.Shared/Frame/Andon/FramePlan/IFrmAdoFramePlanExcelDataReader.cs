using Abp.Dependency;
using System.Collections.Generic;
using System.Threading.Tasks;
using prod.Frame.Andon.Dto;

namespace prod.Frame.Andon.FramePlanA1
{
    public interface IFrmAdoFramePlanExcelDataReader : ITransientDependency
    {
        Task<List<ImportFrmAdoFramePlanDto>> GetFramePlanFromExcel(byte[] fileBytes, string fileName);
    }
}
