using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Dto;
using prod.HistoricalData;
using prod.Inventory.CKD.Dto;
using prod.Master.Cmm;
using prod.Master.Common.GradeColor;
using prod.Master.Common.GradeColor.Dto;
using prod.Master.Common.LotCodeGrade.Exporting;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.Common.LotCodeGrade
{
    public class MstCmmLotCodeGradeAppService : prodAppServiceBase, IMstCommGradeColorAppservice
    {
        private readonly IDapperRepository<MstCmmGradeColor, long> _mstCmmGradeColorRepo;
        private readonly IRepository<MstCmmLotCodeGrade, long> _mstCmmLotCodeGradeRepo;
        private readonly IDapperRepository<MstCmmColor, long> _mstCmmColorRepo;
        private readonly IRepository<MstCmmLookup, long> _repo;
        private readonly IHistoricalDataAppService _historicalDataAppService;

        private readonly IMstCmmLotCodeGradeExcelExporter _calendarListExcelExporter;


        public MstCmmLotCodeGradeAppService(IDapperRepository<MstCmmGradeColor, long> mstCmmGradeColorRepo,
                                          IRepository<MstCmmLotCodeGrade, long> mstCmmLotCodeGradeRepo,
                                          IDapperRepository<MstCmmColor, long> mstCmmColorRepo,
                                          IHistoricalDataAppService historicalDataAppService,
                                          IRepository<MstCmmLookup, long> repo,
                                          IMstCmmLotCodeGradeExcelExporter calendarListExcelExporter)


        {
            _mstCmmGradeColorRepo = mstCmmGradeColorRepo;
            _mstCmmLotCodeGradeRepo = mstCmmLotCodeGradeRepo;
            _mstCmmColorRepo = mstCmmColorRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
            _repo = repo;
            _historicalDataAppService = historicalDataAppService;
        }

        public async Task<List<string>> GetMstCmmGradeColorHistory(GetMstCmmGradeColorHistoryInput input)
        {
            return await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
        }

        public async Task<FileDto> GetHistoricalDataToExcel(GetMstCmmGradeColorHistoryExcelInput input)
        {
            var data = await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
            return _calendarListExcelExporter.ExportToHistoricalFile(data);
        }

        public async Task<ChangedRecordIdDto> GetChangedRecords()
        {
            var gradecolor = await _historicalDataAppService.GetChangedRecordIds("MstCmmLotCodeGrade");
            var gradecolordetail = await _historicalDataAppService.GetChangedRecordIds("MstCmmColor");
            ChangedRecordIdDto result = new ChangedRecordIdDto();
            result.GradeColor = gradecolor;
            result.GradeColorDetail = gradecolordetail;
            return result;
        }

        public async Task<PagedResultDto<MstCmmLotCodeGradeTDto>> GetAllGradeColor(MstCmmLotCodeGradeInput input)
        {
            var filtered = _mstCmmLotCodeGradeRepo.GetAll()
                      .WhereIf(!string.IsNullOrWhiteSpace(input.Cfc), e => e.Cfc.Contains(input.Cfc))
                      .WhereIf(!string.IsNullOrWhiteSpace(input.ModelVin), e => e.ModelVin.Contains(input.ModelVin))
                      .WhereIf(!string.IsNullOrWhiteSpace(input.ModelCode), e => e.ModelCode.Contains(input.ModelCode))
                      .WhereIf(!string.IsNullOrWhiteSpace(input.MaterialCode), e => e.MaterialCode.Contains(input.MaterialCode))
                      .WhereIf(!string.IsNullOrWhiteSpace(input.ProductionGroup), e => e.ProductionGroup.Contains(input.ProductionGroup))
                      .WhereIf(!string.IsNullOrWhiteSpace(input.ValuationType), e => e.ValuationType.Contains(input.ValuationType));

            var pageAndFiltered = filtered.OrderBy(s => s.Grade).ThenBy(e => e.Cfc);

            var system = from o in pageAndFiltered
                         select new MstCmmLotCodeGradeTDto
                         {
                             Id = o.Id,
                             Model = o.Model,
                             LotCode = o.LotCode,
                             Cfc = o.Cfc,
                             Grade = o.Grade,
                             GradeName = o.GradeName,
                             ModelCode = o.ModelCode,
                             ModelVin = o.ModelVin,
                             IdLine = o.IdLine,
                             Spec200 = o.Spec200,
                             SsNo = o.SsNo,
                             Katashiki = o.Katashiki,
                             KatashikiCtl = o.KatashikiCtl,
                             VehNameCd = o.VehNameCd,
                             MarLotCode = o.MarLotCode,
                             TestNo = o.TestNo,
                             VehicleId = o.VehicleId,
                             ProdSfx = o.ProdSfx,
                             SalesSfx = o.SalesSfx,
                             Brand = o.Brand,
                             CarSeries = o.CarSeries,
                             TransmissionType = o.TransmissionType,
                             EngineType = o.EngineType,
                             FuelType = o.FuelType,
                             GoshiCar = o.GoshiCar,
                             MaterialType = o.MaterialType,
                             MaterialCode = o.MaterialCode,
                             ProductionGroup = o.ProductionGroup,
                             ValuationType = o.ValuationType,
                             IndustrySector = o.IndustrySector,
                             Description = o.Description,
                             MaterialGroup = o.MaterialGroup,
                             BaseUnitOfMeasure = o.BaseUnitOfMeasure,
                             DeletionFlag = o.DeletionFlag,
                             Plant = o.Plant,
                             StorageLocation = o.StorageLocation,
                             ProductionPurpose = o.ProductionPurpose,
                             ProductionType = o.ProductionType,
                             ProfitCenter = o.ProfitCenter,
                             BatchManagement = o.BatchManagement,
                             ReservedStock = o.ReservedStock,
                             MrpGroup = o.MrpGroup,
                             MrpType = o.MrpType,
                             ProcurementType = o.ProcurementType,
                             SpecialProcurement = o.SpecialProcurement,
                             ProductionStorageLocation = o.ProductionStorageLocation,
                             RepetManufacturing = o.RepetManufacturing,
                             RemProfile = o.RemProfile,
                             DoNotCost = o.DoNotCost,
                             VarianceKey = o.VarianceKey,
                             CostingLotSize = o.CostingLotSize,
                             ProductionVersion = o.ProductionVersion,
                             SpecialProcurementCtgView = o.SpecialProcurementCtgView,
                             ValuationCategory = o.ValuationCategory,
                             ValuationClass = o.ValuationClass,
                             PriceDetermination = o.PriceDetermination,
                             PriceControl = o.PriceControl,
                             StandardPrice = o.StandardPrice,
                             MovingPrice = o.MovingPrice,
                             WithQtyStructure = o.WithQtyStructure,
                             MaterialOrigin = o.MaterialOrigin,
                             OriginGroup = o.OriginGroup,
                             AuthorizationGroup = o.AuthorizationGroup,
                             MatSrc = o.MatSrc,
                             EffectiveDateFrom = o.EffectiveDateFrom,
                             EffectiveDateTo = o.EffectiveDateTo,
                             IsActive = o.IsActive,

                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstCmmLotCodeGradeTDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }
        public async Task<FileDto> GetGradeColorToExcel(MstCmmLotCodeGradeExportInput input)
        {
            var filtered = _mstCmmLotCodeGradeRepo.GetAll()
                     .WhereIf(!string.IsNullOrWhiteSpace(input.Cfc), e => e.Cfc.Contains(input.Cfc))
                     .WhereIf(!string.IsNullOrWhiteSpace(input.ModelVin), e => e.ModelVin.Contains(input.ModelVin))
                     .WhereIf(!string.IsNullOrWhiteSpace(input.ModelCode), e => e.ModelCode.Contains(input.ModelCode))
                     .WhereIf(!string.IsNullOrWhiteSpace(input.MaterialCode), e => e.MaterialCode.Contains(input.MaterialCode))
                     .WhereIf(!string.IsNullOrWhiteSpace(input.ProductionGroup), e => e.ProductionGroup.Contains(input.ProductionGroup))
                     .WhereIf(!string.IsNullOrWhiteSpace(input.ValuationType), e => e.ValuationType.Contains(input.ValuationType));

            var pageAndFiltered = filtered.OrderBy(s => s.Grade).ThenBy(e => e.Cfc);

            var system = from o in pageAndFiltered
                         select new MstCmmLotCodeGradeTDto
                         {
                             Id = o.Id,
                             Model = o.Model,
                             LotCode = o.LotCode,
                             Cfc = o.Cfc,
                             Grade = o.Grade,
                             GradeName = o.GradeName,
                             ModelCode = o.ModelCode,
                             ModelVin = o.ModelVin,
                             IdLine = o.IdLine,
                             Spec200 = o.Spec200,
                             SsNo = o.SsNo,
                             Katashiki = o.Katashiki,
                             KatashikiCtl = o.KatashikiCtl,
                             VehNameCd = o.VehNameCd,
                             MarLotCode = o.MarLotCode,
                             TestNo = o.TestNo,
                             VehicleId = o.VehicleId,
                             ProdSfx = o.ProdSfx,
                             SalesSfx = o.SalesSfx,
                             Brand = o.Brand,
                             CarSeries = o.CarSeries,
                             TransmissionType = o.TransmissionType,
                             EngineType = o.EngineType,
                             FuelType = o.FuelType,
                             GoshiCar = o.GoshiCar,
                             MaterialType = o.MaterialType,
                             MaterialCode = o.MaterialCode,
                             ProductionGroup = o.ProductionGroup,
                             ValuationType = o.ValuationType,
                             IndustrySector = o.IndustrySector,
                             Description = o.Description,
                             MaterialGroup = o.MaterialGroup,
                             BaseUnitOfMeasure = o.BaseUnitOfMeasure,
                             DeletionFlag = o.DeletionFlag,
                             Plant = o.Plant,
                             StorageLocation = o.StorageLocation,
                             ProductionPurpose = o.ProductionPurpose,
                             ProductionType = o.ProductionType,
                             ProfitCenter = o.ProfitCenter,
                             BatchManagement = o.BatchManagement,
                             ReservedStock = o.ReservedStock,
                             LotCodeM = o.LotCodeM,
                             MrpGroup = o.MrpGroup,
                             MrpType = o.MrpType,
                             ProcurementType = o.ProcurementType,
                             SpecialProcurement = o.SpecialProcurement,
                             ProductionStorageLocation = o.ProductionStorageLocation,
                             RepetManufacturing = o.RepetManufacturing,
                             RemProfile = o.RemProfile,
                             DoNotCost = o.DoNotCost,
                             VarianceKey = o.VarianceKey,
                             CostingLotSize = o.CostingLotSize,
                             ProductionVersion = o.ProductionVersion,
                             SpecialProcurementCtgView = o.SpecialProcurementCtgView,
                             ValuationCategory = o.ValuationCategory,
                             ValuationClass = o.ValuationClass,
                             PriceDetermination = o.PriceDetermination,
                             PriceControl = o.PriceControl,
                             StandardPrice = o.StandardPrice,
                             MovingPrice = o.MovingPrice,
                             WithQtyStructure = o.WithQtyStructure,
                             MaterialOrigin = o.MaterialOrigin,
                             OriginGroup = o.OriginGroup,
                             AuthorizationGroup = o.AuthorizationGroup,
                             MatSrc = o.MatSrc,
                             EffectiveDateFrom = o.EffectiveDateFrom,
                             EffectiveDateTo = o.EffectiveDateTo,
                             IsActive = o.IsActive,
                         };

            var exportToExcel = await system.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        public async Task<PagedResultDto<MstCmmGradeColorDetailDto>> GetAllGradeColorDetail(MstCmmGradeColorDetailInput input)
        {
            string _sqlSearch = "Exec MST_CMM_GRADE_COLOR_DETAIL @GradeId";

            IEnumerable<MstCmmGradeColorDetailDto> result = await _mstCmmGradeColorRepo.QueryAsync<MstCmmGradeColorDetailDto>(_sqlSearch,
                  new
                  {
                      GradeId = input.gradeId
                  });
            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<MstCmmGradeColorDetailDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<List<MstColorDto>> GetColorDetailColorList(int gradeId)
        {
            string _sqlSearch = "Exec MST_CMM_GRADE_COLOR_DETAIL_COLOR_LIST @GradeId";

            IEnumerable<MstColorDto> _result = await _mstCmmColorRepo.QueryAsync<MstColorDto>(_sqlSearch,
                     new
                     {
                         GradeId = gradeId
                     });
            return _result.ToList();
        }


        public async Task CreateOrEditGradeColor(string ListColorId, int GradeId)
        {
            string _sqlUpdateDes = "Exec MST_CMM_GRADECOLOR_CUD @ListColorId,@GradeId,@p_UserId";
            await _mstCmmGradeColorRepo.ExecuteAsync(_sqlUpdateDes,
                   new
                   {
                       ListColorId = ListColorId,
                       GradeId = GradeId,
                       p_UserId = AbpSession.UserId
                   });
        }
        public async Task<FileDto> GetLotCodeToExcel(MstCmmLotCodeGrandeExportInput input)
        {

            var query = from o in _mstCmmLotCodeGradeRepo.GetAll()
                        select new MstCmmLotCodeGradeTDto
                        {

                            Id = o.Id,
                            Model = o.Model,
                            LotCode = o.LotCode,
                            Cfc = o.Cfc,
                            Grade = o.Grade,
                            GradeName = o.GradeName,
                            ModelCode = o.ModelCode,
                            ModelVin = o.ModelVin,
                            IdLine = o.IdLine,
                            Spec200 = o.Spec200,
                            SsNo = o.SsNo,
                            Katashiki = o.Katashiki,
                            KatashikiCtl = o.KatashikiCtl,
                            VehNameCd = o.VehNameCd,
                            MarLotCode = o.MarLotCode,
                            TestNo = o.TestNo,
                            VehicleId = o.VehicleId,
                            ProdSfx = o.ProdSfx,
                            SalesSfx = o.SalesSfx,
                            Brand = o.Brand,
                            CarSeries = o.CarSeries,
                            TransmissionType = o.TransmissionType,
                            EngineType = o.EngineType,
                            FuelType = o.FuelType,
                            GoshiCar = o.GoshiCar,
                            MaterialType = o.MaterialType,
                            MaterialCode = o.MaterialCode,
                            ProductionGroup = o.ProductionGroup,
                            ValuationType = o.ValuationType,
                            IndustrySector = o.IndustrySector,
                            Description = o.Description,
                            MaterialGroup = o.MaterialGroup,
                            BaseUnitOfMeasure = o.BaseUnitOfMeasure,
                            DeletionFlag = o.DeletionFlag,
                            Plant = o.Plant,
                            StorageLocation = o.StorageLocation,
                            ProductionPurpose = o.ProductionPurpose,
                            ProductionType = o.ProductionType,
                            ProfitCenter = o.ProfitCenter,
                            BatchManagement = o.BatchManagement,
                            ReservedStock = o.ReservedStock,
                            MrpGroup = o.MrpGroup,
                            MrpType = o.MrpType,
                            ProcurementType = o.ProcurementType,
                            SpecialProcurement = o.SpecialProcurement,
                            ProductionStorageLocation = o.ProductionStorageLocation,
                            RepetManufacturing = o.RepetManufacturing,
                            RemProfile = o.RemProfile,
                            DoNotCost = o.DoNotCost,
                            VarianceKey = o.VarianceKey,
                            CostingLotSize = o.CostingLotSize,
                            ProductionVersion = o.ProductionVersion,
                            SpecialProcurementCtgView = o.SpecialProcurementCtgView,
                            ValuationCategory = o.ValuationCategory,
                            ValuationClass = o.ValuationClass,
                            PriceDetermination = o.PriceDetermination,
                            PriceControl = o.PriceControl,
                            StandardPrice = o.StandardPrice,
                            MovingPrice = o.MovingPrice,
                            WithQtyStructure = o.WithQtyStructure,
                            MaterialOrigin = o.MaterialOrigin,
                            OriginGroup = o.OriginGroup,
                            AuthorizationGroup = o.AuthorizationGroup,
                            MatSrc = o.MatSrc,
                            EffectiveDateFrom = o.EffectiveDateFrom,
                            EffectiveDateTo = o.EffectiveDateTo,
                            IsActive = o.IsActive,

                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }
    }
}

