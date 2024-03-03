using Abp.Dependency;
using System.Collections.Generic;
using System.Threading.Tasks;
using prod.Frame.Andon.Dto;

namespace prod.Frame.Andon.FramePlanA1
{
    public interface IFrmAdoFramePlanA1ExcelDataReader : ITransientDependency
    {
        Task<List<ImportFrmAdoFramePlanA1Dto>> GetFramePlanA1FromExcel(byte[] fileBytes, string fileName);
    }
}
