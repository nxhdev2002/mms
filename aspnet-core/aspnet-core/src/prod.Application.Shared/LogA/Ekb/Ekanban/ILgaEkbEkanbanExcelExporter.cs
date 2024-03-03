using Abp.Application.Services;
using prod.Dto;
using prod.LogA.Ekb.Dto;
using prod.LogA.Ekb.Ekanban.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.LogA.Ekb.Ekanban
{
    public interface ILgaEkbEkanbanExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<LgaEkbEkanbanDto> lgaekbekanban); 
    }
}
