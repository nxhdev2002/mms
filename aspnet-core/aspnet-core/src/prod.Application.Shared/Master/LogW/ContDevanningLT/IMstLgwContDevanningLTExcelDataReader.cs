using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.LogW
{

    public interface IMstLgwContDevanningLTExcelDataReader : ITransientDependency
    {
        Task<List<ImportContDevanningLTDto>> GetContDevanningLTFromExcel(byte[] fileBytes, string fileName);
    }
}
