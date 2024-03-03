using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using AutoMapper.Internal.Mappers;
using Dapper;
using Microsoft.EntityFrameworkCore;
using NUglify.Helpers;
using prod.Authorization;
using prod.Dto;
using prod.Master.Inventory.Dto;
using prod.Master.Inventory.Exporting;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.EntityFrameworkCore;


namespace prod.Master.Inventory
{
    [AbpAuthorize(AppPermissions.Pages_Master_Inventory_PIOPartType_View)]
    public class MstInvPIOPartTypeAppService : prodAppServiceBase, IMstInvPIOPartTypeAppService
    {
        private readonly IDapperRepository<MstInvPIOPartType, long> _dapperRepo;
        private readonly IRepository<MstInvPIOPartType, long> _repo;
        private readonly IMstInvPIOPartTypeExcelExporter _calendarListExcelExporter;

        public MstInvPIOPartTypeAppService(IRepository<MstInvPIOPartType, long> repo,
                                         IDapperRepository<MstInvPIOPartType, long> dapperRepo,
                                        IMstInvPIOPartTypeExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_Master_Inventory_PIOPartType_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstInvPIOPartTypeDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstInvPIOPartTypeDto input)
        {
            var mainObj = ObjectMapper.Map<MstInvPIOPartType>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstInvPIOPartTypeDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Master_Inventory_PIOPartType_Edit)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstInvPIOPartTypeDto>> GetAll(GetMstInvPIOPartTypeInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Description), e => e.Description.Contains(input.Description))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstInvPIOPartTypeDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Description = o.Description,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstInvPIOPartTypeDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetPIOPartTypeToExcel(MstInvPIOPartTypeExportInput input)
        {
            var filtered = _repo.GetAll()
            .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
            .WhereIf(!string.IsNullOrWhiteSpace(input.Description), e => e.Description.Contains(input.Description));

            var pageAndFiltered = filtered.OrderBy(s => s.Id);
            var query = from o in pageAndFiltered
                        select new MstInvPIOPartTypeDto
                        {
                            Id = o.Id,
                            Code = o.Code,
                            Description = o.Description,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstInvPIOPartTypeConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
