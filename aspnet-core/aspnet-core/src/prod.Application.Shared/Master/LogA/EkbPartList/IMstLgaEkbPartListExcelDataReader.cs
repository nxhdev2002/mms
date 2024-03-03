using Abp.Dependency;
using System.Collections.Generic;
using System.Threading.Tasks;
using prod.Master.LogA.Dto;

namespace prod.Master.LogA.EkbPartList
{
    public interface IMstLgaEkbPartListExcelDataReader : ITransientDependency
    {
            Task<List<MstLgaEkbPartListImportDto>> GetMstLgaEkbPartListFromExcel(byte[] fileBytes, string fileName);
    }
}
