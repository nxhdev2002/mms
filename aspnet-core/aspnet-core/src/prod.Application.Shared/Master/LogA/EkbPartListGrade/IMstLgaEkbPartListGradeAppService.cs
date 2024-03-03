using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Master.LogA.Dto;
using prod.LogA.Pcs.Stock.Dto;
using prod.LogA.Sps.Stock.Dto;

namespace prod.Master.LogA
{

    public interface IMstLgaEkbPartListGradeAppService : IApplicationService
    {

        Task<PagedResultDto<MstLgaEkbPartListGradeDto>> GetAll(GetMstLgaEkbPartListGradeInput input);

        Task CreateOrEdit(CreateOrEditMstLgaEkbPartListGradeDto input);

        Task Delete(EntityDto input);

        Task<List<MstLgaEkbPartListGradeImportDto>> ImportMstLgaEkbPartListGradeFromExcel(List<MstLgaEkbPartListGradeImportDto> ekbPartListGrades);

        Task<List<LgaPcsStockImportDto>> ImportLgaPcsStockFromExcel(List<LgaPcsStockImportDto> pcsStocks);

        Task<List<LgaSpsStockImportDto>> ImportLgaSpsStockFromExcel(List<LgaSpsStockImportDto> spsStocks);

        Task<FileDto> ExportEkbPartListGrade(string model);

    }

}