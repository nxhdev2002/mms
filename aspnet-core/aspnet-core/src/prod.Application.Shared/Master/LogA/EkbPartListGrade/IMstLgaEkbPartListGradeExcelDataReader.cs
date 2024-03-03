using Abp.Dependency;
using System.Collections.Generic;
using System.Threading.Tasks;
using prod.Master.LogA.Dto;

namespace prod.Master.LogA
{
    public interface IMstLgaEkbPartListGradeExcelDataReader : ITransientDependency
    {
            Task<List<MstLgaEkbPartListGradeImportDto>> GetMstLgaEkbPartListGradeFromExcel(byte[] fileBytes, string fileName);
    }
}
