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
using prod.Welding.Andon.Dto;
using prod.Welding.Andon.Exporting;
using Stripe.Terminal;

namespace prod.Welding.Andon
{
	  [AbpAuthorize(AppPermissions.Pages_ProdPlan_WeldingProgress_View)]
	public class WldAdoWeldingProgressAppService : prodAppServiceBase, IWldAdoWeldingProgressAppService
	{
		private readonly IDapperRepository<WldAdoWeldingProgress, long> _dapperRepo;
		private readonly IRepository<WldAdoWeldingProgress, long> _repo;
		private readonly IWldAdoWeldingProgressExcelExporter _calendarListExcelExporter;

		public WldAdoWeldingProgressAppService(IRepository<WldAdoWeldingProgress, long> repo,
										 IDapperRepository<WldAdoWeldingProgress, long> dapperRepo,
										IWldAdoWeldingProgressExcelExporter calendarListExcelExporter
			)
		{
			_repo = repo;
			_dapperRepo = dapperRepo;
			_calendarListExcelExporter = calendarListExcelExporter;
		}

		  [AbpAuthorize(AppPermissions.Pages_ProdPlan_WeldingProgress_Edit)]
		public async Task CreateOrEdit(CreateOrEditWldAdoWeldingProgressDto input)
		{
			if (input.Id == null) await Create(input);
			else await Update(input);
		}

		//CREATE
		private async Task Create(CreateOrEditWldAdoWeldingProgressDto input)
		{
			var mainObj = ObjectMapper.Map<WldAdoWeldingProgress>(input);

			await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
		}

		// EDIT
		private async Task Update(CreateOrEditWldAdoWeldingProgressDto input)
		{
			using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
			{
				var mainObj = await _repo.GetAll()
				.FirstOrDefaultAsync(e => e.Id == input.Id);

				var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
			}
		}

		  [AbpAuthorize(AppPermissions.Pages_ProdPlan_WeldingProgress_Edit)]
		public async Task Delete(EntityDto input)
		{
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

		public async Task<PagedResultDto<WldAdoWeldingProgressDto>> GetAll(GetWldAdoWeldingProgressInput input)
		{
            DateTime dateTime = DateTime.Now.Date;
            var filtered = _repo.GetAll()
                .WhereIf(!input.ScanTimeFrom.HasValue && !input.ScanTimeTo.HasValue, t => t.ScanTime.Value.Date == dateTime)
                .WhereIf(input.ScanTimeFrom.HasValue, t => t.ScanTime.Value.Date >= input.ScanTimeFrom.Value.Date)
                .WhereIf(input.ScanTimeTo.HasValue, t => input.ScanTimeTo.Value.Date >= t.ScanTime.Value.Date)
                .WhereIf(!string.IsNullOrWhiteSpace(input.BodyNo), e => e.BodyNo.Contains(input.BodyNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Model), e => e.Model.Contains(input.Model))
                .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo))
                .WhereIf((input.ProcessGroup == 211), e => e.ProcessGroup == input.ProcessGroup)
                .WhereIf((input.ProcessGroup < 211), e => e.ProcessGroup <= input.ProcessGroup);

            var pageAndFiltered = filtered.OrderByDescending(s => s.ScanTime).ThenByDescending(e => e.ScanLocation);


			var system = from o in pageAndFiltered
						 select new WldAdoWeldingProgressDto
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

			return new PagedResultDto<WldAdoWeldingProgressDto>(
				totalCount,
				 await paged.ToListAsync()
			);
		}


		public async Task<FileDto> GetWeldingProgressToExcel(GetWldAdoWeldingProgressExportInput input)
		{
            DateTime dateTime = DateTime.Now.Date;
            var filtered = _repo.GetAll()
                .WhereIf(!input.ScanTimeFrom.HasValue && !input.ScanTimeTo.HasValue, t => t.ScanTime.Value.Date == dateTime)
                .WhereIf(input.ScanTimeFrom.HasValue, t => t.ScanTime.Value.Date >= input.ScanTimeFrom.Value.Date)
                .WhereIf(input.ScanTimeTo.HasValue, t => input.ScanTimeTo.Value.Date >= t.ScanTime.Value.Date)
                .WhereIf(!string.IsNullOrWhiteSpace(input.BodyNo), e => e.BodyNo.Contains(input.BodyNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Model), e => e.Model.Contains(input.Model))
                .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo))
				.WhereIf((input.ProcessGroup == 211), e => e.ProcessGroup == input.ProcessGroup)
                .WhereIf((input.ProcessGroup < 211), e => e.ProcessGroup <= input.ProcessGroup);

            var pageAndFiltered = filtered.OrderByDescending(s => s.ScanTime).ThenByDescending(e => e.ScanLocation);

            var query = from o in pageAndFiltered
                        select new WldAdoWeldingProgressDto
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
	}
}
