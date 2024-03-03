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
using prod.Master.Plm.Dto;
using prod.Master.Plm.Exporting;
using prod.LogA.Bp2.Dto;

namespace prod.Master.Plm
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_Plm_Matrix)]
    public class MasterPlmMatrixAppService : prodAppServiceBase, IMasterPlmMatrixAppService
    {
        private readonly IDapperRepository<MasterPlmMatrix, long> _dapperRepo;
        private readonly IRepository<MasterPlmMatrix, long> _repo;
        private readonly IMasterPlmMatrixExcelExporter _calendarListExcelExporter;

        public MasterPlmMatrixAppService(IRepository<MasterPlmMatrix, long> repo,
                                         IDapperRepository<MasterPlmMatrix, long> dapperRepo,
                                        IMasterPlmMatrixExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_Plm_Matrix_Edit)]
        public async Task CreateOrEdit(CreateOrEditMasterPlmMatrixDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMasterPlmMatrixDto input)
        {
            var mainObj = ObjectMapper.Map<MasterPlmMatrix>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMasterPlmMatrixDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_Plm_Matrix_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MasterPlmMatrixDto>> GetAll()
        {
            var filtered = _repo.GetAll();
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MasterPlmMatrixDto
                         {
                             Id = o.Id,
                             ScreenId = o.ScreenId,
                             PartId = o.PartId,
                             Ordering = o.Ordering,
                             SideId = o.SideId,
                         };
            var totalCount = (await filtered.ToListAsync()).Count;

            return new PagedResultDto<MasterPlmMatrixDto>(
                totalCount,
                await system.ToListAsync()
            );
        }


        public async Task<FileDto> GetMatrixToExcel()
        {
            var query = from o in _repo.GetAll()
                        select new MasterPlmMatrixDto
                        {
                            Id = o.Id,
                            ScreenId = o.ScreenId,
                            PartId = o.PartId,
                            Ordering = o.Ordering,
                            SideId = o.SideId,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
