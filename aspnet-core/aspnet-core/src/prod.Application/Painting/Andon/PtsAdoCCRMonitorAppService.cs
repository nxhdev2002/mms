using Abp.Authorization;
using Abp.Dapper.Repositories; 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 
using prod.Painting.Andon.Dto;
using prod.Authorization;

namespace prod.Painting.Andon
{

    //  [AbpAuthorize(AppPermissions.Pages_Painting_Andon_CcrMonitor)]
    public class PtsAdoCCRMonitorAppService : prodAppServiceBase, IPtsAdoCCRMonitorAppService
    {
        private readonly IDapperRepository<PtsAdoScanInfo, long> _dapperRepo; 

        public PtsAdoCCRMonitorAppService(IDapperRepository<PtsAdoScanInfo, long> dapperRepo)
        { 
            _dapperRepo = dapperRepo; 
        }

        public async Task<List<GetWeldingDataOutput>> GetWeldingData()
        {

            string _sql = "Exec PTS_ADO_CCR_MONITOR_WELDING";

            var filtered = await _dapperRepo.QueryAsync<GetWeldingDataOutput>(_sql, new { });

            return filtered.ToList();
        }

        public async Task<List<GetPaintingDataOutput>> GetPaintingData()
        {

            string _sql = "Exec PTS_ADO_CCR_MONITOR_PAINT";

            var filtered = await _dapperRepo.QueryAsync<GetPaintingDataOutput>(_sql, new { });
             
            return filtered.ToList();
        }

        public async Task<List<GetAssemblyDataOutput>> GetAssemblyData()
        {

            string _sql = "Exec PTS_ADO_CCR_MONITOR_ASSEMBLY";

            var filtered = await _dapperRepo.QueryAsync<GetAssemblyDataOutput>(_sql, new { });

            return filtered.ToList();
        }

        public async Task<List<GetInspectionDataOutput>> GetInspectionData()
        {

            string _sql = "Exec PTS_ADO_CCR_MONITOR_INSPECTION";

            var filtered = await _dapperRepo.QueryAsync<GetInspectionDataOutput>(_sql, new { });

            return filtered.ToList();
        }

        public async Task<List<GetAllBufferDataOutput>> GetAllBufferData()
        {

            string _sql = "Exec PTS_ADO_CCR_MONITOR_ALL_BUFFER";

            var filtered = await _dapperRepo.QueryAsync<GetAllBufferDataOutput>(_sql, new { });

            return filtered.ToList();
        }

		public async Task<List<GetFrameOutput>> GetFrameData()
        {

            string _sql = "Exec PTS_ADO_CCR_MONITOR_FRAME";

            var filtered = await _dapperRepo.QueryAsync<GetFrameOutput>(_sql, new { });

            return filtered.ToList();
        }

        public async Task<GetVehicleDetailsOutput> GetVehicleDetails(string body_no, string lot_no, string sequence_no, string vin_no)
        {

            string _sql = "Exec PTS_ADO_CCR_MONITOR_GET_VEHICLE_DETAILS @p_body_no, @p_lot_no, @p_sequence_no, @p_vin_no";

            var filtered = await _dapperRepo.QueryAsync<GetVehicleDetailsOutput>(_sql, new { 
                p_body_no = body_no,
                p_lot_no = lot_no,
                p_sequence_no = sequence_no,
                p_vin_no = vin_no
            });

            return filtered.ToList().Count > 0 ? filtered.ToList()[0] : null;
        }

        public async Task<List<GetInventoryStdOutput>> GetInventoryStdData()
        {

            string _sql = "Exec PTS_ADO_CCR_MONITOR_INVENTORY_STD";

            var filtered = await _dapperRepo.QueryAsync<GetInventoryStdOutput>(_sql, new { });

            return filtered.ToList();
        }

    }
}
