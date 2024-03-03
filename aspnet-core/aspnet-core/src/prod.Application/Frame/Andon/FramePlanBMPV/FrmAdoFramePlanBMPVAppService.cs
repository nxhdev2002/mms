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
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NUglify.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Frame.Andon.Dto;
using prod.Frame.Andon.Exporting;
using prod.Master.LogA.Dto;

namespace prod.Frame.Andon
{
    //  [AbpAuthorize(AppPermissions.Pages_Frame_Andon_FramePlanBMPV)]
    public class FrmAdoFramePlanBMPVAppService : prodAppServiceBase, IFrmAdoFramePlanBMPVAppService
    {
        private readonly IDapperRepository<FrmAdoFramePlanBMPV, long> _dapperRepo;
        private readonly IRepository<FrmAdoFramePlanBMPV, long> _repo;
        private readonly IRepository<FrmAdoFramePlanBMPV_T, long> _repo_bmpv_t;
        private readonly IFrmAdoFramePlanBMPVExcelExporter _calendarListExcelExporter;

        public FrmAdoFramePlanBMPVAppService(IRepository<FrmAdoFramePlanBMPV, long> repo,
                                         IRepository<FrmAdoFramePlanBMPV_T, long> repo_bmpv_t,
                                         IDapperRepository<FrmAdoFramePlanBMPV, long> dapperRepo,
                                        IFrmAdoFramePlanBMPVExcelExporter calendarListExcelExporter
            )
        {
            _repo_bmpv_t = repo_bmpv_t;
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Frame_Andon_FramePlanBMPV_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<FrmAdoFramePlanBMPVDto>> GetAll(GetFrmAdoFramePlanBMPVInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;
            var v_PlanMonth = dateTime.ToString("yyyy") + "-" + dateTime.ToString("MM");
            var filtered = await (_repo.GetAll()

                .WhereIf(string.IsNullOrWhiteSpace(input.PlanMonth), e => e.PlanMonth.Contains(v_PlanMonth))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PlanMonth), e => e.PlanMonth.Contains(input.PlanMonth.Substring(0, 4) + "-" + input.PlanMonth.Substring(5, 2)))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Model), e => e.Model.Contains(input.Model))
                .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.BodyNo), e => e.BodyNo.Contains(input.BodyNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.VinNo), e => e.VinNo.Contains(input.VinNo))).ToListAsync();

            var pageAndFiltered = filtered.OrderBy(s => s.No).ThenBy(s => s.PlanMonth);

            var system = from o in pageAndFiltered
                         select new FrmAdoFramePlanBMPVDto
                         {
                             Id = o.Id,
                             No = o.No,
                             Model = o.Model,
                             LotNo = o.LotNo,
                             NoInLot = o.NoInLot,
                             BodyNo = o.BodyNo,
                             Color = o.Color,
                             VinNo = o.VinNo,
                             FrameId = o.FrameId,
                             Status = o.Status,
                             PlanMonth = o.PlanMonth,
                             PlanDate = o.PlanDate,
                             Grade = o.Grade,
                             Version = o.Version,
                             IsActive = o.IsActive,
                         };

            var totalCount = filtered.Count();
            var paged = system.Skip(input.SkipCount).Take(input.MaxResultCount);

            return new PagedResultDto<FrmAdoFramePlanBMPVDto>(
                totalCount,
                 paged.ToList()
            );
        }

        public async Task<FileDto> GetFramePlanBMPVToExcel(GetFrmAdoFramePlanBMPVExportInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;
            var v_PlanMonth = dateTime.ToString("yyyy") + "-" + dateTime.ToString("MM");
            var filtered = _repo.GetAll()

                .WhereIf(string.IsNullOrWhiteSpace(input.PlanMonth), e => e.PlanMonth.Contains(v_PlanMonth))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PlanMonth), e => e.PlanMonth.Contains(input.PlanMonth.Substring(0, 4) + "-" + input.PlanMonth.Substring(5, 2)))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Model), e => e.Model.Contains(input.Model))
                .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.BodyNo), e => e.BodyNo.Contains(input.BodyNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.VinNo), e => e.VinNo.Contains(input.VinNo));

            var pageAndFiltered = filtered.OrderBy(s => s.No).ThenBy(s => s.PlanMonth);

            var query = from o in pageAndFiltered
                        .Where(s => s.IsDeleted == false)
                        select new FrmAdoFramePlanBMPVDto
                        {
                            Id = o.Id,
                            No = o.No,
                            Model = o.Model,
                            LotNo = o.LotNo,
                            NoInLot = o.NoInLot,
                            BodyNo = o.BodyNo,
                            Color = o.Color,
                            VinNo = o.VinNo,
                            FrameId = o.FrameId,
                            Status = o.Status,
                            PlanMonth = o.PlanMonth,
                            PlanDate = o.PlanDate,
                            Grade = o.Grade,
                            Version = o.Version,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        public async Task<List<ImportFrmAdoFramePlanBMPVDto>> ImportFramePlanBMPV(List<ImportFrmAdoFramePlanBMPVDto> framePlanBMPVs)
        {
            try
            {
                //get ver old
                var query = _repo.GetAll().AsNoTracking().OrderByDescending(p => p.No)
                      .Select(p => new FrmAdoFramePlanDto
                      {
                          Version = p.Version,
                      });
                int? version_New = 1;
                if (query.Count() != 0)
                {
                    var data = query.FirstOrDefault();
                    version_New = 1 + Int32.Parse(data.Version);

                }

                //
                List<FrmAdoFramePlanBMPV_T> frmAdoFramePlanBMPV = new List<FrmAdoFramePlanBMPV_T> { };
                foreach (var framePlanBMPV in framePlanBMPVs)
                {
                    DateTime date = (DateTime)framePlanBMPV.PlanDate;
                    var v_planDate = date.ToString("yyyy-MM-dd");
                    var v_planMonth = v_planDate.Substring(0, 4) + "-" + v_planDate.Substring(5, 2);
                    var grade = framePlanBMPV.LotNo.Substring(0, 2);
                    var frameId = framePlanBMPV.BodyNo;
                    FrmAdoFramePlanBMPV_T importData = new FrmAdoFramePlanBMPV_T();
                    {
                        importData.Guid = framePlanBMPV.Guid;
                        importData.No = framePlanBMPV.No;
                        importData.Model = framePlanBMPV.Model;
                        importData.LotNo = framePlanBMPV.LotNo;
                        importData.NoInLot = framePlanBMPV.NoInLot;
                        importData.BodyNo = framePlanBMPV.BodyNo;
                        importData.VinNo = framePlanBMPV.VinNo;
                        importData.FrameId = frameId;
                        importData.PlanMonth = v_planMonth;
                        importData.PlanDate = framePlanBMPV.PlanDate;
                        importData.Grade = grade;
                        importData.Version = version_New.ToString();
                        importData.IsActive = "Y";
                    }
                    frmAdoFramePlanBMPV.Add(importData);
                    // }
                }
                CurrentUnitOfWork.GetDbContext<prodDbContext>().AddRangeAsync(frmAdoFramePlanBMPV);

                return null;
            }

            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }


        //get messError Import
        public async Task<PagedResultDto<FrmAdoFramePlanBMPVDto>>GetMessageErrorImport(string v_Guid)
        {
            var data = await _dapperRepo.QueryAsync<FrmAdoFramePlanBMPVDto>("Exec FRM_ADO_FRAME_PLAN_BMPV_GET_LIST_ERROR_IMPORT @Guid", new { @Guid = v_Guid });
            var rsData = from o in data
                                  select new FrmAdoFramePlanBMPVDto
                                  {
                                      Id = o.Id,
                                      No = o.No,
                                      Model = o.Model,
                                      LotNo = o.LotNo,
                                      NoInLot = o.NoInLot,
                                      BodyNo = o.BodyNo,
                                      Color = o.Color,
                                      VinNo = o.VinNo,
                                      FrameId = o.FrameId,
                                      Status = o.Status,
                                      PlanMonth = o.PlanMonth,
                                      PlanDate = o.PlanDate,
                                      Grade = o.Grade,
                                      Version = o.Version,
                                      IsActive = o.IsActive,
                                      MessagesError = o.MessagesError
                                  };

            var totalCount = rsData.Count();
            return new PagedResultDto<FrmAdoFramePlanBMPVDto>(
                totalCount,
                 rsData.ToList()
            );
        }

        public async Task<FileDto> GetListErrFramePlanBMPVToExcel(string v_Guid)
        {
            var data = await _dapperRepo.QueryAsync<FrmAdoFramePlanBMPVDto>("Exec FRM_ADO_FRAME_PLAN_BMPV_GET_LIST_ERROR_IMPORT @Guid", new { @Guid = v_Guid });
            var rsData = from o in data
                                  select new FrmAdoFramePlanBMPVDto
                        {
                            Id = o.Id,
                            No = o.No,
                            Model = o.Model,
                            LotNo = o.LotNo,
                            NoInLot = o.NoInLot,
                            BodyNo = o.BodyNo,
                            Color = o.Color,
                            VinNo = o.VinNo,
                            FrameId = o.FrameId,
                            Status = o.Status,
                            PlanMonth = o.PlanMonth,
                            PlanDate = o.PlanDate,
                            Grade = o.Grade,
                            Version = o.Version,
                            IsActive = o.IsActive,
                            MessagesError = o.MessagesError
                        };
            var exportToExcel = rsData.ToList();
            return _calendarListExcelExporter.ExportToFileErr(exportToExcel);
        }

        //Merge Data From Temp To FramePlanBMPV
        public async Task MergeDataFramePlanBMPV(string v_Guid)
        {
            string _sql = "Exec FRM_ADO_FRAME_PLAN_BMPV_MERGE @Guid";
            await _dapperRepo.QueryAsync<FrmAdoFramePlanBMPV>(_sql, new { Guid = v_Guid });
        }
    }
}

