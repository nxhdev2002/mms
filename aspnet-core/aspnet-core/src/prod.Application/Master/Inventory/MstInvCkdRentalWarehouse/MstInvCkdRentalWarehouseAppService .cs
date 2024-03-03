using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Master.Inv.Dto;
using prod.Master.Inv;
using prod.Master.Inventory;
using prod.Master.Inventory.Dto;
using prod.Master.Inventory.Exporting;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Master.Pio.DTO;

namespace prod.Master.Inventory
{
    [AbpAuthorize(AppPermissions.Pages_Master_Inventory_CkdRentalWarehouse)]
    public class MstInvCkdRentalWarehouseAppService : prodAppServiceBase, IMstInvCkdRentalWarehouseAppService
    {
        private readonly IDapperRepository<MstInvCkdRentalWarehouse, long> _dapperRepo;
        private readonly IRepository<MstInvCkdRentalWarehouse, long> _repo;
        private readonly IMstInvCkdRentalWarehouseExcelExporter _calendarListExcelExporter;
		private readonly Abp.ObjectMapping.IObjectMapper _objectMapper;

		public MstInvCkdRentalWarehouseAppService(IRepository<MstInvCkdRentalWarehouse, long> repo,
                                         IDapperRepository<MstInvCkdRentalWarehouse, long> dapperRepo,
                                        IMstInvCkdRentalWarehouseExcelExporter calendarListExcelExporter
			                            , Abp.ObjectMapping.IObjectMapper objectMapper)
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
			_objectMapper = objectMapper;
		}

        [AbpAuthorize(AppPermissions.Pages_Master_Inventory_CkdRentalWarehouse_CreateEdit)]
        public async Task CreateOrEdit(CreateOrEditMstInvCkdRentalWarehouseDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstInvCkdRentalWarehouseDto input)
        {
            var mainObj = ObjectMapper.Map<MstInvCkdRentalWarehouse>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstInvCkdRentalWarehouseDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        public async Task<PagedResultDto<MstInvCkdRentalWarehouseDto>> GetAll(GetMstInvCkdRentalWarehouseInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name))
                .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive))
            ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


			var system = _objectMapper.ProjectTo<MstInvCkdRentalWarehouseDto>(pageAndFiltered);

			var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstInvCkdRentalWarehouseDto>(
                totalCount,
                 await paged.ToListAsync()
            );
		}


        public async Task<FileDto> GetCkdRentalWarehouseToExcel(MstInvCkdRentalWarehouseExportInput input)
        {
            var filtered = _repo.GetAll()
                 .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name))
                .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive));

			var pageAndFiltered = filtered.OrderBy(s => s.Id);

			var query = _objectMapper.ProjectTo<MstInvCkdRentalWarehouseDto>(pageAndFiltered);
			var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
