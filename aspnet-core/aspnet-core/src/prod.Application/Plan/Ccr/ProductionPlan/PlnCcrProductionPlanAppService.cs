using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Dapper;
using Microsoft.EntityFrameworkCore;
using NUglify.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Plan.Ccr.Dto;
using prod.Plan.Ccr.Exporting;
using prod.Plan.Ccr.ProductionPlan.ImportDto;

namespace prod.Plan.Ccr
{
    //  [AbpAuthorize(AppPermissions.Pages_Plan_Ccr_ProductionPlan)]
    public class PlnCcrProductionPlanAppService : prodAppServiceBase, IPlnCcrProductionPlanAppService
    {
        private readonly IDapperRepository<PlnCcrProductionPlan, long> _dapperRepo;
        private readonly IRepository<PlnCcrProductionPlan, long> _repo;
        private readonly IPlnCcrProductionPlanExcelExporter _calendarListExcelExporter;

        public PlnCcrProductionPlanAppService(IRepository<PlnCcrProductionPlan, long> repo,
                                         IDapperRepository<PlnCcrProductionPlan, long> dapperRepo,
                                        IPlnCcrProductionPlanExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Plan_Ccr_ProductionPlan_Edit)]
        public async Task CreateOrEdit(CreateOrEditPlnCcrProductionPlanDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditPlnCcrProductionPlanDto input)
        {
            var mainObj = ObjectMapper.Map<PlnCcrProductionPlan>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditPlnCcrProductionPlanDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Plan_Ccr_ProductionPlan_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<PlnCcrProductionPlanDto>> GetAll(GetPlnCcrProductionPlanInput input)
        {
            var filtered = _repo.GetAll()
                 .WhereIf(!string.IsNullOrWhiteSpace(input.Shop), e => e.Shop.Contains(input.Shop))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Model), e => e.Model.Contains(input.Model))
                .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo))
                .WhereIf(input.DateIn.HasValue, t => t.DateIn.Value.Date == input.DateIn.Value.Date);

            var pageAndFiltered = filtered.OrderBy(s => s.DateIn);
            var system = from o in pageAndFiltered
                         select new PlnCcrProductionPlanDto
                         {
                             Id = o.Id,
                             PlanSequence = o.PlanSequence,
                             Shop = o.Shop,
                             Model = o.Model,
                             LotNo = o.LotNo,
                             NoInLot = o.NoInLot,
                             Grade = o.Grade,
                             Body = o.Body,
                             DateIn = o.DateIn,
                             TimeIn = o.TimeIn,
                             DateTimeIn = o.DateTimeIn,
                             SupplierNo = o.SupplierNo,
                             UseLotNo = o.UseLotNo,
                             SupplierNo2 = o.SupplierNo2,
                             UseLotNo2 = o.UseLotNo2,
                             UseNoInLot = o.UseNoInLot,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<PlnCcrProductionPlanDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetProductionPlanToExcel(GetPlnCcrProductionPlanExportInput input)
        {
            var filtered = _repo.GetAll()
               .WhereIf(!string.IsNullOrWhiteSpace(input.Shop), e => e.Shop.Contains(input.Shop))
              .WhereIf(!string.IsNullOrWhiteSpace(input.Model), e => e.Model.Contains(input.Model))
              .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo))
              .WhereIf(input.DateIn.HasValue, t => t.DateIn.Value.Date == input.DateIn.Value.Date);

            var pageAndFiltered = filtered.OrderBy(s => s.DateIn);

            var query = from o in pageAndFiltered
                        select new PlnCcrProductionPlanDto
                        {
                            Id = o.Id,
                            PlanSequence = o.PlanSequence,
                            Shop = o.Shop,
                            Model = o.Model,
                            LotNo = o.LotNo,
                            NoInLot = o.NoInLot,
                            Grade = o.Grade,
                            Body = o.Body,
                            DateIn = o.DateIn,
                            TimeIn = o.TimeIn,
                            DateTimeIn = o.DateTimeIn,
                            SupplierNo = o.SupplierNo,
                            UseLotNo = o.UseLotNo,
                            SupplierNo2 = o.SupplierNo2,
                            UseLotNo2 = o.UseLotNo2,
                            UseNoInLot = o.UseNoInLot,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        //import

        public void ImportPlnCcrProductionPlanFMKFromExcel(List<ImportPlnCcrProductionPlanDto> plnCcrProductionPlanFmks)
        {
            try
            {
                List<PlnCcrProductionPlan_T> plnCcrProductionPlan = new List<PlnCcrProductionPlan_T> { };
                foreach (var item in plnCcrProductionPlanFmks)
                {
                    PlnCcrProductionPlan_T importDataFmkT = new PlnCcrProductionPlan_T();
                    {
                        importDataFmkT.Guid = item.Guid;
                        importDataFmkT.PlanSequence = item.PlanSequence;
                        importDataFmkT.Shop = item.Shop;
                        importDataFmkT.LotNo = item.LotNo;
                        importDataFmkT.NoInLot = item.NoInLot;
                        importDataFmkT.Model = item.Model;
                        importDataFmkT.Body = item.Body;
                        importDataFmkT.SupplierNo = item.SupplierNo;
                        importDataFmkT.UseLotNo = item.UseLotNo;
                    }
                    plnCcrProductionPlan.Add(importDataFmkT);
                }
                CurrentUnitOfWork.GetDbContext<prodDbContext>().AddRange(plnCcrProductionPlan);
                CurrentUnitOfWork.SaveChanges();
                
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }

        public void ImportPlnCcrProductionPlanFPFromExcel(List<ImportPlnCcrProductionPlanDto> plnCcrProductionPlanFps)
        {
            try
            {
                List<PlnCcrProductionPlan_T> plnCcrProductionPlanFp = new List<PlnCcrProductionPlan_T> { };
                foreach (var item in plnCcrProductionPlanFps)
                {
                    PlnCcrProductionPlan_T importDataFpT = new PlnCcrProductionPlan_T();
                    {
                        importDataFpT.Guid = item.Guid;
                        importDataFpT.PlanSequence = 0;
                        importDataFpT.LotNo = item.LotNo;
                        importDataFpT.NoInLot = item.NoInLot;
                        importDataFpT.Grade = item.Grade;
                        importDataFpT.Body = item.Body;
                        importDataFpT.DateIn = item.DateIn;
                    }
                    plnCcrProductionPlanFp.Add(importDataFpT);
                }
                CurrentUnitOfWork.GetDbContext<prodDbContext>().AddRange(plnCcrProductionPlanFp);
                CurrentUnitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }

        public void ImportPlnCcrProductionPlanW1FromExcel(List<ImportPlnCcrProductionPlanDto> plnCcrProductionPlanW1s)
        {
            try
            {
                List<PlnCcrProductionPlan_T> plnCcrProductionPlanW1 = new List<PlnCcrProductionPlan_T> { };
                foreach (var item in plnCcrProductionPlanW1s)
                {
                    PlnCcrProductionPlan_T importDataW1T = new PlnCcrProductionPlan_T();
                    {
                        importDataW1T.Guid = item.Guid;
                        importDataW1T.PlanSequence = item.PlanSequence;
                        importDataW1T.Shop = item.Shop;
                        importDataW1T.Model = item.Model;
                        importDataW1T.LotNo = item.LotNo;
                        importDataW1T.NoInLot = item.NoInLot;
                        importDataW1T.Grade = item.Grade;
                        importDataW1T.Body = item.Body;
                        importDataW1T.DateIn = item.DateIn;
                        importDataW1T.TimeIn = item.TimeIn;
                        // importDataW1T.SupplierNo = item.SupplierNo;
                        importDataW1T.SupplierNo2 = item.SupplierNo2;
                        //  importDataW1T.UseLotNo = item.UseLotNo;
                        importDataW1T.UseLotNo2 = item.UseLotNo2;
                    }
                    plnCcrProductionPlanW1.Add(importDataW1T);
                }
                CurrentUnitOfWork.GetDbContext<prodDbContext>().AddRange(plnCcrProductionPlanW1);
                CurrentUnitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }

        public void ImportPlnCcrProductionPlanW2FromExcel(List<ImportPlnCcrProductionPlanDto> plnCcrProductionPlanW2s)
        {
            try
            {
                List<PlnCcrProductionPlan_T> plnCcrProductionPlanW2 = new List<PlnCcrProductionPlan_T> { };
                foreach (var item in plnCcrProductionPlanW2s)
                {
                    PlnCcrProductionPlan_T importDataW2T = new PlnCcrProductionPlan_T();
                    {
                        importDataW2T.Guid = item.Guid;
                        importDataW2T.PlanSequence = item.PlanSequence;
                        importDataW2T.Shop = item.Shop;
                        importDataW2T.Model = item.Model;
                        importDataW2T.LotNo = item.LotNo;
                        importDataW2T.NoInLot = item.NoInLot;
                        importDataW2T.Grade = item.Grade;
                        importDataW2T.Body = item.Body;
                        importDataW2T.DateIn = item.DateIn;
                        importDataW2T.TimeIn = item.TimeIn;
                        importDataW2T.SupplierNo = item.SupplierNo;
                        // importDataW2T.SupplierNo2 = item.SupplierNo2;
                        importDataW2T.UseLotNo = item.UseLotNo;
                        //importDataW2T.UseLotNo2 = item.UseLotNo2;
                    }
                    plnCcrProductionPlanW2.Add(importDataW2T);
                }
                CurrentUnitOfWork.GetDbContext<prodDbContext>().AddRange(plnCcrProductionPlanW2);
                CurrentUnitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }

        public void ImportPlnCcrProductionPlanW3FromExcel(List<ImportPlnCcrProductionPlanDto> plnCcrProductionPlanW3s)
        {
            try
            {
                List<PlnCcrProductionPlan_T> plnCcrProductionPlanW3 = new List<PlnCcrProductionPlan_T> { };
                foreach (var item in plnCcrProductionPlanW3s)
                {
                    PlnCcrProductionPlan_T importDataW3T = new PlnCcrProductionPlan_T();
                    {
                        importDataW3T.Guid = item.Guid;
                        importDataW3T.PlanSequence = item.PlanSequence;
                        importDataW3T.Shop = item.Shop;
                        importDataW3T.Model = item.Model;
                        importDataW3T.LotNo = item.LotNo;
                        importDataW3T.NoInLot = item.NoInLot;
                        importDataW3T.Grade = item.Grade;
                        importDataW3T.Body = item.Body;
                        importDataW3T.DateIn = item.DateIn;
                        importDataW3T.TimeIn = item.TimeIn;
                        importDataW3T.SupplierNo = item.SupplierNo;
                        // importDataW3T.SupplierNo2 = item.SupplierNo2;
                        importDataW3T.UseLotNo = item.UseLotNo;
                        //importDataW3T.UseLotNo2 = item.UseLotNo2;
                    }
                    plnCcrProductionPlanW3.Add(importDataW3T);
                }
                CurrentUnitOfWork.GetDbContext<prodDbContext>().AddRange(plnCcrProductionPlanW3);
                CurrentUnitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }

        public void ImportPlnCcrProductionPlanA1FromExcel(List<ImportPlnCcrProductionPlanDto> plnCcrProductionPlanA1s)
        {
            try
            {
                List<PlnCcrProductionPlan_T> plnCcrProductionPlanA1 = new List<PlnCcrProductionPlan_T> { };
                foreach (var item in plnCcrProductionPlanA1s)
                {
                    PlnCcrProductionPlan_T importDataA1T = new PlnCcrProductionPlan_T();
                    {
                        importDataA1T.Guid = item.Guid;
                        importDataA1T.PlanSequence = item.PlanSequence;
                        importDataA1T.Shop = item.Shop;
                        importDataA1T.Model = item.Model;
                        importDataA1T.LotNo = item.LotNo;
                        importDataA1T.NoInLot = item.NoInLot;
                        importDataA1T.Grade = item.Grade;
                        importDataA1T.Body = item.Body;
                        importDataA1T.DateIn = item.DateIn;
                        importDataA1T.TimeIn = item.TimeIn;
                        importDataA1T.SupplierNo = item.SupplierNo;
                        importDataA1T.SupplierNo2 = item.SupplierNo2;
                        importDataA1T.UseLotNo = item.UseLotNo;
                        importDataA1T.UseLotNo2 = item.UseLotNo2;
                    }
                    plnCcrProductionPlanA1.Add(importDataA1T);
                }
                CurrentUnitOfWork.GetDbContext<prodDbContext>().AddRange(plnCcrProductionPlanA1);
                CurrentUnitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }

        public void ImportPlnCcrProductionPlanA2FromExcel(List<ImportPlnCcrProductionPlanDto> plnCcrProductionPlanA2s)
        {
            try
            {
                List<PlnCcrProductionPlan_T> plnCcrProductionPlanA2 = new List<PlnCcrProductionPlan_T> { };
                foreach (var item in plnCcrProductionPlanA2s)
                {
                    PlnCcrProductionPlan_T importDataA2T = new PlnCcrProductionPlan_T();
                    {
                        importDataA2T.Guid = item.Guid;
                        importDataA2T.PlanSequence = item.PlanSequence;
                        importDataA2T.Shop = item.Shop;
                        importDataA2T.Model = item.Model;
                        importDataA2T.LotNo = item.LotNo;
                        importDataA2T.NoInLot = item.NoInLot;
                        importDataA2T.Grade = item.Grade;
                        importDataA2T.Body = item.Body;
                        importDataA2T.DateIn = item.DateIn;
                        importDataA2T.TimeIn = item.TimeIn;
                        importDataA2T.SupplierNo = item.SupplierNo;
                        importDataA2T.SupplierNo2 = item.SupplierNo2;
                        importDataA2T.UseLotNo = item.UseLotNo;
                        importDataA2T.UseLotNo2 = item.UseLotNo2;
                    }
                    plnCcrProductionPlanA2.Add(importDataA2T);
                }
                CurrentUnitOfWork.GetDbContext<prodDbContext>().AddRange(plnCcrProductionPlanA2);
                CurrentUnitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }

        //public async Task<List<ImportPlnCcrProductionPlanDto>> ImportPlnCcrProductionPlanFPFromExcel(List<ImportPlnCcrProductionPlanDto> plnCcrProductionPlanFps)
        //{
        //    try
        //    {
        //        List<PlnCcrProductionPlan_T> plnCcrProductionPlanFp = new List<PlnCcrProductionPlan_T> { };
        //        foreach (var item in plnCcrProductionPlanFps)
        //        {
        //            PlnCcrProductionPlan_T importDataFpT = new PlnCcrProductionPlan_T();
        //            {
        //                importDataFpT.Guid = item.Guid;
        //                importDataFpT.PlanSequence = 0;
        //                importDataFpT.LotNo = item.LotNo;
        //                importDataFpT.NoInLot = item.NoInLot;
        //                importDataFpT.Grade = item.Grade;
        //                importDataFpT.Body = item.Body;
        //                importDataFpT.DateIn = item.DateIn;
        //            }
        //            plnCcrProductionPlanFp.Add(importDataFpT);
        //        }
        //        CurrentUnitOfWork.GetDbContext<prodDbContext>().AddRange(plnCcrProductionPlanFp);
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(400, ex.Message);
        //    }
        //}

        //public async Task<List<ImportPlnCcrProductionPlanDto>> ImportPlnCcrProductionPlanW1FromExcel(List<ImportPlnCcrProductionPlanDto> plnCcrProductionPlanW1s)
        //{
        //    try
        //    {
        //        List<PlnCcrProductionPlan_T> plnCcrProductionPlanW1 = new List<PlnCcrProductionPlan_T> { };
        //        foreach (var item in plnCcrProductionPlanW1s)
        //        {
        //            PlnCcrProductionPlan_T importDataW1T = new PlnCcrProductionPlan_T();
        //            {
        //                importDataW1T.Guid = item.Guid;
        //                importDataW1T.PlanSequence = item.PlanSequence;
        //                importDataW1T.Shop = item.Shop;
        //                importDataW1T.Model = item.Model;
        //                importDataW1T.LotNo = item.LotNo;
        //                importDataW1T.NoInLot = item.NoInLot;
        //                importDataW1T.Grade = item.Grade;
        //                importDataW1T.Body = item.Body;
        //                importDataW1T.DateIn = item.DateIn;
        //                importDataW1T.TimeIn = item.TimeIn;
        //               // importDataW1T.SupplierNo = item.SupplierNo;
        //                importDataW1T.SupplierNo2 = item.SupplierNo2;
        //              //  importDataW1T.UseLotNo = item.UseLotNo;
        //                importDataW1T.UseLotNo2 = item.UseLotNo2;
        //            }
        //            plnCcrProductionPlanW1.Add(importDataW1T);
        //        }
        //        CurrentUnitOfWork.GetDbContext<prodDbContext>().AddRange(plnCcrProductionPlanW1);
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(400, ex.Message);
        //    }
        //}

        //public async Task<List<ImportPlnCcrProductionPlanDto>> ImportPlnCcrProductionPlanW2FromExcel(List<ImportPlnCcrProductionPlanDto> plnCcrProductionPlanW2s)
        //{
        //    try
        //    {
        //        List<PlnCcrProductionPlan_T> plnCcrProductionPlanW2 = new List<PlnCcrProductionPlan_T> { };
        //        foreach (var item in plnCcrProductionPlanW2s)
        //        {
        //            PlnCcrProductionPlan_T importDataW2T = new PlnCcrProductionPlan_T();
        //            {
        //                importDataW2T.Guid = item.Guid;
        //                importDataW2T.PlanSequence = item.PlanSequence;
        //                importDataW2T.Shop = item.Shop;
        //                importDataW2T.Model = item.Model;
        //                importDataW2T.LotNo = item.LotNo;
        //                importDataW2T.NoInLot = item.NoInLot;
        //                importDataW2T.Grade = item.Grade;
        //                importDataW2T.Body = item.Body;
        //                importDataW2T.DateIn = item.DateIn;
        //                importDataW2T.TimeIn = item.TimeIn;
        //                importDataW2T.SupplierNo = item.SupplierNo;
        //               // importDataW2T.SupplierNo2 = item.SupplierNo2;
        //                importDataW2T.UseLotNo = item.UseLotNo;
        //                //importDataW2T.UseLotNo2 = item.UseLotNo2;
        //            }
        //            plnCcrProductionPlanW2.Add(importDataW2T);
        //        }
        //        CurrentUnitOfWork.GetDbContext<prodDbContext>().AddRange(plnCcrProductionPlanW2);
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(400, ex.Message);
        //    }
        //}

        //public async Task<List<ImportPlnCcrProductionPlanDto>> ImportPlnCcrProductionPlanA1FromExcel(List<ImportPlnCcrProductionPlanDto> plnCcrProductionPlanA1s)
        //{
        //    try
        //    {
        //        List<PlnCcrProductionPlan_T> plnCcrProductionPlanA1 = new List<PlnCcrProductionPlan_T> { };
        //        foreach (var item in plnCcrProductionPlanA1s)
        //        {
        //            PlnCcrProductionPlan_T importDataA1T = new PlnCcrProductionPlan_T();
        //            {
        //                importDataA1T.Guid = item.Guid;
        //                importDataA1T.PlanSequence = item.PlanSequence;
        //                importDataA1T.Shop = item.Shop;
        //                importDataA1T.Model = item.Model;
        //                importDataA1T.LotNo = item.LotNo;
        //                importDataA1T.NoInLot = item.NoInLot;
        //                importDataA1T.Grade = item.Grade;
        //                importDataA1T.Body = item.Body;
        //                importDataA1T.DateIn = item.DateIn;
        //                importDataA1T.TimeIn = item.TimeIn;
        //                importDataA1T.SupplierNo = item.SupplierNo;
        //                importDataA1T.SupplierNo2 = item.SupplierNo2;
        //                importDataA1T.UseLotNo = item.UseLotNo;
        //                importDataA1T.UseLotNo2 = item.UseLotNo2;
        //            }
        //            plnCcrProductionPlanA1.Add(importDataA1T);
        //        }
        //        CurrentUnitOfWork.GetDbContext<prodDbContext>().AddRange(plnCcrProductionPlanA1);
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(400, ex.Message);
        //    }
        //}

        //public async Task<List<ImportPlnCcrProductionPlanDto>> ImportPlnCcrProductionPlanA2FromExcel(List<ImportPlnCcrProductionPlanDto> plnCcrProductionPlanA2s)
        //{
        //    try
        //    {
        //        List<PlnCcrProductionPlan_T> plnCcrProductionPlanA2 = new List<PlnCcrProductionPlan_T> { };
        //        foreach (var item in plnCcrProductionPlanA2s)
        //        {
        //            PlnCcrProductionPlan_T importDataA2T = new PlnCcrProductionPlan_T();
        //            {
        //                importDataA2T.Guid = item.Guid;
        //                importDataA2T.PlanSequence = item.PlanSequence;
        //                importDataA2T.Shop = item.Shop;
        //                importDataA2T.Model = item.Model;
        //                importDataA2T.LotNo = item.LotNo;
        //                importDataA2T.NoInLot = item.NoInLot;
        //                importDataA2T.Grade = item.Grade;
        //                importDataA2T.Body = item.Body;
        //                importDataA2T.DateIn = item.DateIn;
        //                importDataA2T.TimeIn = item.TimeIn;
        //                importDataA2T.SupplierNo = item.SupplierNo;
        //                importDataA2T.SupplierNo2 = item.SupplierNo2;
        //                importDataA2T.UseLotNo = item.UseLotNo;
        //                importDataA2T.UseLotNo2 = item.UseLotNo2;
        //            }
        //            plnCcrProductionPlanA2.Add(importDataA2T);
        //        }
        //        CurrentUnitOfWork.GetDbContext<prodDbContext>().AddRange(plnCcrProductionPlanA2);
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(400, ex.Message);
        //    }
        //}

        public async Task MergeDataPlnCcrProductionPlan(string v_guid)
        {
            string _sql = "Exec PLAN_CCR_PRODUCTIONPLAN_MERGE @Guid";
            await _dapperRepo.QueryAsync<PlnCcrProductionPlan>(_sql, new { Guid = v_guid });
        }


    }
}
