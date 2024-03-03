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
    //  [AbpAuthorize(AppPermissions.Pages_Frame_Andon_FramePlanA1)]
    public class FrmAdoFramePlanA1AppService : prodAppServiceBase, IFrmAdoFramePlanA1AppService
    {
        private readonly IDapperRepository<FrmAdoFramePlanA1, long> _dapperRepo;
        private readonly IRepository<FrmAdoFramePlanA1, long> _repo;
        private readonly IRepository<FrmAdoFramePlanA1_T, long> _repo_a1_t;
        private readonly IFrmAdoFramePlanA1ExcelExporter _calendarListExcelExporter;

        public FrmAdoFramePlanA1AppService(IRepository<FrmAdoFramePlanA1, long> repo,
                                         IRepository<FrmAdoFramePlanA1_T, long> repo_a1_t,
                                         IDapperRepository<FrmAdoFramePlanA1, long> dapperRepo,
                                        IFrmAdoFramePlanA1ExcelExporter calendarListExcelExporter
            )
        {
            _repo_a1_t = repo_a1_t;
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Frame_Andon_FramePlanA1_Edit)]
        public async Task CreateOrEdit(CreateOrEditFrmAdoFramePlanA1Dto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditFrmAdoFramePlanA1Dto input)
        {
            var mainObj = ObjectMapper.Map<FrmAdoFramePlanA1>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditFrmAdoFramePlanA1Dto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Frame_Andon_FramePlanA1_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<FrmAdoFramePlanA1Dto>> GetAll(GetFrmAdoFramePlanA1Input input)
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



            var pageAndFiltered = filtered.OrderBy(s => s.No).ThenBy(s => s.PlanMonth);


            var system = from o in pageAndFiltered
                         select new FrmAdoFramePlanA1Dto
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

            return new PagedResultDto<FrmAdoFramePlanA1Dto>(
                totalCount,
                 paged.ToList()
            );
        }

        public async Task<FileDto> GetFramePlanA1ToExcel(GetFrmAdoFramePlanA1ExportInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;
            var v_PlanMonth = dateTime.ToString("yyyy") + "-" + dateTime.ToString("MM");
            var filtered =_repo.GetAll()
                .WhereIf(string.IsNullOrWhiteSpace(input.PlanMonth), e => e.PlanMonth.Contains(v_PlanMonth))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PlanMonth), e => e.PlanMonth.Contains(input.PlanMonth.Substring(0, 4) + "-" + input.PlanMonth.Substring(5, 2)))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Model), e => e.Model.Contains(input.Model))
                .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.BodyNo), e => e.BodyNo.Contains(input.BodyNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.VinNo), e => e.VinNo.Contains(input.VinNo));

            var pageAndFiltered = filtered.OrderBy(s => s.No).ThenBy(s => s.PlanMonth);

            var query = from o in pageAndFiltered
                        .Where(s => s.IsDeleted == false)
                        select new FrmAdoFramePlanA1Dto
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

        public async Task<List<ImportFrmAdoFramePlanA1Dto>> ImportFramePlanA1_V2(List<ImportFrmAdoFramePlanA1Dto> framePlanA1s)
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
                List<FrmAdoFramePlanA1_T> frmAdoFramePlanA1 = new List<FrmAdoFramePlanA1_T> { };
                foreach (var framePlanA1 in framePlanA1s)
                {
                    DateTime date = (DateTime)framePlanA1.PlanDate;
                    var v_planDate = date.ToString("yyyy-MM-dd");
                    var v_planMonth = v_planDate.Substring(0, 4) + "-" + v_planDate.Substring(5, 2);
                    var grade = framePlanA1.LotNo.Substring(0, 2);
                    var frameId = framePlanA1.BodyNo;
                        FrmAdoFramePlanA1_T importData = new FrmAdoFramePlanA1_T();
                        {
                            importData.Guid = framePlanA1.Guid;
                            importData.No = framePlanA1.No;
                            importData.Model = framePlanA1.Model;
                            importData.LotNo = framePlanA1.LotNo;
                            importData.NoInLot = framePlanA1.NoInLot;
                            importData.BodyNo = framePlanA1.BodyNo;
                            importData.VinNo = framePlanA1.VinNo;
                            importData.FrameId = frameId;
                            importData.PlanMonth = v_planMonth;
                            importData.PlanDate = framePlanA1.PlanDate;
                            importData.Grade = grade;
                            importData.Version = version_New.ToString();
                            importData.IsActive = "Y";
                        }
                        frmAdoFramePlanA1.Add(importData);
                   // }
                }
                 CurrentUnitOfWork.GetDbContext<prodDbContext>().AddRangeAsync(frmAdoFramePlanA1);

                return null;
            }

            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }       
        }

        //Merge Data From Temp To FramePlanA1
        public async Task MergeDataFramePlanA1(string v_Guid)
        {
            string _sql = "Exec FRM_ADO_FRAME_PLAN_A1_MERGE @Guid";
            await _dapperRepo.QueryAsync<FrmAdoFramePlanA1>(_sql, new { Guid = v_Guid });
        }


        //get messError Frame Plan A1 Import
        public async Task<PagedResultDto<FrmAdoFramePlanA1Dto>> GetMessageErrorFramePlanA1Import(string v_Guid)
        {
            var data = await _dapperRepo.QueryAsync<FrmAdoFramePlanA1Dto>("Exec FRM_ADO_FRAME_PLAN_A1_GET_LIST_ERROR_IMPORT @Guid", new { @Guid = v_Guid });
            var rsData = from o in data
                         select new FrmAdoFramePlanA1Dto
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
            return new PagedResultDto<FrmAdoFramePlanA1Dto>(
                totalCount,
                 rsData.ToList()
            );
        }

        public async Task<FileDto> GetListErrFramePlanA1ToExcel(string v_Guid)
        {
            var data = await _dapperRepo.QueryAsync<FrmAdoFramePlanA1Dto>("Exec FRM_ADO_FRAME_PLAN_A1_GET_LIST_ERROR_IMPORT @Guid", new { @Guid = v_Guid });
            var rsData = from o in data
                         select new FrmAdoFramePlanA1Dto
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
    }
}

