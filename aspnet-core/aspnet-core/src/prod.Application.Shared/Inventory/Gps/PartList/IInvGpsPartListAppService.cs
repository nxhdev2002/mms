using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.Gps.PartList.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.Gps.PartList
{
    public interface IInvGpsPartListAppService : IApplicationService
    {
        Task<PagedResultDto<InvGpsPartListDto>> GetGpsPartList(GetInvGpsPartListInput input);
        Task<PagedResultDto<InvGpsPartGradeByPartListDto>> GetGpsPartGradeByPartList(GetInvGpsPartGradeByPartListInput input);

        Task<List<InvGpsPartListImportDto>> GetInvGpsPartListFromExcel(byte[] fileBytes, string fileName);

        Task MergeDataInvGpsPartList(string v_Guid);

        Task<FileDto> getExportInvGpsPartList(GetInvGpsPartListInputExport input);

        Task<List<InvGpsPartListImportDto>> GetInvGpsPartListNoColorFromExcel(byte[] fileBytes, string fileName);

        Task MergeDataInvGpsPartListNoColor(string v_Guid);
     
    }
}
