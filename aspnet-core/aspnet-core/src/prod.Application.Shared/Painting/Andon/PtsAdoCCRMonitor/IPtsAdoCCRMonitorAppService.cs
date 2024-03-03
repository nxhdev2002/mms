using Abp.Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using prod.Painting.Andon.Dto;

namespace prod.Painting.Andon
{

	public interface IPtsAdoCCRMonitorAppService : IApplicationService
	{

		Task<List<GetWeldingDataOutput>> GetWeldingData(); 
		Task<List<GetPaintingDataOutput>> GetPaintingData();
		Task<List<GetAssemblyDataOutput>> GetAssemblyData();
		Task<List<GetInspectionDataOutput>> GetInspectionData();
		Task<List<GetAllBufferDataOutput>> GetAllBufferData();
		Task<List<GetFrameOutput>> GetFrameData();
		Task<List<GetInventoryStdOutput>> GetInventoryStdData();
        Task<GetVehicleDetailsOutput> GetVehicleDetails(string p_body_no, string p_lot_no, string p_sequence_no, string p_vin_no);
	  
 

	}

}


