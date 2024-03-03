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
    [AbpAuthorize(AppPermissions.Pages_Master_Gps_GpsSupplierPic_View)]
    public class MstInvGpsSupplierPicAppService : prodAppServiceBase, IMstInvGpsSupplierPicAppService
    {
        private readonly IDapperRepository<MstInvGpsSupplierPic, long> _dapperRepo;
        private readonly IRepository<MstInvGpsSupplierPic, long> _repo;
        private readonly IMstInvGpsSupplierPicExcelExporter _calendarListExcelExporter;
        public MstInvGpsSupplierPicAppService(IRepository<MstInvGpsSupplierPic, long> repo,
                                         IDapperRepository<MstInvGpsSupplierPic, long> dapperRepo,
                                        IMstInvGpsSupplierPicExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_Master_Gps_GpsSupplierPic_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstInvGpsSupplierPicDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstInvGpsSupplierPicDto input)
        {
            var mainObj = ObjectMapper.Map<MstInvGpsSupplierPic>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstInvGpsSupplierPicDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Master_Gps_GpsSupplierPic_Edit)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstInvGpsSupplierPicDto>> GetAll(GetMstInvGpsSupplierPicInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.PicName), e => e.PicName.Contains(input.PicName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PicTelephone), e => e.PicTelephone.Contains(input.PicTelephone))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PicEmail), e => e.PicEmail.Contains(input.PicEmail))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstInvGpsSupplierPicDto
                         {
                             Id = o.Id,
                             SupplierId = o.SupplierId,
                             PicName = o.PicName,
                             PicTelephone = o.PicTelephone,
                             PicEmail = o.PicEmail,
                             IsMainPic = o.IsMainPic,
                             IsSendEmail = o.IsSendEmail,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstInvGpsSupplierPicDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetGpsSupplierPicToExcel(MstInvGpsSupplierPicExportInput input)
        {
            var filtered = _repo.GetAll()
               .WhereIf(!string.IsNullOrWhiteSpace(input.PicName), e => e.PicName.Contains(input.PicName))
               .WhereIf(!string.IsNullOrWhiteSpace(input.PicTelephone), e => e.PicTelephone.Contains(input.PicTelephone))
               .WhereIf(!string.IsNullOrWhiteSpace(input.PicEmail), e => e.PicEmail.Contains(input.PicEmail))
               ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var query = from o in pageAndFiltered
                        select new MstInvGpsSupplierPicDto
                        {
                            Id = o.Id,
                            SupplierId = o.SupplierId,
                            PicName = o.PicName,
                            PicTelephone = o.PicTelephone,
                            PicEmail = o.PicEmail,
                            IsMainPic = o.IsMainPic,
                            IsSendEmail = o.IsSendEmail,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
