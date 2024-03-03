using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.BOM.Dto;
using prod.Inventory.BOM.Exporting;
using prod.Inventory.BOM.GenBOMData;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Inventory.BOM.InvGenBomData
{
    [AbpAuthorize(AppPermissions.Pages_Interface_Bom_InvGenBOMData_View)]
    public class InvGenBOMDataAppService : prodAppServiceBase, IInvGenBOMDataAppService
    {
        private readonly IRepository<InvGenBOMData, long> _repo;
        private readonly IDapperRepository<InvGenBOMData, long> _dapperRepo;
        private readonly IInvGenBOMDataExcelExporter _InvGenBOMDataExcelExporter;
        public InvGenBOMDataAppService(IRepository<InvGenBOMData, long> repo,
            IDapperRepository<InvGenBOMData, long> dapperRepo,
            IInvGenBOMDataExcelExporter InvGenBOMDataExcelExporter)
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _InvGenBOMDataExcelExporter = InvGenBOMDataExcelExporter;
        }


        public async Task<PagedResultDto<InvGenBOMDataDto>> GetAll(GetInvGenBOMDataInput input)
        {
            string _sql = "Exec INV_GEN_BOM_DATA_SEARCH @p_typempp, @p_periodmpp";

            IEnumerable<InvGenBOMDataDto> result = await _dapperRepo.QueryAsync<InvGenBOMDataDto>(_sql, new
            {
                p_typempp = input.TypeMPP,
                p_periodmpp = input.PeriodMpp
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvGenBOMDataDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetInvGenBOMDataToExcel(GetInvGenBOMDataExportInput input)
        {
            string _sql = "Exec INV_GEN_BOM_DATA_SEARCH @p_typempp, @p_periodmpp";

            IEnumerable<InvGenBOMDataDto> result = await _dapperRepo.QueryAsync<InvGenBOMDataDto>(_sql, new
            {
                p_typempp = input.TypeMPP,
                p_periodmpp = input.PeriodMpp
            });

            var exportToExcel = result.ToList();
            return _InvGenBOMDataExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
