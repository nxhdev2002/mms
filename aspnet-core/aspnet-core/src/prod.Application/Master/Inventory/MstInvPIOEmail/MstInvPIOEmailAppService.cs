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
using prod.Master.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod;
using prod.EntityFrameworkCore;

namespace vovina.Master.Inventory
{
    [AbpAuthorize(AppPermissions.Pages_Master_Inventory_PIOEmail_View)]
    public class MstInvPIOEmailAppService : prodAppServiceBase, IMstInvPIOEmailAppService
    {
        private readonly IDapperRepository<MstInvPIOEmail, long> _dapperRepo;
        private readonly IRepository<MstInvPIOEmail, long> _repo;
        private readonly IMstInvPIOEmailExcelExporter _calendarListExcelExporter;

        public MstInvPIOEmailAppService(IRepository<MstInvPIOEmail, long> repo,
                                         IDapperRepository<MstInvPIOEmail, long> dapperRepo,
                                        IMstInvPIOEmailExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_Master_Inventory_PIOEmail_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstInvPIOEmailDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstInvPIOEmailDto input)
        {
            var mainObj = ObjectMapper.Map<MstInvPIOEmail>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstInvPIOEmailDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Master_Inventory_PIOEmail_Edit)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstInvPIOEmailDto>> GetAll(GetMstInvPIOEmailInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Subject), e => e.Subject.Contains(input.Subject))
                .WhereIf(!string.IsNullOrWhiteSpace(input.To), e => e.To.Contains(input.To))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Cc), e => e.Cc.Contains(input.Cc))
                .WhereIf(!string.IsNullOrWhiteSpace(input.BodyMess), e => e.BodyMess.Contains(input.BodyMess))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierCd), e => e.SupplierCd.Contains(input.SupplierCd))
                .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstInvPIOEmailDto
                         {
                             Id = o.Id,
                             Subject = o.Subject,
                             To = o.To,
                             Cc = o.Cc,
                             BodyMess = o.BodyMess,
                             SupplierCd = o.SupplierCd,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstInvPIOEmailDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetPIOEmailToExcel(MstInvPIOEmailExportInput input)
        {
            var filtered = _repo.GetAll()
           .WhereIf(!string.IsNullOrWhiteSpace(input.Subject), e => e.Subject.Contains(input.Subject))
           .WhereIf(!string.IsNullOrWhiteSpace(input.To), e => e.To.Contains(input.To))
           .WhereIf(!string.IsNullOrWhiteSpace(input.Cc), e => e.Cc.Contains(input.Cc))
           .WhereIf(!string.IsNullOrWhiteSpace(input.BodyMess), e => e.BodyMess.Contains(input.BodyMess))
           .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierCd), e => e.SupplierCd.Contains(input.SupplierCd))
           .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive));

            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var query = from o in pageAndFiltered
                        select new MstInvPIOEmailDto
                        {
                            Id = o.Id,
                            Subject = o.Subject,
                            To = o.To,
                            Cc = o.Cc,
                            BodyMess = o.BodyMess,
                            SupplierCd = o.SupplierCd,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstInvPIOEmailConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
