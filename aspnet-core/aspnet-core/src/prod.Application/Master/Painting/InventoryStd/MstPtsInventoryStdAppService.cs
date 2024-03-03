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
using prod.Master.Painting.Dto;
using prod.Master.Painting.Exporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.Painting
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_Painting_InventoryStd)]
    public class MstPtsInventoryStdAppService : prodAppServiceBase, IMstPtsInventoryStdAppService
    {
        private readonly IDapperRepository<MstPtsInventoryStd, long> _dapperRepo;
        private readonly IRepository<MstPtsInventoryStd, long> _repo;
        private readonly IMstPtsInventoryStdExcelExporter _calendarListExcelExporter;

        public MstPtsInventoryStdAppService(IRepository<MstPtsInventoryStd, long> repo,
                                         IDapperRepository<MstPtsInventoryStd, long> dapperRepo,
                                        IMstPtsInventoryStdExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_Painting_InventoryStd_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstPtsInventoryStdDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstPtsInventoryStdDto input)
        {
            var mainObj = ObjectMapper.Map<MstPtsInventoryStd>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstPtsInventoryStdDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_Painting_InventoryStd_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstPtsInventoryStdDto>> GetAll(GetMstPtsInventoryStdInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Model), e => e.Model.Contains(input.Model))
                .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstPtsInventoryStdDto
                         {
                             Id = o.Id,
                             Model = o.Model,
                             InventoryStd = o.InventoryStd,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstPtsInventoryStdDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetInventoryStdToExcel(MstPtsInventoryStdExportInput input)
        {
            var query = from o in _repo.GetAll()
                        select new MstPtsInventoryStdDto
                        {
                            Id = o.Id,
                            Model = o.Model,
                            InventoryStd = o.InventoryStd,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstPtsInventoryStdConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}

