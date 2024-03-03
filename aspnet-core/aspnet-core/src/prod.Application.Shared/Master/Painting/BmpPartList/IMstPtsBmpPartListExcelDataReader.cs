using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using prod.Master.Painting.BmpPartList.ImportDto;

namespace prod.Master.Painting
{
    public interface IMstPtsBmpPartListExcelDataReader: ITransientDependency
    {
        Task<List<ImportMstPtsBmpPartListDto>> GetBmpPartListFromExcel(byte[] fileBytes, string fileName);
    }
}
