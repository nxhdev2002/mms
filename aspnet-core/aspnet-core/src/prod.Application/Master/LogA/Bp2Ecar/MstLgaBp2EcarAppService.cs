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
using prod.Master.LogA.Dto;
using prod.Master.LogA.Exporting;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.LogA
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_Bp2Ecar)]
    public class MstLgaBp2EcarAppService : prodAppServiceBase, IMstLgaBp2EcarAppService
    {
        private readonly IDapperRepository<MstLgaBp2Ecar, long> _dapperRepo;
        private readonly IRepository<MstLgaBp2Ecar, long> _repo;
        private readonly IMstLgaBp2EcarExcelExporter _calendarListExcelExporter;

        public MstLgaBp2EcarAppService(IRepository<MstLgaBp2Ecar, long> repo,
                                         IDapperRepository<MstLgaBp2Ecar, long> dapperRepo,
                                        IMstLgaBp2EcarExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_Bp2Ecar_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstLgaBp2EcarDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstLgaBp2EcarDto input)
        {
            var mainObj = ObjectMapper.Map<MstLgaBp2Ecar>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstLgaBp2EcarDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_Bp2Ecar_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstLgaBp2EcarDto>> GetAll(GetMstLgaBp2EcarInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.EcarName), e => e.EcarName.Contains(input.EcarName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine));

            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var system = from o in pageAndFiltered
                         select new MstLgaBp2EcarDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             EcarName = o.EcarName,
                             ProdLine = o.ProdLine,
                             EcarType = o.EcarType,
                             Sorting = o.Sorting,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstLgaBp2EcarDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }

        public async Task<FileDto> GetBp2EcarToExcel(GetMstLgaBp2EcarExcelInput input)
        {
            var filtered = _repo.GetAll()
             .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
             .WhereIf(!string.IsNullOrWhiteSpace(input.EcarName), e => e.EcarName.Contains(input.EcarName))
             .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine));

            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var query = from o in pageAndFiltered
                         select new MstLgaBp2EcarDto
                        {
                            Id = o.Id,
                            Code = o.Code,
                            EcarName = o.EcarName,
                            ProdLine = o.ProdLine,
                            EcarType = o.EcarType,
                            Sorting = o.Sorting,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }
    }
}
