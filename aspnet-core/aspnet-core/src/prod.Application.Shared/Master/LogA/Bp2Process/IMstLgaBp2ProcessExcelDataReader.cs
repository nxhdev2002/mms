using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using prod.Master.LogA.Bp2Process.ImportDto;

namespace prod.Master.LogA.Bp2Process
{
    public interface IMstLgaBp2ProcessExcelDataReader  : ITransientDependency
    {
        Task<List<ImportMstLgaBp2ProcessDto>> GetMstLgaBp2ProcessFromExcel(byte[] fileBytes, string fileName);
    }
}
