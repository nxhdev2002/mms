using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.CKD.Dto;

namespace prod.Master.CKD.Exporting
{

    public interface IMstCkdCustomsLeadtimeExcelExporter : IApplicationService
    {

        FileDto ExportToFile(List<MstCkdCustomsLeadtimeDto> mstckdcustomsleadtime);

    }

}


