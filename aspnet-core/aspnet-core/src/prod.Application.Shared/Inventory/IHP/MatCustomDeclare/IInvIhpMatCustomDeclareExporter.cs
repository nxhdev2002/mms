using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.IHP.Dto;
using System.Collections.Generic;

namespace prod.Inventory.IHP
{
    public interface IInvIhpMatCustomDeclareExporter : IApplicationService
    {
        FileDto CustomDeclareExportToFile(List<InvIphMatCustomDeclareDto> invihpmatcustomdeclare);

        FileDto CustomDeclareDetailsExportToFile(List<InvIphMatCustomDeclareDetailsDto> invihpmatcustomdeclaredetails);

    }
}
