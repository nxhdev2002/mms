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
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Master.Painting;
using prod.Painting.Andon.Dto;
using prod.Painting.Andon.Exporting;

namespace prod.Painting.Andon
{
      [AbpAuthorize(AppPermissions.Pages_ProdPlan_PaintingData_View)]
    public class PtsAdoPaintingDataAppService : prodAppServiceBase, IPtsAdoPaintingDataAppService
    {
        private readonly IDapperRepository<PtsAdoPaintingData, long> _dapperRepoBumperIn;
        private readonly IDapperRepository<PtsAdoPaintingData, long> _dapperRepo;
        private readonly IRepository<PtsAdoPaintingData, long> _repo;
        private readonly IRepository<MstPtsBmpPartType, long> _repoPartType;
        private readonly IPtsAdoPaintingDataExcelExporter _calendarListExcelExporter;

        public PtsAdoPaintingDataAppService(IRepository<PtsAdoPaintingData, long> repo,
                                         IDapperRepository<PtsAdoPaintingData, long> dapperRepo,
                                            IDapperRepository<PtsAdoPaintingData, long> dapperRepoBumperIn,
                                        IPtsAdoPaintingDataExcelExporter calendarListExcelExporter
            )
        {
            _dapperRepoBumperIn = dapperRepoBumperIn;
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<PtsAdoPaintingDataDto>> GetAll(GetPtsAdoPaintingDataInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;

            var filtered = _repo.GetAll()
                .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Model), e => e.Model.Contains(input.Model))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Cfc), e => e.Cfc.Contains(input.Cfc))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Grade), e => e.Grade.Contains(input.Grade))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Shift), e => e.ProdLine.Contains(input.Shift))
                .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom <= t.WorkingDate)
                .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo >= t.WorkingDate);

            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate).ThenBy(a => a.ProdLine).ThenBy(n => n.Shift).ThenBy(m => m.NoInLot);

            var system = from o in pageAndFiltered
                         select new PtsAdoPaintingDataDto
                         {
                             Id = o.Id,
                             LifetimeNo = o.LifetimeNo,
                             Model = o.Model,
                             Cfc = o.Cfc,
                             Grade = o.Grade,
                             LotNo = o.LotNo,
                             NoInLot = o.NoInLot,
                             BodyNo = o.BodyNo,
                             Color = o.Color,
                             ProdLine = o.ProdLine,
                             ProcessGroup = o.ProcessGroup,
                             SubGroup = o.SubGroup,
                             ProcessSeq = o.ProcessSeq,
                             Filler = o.Filler,
                             WorkingDate = o.WorkingDate,
                             Shift = o.Shift,
                             NoInDate = o.NoInDate,
                             ProcessCode = o.ProcessCode,
                             InfoProcess = o.InfoProcess,
                             InfoProcessNo = o.InfoProcessNo,
                             IsProject = o.IsProject,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<PtsAdoPaintingDataDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetPaintingDataToExcel(PtsAdoPaintingDataExportInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;

            var filtered = _repo.GetAll()
                .WhereIf(!input.WorkingDateFrom.HasValue && !input.WorkingDateTo.HasValue, t => t.WorkingDate == dateTime)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Model), e => e.Model.Contains(input.Model))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Cfc), e => e.Cfc.Contains(input.Cfc))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Grade), e => e.Grade.Contains(input.Grade))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Shift), e => e.ProdLine.Contains(input.Shift))
                .WhereIf(input.WorkingDateFrom.HasValue, t => input.WorkingDateFrom <= t.WorkingDate)
                .WhereIf(input.WorkingDateTo.HasValue, t => input.WorkingDateTo >= t.WorkingDate);

            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate).ThenBy(a => a.ProdLine).ThenBy(n => n.Shift).ThenBy(m => m.NoInDate);


            var query = from o in pageAndFiltered
                        select new PtsAdoPaintingDataDto
                        {
                            Id = o.Id,
                            LifetimeNo = o.LifetimeNo,
                            Model = o.Model,
                            Cfc = o.Cfc,
                            Grade = o.Grade,
                            LotNo = o.LotNo,
                            NoInLot = o.NoInLot,
                            BodyNo = o.BodyNo,
                            Color = o.Color,
                            ProdLine = o.ProdLine,
                            ProcessGroup = o.ProcessGroup,
                            SubGroup = o.SubGroup,
                            ProcessSeq = o.ProcessSeq,
                            Filler = o.Filler,
                            WorkingDate = o.WorkingDate,
                            Shift = o.Shift,
                            NoInDate = o.NoInDate,
                            ProcessCode = o.ProcessCode,
                            InfoProcess = o.InfoProcess,
                            InfoProcessNo = o.InfoProcessNo,
                            IsProject = o.IsProject,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }
        public async Task<List<PtsAdoBumperGetDataBumperInDto>> GetBumperGetdataBumperIn(string prod_line,string process)
        {

            string _sql = "Exec PTS_BMP_BUMPER_PROGRESS_GET_DATA @prod_line, @process";

            var filtered = await _dapperRepoBumperIn.QueryAsync<PtsAdoBumperGetDataBumperInDto>(_sql, new
            {
                prod_line = prod_line,
                process = process,
            }); 

            return filtered.ToList();
        }

        public async Task<List<PtsBmpPartTypeDto>> GetPartTypeBumperIn(string prod_line, string process)
        {
            string _sqlpt = "Exec PTS_BMP_BUMPER_PROGRES_GET_PARTTYPE @prod_line, @process";

            var listPartType = await _dapperRepoBumperIn.QueryAsync<PtsBmpPartTypeDto>(_sqlpt, new
            {
                prod_line = prod_line,
                process = process,
            });

            return listPartType.ToList();

        }

        public async Task<List<PtsAdoBumperGetDataBumperInDto>> ConfirmPartBumperIn(string progress_id)
        {

            string _sql = "Exec PTS_BMP_BUMPER_PROGRESS_CONFIRM @progress_id";

            var filtered = await _dapperRepoBumperIn.QueryAsync<PtsAdoBumperGetDataBumperInDto>(_sql, new
            {
                progress_id = progress_id
            });

            return filtered.ToList();
        }

        public async Task<List<PtsAdoBumperGetDataBumperInDto>> UpdateStatusBumperIn(string progress_id)
        {

            string _sql = "Exec PTS_BMP_BUMPER_PROGRESS_UPDATE_STATUS @progress_id";

            var filtered = await _dapperRepoBumperIn.QueryAsync<PtsAdoBumperGetDataBumperInDto>(_sql, new
            {
                progress_id = progress_id
            });

            return filtered.ToList();
        }

    }

}