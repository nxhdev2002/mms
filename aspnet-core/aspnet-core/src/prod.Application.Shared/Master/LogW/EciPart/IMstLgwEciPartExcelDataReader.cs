using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using prod.Master.LogW.EciPart.ImportDto;

namespace prod.Master.LogW
{
    public interface IMstLgwEciPartExcelDataReader : ITransientDependency
    {
        Task<List<ImportMstLgwEciPartDto>> GetEciPartFromExcel(byte[] fileBytes, string fileName);

    }
}
