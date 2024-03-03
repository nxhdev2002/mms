using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Master.Pio.DTO;
using prod.Master.Pio.Exporting;
using System.Linq;
using System.Threading.Tasks;

namespace prod.Master.Pio
{
    [AbpAuthorize(AppPermissions.Pages_Master_Gps_GpsSupplierInfo_View)]
	public class MstLspSupplierInforAppService : prodAppServiceBase, IMstLspSupplierInforAppService
	{
		private readonly IDapperRepository<MstLspSupplierInfor, long> _dapperRepo;
		private readonly IRepository<MstLspSupplierInfor, long> _repo;
		private readonly IMstLspSupplierInforExporter _mstlspSupplierinfoExporter;
		private readonly Abp.ObjectMapping.IObjectMapper _objectMapper;

		public MstLspSupplierInforAppService(IRepository<MstLspSupplierInfor, long> repo,
										 IDapperRepository<MstLspSupplierInfor, long> dapperRepo,
										IMstLspSupplierInforExporter mstlspSupplierinfoExporter
										, Abp.ObjectMapping.IObjectMapper objectMapper)
		{
			_repo = repo;
			_dapperRepo = dapperRepo;
			_mstlspSupplierinfoExporter = mstlspSupplierinfoExporter;
			_objectMapper = objectMapper;
		}

		[AbpAuthorize(AppPermissions.Pages_PIO_Master_Supplier_Info_CreateEdit)]
		public async Task CreateOrEdit(CreateOrEditMstLspSupplierInforDto input)
		{
			if (input.Id == null) await Create(input);
			else await Update(input);
		}
		//CREATE
		private async Task Create(CreateOrEditMstLspSupplierInforDto input)
		{
			var mainObj = ObjectMapper.Map<MstLspSupplierInfor>(input);

			await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
		}
		// EDIT
		private async Task Update(CreateOrEditMstLspSupplierInforDto input)
		{
			using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
			{
				var mainObj = await _repo.GetAll()
				.FirstOrDefaultAsync(e => e.Id == input.Id);

				var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
			}
		}
		[AbpAuthorize(AppPermissions.Pages_PIO_Master_Supplier_Info_Delete)]
		public async Task Delete(EntityDto input)
		{
			var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
			_repo.HardDelete(mainObj);
			CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
		}


		public async Task<PagedResultDto<MstLspSupplierInforDto>> GetAll(GetMstLspSupplierInforInput input)
		{
			var filtered = _repo.GetAll()
				.WhereIf(!string.IsNullOrWhiteSpace(input.SupplierCode), e => e.SupplierCode.Contains(input.SupplierCode))
				.WhereIf(!string.IsNullOrWhiteSpace(input.DeliveryMethod), e => e.DeliveryMethod.Contains(input.DeliveryMethod))
				.WhereIf(!string.IsNullOrWhiteSpace(input.DeliveryFrequency), e => e.DeliveryFrequency.Contains(input.DeliveryFrequency))
				.WhereIf(!string.IsNullOrWhiteSpace(input.OrderDateType), e => e.OrderDateType.Contains(input.OrderDateType))
				.WhereIf(!string.IsNullOrWhiteSpace(input.KeihenType), e => e.KeihenType.Contains(input.KeihenType))
				;
			var pageAndFiltered = filtered.OrderBy(s => s.Id);
			var system = _objectMapper.ProjectTo<MstLspSupplierInforDto>(pageAndFiltered);
			

			var totalCount = await filtered.CountAsync();
			var paged = system.PageBy(input);

			return new PagedResultDto<MstLspSupplierInforDto>(
				totalCount,
				 await paged.ToListAsync()
			);
		}

		public async Task<FileDto> GetMstLspSupplierInfoToExcel(MstLspSupplierInfoExportInput input)
		{

			var filtered = _repo.GetAll()
				.WhereIf(!string.IsNullOrWhiteSpace(input.SupplierCode), e => e.SupplierCode.Contains(input.SupplierCode))
				.WhereIf(!string.IsNullOrWhiteSpace(input.DeliveryMethod), e => e.DeliveryMethod.Contains(input.DeliveryMethod))
				.WhereIf(!string.IsNullOrWhiteSpace(input.DeliveryFrequency), e => e.DeliveryFrequency.Contains(input.DeliveryFrequency))
				.WhereIf(!string.IsNullOrWhiteSpace(input.OrderDateType), e => e.OrderDateType.Contains(input.OrderDateType))
				.WhereIf(!string.IsNullOrWhiteSpace(input.KeihenType), e => e.KeihenType.Contains(input.KeihenType));
			var pageAndFiltered = filtered.OrderBy(s => s.Id);

			var query = _objectMapper.ProjectTo<MstLspSupplierInforDto>(pageAndFiltered);

			var exportToExcel = await query.ToListAsync();
			return _mstlspSupplierinfoExporter.ExportToFile(exportToExcel);
		}
	}
}
