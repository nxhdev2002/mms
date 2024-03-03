using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.HistoricalData;
using prod.Master.Common;
using prod.Master.Common.Dto;
using prod.Master.Common.Exporting;
using prod.Master.Common.VehicleCBU.Dto;
using prod.Master.Inventory.Dto;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Inventory
{
	public class MstInvDevanningCaseTypeAppService : prodAppServiceBase, IMstInvDevanningCaseTypeAppService
	{
		private readonly IDapperRepository<MstInvDevanningCaseType, long> _dapperRepo;
		private readonly IRepository<MstInvDevanningCaseType, long> _repo;
		private readonly IMstInvDevanningCaseTypeExcelExporter _mstinvdevanningcasetypeListExcelExporter;
        private readonly IHistoricalDataAppService _historicalDataAppService;

        public MstInvDevanningCaseTypeAppService(IRepository<MstInvDevanningCaseType, long> repo,
										 IDapperRepository<MstInvDevanningCaseType, long> dapperRepo,
										 IMstInvDevanningCaseTypeExcelExporter mstinvdevanningcasetypeListExcelExporter,
										 IHistoricalDataAppService historicalDataAppService
			)
		{
			_repo = repo;
			_dapperRepo = dapperRepo;
			_mstinvdevanningcasetypeListExcelExporter = mstinvdevanningcasetypeListExcelExporter;
            _historicalDataAppService = historicalDataAppService;

        }

        public async Task<List<string>> GetMstInvDevanningCaseTypeHistory(GetMstInvDevanningCaseTypeHistoryInput input)
        {
            return await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
        }

        public async Task<FileDto> GetHistoricalDataToExcel(GetMstInvDevanningCaseTypeHistoryExcelInput input)
        {
            var data = await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
            return _mstinvdevanningcasetypeListExcelExporter.ExportToHistoricalFile(data);
        }

        public async Task<List<long?>> GetChangedRecords()
        {
            return await _historicalDataAppService.GetChangedRecordIds("MstInvDevanningCaseType");
        }


        public async Task CreateOrEdit(CreateOrEditMstInvDevanningCaseTypeDto input)
		{
			if (input.Id == null) await Create(input);
			else await Update(input);
		}
		//CREATE
		private async Task Create(CreateOrEditMstInvDevanningCaseTypeDto input)
		{
			var mainObj = ObjectMapper.Map<MstInvDevanningCaseType>(input);

			await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
		}
		// EDIT
		private async Task Update(CreateOrEditMstInvDevanningCaseTypeDto input)
		{
			using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
			{
				var mainObj = await _repo.GetAll()
				.FirstOrDefaultAsync(e => e.Id == input.Id);

				var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
			}
		}
		//DELETE
		public async Task Delete(EntityDto input)
		{
			var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
			_repo.HardDelete(mainObj);
			CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
		}

		//GET ALL
		public async Task<PagedResultDto<MstInvDevanningCaseTypeDto>> GetAll(GetMstInvDevanningCaseTypeInput input)
		{
			var filtered = _repo.GetAll()
				.WhereIf(!string.IsNullOrWhiteSpace(input.Source), e => e.Source.Contains(input.Source))
				.WhereIf(!string.IsNullOrWhiteSpace(input.CaseNo), e => e.CaseNo.Contains(input.CaseNo))
				.WhereIf(!string.IsNullOrWhiteSpace(input.CarFamilyCode), e => e.CarFamilyCode.Contains(input.CarFamilyCode))
				.WhereIf(!string.IsNullOrWhiteSpace(input.ShoptypeCode), e => e.ShoptypeCode.Contains(input.ShoptypeCode))
				;
				
			var pageAndFiltered = filtered.OrderBy(s => s.Id);


			var system = from o in pageAndFiltered
						 select new MstInvDevanningCaseTypeDto
						 {
							 Id = o.Id,
							 Source = o.Source,
							 CaseNo = o.CaseNo,
							 ShoptypeCode = o.ShoptypeCode,
							 Type = o.Type,
							 CarFamilyCode = o.CarFamilyCode,
							 IsActive = o.IsActive,
						 };

			var totalCount = await filtered.CountAsync();
			var paged = system.PageBy(input);

			return new PagedResultDto<MstInvDevanningCaseTypeDto>(
				totalCount,
				 await paged.ToListAsync()
			);
		}
		public async Task<FileDto> GetMstInvDevanningCaseTypeToExcel(MstInvDevanningCaseTypeExportInput input)
		{
			var filtered = _repo.GetAll()
				.WhereIf(!string.IsNullOrWhiteSpace(input.Source), e => e.Source.Contains(input.Source))
				.WhereIf(!string.IsNullOrWhiteSpace(input.CaseNo), e => e.CaseNo.Contains(input.CaseNo))
				.WhereIf(!string.IsNullOrWhiteSpace(input.CarFamilyCode), e => e.CarFamilyCode.Contains(input.CarFamilyCode))
				.WhereIf(!string.IsNullOrWhiteSpace(input.ShoptypeCode), e => e.ShoptypeCode.Contains(input.ShoptypeCode))
				;

			var pageAndFiltered = filtered.OrderBy(s => s.Id);
			var query = from o in pageAndFiltered
						select new MstInvDevanningCaseTypeDto
						{
							Id = o.Id,
							Source = o.Source,
							CaseNo = o.CaseNo,
							ShoptypeCode = o.ShoptypeCode,
							Type = o.Type,
							CarFamilyCode= o.CarFamilyCode,
							IsActive = o.IsActive,
						};
			var exportToExcel = await query.ToListAsync();
			return _mstinvdevanningcasetypeListExcelExporter.ExportToFile(exportToExcel);
		}
	}
}
