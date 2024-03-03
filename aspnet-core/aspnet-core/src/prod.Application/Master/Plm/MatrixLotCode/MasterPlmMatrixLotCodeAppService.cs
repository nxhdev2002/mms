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


namespace prod.Master.Plm
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_Plm_MatrixLotCode)]
    public class MasterPlmMatrixLotCodeAppService : prodAppServiceBase, IMasterPlmMatrixLotCodeAppService
    {
        private readonly IDapperRepository<MasterPlmMatrixLotCode, long> _dapperRepo;
        private readonly IRepository<MasterPlmMatrixLotCode, long> _repo;
        private readonly IMasterPlmMatrixLotCodeExcelExporter _calendarListExcelExporter;

        public MasterPlmMatrixLotCodeAppService(IRepository<MasterPlmMatrixLotCode, long> repo,
                                         IDapperRepository<MasterPlmMatrixLotCode, long> dapperRepo,
                                        IMasterPlmMatrixLotCodeExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_Plm_MatrixLotCode_Edit)]
        public async Task CreateOrEdit(CreateOrEditMasterPlmMatrixLotCodeDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMasterPlmMatrixLotCodeDto input)
        {
            var mainObj = ObjectMapper.Map<MasterPlmMatrixLotCode>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMasterPlmMatrixLotCodeDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_Plm_MatrixLotCode_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MasterPlmMatrixLotCodeDto>> GetAll()
        {
            var filtered = _repo.GetAll()

                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MasterPlmMatrixLotCodeDto
                         {
                             Id = o.Id,
                             ScreenId = o.ScreenId,
                             LotcodeGradeId = o.LotcodeGradeId,
                             PartId = o.PartId,
                         };
            var totalCount = filtered.ToList().Count;

            
            return new PagedResultDto<MasterPlmMatrixLotCodeDto>(
                totalCount,
                system.ToList()
            );
        }


        public async Task<FileDto> GetMatrixLotCodeToExcel()
        {
            var query = from o in _repo.GetAll()
                        select new MasterPlmMatrixLotCodeDto
                        {
                            Id = o.Id,
                            ScreenId = o.ScreenId,
                            LotcodeGradeId = o.LotcodeGradeId,
                            PartId = o.PartId,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        

    }
}
