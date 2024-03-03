using Abp.Application.Services;
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
using prod.Master.Common;
using prod.Painting.Andon.Dto;
using prod.Painting.Andon.Exporting;

namespace prod.Painting.Andon
{
    //  [AbpAuthorize(AppPermissions.Pages_Painting_Andon_Bumper)]
    public class PtsAdoBumperAppService : prodAppServiceBase, IPtsAdoBumperGetdataClrIndicatorAppService
    {
        private readonly IDapperRepository<PtsAdoBumperGetdataClrIndicator, long> _dapperRepoClr;
        private readonly IDapperRepository<PtsAdoBumper, long> _dapperRepo;
        private readonly IRepository<PtsAdoBumper, long> _repo;
        private readonly IPtsAdoBumperExcelExporter _calendarListExcelExporter;

        public PtsAdoBumperAppService(IRepository<PtsAdoBumper, long> repo,
                                         IDapperRepository<PtsAdoBumper, long> dapperRepo,
                                         IDapperRepository<PtsAdoBumperGetdataClrIndicator, long> dapperRepoClr,
                                        IPtsAdoBumperExcelExporter calendarListExcelExporter
            )
        {
            _dapperRepoClr = dapperRepoClr;
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<PtsAdoBumperDto>> GetAll(GetPtsAdoBumperInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Model), e => e.Model.Contains(input.Model))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Grade), e => e.Grade.Contains(input.Grade))
                .WhereIf(!string.IsNullOrWhiteSpace(input.BodyNo), e => e.BodyNo.Contains(input.BodyNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo))      
                ;
            var pageAndFiltered = filtered.OrderByDescending(s => s.InitialDate);


            var system = from o in pageAndFiltered
                         select new PtsAdoBumperDto
                         {
                             Id = o.Id,
                             WipId = o.WipId,
                             Model = o.Model,
                             Grade = o.Grade,
                             BodyNo = o.BodyNo,
                             LotNo = o.LotNo,
                             NoInLot = o.NoInLot,
                             Color = o.Color,
                             BumperStatus = o.BumperStatus,
                             InitialDate = o.InitialDate,
                             I1Date = o.I1Date,
                             I2Date = o.I2Date,
                             InlInDate = o.InlInDate,
                             InlOutDate = o.InlOutDate,
                             PreparationDate = o.PreparationDate,
                             UnpackingDate = o.UnpackingDate,
                             JigSettingDate = o.JigSettingDate,
                             BoothDate = o.BoothDate,
                             FinishedDate = o.FinishedDate,
                             ExtSeq = o.ExtSeq,
                             UnpSeq = o.UnpSeq,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<PtsAdoBumperDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetBumperToExcel(GetPtsAdoBumperExportInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Model), e => e.Model.Contains(input.Model))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Grade), e => e.Grade.Contains(input.Grade))
                .WhereIf(!string.IsNullOrWhiteSpace(input.BodyNo), e => e.BodyNo.Contains(input.BodyNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo))
                ;
            var pageAndFiltered = filtered.OrderByDescending(s => s.InitialDate);


            var query = from o in pageAndFiltered
                         select new PtsAdoBumperDto
                        {
                            Id = o.Id,
                            WipId = o.WipId,
                            Model = o.Model,
                            Grade = o.Grade,
                            BodyNo = o.BodyNo,
                            LotNo = o.LotNo,
                            NoInLot = o.NoInLot,
                            Color = o.Color,
                            BumperStatus = o.BumperStatus,
                            InitialDate = o.InitialDate,
                            I1Date = o.I1Date,
                            I2Date = o.I2Date,
                            InlInDate = o.InlInDate,
                            InlOutDate = o.InlOutDate,
                            PreparationDate = o.PreparationDate,
                            UnpackingDate = o.UnpackingDate,
                            JigSettingDate = o.JigSettingDate,
                            BoothDate = o.BoothDate,
                            FinishedDate = o.FinishedDate,
                            ExtSeq = o.ExtSeq,
                            UnpSeq = o.UnpSeq,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }
        public async Task<PagedResultDto<PtsAdoBumperGetDataSmallSubassyDto>> GetBumperDataSmallSubassy()
        {
            string _sql = "Exec PTS_ADO_BUMPER_GETDATA_SMALL_SUBASSY";

            var filtered = await _dapperRepo.QueryAsync<PtsAdoBumperGetDataSmallSubassyDto>(_sql, new { });
            var results = from d in filtered
                          select new PtsAdoBumperGetDataSmallSubassyDto
                          {
                              Process = d.Process,
                              Grade = d.Grade,
                              Color = d.Color,
                              BodyNo = d.BodyNo,
                              CarSequence = d.CarSequence,
                              Model = d.Model,
                              Line = d.Line,
                              OrderDate = d.OrderDate,
                          };

            var totalCount = filtered.ToList().Count;

            return new PagedResultDto<PtsAdoBumperGetDataSmallSubassyDto>(
                totalCount,
                results.ToList()
            );
        }
        //bumper clr
        public async Task<PagedResultDto<PtsAdoBumperGetdataClrIndicatorDto>> GetBumperGetdataClrIndicator()
        {

            string _sql = "Exec PTS_ADO_BUMPER_GETDATA_CLR_INDICATOR ";

            var filtered = await _dapperRepoClr.QueryAsync<PtsAdoBumperGetdataClrIndicator>(_sql, new { });

            var results = from d in filtered
                          select new PtsAdoBumperGetdataClrIndicatorDto
                          {
                              BodyNo = d.BodyNo,
                              LotNo = d.LotNo,
                              Model = d.Model,
                              Grade = d.Grade,
                              Color = d.Color,
                              OrderDate = d.OrderDate,
                              Line = d.Line,
                              Process = d.Process,
                          };


            var totalCount = filtered.ToList().Count;

            return new PagedResultDto<PtsAdoBumperGetdataClrIndicatorDto>(
                totalCount,
                Enumerable.ToList<PtsAdoBumperGetdataClrIndicatorDto>(results)
            );
        }
      

    }



 

    }
