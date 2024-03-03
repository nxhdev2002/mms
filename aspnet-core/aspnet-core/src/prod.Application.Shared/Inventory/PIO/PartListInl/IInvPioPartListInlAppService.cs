using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.PIO.PartListInl.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.PIO.PartListInl
{
    public  interface IInvPioPartListInlAppService : IApplicationService
    {
        Task<PagedResultDto<InvPioPartListInlDto>> GetAll(GetInvPioPartListInlInput input);
        Task<PagedResultDto<InvPioPartGradeInlDto>> GetPartGradeInl(GetInvPioPartListGradeInlInput input);
        Task<PagedResultDto<ValidatePartListDto>> GetValidateInvPioPartList(PagedAndSortedResultRequestDto input);
        Task<FileDto> GetValidateInvPioPartListExcel();
        Task<PagedResultDto<CheckExistPartListInlDto>> CheckExistPartNo(string PartNo, string Cfc);
        Task PartListInlInsert(GetPartListGradePartListInlDto input);
        Task PartListInlEdit(GetPartListGradePartListInlDto input);
        Task<List<ImportPioPartListDto>> ImportDataInvPioPartListFromExcel(byte[] fileBytes, string fileName);
        Task<List<ImportPioPartListDto>> ImportDataInvPioPartListLotFromExcel(byte[] fileBytes, string fileName);
        Task MergeDataInvPioPartList(string v_Guid);
        Task<PagedResultDto<ImportCkdPartListDto>> GetMessageErrorImport(string v_Guid, string v_screen);
        Task<FileDto> GetListErrPartListInlToExcel(string v_Guid, string v_Screen);
        Task<FileDto> GetPioPartExportToFile(InvCkdPartListExportInput input);
        Task<FileDto> GetPioPartGroupBodyColorExportToFile(InvCkdPartListExportInput input);

    }
}
