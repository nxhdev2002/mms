using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using prod.LogA.Bp2.Dto;
using prod.Master.Common.Dto;

namespace prod.LogA.Bp2
{ 
    public interface ILgaBp2BigPartDDLAppServices
    {

        Task<PagedResultDto<LgaBp2BigPartDDpDto>> GetDataBigPartDDL(string prod_line);

        Task<MstCmmLookupDto> GetItemValue(string p_DomainCode, string p_ItemCode);
        Task<List<BigPartTabletAndonOutput>> GetDataBigPartTabletAndon(string p_Line, string p_ecar_id); 
        Task<List<BigPartTabletEcarOutput>> GetDataBigPartEcarAndon();

        Task<int> BigPartTabletAndon_START_FINISH(string p_progress_id);

        Task<int> BigPartTabletAndon_CALL_LEADER(string p_progress_id);
        Task<int> BigPartTabletAndon_UNDO(string p_progress_id);

    }
}
