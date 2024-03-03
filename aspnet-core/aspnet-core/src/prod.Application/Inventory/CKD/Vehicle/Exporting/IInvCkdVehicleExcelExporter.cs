using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Assy.Andon.Dto;
using prod.Inventory.CKD.Vehicle.Dto;
using prod.Inventory.CKD.Dto;

namespace prod.Assy.Andon.Exporting
{

	public interface IInvCkdVehicleExcelExporter : IApplicationService
	{

		FileDto ExportToFile(List<InvCkdVehicleDto> ivnckdvehicle);
        FileDto ExportToFileGIVehicle(List<InvCkdVehicleGIDto> ivnckdvehiclegi);
        FileDto ExportToFileOutDetailsVehicle(List<InvCkdVehicleOutDetailsDto> ivnckdvehicleoutdetails);
        FileDto DetailOutPartExportToFile(List<InvCkdVehicleDetailOutPartDto> invckdvehicledetailoutpart);
        FileDto OutPartByVehicleExportToFile(List<InvCkdOutPartByVehicleDto> invckdoutpartbyvehicle);

        FileDto ListViewIFExportToFile(List<ViewIF> input);

        FileDto ExportToFileByPeriod(List<InvCkdVehicleDto> ivnckdvehicle);
    }

}


