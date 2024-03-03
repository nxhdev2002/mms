using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.CKD.CustomsDeclare.Exporting
{
    public interface IInvCkdCustomsDeclareExcelExporter : IApplicationService
    {

        FileDto CustomerDeclareExportToFile(List<InvCkdCustomsDeclareDto> customsdeclare);

        FileDto PreCustomsExportToFile(List<PreCustomerDto> precustoms);

        FileDto InvoiceExportToFile(List<InVoiceListDto> invoice);


    }
}
