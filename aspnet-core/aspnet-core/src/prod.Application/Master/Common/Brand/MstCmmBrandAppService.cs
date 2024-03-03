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
using prod.Master.Common.Dto;
using prod.Master.Common.Exporting;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.Common
{
    [AbpAuthorize(AppPermissions.Pages_InvtSetup_Brand_View)]
    public class MstCmmBrandAppService : prodAppServiceBase, IMstCmmBrandAppService
    {
        private readonly IDapperRepository<MstCmmBrand, long> _dapperRepo;
        private readonly IRepository<MstCmmBrand, long> _repo;
        private readonly IMstCmmBrandExcelExporter _calendarListExcelExporter;

        public MstCmmBrandAppService(IRepository<MstCmmBrand, long> repo,
                                         IDapperRepository<MstCmmBrand, long> dapperRepo,
                                        IMstCmmBrandExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        // [AbpAuthorize(AppPermissions.Pages_InvtSetup_Brand_Edit)]
        //public async Task CreateOrEdit(CreateOrEditMstCmmBrandDto input)
        //{
        //    if (input.Id == null) await Create(input);
        //    else await Update(input);
        //}

        ////CREATE
        //private async Task Create(CreateOrEditMstCmmBrandDto input)
        //{
        //    var mainObj = ObjectMapper.Map<MstCmmBrand>(input);

        //    await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        //}

        //// EDIT
        //private async Task Update(CreateOrEditMstCmmBrandDto input)
        //{
        //    using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
        //    {
        //        var mainObj = await _repo.GetAll()
        //        .FirstOrDefaultAsync(e => e.Id == input.Id);

        //        var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
        //    }
        //}

        // [AbpAuthorize(AppPermissions.Pages_InvtSetup_Brand_Delete)]
        //public async Task Delete(EntityDto input)
        //{
        //    var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
        //    _repo.HardDelete(mainObj);
        //    CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        //}

        public async Task<PagedResultDto<MstCmmBrandDto>> GetAll(GetMstCmmBrandInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name));

            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstCmmBrandDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Name = o.Name,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstCmmBrandDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetBrandToExcel(MstCmmBrandExportInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var query = from o in pageAndFiltered
                        select new MstCmmBrandDto
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
        //    await _dapperRepo.ExecuteAsync(MstCmmBrandConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
