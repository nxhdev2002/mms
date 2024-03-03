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
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.Exporting;
using System.Globalization;

namespace prod.Inventory.CKD
{
      [AbpAuthorize(AppPermissions.Pages_Ckd_Physical_PhysicalStockPeriod_View)]
    public class InvCkdPhysicalStockPeriodAppService : prodAppServiceBase, IInvCkdPhysicalStockPeriodAppService
    {
        private readonly IDapperRepository<InvCkdPhysicalStockPeriod, long> _dapperRepo;
        private readonly IRepository<InvCkdPhysicalStockPeriod, long> _repo;
        private readonly IInvCkdPhysicalStockPeriodExcelExporter _calendarListExcelExporter;

        public InvCkdPhysicalStockPeriodAppService(IRepository<InvCkdPhysicalStockPeriod, long> repo,
                                         IDapperRepository<InvCkdPhysicalStockPeriod, long> dapperRepo,
                                        IInvCkdPhysicalStockPeriodExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

          [AbpAuthorize(AppPermissions.Pages_Ckd_Physical_PhysicalStockPeriod_Edit)]
        public async Task CreateOrEdit(CreateOrEditInvCkdPhysicalStockPeriodDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditInvCkdPhysicalStockPeriodDto input)
        {
            var mainObj = ObjectMapper.Map<InvCkdPhysicalStockPeriod>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditInvCkdPhysicalStockPeriodDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

         [AbpAuthorize(AppPermissions.Pages_Ckd_Physical_PhysicalStockPeriod_Edit)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<InvCkdPhysicalStockPeriodDto>> GetAll(GetInvCkdPhysicalStockPeriodInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Description), e => e.Description.Contains(input.Description)) ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var system = from o in pageAndFiltered
                         select new InvCkdPhysicalStockPeriodDto
                         {
                             Id = o.Id,
                             Description = o.Description,
                             FromDate = DateTime.Parse(o.FromDate.ToString() + " " + o.FromTime),
                             FromTime = o.FromTime,
                             ToDate = DateTime.Parse(o.ToDate.ToString() + " " + o.ToTime),
                             ToTime = o.ToTime,
                             Status = o.Status,
                            
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<InvCkdPhysicalStockPeriodDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }

        public async Task GetAllCreate(GetInvCkdPhysicalStockPeriodPeriosInput input)
        {
            Dapper.SqlMapper.Settings.CommandTimeout = 900;
            string _sql = "INV_CKD_PHYSICAL_STOCK_PERIOD_NEW_PERIOD @Description, @FromDate, @FromTime, @ToDate, @ToTime";

            IEnumerable<InvCkdPhysicalStockPeriodDto> result = await _dapperRepo.QueryAsync<InvCkdPhysicalStockPeriodDto>(_sql, new
            {
                Description = input.Description,
                FromDate = input.FromDate,
                FromTime = input.FromTime,
                ToDate = input.ToDate,
                ToTime = input.ToTime
            });

        }


        public async Task<FileDto> GetPhysicalStockPeriodToExcel(InvCkdPhysicalStockPeriodExportInput input)
        {
            var filtered = _repo.GetAll()
                 .WhereIf(!string.IsNullOrWhiteSpace(input.Description), e => e.Description.Contains(input.Description));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var system = from o in pageAndFiltered
                         select new InvCkdPhysicalStockPeriodDto
                         {
                             Id = o.Id,
                             Description = o.Description,
                             FromDate = DateTime.Parse(o.FromDate.ToString() + " " + o.FromTime),
                             ToDate = DateTime.Parse(o.ToDate.ToString() + " " + o.ToTime),
                             Status = o.Status,

                         };
            var exportToExcel = await system.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        public async Task<PagedResultDto<InvCkdPhysicalStockPeriodDto>> GetIdInvPhysicalStockPeriod()
        {
            string _sql = "Exec INV_PHYSICAL_STOCK_PERIOD_GETID";

            var data = await _dapperRepo.QueryAsync<InvCkdPhysicalStockPeriodDto>(_sql, new { });

            var results = from o in data
                          select new InvCkdPhysicalStockPeriodDto
                          {
                              Id = o.Id,
                              Description = o.Description,
                              FromDate = o.FromDate,
                              FromTime = o.FromTime,
                              ToDate = o.ToDate,
                              ToTime = o.ToTime,
                              Status = o.Status,
                              InfoPeriod = o.Description + " : " + o.FromDate.ToString().Substring(0,10) + " -> " + o.ToDate.ToString().Substring(0, 10)
                          };

            var totalCount = data.ToList().Count;
            return new PagedResultDto<InvCkdPhysicalStockPeriodDto>(
                totalCount,
                results.ToList()
            );
        }
        public async Task<PagedResultDto<InvCkdPhysicalStockPeriodDto>> GetIdInvPhysicalStockPeriodImport()
        {
            string _sql = "Exec INV_PHYSICAL_STOCK_PERIOD_GETIMPORT";

            var data = await _dapperRepo.QueryAsync<InvCkdPhysicalStockPeriodDto>(_sql, new { });

            var results = from o in data
                          select new InvCkdPhysicalStockPeriodDto
                          {
                              Id = o.Id,
                              Description = o.Description,
                              FromDate = o.FromDate,
                              FromTime = o.FromTime,
                              ToDate = o.ToDate,
                              ToTime = o.ToTime,
                              Status = o.Status,
                              InfoPeriod = o.Description + " : " + o.FromDate.ToString().Substring(0, 10) + " -> " + o.ToDate.ToString().Substring(0, 10)
                          };

            var totalCount = data.ToList().Count;
            return new PagedResultDto<InvCkdPhysicalStockPeriodDto>(
                totalCount,
                results.ToList()
            );
        }     

    }
}