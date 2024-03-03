using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using prod.Master.LogA.Bp2PartList.ImportDto;


namespace prod.Master.LogW.Bp2PartList
{
    public interface IMstLgaBp2PartListExcelDataReader : ITransientDependency
    {
        Task<List<ImportMstLgaBp2PartListDto>> GetBp2PartListFromExcel(byte[] fileBytes, string fileName);

    }
}
