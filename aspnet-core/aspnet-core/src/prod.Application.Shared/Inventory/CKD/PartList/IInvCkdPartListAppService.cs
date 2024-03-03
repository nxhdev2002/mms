using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using prod.Inventory.CKD.Dto;
using prod.Dto;
using System.Collections.Generic;
using prod.Inventory.GPS.Dto;

namespace prod.Inventory.CKD
{

    public interface IInvCkdPartListAppService : IApplicationService
    {

        Task<PagedResultDto<InvCkdPartListDto>> GetAll(GetInvCkdPartListInput input);

        Task<List<ImportCkdPartListDto>> ImportDataInvCkdPartListFromExcel(byte[] fileBytes, string fileName);

        Task<List<ImportInvCkdPartGradeDto>> ImportDataInvCkdPartGradeFromExcel(byte[] fileBytes, string fileName);


        Task<List<ImportCkdPartListDto>> ImportDataInvCkdPartListLotFromExcel(byte[] fileBytes, string fileName);

        Task<PagedResultDto<CheckExistDto>> CheckExistPartNo(string PartNo, string Cfc);

        Task<PagedResultDto<CheckExistDto>> CheckExistBodyColor(ValidatePartGradeBodyColorInput input);

        Task<int> PartGradeDel(long? v_id);

        Task EciPartGrade(long? v_id, string v_StartLot, string v_StartRun, string v_Grade);
        Task<List<ImportCkdPartListNormalDto>> ImportDataInvCkdPartListNormalFromExcel(byte[] fileBytes, string fileName);
    }

}


