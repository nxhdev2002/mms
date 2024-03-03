using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using prod.Master.LogW.RenbanModule.ImportDto;

namespace prod.Master.LogW.RenbanModule
{
 
    public interface IMstLgwRenbanModuleExcelDataReader : ITransientDependency
    {
        Task<List<ImportMstLgwRenbanModuleDto>> GetMstLgwRenbanModuleFromExcel(byte[] fileBytes, string fileName);
    }
}
