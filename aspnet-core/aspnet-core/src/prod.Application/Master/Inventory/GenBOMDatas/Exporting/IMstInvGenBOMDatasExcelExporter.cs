using Abp.Application.Services;
using prod.Dto;
using prod.Master.Inventory.MstInvGenBOMData.Dto;
using System.Collections.Generic;

namespace vovina.Master.Inventory.Exporting
{
        public interface IMstInvGenBOMDatasExcelExporter : IApplicationService
        {

            FileDto ExportToFile(List<MstInvGenBOMDataDto> mstinvgenbomdatas);

        }
}