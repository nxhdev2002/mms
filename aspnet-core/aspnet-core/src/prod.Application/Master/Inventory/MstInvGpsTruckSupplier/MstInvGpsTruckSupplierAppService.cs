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
using prod.Master.Inv.Dto;
using prod.Master.Inv.Exporting;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.Inv
{
    [AbpAuthorize(AppPermissions.Pages_Master_Gps_GpsTruckSupplier_View)]
    public class MstInvGpsTruckSupplierAppService : prodAppServiceBase, IMstInvGpsTruckSupplierAppService
    {
        private readonly IDapperRepository<MstInvGpsTruckSupplier, long> _dapperRepo;
        private readonly IRepository<MstInvGpsTruckSupplier, long> _repo;
        private readonly IMstInvGpsTruckSupplierExcelExporter _calendarListExcelExporter;

        public MstInvGpsTruckSupplierAppService(IRepository<MstInvGpsTruckSupplier, long> repo,
                                         IDapperRepository<MstInvGpsTruckSupplier, long> dapperRepo,
                                        IMstInvGpsTruckSupplierExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_Master_Gps_GpsTruckSupplier_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstInvGpsTruckSupplierDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstInvGpsTruckSupplierDto input)
        {
            var mainObj = ObjectMapper.Map<MstInvGpsTruckSupplier>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstInvGpsTruckSupplierDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Master_Gps_GpsTruckSupplier_Edit)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstInvGpsTruckSupplierDto>> GetAll(GetMstInvGpsTruckSupplierInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.TruckName), e => e.TruckName.Contains(input.TruckName));

            var pageAndFiltered = filtered.OrderBy(s => s.SupplierId);


            var system = from o in pageAndFiltered
                         select new MstInvGpsTruckSupplierDto
                         {
                             Id = o.Id,
                             SupplierId = o.SupplierId,
                             TruckName = o.TruckName,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstInvGpsTruckSupplierDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetGpsTruckSupplierToExcel(MstInvGpsTruckSupplierExportInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.TruckName), e => e.TruckName.Contains(input.TruckName));

            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var query = from o in pageAndFiltered
                        select new MstInvGpsTruckSupplierDto
                        {
                            Id = o.Id,
                            SupplierId = o.SupplierId,
                            TruckName = o.TruckName,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
