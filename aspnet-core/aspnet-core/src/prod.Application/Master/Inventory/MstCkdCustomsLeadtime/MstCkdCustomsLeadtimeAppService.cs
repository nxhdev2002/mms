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
using prod.Master.CKD.Dto;
using prod.Master.CKD.Exporting;

namespace prod.Master.CKD
{
    [AbpAuthorize(AppPermissions.Pages_Master_CKD_CustomsLeadtime_View)]
    public class MstCkdCustomsLeadtimeAppService : prodAppServiceBase, IMstCkdCustomsLeadtimeAppService
    {
        private readonly IDapperRepository<MstCkdCustomsLeadtime, long> _dapperRepo;
        private readonly IRepository<MstCkdCustomsLeadtime, long> _repo;
        private readonly IMstCkdCustomsLeadtimeExcelExporter _calendarListExcelExporter;

        public MstCkdCustomsLeadtimeAppService(IRepository<MstCkdCustomsLeadtime, long> repo,
                                         IDapperRepository<MstCkdCustomsLeadtime, long> dapperRepo,
                                        IMstCkdCustomsLeadtimeExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_Master_CKD_CustomsLeadtime_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstCkdCustomsLeadtimeDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstCkdCustomsLeadtimeDto input)
        {
            var mainObj = ObjectMapper.Map<MstCkdCustomsLeadtime>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstCkdCustomsLeadtimeDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Master_CKD_CustomsLeadtime_Edit)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstCkdCustomsLeadtimeDto>> GetAll(GetMstCkdCustomsLeadtimeInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierNo), e => e.SupplierNo.Contains(input.SupplierNo))

                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstCkdCustomsLeadtimeDto
                         {
                             Id = o.Id,
                             SupplierNo = o.SupplierNo,
                             Leadtime = o.Leadtime,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstCkdCustomsLeadtimeDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetCustomsLeadtimeToExcel(MstCkdCustomsLeadtimeExportInput input)
        {
            var query = from o in _repo.GetAll()
                        select new MstCkdCustomsLeadtimeDto
                        {
                            Id = o.Id,
                            SupplierNo = o.SupplierNo,
                            Leadtime = o.Leadtime,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
