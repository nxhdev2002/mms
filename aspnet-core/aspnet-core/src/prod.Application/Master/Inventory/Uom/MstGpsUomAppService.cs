using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Dapper;
using Microsoft.EntityFrameworkCore;
using NUglify.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Master.Inventory.Dto;
using prod.Master.Inventory.Exporting;

namespace prod.Master.Inventory
{
    [AbpAuthorize(AppPermissions.Pages_Master_Gps_Uom_View)]
    public class MstGpsUomAppService : prodAppServiceBase, IMstInvUomAppService
    {
        private readonly IDapperRepository<MstGpsUom, long> _dapperRepo;
        private readonly IRepository<MstGpsUom, long> _repo;
        private readonly IMstGpsUomExcelExporter _calendarListExcelExporter;

        public MstGpsUomAppService(IRepository<MstGpsUom, long> repo,
                                         IDapperRepository<MstGpsUom, long> dapperRepo,
                                        IMstGpsUomExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_Master_Gps_Uom_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstGpsUomDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstGpsUomDto input)
        {
            var mainObj = ObjectMapper.Map<MstGpsUom>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstGpsUomDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

     //   [AbpAuthorize(AppPermissions.Pages_InvtSetup_Uom_Edit)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstGpsUomDto>> GetAll(GetMstGpsUomInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstGpsUomDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Name = o.Name,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstGpsUomDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetUomToExcel(MstGpsUomExportInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var query = from o in pageAndFiltered
                         select new MstGpsUomDto
                        {
                            Id = o.Id,
                            Code = o.Code,
                            Name = o.Name,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstGpsUomConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

        public async Task<List<MstGpsUomDto>> GetListGpsUom()
        {
            string _sql = "Exec INV_GPS_GET_LIST_UOM";
            IEnumerable<MstGpsUomDto> result = await _dapperRepo.QueryAsync<MstGpsUomDto>(_sql);
            return result.ToList();
        }
    }
}
