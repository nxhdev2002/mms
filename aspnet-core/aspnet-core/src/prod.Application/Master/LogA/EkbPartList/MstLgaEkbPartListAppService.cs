using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Master.LogA;
using prod.Master.LogA.Dto;
using prod.Master.LogA.Exporting;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.LogA
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_EkbPartList)]
    public class MstLgaEkbPartListAppService : prodAppServiceBase, IMstLgaEkbPartListAppService
    {
        private readonly IDapperRepository<MstLgaEkbPartList, long> _dapperRepo;
        private readonly IRepository<MstLgaEkbPartList, long> _repo;
        private readonly IMstLgaEkbPartListExcelExporter _calendarListExcelExporter;

        public MstLgaEkbPartListAppService(IRepository<MstLgaEkbPartList, long> repo,
                                         IDapperRepository<MstLgaEkbPartList, long> dapperRepo,
                                        IMstLgaEkbPartListExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_EkbPartList_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstLgaEkbPartListDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstLgaEkbPartListDto input)
        {
            var mainObj = ObjectMapper.Map<MstLgaEkbPartList>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstLgaEkbPartListDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_EkbPartList_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstLgaEkbPartListDto>> GetAll(GetMstLgaEkbPartListInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo), e => e.PartNo.Contains(input.PartNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.BackNo), e => e.BackNo.Contains(input.BackNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierNo), e => e.SupplierNo.Contains(input.SupplierNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Model), e => e.Model.Contains(input.Model))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProcessCode), e => e.ProcessCode.Contains(input.ProcessCode))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstLgaEkbPartListDto
                         {
                             Id = o.Id,
                             PartNo = o.PartNo,
                             PartNoNormanlized = o.PartNoNormanlized,
                             PartName = o.PartName,
                             BackNo = o.BackNo,
                             ProdLine = o.ProdLine,
                             SupplierNo = o.SupplierNo,
                             Model = o.Model,
                             ProcessId = o.ProcessId,
                             ProcessCode = o.ProcessCode,
                             ModuleCode = o.ModuleCode,
                             Cfc = o.Cfc,
                             ExporterBackNo = o.ExporterBackNo,
                             BodyColor = o.BodyColor,
                             Grade = o.Grade,
                             UsageQty = o.UsageQty,
                             BoxQty = o.BoxQty,
                             PcAddress = o.PcAddress,
                             PcSorting = o.PcSorting,
                             SpsAddress = o.SpsAddress,
                             SpsSorting = o.SpsSorting,
                             Remark = o.Remark,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstLgaEkbPartListDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetEkbPartListToExcel(GetMstLgaEkbPartListExcelInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo), e => e.PartNo.Contains(input.PartNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.BackNo), e => e.BackNo.Contains(input.BackNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierNo), e => e.SupplierNo.Contains(input.SupplierNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Model), e => e.Model.Contains(input.Model))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProcessCode), e => e.ProcessCode.Contains(input.ProcessCode))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var query = from o in pageAndFiltered
                        select new MstLgaEkbPartListDto
                        {
                            Id = o.Id,
                            PartNo = o.PartNo,
                            PartNoNormanlized = o.PartNoNormanlized,
                            PartName = o.PartName,
                            BackNo = o.BackNo,
                            ProdLine = o.ProdLine,
                            SupplierNo = o.SupplierNo,
                            Model = o.Model,
                            ProcessId = o.ProcessId,
                            ProcessCode = o.ProcessCode,
                            UsageQty = o.UsageQty,
                            BoxQty = o.BoxQty,
                            PcAddress = o.PcAddress,
                            PcSorting = o.PcSorting,
                            SpsAddress = o.SpsAddress,
                            SpsSorting = o.SpsSorting,
                            Remark = o.Remark,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        //Import Data From Excel
        //public async Task<List<MstLgaEkbPartListImportDto>> ImportEkbPartListFromExcel(List<MstLgaEkbPartListImportDto> LotUpPlans)
        //{
        //    try
        //    {
        //        List<LgwLupLotUpPlan_T> LotUpPlan = new List<LgwLupLotUpPlan_T> { };
        //        foreach (var item in LotUpPlans)
        //        {
        //            LgwLupLotUpPlan_T importData = new LgwLupLotUpPlan_T();
        //            {
        //                importData.Guid = item.Guid;
        //                importData.WorkingDate = item.WorkingDate;
        //                importData.ProdLine = item.ProdLine;
        //                importData.LotNo = item.LotNo;
        //                importData.Shift = item.Shift;
        //                importData.UnpackingStartDatetime = item.UnpackingStartDatetime;
        //                importData.UnpackingFinishDatetime = item.UnpackingFinishDatetime;
        //                importData.Tpm = item.Tpm;
        //                importData.Remarks = item.Remarks;
        //            }
        //            LotUpPlan.Add(importData);
        //        }
        //        CurrentUnitOfWork.GetDbContext<prodDbContext>().AddRangeAsync(LotUpPlan);
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(400, ex.Message);
        //    }
        //}

        ////Merge Data From Temp To PxPUpPlan
        //public async Task MergeDataLotUpPlan(string v_Guid)
        //{
        //    string _sql = "Exec LGW_LOT_UP_PLAN_MERGE @Guid";
        //    await _dapperRepo.QueryAsync<LgwLupLotUpPlan>(_sql, new { Guid = v_Guid });
        //}

    }
}
