using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Master.Common.Dto;
using prod.Master.Common.Exporting;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Common
{
	[AbpAuthorize(AppPermissions.Pages_InvtSetup_ProductType_View)]
	public class MstCmmProductTypeAppService : prodAppServiceBase, IMstCmmProductTypeAppService
	{
		private readonly IDapperRepository<MstCmmProductType, long> _dapperRepo;
		private readonly IRepository<MstCmmProductType, long> _repo;
		private readonly IMstCmmProductTypeExcelExporter _calendarListExcelExporter;

		public MstCmmProductTypeAppService(IRepository<MstCmmProductType, long> repo,
										 IDapperRepository<MstCmmProductType, long> dapperRepo,
										IMstCmmProductTypeExcelExporter calendarListExcelExporter
			)
		{
			_repo = repo;
			_dapperRepo = dapperRepo;
			_calendarListExcelExporter = calendarListExcelExporter;
		}

		//[AbpAuthorize(AppPermissions.Pages_InvtSetup_ProductType_Edit)]
		//public async Task CreateOrEdit(CreateOrEditMstCmmProductTypeDto input)
		//{
		//	if (input.Id == null) await Create(input);
		//	else await Update(input);
		//}

		////CREATE
		//private async Task Create(CreateOrEditMstCmmProductTypeDto input)
		//{
		//	var mainObj = ObjectMapper.Map<MstCmmProductType>(input);

		//	await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
		//}

		//// EDIT
		//private async Task Update(CreateOrEditMstCmmProductTypeDto input)
		//{
		//	using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
		//	{
		//		var mainObj = await _repo.GetAll()
		//		.FirstOrDefaultAsync(e => e.Id == input.Id);

		//		var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
		//	}
		//}

		//[AbpAuthorize(AppPermissions.Pages_InvtSetup_ProductType_Delete)]
		//public async Task Delete(EntityDto input)
		//{
  //          var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
  //          _repo.HardDelete(mainObj);
  //          CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
  //      }

		public async Task<PagedResultDto<MstCmmProductTypeDto>> GetAll(GetMstCmmProductTypeInput input)
		{
			var filtered = _repo.GetAll()
				.WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
				.WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name))
				.WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive))
				;
			var pageAndFiltered = filtered.OrderBy(s => s.Id);


			var system = from o in pageAndFiltered
						 select new MstCmmProductTypeDto
						 {
							 Id = o.Id,
							 Code = o.Code,
							 Name = o.Name,
							 IsActive = o.IsActive,
						 };

			var totalCount = await filtered.CountAsync();
			var paged = system.PageBy(input);

			return new PagedResultDto<MstCmmProductTypeDto>(
				totalCount,
				 await paged.ToListAsync()
			);
		}


		public async Task<FileDto> GetProductTypeToExcel(MstCmmProductTypeExportInput input)
		{
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name))
                .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var query = from o in pageAndFiltered
                         select new MstCmmProductTypeDto
						{
							Id = o.Id,
							Code = o.Code,
							Name = o.Name,
							IsActive = o.IsActive,
						};
			var exportToExcel = await query.ToListAsync();
			return _calendarListExcelExporter.ExportToFile(exportToExcel);
		}

		// public async Task GenerateAsync()
		//  {
		//    await _dapperRepo.ExecuteAsync(MstCmmProductTypeConsts.SP_MST_WPT_CALENDAR_GENERATE);
		// }

	}
}

