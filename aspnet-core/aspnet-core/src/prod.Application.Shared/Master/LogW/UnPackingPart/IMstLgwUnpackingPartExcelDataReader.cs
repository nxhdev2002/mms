using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using prod.Master.LogW.UnPackingPart.ImportDto;

namespace prod.Master.LogW.UnPackingPart
{
    public interface IMstLgwUnpackingPartExcelDataReader : ITransientDependency
    {
        Task<List<ImportMstLgwUnpackingPartDto>> GetUnpackingPartFromExcel(byte[] fileBytes, string fileName);
    }

}
