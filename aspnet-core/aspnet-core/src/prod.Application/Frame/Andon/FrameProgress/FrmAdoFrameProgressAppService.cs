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
using prod.Frame.Andon.Dto;
using prod.Frame.Andon.Exporting;

namespace prod.Frame.Andon
{
	//  [AbpAuthorize(AppPermissions.Pages_Frame_Andon_FrameProgress)]
	public class FrmAdoFrameProgressAppService : prodAppServiceBase, IFrmAdoFrameProgressAppService
	{
		private readonly IDapperRepository<FrmAdoFrameProgress, long> _dapperRepo;
		private readonly IRepository<FrmAdoFrameProgress, long> _repo;
		private readonly IFrmAdoFrameProgressExcelExporter _calendarListExcelExporter;

		public FrmAdoFrameProgressAppService(IRepository<FrmAdoFrameProgress, long> repo,
										 IDapperRepository<FrmAdoFrameProgress, long> dapperRepo,
										IFrmAdoFrameProgressExcelExporter calendarListExcelExporter
			)
		{
			_repo = repo;
			_dapperRepo = dapperRepo;
			_calendarListExcelExporter = calendarListExcelExporter;
		}

		

		public async Task<PagedResultDto<FrmAdoFrameProgressDto>> GetAll(GetFrmAdoFrameProgressInput input)
		{
            DateTime dateTime = DateTime.Now.Date;
            var filtered = _repo.GetAll()
                .WhereIf(!input.ScanTimeFrom.HasValue && !input.ScanTimeTo.HasValue, t => t.ScanTime.Value.Date == dateTime)
                .WhereIf(input.ScanTimeFrom.HasValue, t => t.ScanTime.Value.Date >= input.ScanTimeFrom.Value.Date)
                .WhereIf(input.ScanTimeTo.HasValue, t => input.ScanTimeTo.Value.Date >= t.ScanTime.Value.Date)
                .WhereIf(!string.IsNullOrWhiteSpace(input.BodyNo), e => e.BodyNo.Contains(input.BodyNo))
				.WhereIf(!string.IsNullOrWhiteSpace(input.VinNo), e => e.VinNo.Contains(input.VinNo))
				.WhereIf(!string.IsNullOrWhiteSpace(input.FrameNo), e => e.FrameNo.Contains(input.FrameNo))
				.WhereIf(!string.IsNullOrWhiteSpace(input.Model), e => e.Model.Contains(input.Model))
				.WhereIf(!string.IsNullOrWhiteSpace(input.Grade), e => e.Grade.Contains(input.Grade))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ScanLocation), e => e.ScanLocation.Contains(input.ScanLocation))
                .WhereIf((input.ProcessGroup != 0), e => e.ProcessGroup == input.ProcessGroup);

            var pageAndFiltered = filtered.OrderByDescending(s => s.ScanTime).ThenByDescending(a => a.ScanLocation);


			var system = from o in pageAndFiltered
						 select new FrmAdoFrameProgressDto
						 {
							 Id = o.Id,
							 ScanningId = o.ScanningId,
							 BodyNo = o.BodyNo,
							 Color = o.Color,
							 ScanTypeCd = o.ScanTypeCd,
							 ScanLocation = o.ScanLocation,
							 ScanTime = o.ScanTime,
							 ScanValue = o.ScanValue,
							 Mode = o.Mode,
							 ProcessGroup = o.ProcessGroup,
							 ProcessSubgroup = o.ProcessSubgroup,
							 VinNo = o.VinNo,
							 FrameNo = o.FrameNo,
							 Model = o.Model,
							 Grade = o.Grade,
							 LotNo = o.LotNo,
							 NoInLot = o.NoInLot,
							 SequenceNo = o.SequenceNo,
							 Location = o.Location,
							 AndonTransfer = o.AndonTransfer,
							 RescanBodyNo = o.RescanBodyNo,
							 RescanLotNo = o.RescanLotNo,
							 RescanMode = o.RescanMode,
							 ErrorCd = o.ErrorCd,
							 IsRescan = o.IsRescan,
							 IsActive = o.IsActive,
						 };

			var totalCount = await filtered.CountAsync();
			var paged = system.PageBy(input);

			return new PagedResultDto<FrmAdoFrameProgressDto>(
				totalCount,
				 await paged.ToListAsync()
			);
		}


		public async Task<FileDto> GetFrameProgressToExcel(GetFrmAdoFrameProgressExportInput input)
		{
            DateTime dateTime = DateTime.Now.Date;
            var filtered = _repo.GetAll()
                .WhereIf(!input.ScanTimeFrom.HasValue && !input.ScanTimeTo.HasValue, t => t.ScanTime.Value.Date == dateTime)
                .WhereIf(input.ScanTimeFrom.HasValue, t => t.ScanTime.Value.Date >= input.ScanTimeFrom.Value.Date)
                .WhereIf(input.ScanTimeTo.HasValue, t => input.ScanTimeTo.Value.Date >= t.ScanTime.Value.Date)
				.WhereIf(!string.IsNullOrWhiteSpace(input.BodyNo), e => e.BodyNo.Contains(input.BodyNo))
				.WhereIf(!string.IsNullOrWhiteSpace(input.VinNo), e => e.VinNo.Contains(input.VinNo))
				.WhereIf(!string.IsNullOrWhiteSpace(input.FrameNo), e => e.FrameNo.Contains(input.FrameNo))
				.WhereIf(!string.IsNullOrWhiteSpace(input.Model), e => e.Model.Contains(input.Model))
				.WhereIf(!string.IsNullOrWhiteSpace(input.Grade), e => e.Grade.Contains(input.Grade))
				.WhereIf(!string.IsNullOrWhiteSpace(input.ScanLocation), e => e.ScanLocation.Contains(input.ScanLocation))
				.WhereIf((input.ProcessGroup != 0), e => e.ProcessGroup == input.ProcessGroup);

            var pageAndFiltered = filtered.OrderByDescending(s => s.ScanTime);

            var query = from o in pageAndFiltered

                        select new FrmAdoFrameProgressDto
						{
							Id = o.Id,
							ScanningId = o.ScanningId,
							BodyNo = o.BodyNo,
							Color = o.Color,
							ScanTypeCd = o.ScanTypeCd,
							ScanLocation = o.ScanLocation,
							ScanTime = o.ScanTime,
							ScanValue = o.ScanValue,
							Mode = o.Mode,
							ProcessGroup = o.ProcessGroup,
							ProcessSubgroup = o.ProcessSubgroup,
							VinNo = o.VinNo,
							FrameNo = o.FrameNo,
							Model = o.Model,
							Grade = o.Grade,
							LotNo = o.LotNo,
							NoInLot = o.NoInLot,
							SequenceNo = o.SequenceNo,
							Location = o.Location,
							AndonTransfer = o.AndonTransfer,
							RescanBodyNo = o.RescanBodyNo,
							RescanLotNo = o.RescanLotNo,
							RescanMode = o.RescanMode,
							ErrorCd = o.ErrorCd,
							IsRescan = o.IsRescan,
							IsActive = o.IsActive,
						};
			var exportToExcel = await query.ToListAsync();
			return _calendarListExcelExporter.ExportToFile(exportToExcel);
		}

	}
}
