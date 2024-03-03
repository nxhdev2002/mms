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
using prod.Painting.Andon.Dto;
using prod.Painting.Andon.Exporting;

namespace prod.Painting.Andon
{
	//  [AbpAuthorize(AppPermissions.Pages_Painting_Andon_PaintingProgress)]
	public class PtsAdoPaintingProgressAppService : prodAppServiceBase, IPtsAdoPaintingProgressAppService
	{
		private readonly IDapperRepository<PtsAdoPaintingProgress, long> _dapperRepo;
		private readonly IRepository<PtsAdoPaintingProgress, long> _repo;
		private readonly IPtsAdoPaintingProgressExcelExporter _calendarListExcelExporter;

		public PtsAdoPaintingProgressAppService(IRepository<PtsAdoPaintingProgress, long> repo,
										 IDapperRepository<PtsAdoPaintingProgress, long> dapperRepo,
										IPtsAdoPaintingProgressExcelExporter calendarListExcelExporter
			)
		{
			_repo = repo;
			_dapperRepo = dapperRepo;
			_calendarListExcelExporter = calendarListExcelExporter;
		}

		//  [AbpAuthorize(AppPermissions.Pages_Painting_Andon_PaintingProgress_Edit)]
		public async Task CreateOrEdit(CreateOrEditPtsAdoPaintingProgressDto input)
		{
			if (input.Id == null) await Create(input);
			else await Update(input);
		}

		//CREATE
		private async Task Create(CreateOrEditPtsAdoPaintingProgressDto input)
		{
			var mainObj = ObjectMapper.Map<PtsAdoPaintingProgress>(input);

			await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
		}

		// EDIT
		private async Task Update(CreateOrEditPtsAdoPaintingProgressDto input)
		{
			using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
			{
				var mainObj = await _repo.GetAll()
				.FirstOrDefaultAsync(e => e.Id == input.Id);

				var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
			}
		}

		//  [AbpAuthorize(AppPermissions.Pages_Painting_Andon_PaintingProgress_Delete)]
		public async Task Delete(EntityDto input)
		{
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

		public async Task<PagedResultDto<PtsAdoPaintingProgressDto>> GetAll(GetPtsAdoPaintingProgressInput input)
		{
			var filtered = _repo.GetAll()
				.WhereIf(!string.IsNullOrWhiteSpace(input.BodyNo), e => e.BodyNo.Contains(input.BodyNo))				
				.WhereIf(!string.IsNullOrWhiteSpace(input.Model), e => e.Model.Contains(input.Model))
				.WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ScanLocation), e => e.ScanLocation.Contains(input.ScanLocation))
                .WhereIf((input.ProcessGroup != 0), e => e.ProcessGroup == input.ProcessGroup);

            var pageAndFiltered = filtered.OrderByDescending(s => s.ScanTime).ThenByDescending(a => a.ScanLocation);


            var system = from o in pageAndFiltered
						 select new PtsAdoPaintingProgressDto
						 {
							 Id = o.Id,
							 ScanningId = o.ScanningId,
							 BodyNo = o.BodyNo,
							 Color = o.Color,
							 ColorOrg = o.ColorOrg,
							 ScanTypeCd = o.ScanTypeCd,
							 ScanLocation = o.ScanLocation,
							 ScanTime = o.ScanTime,
							 ScanValue = o.ScanValue,
							 Mode = o.Mode,
							 ProcessGroup = o.ProcessGroup,
							 ProcessSubgroup = o.ProcessSubgroup,
							 ProcessSeq = o.ProcessSeq,
							 ConveyerStatus = o.ConveyerStatus,
							 LastConveyerRun = o.LastConveyerRun,
							 TcStatus = o.TcStatus,
							 Model = o.Model,
							 LotNo = o.LotNo,
							 NoInLot = o.NoInLot,
							 SequenceNo = o.SequenceNo,
							 DefectDesc = o.DefectDesc,
							 TargetRepair = o.TargetRepair,
							 Location = o.Location,
							 DuplicateLot = o.DuplicateLot,
							 WeldTransfer = o.WeldTransfer,
							 RescanBodyNo = o.RescanBodyNo,
							 RescanLotNo = o.RescanLotNo,
							 RescanMode = o.RescanMode,
							 ErrorCd = o.ErrorCd,
							 IsRescan = o.IsRescan,
							 IsPaintOut = o.IsPaintOut,
							 IsActive = o.IsActive,
						 };

			var totalCount = await filtered.CountAsync();
			var paged = system.PageBy(input);

			return new PagedResultDto<PtsAdoPaintingProgressDto>(
				totalCount,
				 await paged.ToListAsync()
			);
		}


		public async Task<FileDto> GetPaintingProgressToExcel(GetPtsAdoPaintingProgressExportInput input)
		{
            var filtered = _repo.GetAll()
            .WhereIf(!string.IsNullOrWhiteSpace(input.BodyNo), e => e.BodyNo.Contains(input.BodyNo))
            .WhereIf(!string.IsNullOrWhiteSpace(input.Model), e => e.Model.Contains(input.Model))
            .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo))
            .WhereIf(!string.IsNullOrWhiteSpace(input.ScanLocation), e => e.ScanLocation.Contains(input.ScanLocation))
            .WhereIf((input.ProcessGroup != 0), e => e.ProcessGroup == input.ProcessGroup);

            var pageAndFiltered = filtered.OrderByDescending(s => s.ScanTime).ThenByDescending(a => a.ScanLocation);

            var query = from o in pageAndFiltered
                         select new PtsAdoPaintingProgressDto
						{
							Id = o.Id, 
							ScanningId = o.ScanningId,
							BodyNo = o.BodyNo,
							Color = o.Color,
							ColorOrg = o.ColorOrg,
							ScanTypeCd = o.ScanTypeCd,
							ScanLocation = o.ScanLocation,
							ScanTime = o.ScanTime,
							ScanValue = o.ScanValue,
							Mode = o.Mode,
							ProcessGroup = o.ProcessGroup,
							ProcessSubgroup = o.ProcessSubgroup,
							ProcessSeq = o.ProcessSeq,
							ConveyerStatus = o.ConveyerStatus,
							LastConveyerRun = o.LastConveyerRun,
							TcStatus = o.TcStatus,
							Model = o.Model,
							LotNo = o.LotNo,
							NoInLot = o.NoInLot,
							SequenceNo = o.SequenceNo,
							DefectDesc = o.DefectDesc,
							TargetRepair = o.TargetRepair,
							Location = o.Location,
							DuplicateLot = o.DuplicateLot,
							WeldTransfer = o.WeldTransfer,
							RescanBodyNo = o.RescanBodyNo,
							RescanLotNo = o.RescanLotNo,
							RescanMode = o.RescanMode,
							ErrorCd = o.ErrorCd,
							IsRescan = o.IsRescan,
							IsPaintOut = o.IsPaintOut,
							IsActive = o.IsActive,
						};
			var exportToExcel = await query.ToListAsync();
			return _calendarListExcelExporter.ExportToFile(exportToExcel);
		}

		// public async Task GenerateAsync()
		//  {
		//    await _dapperRepo.ExecuteAsync(PtsAdoPaintingProgressConsts.SP_MST_WPT_CALENDAR_GENERATE);
		// }

	}
}
