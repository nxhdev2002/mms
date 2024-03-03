using Abp.Application.Services;
using prod.Dto;
using prod.Master.Inv.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Master.Inv.Exporting
{
    public interface IMstInvGpsSupplierOrderTimeExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<MstInvGpsSupplierOrderTimeDto> mstinvgpssupplierordertime);
    }
}
