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
using prod.Master.WorkingPattern;
using prod.Master.WorkingPattern.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Master.WorkingPattern.Exporting;

namespace prod.Master.WorkingPattern
{
  //  //  [AbpAuthorize(AppPermissions.Pages_WorkingPattern_Shop)]
    public class MstWptShopAppService : prodAppServiceBase, IMstWptShopAppService
    {
        private readonly IDapperRepository<MstWptShop, long> _dapperRepo;
        private readonly IRepository<MstWptShop, long> _repo;
        private readonly IMstWptShopExcelExporter _calendarListExcelExporter;

        public MstWptShopAppService(IRepository<MstWptShop, long> repo,
                                         IDapperRepository<MstWptShop, long> dapperRepo,
                                        IMstWptShopExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

    //    //  [AbpAuthorize(AppPermissions.Pages_WorkingPattern_Shop_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstWptShopDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstWptShopDto input)
        {
            var mainObj = ObjectMapper.Map<MstWptShop>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstWptShopDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

      //  //  [AbpAuthorize(AppPermissions.Pages_WorkingPattern_Shop_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstWptShopDto>> GetAll(GetMstWptShopInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.ShopName), e => e.ShopName.Contains(input.ShopName))
               // .WhereIf(!string.IsNullOrWhiteSpace(input.Description), e => e.Description.Contains(input.Description))
                //.WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstWptShopDto
                         {
                             Id = o.Id,
                             ShopName = o.ShopName,
                             Description = o.Description,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstWptShopDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetShopToExcel(MstWptShopExportInput input)
        {
            var filtered = _repo.GetAll()
                 .WhereIf(!string.IsNullOrWhiteSpace(input.ShopName), e => e.ShopName.Contains(input.ShopName))
                 // .WhereIf(!string.IsNullOrWhiteSpace(input.Description), e => e.Description.Contains(input.Description))
                 //.WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive))
                 ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var query = from o in pageAndFiltered
                         select new MstWptShopDto
                        {
                            Id = o.Id,
                            ShopName = o.ShopName,
                            Description = o.Description,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstWptShopConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
