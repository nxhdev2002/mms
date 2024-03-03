using Abp.Dependency;
using System.Collections.Generic;
using System.Threading.Tasks;
using prod.Frame.Andon.Dto;

namespace prod.Frame.Andon.FramePlanBMPV
{
    public interface IFrmAdoFramePlanBMPVExcelDataReader : ITransientDependency
    {
        Task<List<ImportFrmAdoFramePlanBMPVDto>> GetFramePlanBMPVFromExcel(byte[] fileBytes, string fileName);
    }
}
