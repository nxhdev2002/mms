using Abp.Application.Services;

using System.Collections.Generic;
using prod.Dto;
using prod.Inventory.CKD.Dto;
namespace prod.Inventory.CKD.Exporting
{
    public interface IInvCkdContainerListExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<InvCkdContainerListDto> invckdcontainerlist);

        FileDto ShipmentInfoDetailExportToFile(List<ShipmentInfoDetailListDto> input);

        FileDto ShipmentInfoDetailPXPExportToFile(List<ShipmentInfoDetailListDto> shipment);

        FileDto InvoiceInfoDetailExportToFile(List<GetInvCkdContainerListExportInvoiceList> input);

        FileDto ListNoCutomsDeclareExportToFile(List<GetInvCkdContainerListExportNoDeclareCustomsList> input);

        FileDto ExportContainerInvoicebyContToFile(List<InvCkdContainerInvoiceDto> containerinvoice);

        FileDto ListImPortDeVanExportToFile(List<ImportDevanDto> input);
        FileDto ExportToHistoricalFile(List<string> data);

        FileDto ExportToFileByPeriod(List<InvCkdContainerListDto> invckdcontainerlist);
    }
}


