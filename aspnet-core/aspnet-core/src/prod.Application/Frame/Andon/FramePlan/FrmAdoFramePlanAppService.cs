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


namespace prod.Frame.Andon
{
    //  [AbpAuthorize(AppPermissions.Pages_Frame_Andon_FramePlan)]
    public class FrmAdoFramePlanAppService : prodAppServiceBase, IFrmAdoFramePlanAppService
    {
        private readonly IDapperRepository<FrmAdoFramePlan, long> _dapperRepo;
        private readonly IRepository<FrmAdoFramePlan, long> _repo;
        private readonly IRepository<FrmAdoFramePlan_T, long> _repo_t;
        private readonly IFrmAdoFramePlanExcelExporter _calendarListExcelExporter;

        public FrmAdoFramePlanAppService(IRepository<FrmAdoFramePlan, long> repo,
                                         IRepository<FrmAdoFramePlan_T, long> repo_t,
                                         IDapperRepository<FrmAdoFramePlan, long> dapperRepo,
                                         IFrmAdoFramePlanExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _repo_t = repo_t;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<FrmAdoFramePlanDto>> GetAll(GetFrmAdoFramePlanInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;
            var v_PlanMonth = dateTime.ToString("yyyy") + "-" + dateTime.ToString("MM");
            var filtered = await (_repo.GetAll()

                .WhereIf(string.IsNullOrWhiteSpace(input.PlanMonth), e => e.PlanMonth.Contains(v_PlanMonth))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PlanMonth), e => e.PlanMonth.Contains(input.PlanMonth))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Model), e => e.Model.Contains(input.Model))
                .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.BodyNo), e => e.BodyNo.Contains(input.BodyNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.VinNo), e => e.VinNo.Contains(input.VinNo))).ToListAsync();

            var pageAndFiltered = filtered.OrderBy(s =>  s.No).ThenBy(s => s.PlanMonth);


            var system = from o in pageAndFiltered 
                         select new FrmAdoFramePlanDto
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

            return new PagedResultDto<FrmAdoFramePlanDto>(
                totalCount,
                 paged.ToList()
            );
        }


        public async Task<FileDto> GetFramePlanToExcel(GetFrmAdoFramePlanExportInput input)
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
                        select new FrmAdoFramePlanDto
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

        public async Task<List<ImportFrmAdoFramePlanDto>> ImportFramePlan_V2(List<ImportFrmAdoFramePlanDto> framePlans)
        {
            try
            {
                //get no_old and version_old
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

                List<FrmAdoFramePlan_T> frmAdoFramePlan = new List<FrmAdoFramePlan_T> { };
                foreach (var framePlan in framePlans)
                {
                    DateTime date = (DateTime)framePlan.PlanDate;
                    var v_planDate = date.ToString("yyyy-MM-dd");
                    var v_planMonth = v_planDate.Substring(0, 4) + "-" + v_planDate.Substring(5, 2);
                    var grade = framePlan.LotNo.Substring(0, 2);
                    var frameId = framePlan.BodyNo;
                        FrmAdoFramePlan_T importData = new FrmAdoFramePlan_T();
                        {
                            importData.Guid = framePlan.Guid;
                            importData.No = framePlan.No;
                            importData.Model = framePlan.Model;
                            importData.LotNo = framePlan.LotNo;
                            importData.NoInLot = framePlan.NoInLot;
                            importData.BodyNo = framePlan.BodyNo;
                            importData.VinNo = framePlan.VinNo;
                            importData.FrameId = frameId;
                            importData.PlanMonth = v_planMonth;
                            importData.PlanDate = framePlan.PlanDate;
                            importData.Grade = grade;
                            importData.Version = version_New.ToString();
                            importData.IsActive = "Y";
                        }
                        frmAdoFramePlan.Add(importData);
                }
                CurrentUnitOfWork.GetDbContext<prodDbContext>().AddRangeAsync(frmAdoFramePlan);

                return null;
            }

            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }

        //Merge Data From Temp To FramePlanA1
        public async Task MergeDataFramePlan(string v_Guid)
        {
            string _sql = "Exec FRM_ADO_FRAME_PLAN_MERGE @Guid";
            await _dapperRepo.QueryAsync<FrmAdoFramePlan>(_sql, new { Guid = v_Guid });
        }

    }
}
