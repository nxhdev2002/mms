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
using prod.Master.Common.Dto;
using prod.Master.Common.Exporting;

namespace prod.Master.Common
{
    [AbpAuthorize(AppPermissions.Pages_InvtSetup_Uom_View)]
    public class MstCmmUomAppService : prodAppServiceBase, IMstCmmUomAppService
    {
        private readonly IDapperRepository<MstCmmUom, long> _dapperRepo;
        private readonly IRepository<MstCmmUom, long> _repo;
        private readonly IMstCmmUomExcelExporter _calendarListExcelExporter;

        public MstCmmUomAppService(IRepository<MstCmmUom, long> repo,
                                         IDapperRepository<MstCmmUom, long> dapperRepo,
                                        IMstCmmUomExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_InvtSetup_Uom_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstCmmUomDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstCmmUomDto input)
        {
            var mainObj = ObjectMapper.Map<MstCmmUom>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstCmmUomDto input)
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

        public async Task<PagedResultDto<MstCmmUomDto>> GetAll(GetMstCmmUomInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstCmmUomDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Name = o.Name,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstCmmUomDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetUomToExcel(MstCmmUomExportInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var query = from o in pageAndFiltered
                         select new MstCmmUomDto
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
        //    await _dapperRepo.ExecuteAsync(MstCmmUomConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
