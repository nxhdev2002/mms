using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Master.LogW.Dto;
using prod.Master.LogW.Exporting;

namespace prod.Master.LogW
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_LogW_AdoCallingLight)]
    public class MstLgwAdoCallingLightAppService : prodAppServiceBase, IMstLgwAdoCallingLightAppService
    {
        private readonly IDapperRepository<MstLgwAdoCallingLight, long> _dapperRepo;
        private readonly IRepository<MstLgwAdoCallingLight, long> _repo;
        private readonly IMstLgwAdoCallingLightExcelExporter _calendarListExcelExporter;

        public MstLgwAdoCallingLightAppService(IRepository<MstLgwAdoCallingLight, long> repo,
                                         IDapperRepository<MstLgwAdoCallingLight, long> dapperRepo,
                                        IMstLgwAdoCallingLightExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogW_AdoCallingLight_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstLgwAdoCallingLightDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstLgwAdoCallingLightDto input)
        {
            var mainObj = ObjectMapper.Map<MstLgwAdoCallingLight>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstLgwAdoCallingLightDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogW_AdoCallingLight_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstLgwAdoCallingLightDto>> GetAll(GetMstLgwAdoCallingLightInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.LightName), e => e.LightName.Contains(input.LightName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.SignalId);


            var system = from o in pageAndFiltered
                         select new MstLgwAdoCallingLightDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             LightName = o.LightName,
                             ProdLine = o.ProdLine,
                             Process = o.Process,
                             BlockCode = o.BlockCode,
                             BlockDescription = o.BlockDescription,
                             Sorting = o.Sorting,
                             SignalId = o.SignalId,
                             SignalCode = o.SignalCode,
                             IsActive = o.IsActive
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstLgwAdoCallingLightDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetCallingLightToExcel(MstLgwAdoCallingLightExportInput input)
        {
            var query = from o in _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.LightName), e => e.LightName.Contains(input.LightName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                        select new MstLgwAdoCallingLightDto
                        {
                            Id = o.Id,
                            Code = o.Code,
                            LightName = o.LightName,
                            ProdLine = o.ProdLine,
                            Process = o.Process,
                            BlockCode = o.BlockCode,
                            BlockDescription = o.BlockDescription,
                            Sorting = o.Sorting,
                            SignalId = o.SignalId,
                            SignalCode = o.SignalCode,
                            IsActive = o.IsActive
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }
    }
}
