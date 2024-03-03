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
using prod.Master.LogA;
using prod.Master.LogA.Dto;
using prod.Master.LogA.Exporting;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.LogA
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_BarProcess)]
    public class MstLgaBarProcessAppService : prodAppServiceBase, IMstLgaBarProcessAppService
    {
        private readonly IDapperRepository<MstLgaBarProcess, long> _dapperRepo;
        private readonly IRepository<MstLgaBarProcess, long> _repo;
        private readonly IMstLgaBarProcessExcelExporter _calendarListExcelExporter;

        public MstLgaBarProcessAppService(IRepository<MstLgaBarProcess, long> repo,
                                         IDapperRepository<MstLgaBarProcess, long> dapperRepo,
                                        IMstLgaBarProcessExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_BarProcess_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstLgaBarProcessDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstLgaBarProcessDto input)
        {
            var mainObj = ObjectMapper.Map<MstLgaBarProcess>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstLgaBarProcessDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_BarProcess_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstLgaBarProcessDto>> GetAll(GetMstLgaBarProcessInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProcessName), e => e.ProcessName.Contains(input.ProcessName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstLgaBarProcessDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             ProcessName = o.ProcessName,
                             ProcessGroup = o.ProcessGroup,
                             ProcessSubgroup = o.ProcessSubgroup,
                             ProdLine = o.ProdLine,
                             ProcessType = o.ProcessType,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstLgaBarProcessDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }

        public async Task<FileDto> GetBarProcessToExcel(GetMstLgaBarProcesExcelInput input)
        {
            var filtered = _repo.GetAll()
               .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProcessName), e => e.ProcessName.Contains(input.ProcessName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var query = from o in pageAndFiltered
                         select new MstLgaBarProcessDto
                        {
                            Id = o.Id,
                            Code = o.Code,
                            ProcessName = o.ProcessName,
                            ProcessGroup = o.ProcessGroup,
                            ProcessSubgroup = o.ProcessSubgroup,
                            ProdLine = o.ProdLine,
                            ProcessType = o.ProcessType,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }
    }
}
