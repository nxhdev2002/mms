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
using prod;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Master.LogA;
using prod.Master.LogA.Dto;
using prod.Master.LogA.Exporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.LogA
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_EkbProcess)]
    public class MstLgaEkbProcessAppService : prodAppServiceBase, IMstLgaEkbProcessAppService
    {
        private readonly IDapperRepository<MstLgaEkbProcess, long> _dapperRepo;
        private readonly IRepository<MstLgaEkbProcess, long> _repo;
        private readonly IMstLgaEkbProcessExcelExporter _calendarListExcelExporter;


        public MstLgaEkbProcessAppService(IRepository<MstLgaEkbProcess, long> repo,
                                         IDapperRepository<MstLgaEkbProcess, long> dapperRepo,
                                        IMstLgaEkbProcessExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_EkbProcess_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstLgaEkbProcessDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstLgaEkbProcessDto input)
        {
            var mainObj = ObjectMapper.Map<MstLgaEkbProcess>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstLgaEkbProcessDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_EkbProcess_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstLgaEkbProcessDto>> GetAll(GetMstLgaEkbProcessInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProcessName), e => e.ProcessName.Contains(input.ProcessName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var system = from o in pageAndFiltered
                         select new MstLgaEkbProcessDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             ProcessName = o.ProcessName,
                             ProcessGroup = o.ProcessGroup,
                             ProcessSubgroup = o.ProcessSubgroup,
                             ProdLine = o.ProdLine,
                             LeadTime = o.LeadTime,
                             Sorting = o.Sorting,
                             ProcessType = o.ProcessType,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstLgaEkbProcessDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }

        public async Task<FileDto> GetEkbProcessToExcel(GetMstLgaEkbProcessExcelInput input)
        {
            var filtered = _repo.GetAll()
               .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
               .WhereIf(!string.IsNullOrWhiteSpace(input.ProcessName), e => e.ProcessName.Contains(input.ProcessName))
               .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
               ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var query = from o in pageAndFiltered
                         select new MstLgaEkbProcessDto
                        {
                            Id = o.Id,
                            Code = o.Code,
                            ProcessName = o.ProcessName,
                            ProcessGroup = o.ProcessGroup,
                            ProcessSubgroup = o.ProcessSubgroup,
                            ProdLine = o.ProdLine,
                            LeadTime = o.LeadTime,
                            Sorting = o.Sorting,
                            ProcessType = o.ProcessType,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }
    }
}
