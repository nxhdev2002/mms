using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using prod.Authorization;
using prod.Dto;
using prod.HistoricalData;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.Exporting;
using prod.Inventory.GPS.Dto;
using prod.Inventory.IHP.Dto;
using prod.Inventory.IHP.Exporting;
using prod.Inventory.IHP.PartGrade.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prod.Inventory.IHP
{
    [AbpAuthorize(AppPermissions.Pages_DMIHP_Mst_IHPPartList_View)]
    public class InvIhpPartListAppService : prodAppServiceBase, IInvIhpPartListAppService
    {
        private readonly IDapperRepository<InvIhpPartList, long> _dapperRepo;
        private readonly IRepository<InvIhpPartList, long> _repo;
        private readonly IDapperRepository<InvIhpPartGrade, long> _invIhpPartGradeRepo;
        private readonly IInvIhpPartListExcelExporter _partListExcelExporter;
        private readonly IHistoricalDataAppService _historicalDataAppService;

        public InvIhpPartListAppService(IRepository<InvIhpPartList, long> repo,
                                         IDapperRepository<InvIhpPartList, long> dapperRepo,
                                         IDapperRepository<InvIhpPartGrade, long> invIhpPartGradeRepo,
                                         IInvIhpPartListExcelExporter partListExcelExporter,
                                         IHistoricalDataAppService historicalDataAppService
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _partListExcelExporter = partListExcelExporter;
            _invIhpPartGradeRepo = invIhpPartGradeRepo;
            _historicalDataAppService = historicalDataAppService;
        }
        //PART LIST
        public async Task<PagedResultDto<InvIhpPartListDto>> GetDataPartList(GetInvIhpPartListInput input)
        {

            string _sql = "Exec INV_IHP_PART_LIST_SEARCH @p_SupplierCd,@p_Cfc , @p_PartNo, @p_Grade,@p_PartName,@p_MaterialCode,@p_MaterialSpec";


            IEnumerable<InvIhpPartListDto> result = await _dapperRepo.QueryAsync<InvIhpPartListDto>(_sql, new
            {
                p_SupplierCd = input.SupplierCd,
                p_Cfc = input.Cfc,
                p_PartNo = input.PartNo,
                p_Grade = input.Grade,
                p_PartName = input.PartName,
                p_MaterialCode = input.MaterialCode,
                p_MaterialSpec = input.MaterialSpec,
            });
            var listResult = result.ToList();
            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<InvIhpPartListDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetIhpPartListToExcel(GetInvIhpPartListExportInput input)
        {

            string _sql = "Exec INV_IHP_PART_LIST_SEARCH @p_SupplierCd,@p_Cfc , @p_PartNo, @p_Grade,@p_PartName,@p_MaterialCode,@p_MaterialSpec";

            IEnumerable<InvIhpPartListDto> result = await _dapperRepo.QueryAsync<InvIhpPartListDto>(_sql, new
            {
                p_SupplierCd = input.SupplierCd,
                p_Cfc = input.Cfc,
                p_PartNo = input.PartNo,
                p_Grade = input.Grade,
                p_PartName = input.PartName,
                p_MaterialCode = input.MaterialCode,
                p_MaterialSpec = input.MaterialSpec,
            });

            var exportToExcel = result.ToList();
            return _partListExcelExporter.ExportToFile(exportToExcel);
        }
        public async Task<List<string>> GetInhousePartListHistory(GetInvIhpPartListHistoryInput input)
        {
            return await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
        }
        public async Task<FileDto> GetHistoricalDataToExcel(GetInvIhpPartListHistoryExcelInput input)
        {
            var data = await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
            return _partListExcelExporter.ExportToHistoricalFile(data, input.TableName);
        }
        public async Task<ChangedRecordIdsDto> GetChangedRecords()
        {
            var listPartNo = await _historicalDataAppService.GetChangedRecordIds("InvIhpPartList");
            var listPartGrade = await _historicalDataAppService.GetChangedRecordIds("InvIhpPartGrade");

            ChangedRecordIdsDto result = new ChangedRecordIdsDto();
            result.PartList = listPartNo;
            result.PartGrade = listPartGrade;
            return result;
        }

        //PART GRADE
        public async Task<PagedResultDto<InvIhpPartGradeDto>> GetDataPartGradebyId(GetInvIhpPartGradeInput input)
        {
            string _sql = "Exec INV_IHP_PART_GRADE_BY_PARTLISTID @p_PartListId";

            IEnumerable<InvIhpPartGradeDto> result = await _dapperRepo.QueryAsync<InvIhpPartGradeDto>(_sql, new
            {
                p_PartListId = input.PartListId
            });
            var listResult = result.ToList();
            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<InvIhpPartGradeDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetInvIhpPartGradeToExcel(GetInvIhpPartGradeExportInput input)
        {
            string _sql = "Exec INV_IHP_PART_GRADE_BY_PARTLISTID @p_PartListId";

            IEnumerable<InvIhpPartGradeDto> result = await _dapperRepo.QueryAsync<InvIhpPartGradeDto>(_sql, new
            {
                p_PartListId = input.PartListId
            });

            var exportToExcel = result.ToList();
            return _partListExcelExporter.ExportPartGradeToFile(exportToExcel);
        }

        [AbpAuthorize(AppPermissions.Pages_DMIHP_Master_PartList_Validate)]
        public async Task<PagedResultDto<ValidateIhpPartListDto>> GetValidateInvIhpPartList(PagedAndSortedResultRequestDto input)
        {
            string _sqlSearch = "Exec [INV_IHP_PART_LIST_VALIDATE]";

            IEnumerable<ValidateIhpPartListDto> result = await _invIhpPartGradeRepo.QueryAsync<ValidateIhpPartListDto>(_sqlSearch, new { });

            var listResult = result.ToList();
            var totalCount = result.ToList().Count();
            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            return new PagedResultDto<ValidateIhpPartListDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetValidateInvIhpPartListExcel()
        {

            string _sql = "Exec [INV_IHP_PART_LIST_VALIDATE]";

            IEnumerable<ValidateIhpPartListDto> result = await _dapperRepo.QueryAsync<ValidateIhpPartListDto>(_sql, new
            { });

            var exportToExcel = result.ToList();
            return _partListExcelExporter.ExportValidateToFile(exportToExcel);
        }

        public async Task<bool> CheckExistPart(string PartNo, string Cfc)
        {
            string _sqlSearch = "Exec INV_IHP_PART_LIST_CHECK_EXISTS @p_PartNo, @p_Cfc";

            IEnumerable<CheckExistDto> result = await _dapperRepo.QueryAsync<CheckExistDto>(_sqlSearch, new { p_PartNo = PartNo, p_Cfc = Cfc });
            var listResult = result.ToList();

            var totalCount = result.ToList().Count();

            if(totalCount > 0) return true;
            else return false;
        }

        public async Task<List<GetCfcDto>> GetListCfc()
        {
            IEnumerable<GetCfcDto> result = await _dapperRepo.QueryAsync<GetCfcDto>("Exec MST_CMM_LOT_CODE_GRADE_CFC_GETS ");
            return result.ToList();
        }

        public async Task<List<GetGradeDto>> GetListGrade()
        {
            IEnumerable<GetGradeDto> result = await _dapperRepo.QueryAsync<GetGradeDto>("Exec MST_CMM_LOT_CODE_GRADE_GRADE_GETS ");
            return result.ToList();
        }

        public async Task<List<GetPartNoDto>> GetListInhousePartNo()
        {
            IEnumerable<GetPartNoDto> result = await _dapperRepo.QueryAsync<GetPartNoDto>("Exec INV_IHP_GET_LIST_PART");
            return result.ToList();
        }

        public async Task<List<GetListMaterialDto>> GetListMaterialByCfc(string cfc)
        {
            IEnumerable<GetListMaterialDto> result = await _dapperRepo.QueryAsync<GetListMaterialDto>("Exec INV_IHP_GET_LIST_MATERIAL_BY_CFC @p_Cfc", new { p_Cfc = cfc });
            return result.ToList();
        }

        public async Task EditIhpPartGrade(InvIhpPartGradeDto input)
        {
            string _sql = "Exec INV_IHP_PART_GRADE_EDIT @p_PartGradeId, @p_Grade, @p_UsageQty, @p_FirstDayProduct, @p_LastDayProduct, @p_UserId";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_PartGradeId = input.Id,
                p_Grade = input.Grade,
                p_UsageQty = input.UsageQty,
                p_FirstDayProduct = input.FirstDayProduct,
                p_LastDayProduct = input.LastDayProduct,
                p_UserId = AbpSession.UserId
            });
        }

        public async Task InsertIhpPartList(GetEditIhpPartListDto input)
        {
            string _sql = "Exec INV_IHP_PART_LIST_INSERT @p_SupplierType, @p_SupplierCd, @p_Cfc, @p_PartNo, @p_PartName, @p_MaterialCode, @p_MaterialSpec, " +
                "@p_Sourcing, @p_Cutting, @p_Packing, @p_SheetWeight, @p_YiledRation, @p_DrmPartListId, @p_UserId";
            var result = (await _dapperRepo.QueryAsync<GetPartListId>(_sql, new
            {
                p_SupplierType = input.SupplierType,
                p_SupplierCd = input.SupplierCd,
                p_Cfc = input.Cfc,
                p_PartNo = input.PartNo,
                p_PartName = input.PartName,
                p_MaterialCode = input.MaterialCode,
                p_MaterialSpec = input.MaterialSpec,
                p_Sourcing = input.Sourcing,
                p_Cutting = input.Cutting,
                p_Packing = input.Packing,
                p_SheetWeight = input.SheetWeight,
                p_YiledRation = input.YiledRation,
                p_DrmPartListId = input.DrmPartListId,
                p_UserId = AbpSession.UserId
            })).FirstOrDefault();

            string _sql2 = "Exec INV_IHP_PART_GRADE_INSERT @p_PartListId, @p_Grade, @p_UsageQty, @p_FirstDayProduct, @p_LastDayProduct, @p_UserId";
            for (int i = 0; i < input.listGrade.Count; i++)
            {
                await _dapperRepo.ExecuteAsync(_sql2, new
                {
                    p_PartListId = result.PartListId,
                    p_Grade = input.listGrade[i].Grade,
                    p_UsageQty = input.listGrade[i].UsageQty,
                    p_FirstDayProduct = input.listGrade[i].FirstDayProduct,
                    p_LastDayProduct = input.listGrade[i].LastDayProduct,
                    p_UserId = AbpSession.UserId
                });
            }
        }

        public async Task EditInhousePart(GetEditIhpPartListDto input)
        {
            string _sql = "Exec INV_IHP_PART_LIST_EDIT @p_Id, @p_SupplierType, @p_SupplierCd, @p_PartName, @p_MaterialCode, @p_MaterialSpec, " +
                "@p_Sourcing, @p_Cutting, @p_Packing, @p_SheetWeight, @p_YiledRation, @p_DrmPartListId, @p_UserId";
            await _dapperRepo.QueryAsync<GetPartListId>(_sql, new
            {
                p_Id = input.Id,
                p_SupplierType = input.SupplierType,
                p_SupplierCd = input.SupplierCd,
                p_PartName = input.PartName,
                p_MaterialCode = input.MaterialCode,
                p_MaterialSpec = input.MaterialSpec,
                p_Sourcing = input.Sourcing,
                p_Cutting = input.Cutting,
                p_Packing = input.Packing,
                p_SheetWeight = input.SheetWeight,
                p_YiledRation = input.YiledRation,
                p_DrmPartListId = input.DrmPartListId,
                p_UserId = AbpSession.UserId
            });

            string listGradeSelected = ""; 
            for (int i = 0; i < input.listGrade.Count; i++)
            {
                listGradeSelected += "," + input.listGrade[i].Grade;
            }

            string _sql2 = "Exec INV_IHP_PART_GRADE_UPDATE @p_Id, @p_PartListId, @p_Grade, @p_UsageQty, @p_FirstDayProduct, @p_LastDayProduct, @p_ListGradeSelect, @p_UserId";
            for (int i = 0; i < input.listGrade.Count; i++)
            {
                await _dapperRepo.ExecuteAsync(_sql2, new
                {
                    p_Id = input.listGrade[i].Id,
                    p_PartListId = input.Id,
                    p_Grade = input.listGrade[i].Grade,
                    p_UsageQty = input.listGrade[i].UsageQty,
                    p_FirstDayProduct = input.listGrade[i].FirstDayProduct,
                    p_LastDayProduct = input.listGrade[i].LastDayProduct,
                    p_ListGradeSelect = listGradeSelected,
                    p_UserId = AbpSession.UserId
                });
            }
        }

        public async Task DeleteInhousePartGrade(long? id)
        {
            await _dapperRepo.ExecuteAsync("Exec INV_IHP_PART_GRADE_DELETE @p_Id", new { p_Id = id });
        }
    }
}
