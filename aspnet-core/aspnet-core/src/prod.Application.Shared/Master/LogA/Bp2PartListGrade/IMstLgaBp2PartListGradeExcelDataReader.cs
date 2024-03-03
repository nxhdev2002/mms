using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using prod.Master.LogA.Bp2Process.ImportDto;
using prod.Master.LogA.ImportDto;

namespace prod.Master.LogA
{
    public interface IMstLgaBp2PartListGradeExcelDataReader : ITransientDependency
    {
        Task<List<ImportMstLgaBp2PartListGradeDto>> GetMstLgaBp2PartListGradeFromExcel(byte[] fileBytes, string fileName);
    }
}
