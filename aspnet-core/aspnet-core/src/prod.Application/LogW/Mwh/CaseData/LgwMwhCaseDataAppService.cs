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
using prod.LogW.Mwh.Dto;
using prod.LogW.Mwh.Exporting;

namespace prod.LogW.Mwh
{
	//  [AbpAuthorize(AppPermissions.Pages_LogW_Mwh_CaseData)]
	public class LgwMwhCaseDataAppService : prodAppServiceBase, ILgwMwhCaseDataAppService
	{
		private readonly IDapperRepository<LgwMwhCaseData, long> _dapperRepo;
		private readonly IRepository<LgwMwhCaseData, long> _repo;
		private readonly ILgwMwhCaseDataExcelExporter _caseDataListExcelExporter;

		public LgwMwhCaseDataAppService(IRepository<LgwMwhCaseData, long> repo,
										 IDapperRepository<LgwMwhCaseData, long> dapperRepo,
										ILgwMwhCaseDataExcelExporter caseDataListExcelExporter
			)
		{
			_repo = repo;
			_dapperRepo = dapperRepo;
			_caseDataListExcelExporter = caseDataListExcelExporter;
		}

		public async Task<PagedResultDto<LgwMwhCaseDataDto>> GetAll(GetLgwMwhCaseDataInput input)
		{

            var filtered = _repo.GetAll()
                .WhereIf(input.DevanningDateFrom.HasValue, t => t.DevanningDate >= input.DevanningDateFrom)
                .WhereIf(input.DevanningDateTo.HasValue, t => t.DevanningDate <= input.DevanningDateTo)
                .WhereIf(!string.IsNullOrWhiteSpace(input.CaseNo), e => e.CaseNo.Contains(input.CaseNo))
				.WhereIf(!string.IsNullOrWhiteSpace(input.Renban), e => e.Renban.Contains(input.Renban))	
				.WhereIf(!string.IsNullOrWhiteSpace(input.ZoneCd), e => e.ZoneCd.Contains(input.ZoneCd))
				.WhereIf(!string.IsNullOrWhiteSpace(input.AreaCd), e => e.AreaCd.Contains(input.AreaCd))
				;
			var pageAndFiltered = filtered.OrderByDescending(s => s.DevanningDate);

			var system = from o in pageAndFiltered
						 select new LgwMwhCaseDataDto
						 {
							 Id = o.Id,
							 CaseNo = o.CaseNo,
							 LotNo = o.LotNo,
							 Grade = o.Grade,
							 Model = o.Model,
							 CaseQty = o.CaseQty,
							 ContainerNo = o.ContainerNo,
							 Renban = o.Renban,
							 SupplierNo = o.SupplierNo,
							 OrderType = o.OrderType,
							 CasePrefix = o.CasePrefix,
							 ProdLine = o.ProdLine,
							 PxpCaseId = o.PxpCaseId,
							 ContScheduleId = o.ContScheduleId,
							 Status = o.Status,
							 DevanningDate = o.DevanningDate,
							 StartDevanningDate = o.StartDevanningDate,
							 FinishDevanningDate = o.FinishDevanningDate,
							 ZoneCd = o.ZoneCd,
							 AreaCd = o.AreaCd,
							 LocId = o.LocId,
							 LocCd = o.LocCd,
							 LocDate = o.LocDate,
							 LocBy = o.LocBy,
							 Shop = o.Shop,
							 IsBigpart = o.IsBigpart,
							 IsActive = o.IsActive,
						 };

			var totalCount = await filtered.CountAsync();
			var paged = system.PageBy(input);

			return new PagedResultDto<LgwMwhCaseDataDto>(
				totalCount,
				 await paged.ToListAsync()
			);
		}

		public async Task<PagedResultDto<LgwMwhCaseDataDto>> GetCaseDataHisByCaseNo(GetLgwMwhCaseDataHisByCaseNoInput input)
		{
			var v_caseno = input.CaseNo;
			var v_fromdata = DateTime.Now;
			var v_ToDate = DateTime.Now;
			var svd = input.StartDevanningDate;

            if (svd != null)
            {
                v_caseno = input.CaseNo;
                v_fromdata = input.StartDevanningDate.Value;
                v_ToDate = input.FinishDevanningDate.Value;
            }

            string _sql = "Exec LOGW_MWH_CASE_DATA_HIST_GETBYCASENO @CaseNo,@FromData,@ToDate";
			var filtered = await _dapperRepo.QueryAsync<LgwMwhCaseDataDto>(_sql, new
			{
				CaseNo = v_caseno,
				FromData = v_fromdata,
				ToDate = v_ToDate
			});
			var pageAndFiltered = filtered.OrderBy(s => s.Id);
			var system = from o in pageAndFiltered
						 select new LgwMwhCaseDataDto
						 {
							 Id = o.Id,
							 CaseNo = o.CaseNo,							
							 ZoneCd = o.ZoneCd,
							 AreaCd = o.AreaCd,						
							 LocCd = o.LocCd,
							 LocDate = o.LocDate						
						 };

			var totalCount = filtered.ToList().Count;
			var paged = system.AsQueryable().PageBy(input);

			return new PagedResultDto<LgwMwhCaseDataDto>(
				totalCount,
				 paged.ToList()
			);
		}

		public async Task<FileDto> GetCaseDataToExcel(GetLgwMwhCaseDataInput input)
		{
            var filtered = _repo.GetAll()
             .WhereIf(input.DevanningDateFrom.HasValue, t => t.DevanningDate >= input.DevanningDateFrom)
             .WhereIf(input.DevanningDateTo.HasValue, t => t.DevanningDate <= input.DevanningDateTo)
             .WhereIf(!string.IsNullOrWhiteSpace(input.CaseNo), e => e.CaseNo.Contains(input.CaseNo))
             .WhereIf(!string.IsNullOrWhiteSpace(input.Renban), e => e.Renban.Contains(input.Renban))
             .WhereIf(!string.IsNullOrWhiteSpace(input.ZoneCd), e => e.ZoneCd.Contains(input.ZoneCd))
             .WhereIf(!string.IsNullOrWhiteSpace(input.AreaCd), e => e.AreaCd.Contains(input.AreaCd))
             ;
            var pageAndFiltered = filtered.OrderByDescending(s => s.DevanningDate);

            var query = from o in pageAndFiltered
                        select new LgwMwhCaseDataDto
						{
							Id = o.Id,
							CaseNo = o.CaseNo,
							LotNo = o.LotNo,
							Grade = o.Grade,
							Model = o.Model,
							CaseQty = o.CaseQty,
							ContainerNo = o.ContainerNo,
							Renban = o.Renban,
							SupplierNo = o.SupplierNo,
							OrderType = o.OrderType,
							CasePrefix = o.CasePrefix,
							ProdLine = o.ProdLine,
							PxpCaseId = o.PxpCaseId,
							ContScheduleId = o.ContScheduleId,
							Status = o.Status,
							DevanningDate = o.DevanningDate,
							StartDevanningDate = o.StartDevanningDate,
							FinishDevanningDate = o.FinishDevanningDate,
							ZoneCd = o.ZoneCd,
							AreaCd = o.AreaCd,
							LocId = o.LocId,
							LocCd = o.LocCd,
							LocDate = o.LocDate,
							LocBy = o.LocBy,
							Shop = o.Shop,
							IsBigpart = o.IsBigpart,
							IsActive = o.IsActive,
						};
			var exportToExcel = await query.ToListAsync();
			return _caseDataListExcelExporter.ExportToFile(exportToExcel);
		}

		public async Task<FileDto> GetMwLayOutGetActiveAllToExcel()
		{
			string _sql = "Exec LOGW_MWH_LAYOUT_GETACTIVE_ALL";
			var filtered = await _dapperRepo.QueryAsync<LgwMwhCaseDataDto>(_sql, new{});
			var pageAndFiltered = filtered.OrderBy(s => s.CaseNo);
			var query = from o in pageAndFiltered
						select new LgwMwhCaseDataDto
						{
							Id = o.Id,
							CaseNo = o.CaseNo,						
							SupplierNo = o.SupplierNo,
							Loc = o.Loc,
							LocDetails = o.LocDetails	
						};
			var exportWhDataToExcel = await query.AsQueryable().ToListAsync();
			return _caseDataListExcelExporter.ExportWhBigCaseDataToFile(exportWhDataToExcel);
		}

		public async Task<FileDto> GetMwLayOutTmcGetActiveAllToExcel()
		{
			string _sql = "Exec LOGW_MWH_LAYOUT_TMC_GETACTIVE_ALL";
			var filtered = await _dapperRepo.QueryAsync<LgwMwhCaseDataDto>(_sql, new { });
			var pageAndFiltered = filtered.OrderBy(s => s.CaseNo);
			var query = from o in pageAndFiltered
						select new LgwMwhCaseDataDto
						{
							Id = o.Id,
							CaseNo = o.CaseNo,
							SupplierNo = o.SupplierNo,
							Loc = o.Loc,
							LocDetails = o.LocDetails
						};
			var exportRobbingDataToExcel = await query.AsQueryable().ToListAsync();
			return _caseDataListExcelExporter.ExportRobbingDataToFile(exportRobbingDataToExcel);
		}


		// public async Task GenerateAsync()
		//  {
		//    await _dapperRepo.ExecuteAsync(LgwMwhCaseDataConsts.SP_MST_WPT_CALENDAR_GENERATE);
		// }

	}
}
