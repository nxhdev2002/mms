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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.LogA.SpsRack
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_SpsRack)]
    public class MstLgaSpsRackAppService : prodAppServiceBase, IMstLgaSpsRackAppService
    {
        private readonly IDapperRepository<MstLgaSpsRack, long> _dapperRepo;
        private readonly IRepository<MstLgaSpsRack, long> _repo;
        private readonly IMstLgaSpsRackExcelExporter _calendarListExcelExporter;

        public MstLgaSpsRackAppService(IRepository<MstLgaSpsRack, long> repo,
                                         IDapperRepository<MstLgaSpsRack, long> dapperRepo,
                                        IMstLgaSpsRackExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_SpsRack_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstLgaSpsRackDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstLgaSpsRackDto input)
        {
            var mainObj = ObjectMapper.Map<MstLgaSpsRack>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstLgaSpsRackDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_SpsRack_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstLgaSpsRackDto>> GetAll(GetMstLgaSpsRackInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Address), e => e.Address.Contains(input.Address));

            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstLgaSpsRackDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Address = o.Address,
                             Ordering = o.Ordering,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstLgaSpsRackDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetSpsRackToExcel(MstLgaSpsRackExportInput input)
        {
            var query = from o in _repo.GetAll()
                        select new MstLgaSpsRackDto
                        {
                            Id = o.Id,
                            Code = o.Code,
                            Address = o.Address,
                            Ordering = o.Ordering,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
