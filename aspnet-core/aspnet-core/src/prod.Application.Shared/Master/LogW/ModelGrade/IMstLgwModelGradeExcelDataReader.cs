using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using prod.Master.LogW.ModelGrade.ImportDto;

namespace prod.Master.LogW.ModelGrade
{

    public interface IMstLgwModelGradeExcelDataReader : ITransientDependency
    {
        Task<List<ImportMstlgwModelGradeDto>> GetMstLgwModelgradeFromExcel(byte[] fileBytes, string fileName);
    }
}
