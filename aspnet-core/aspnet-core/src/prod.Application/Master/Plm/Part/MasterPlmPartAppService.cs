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
using prod.Master.Plm.Dto;
using prod.Master.Plm.Exporting;
using prod.Master.Plm.Dto;
using prod.Master.Plm;
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
    //  [AbpAuthorize(AppPermissions.Pages_Master_Plm_Part)]
    public class MasterPlmPartAppService : prodAppServiceBase, IMasterPlmPartAppService
    {
        private readonly IDapperRepository<MasterPlmPart, long> _dapperRepo;
        private readonly IRepository<MasterPlmPart, long> _repo;
        private readonly IMasterPlmPartExcelExporter _calendarListExcelExporter;

        public MasterPlmPartAppService(IRepository<MasterPlmPart, long> repo,
                                         IDapperRepository<MasterPlmPart, long> dapperRepo,
                                        IMasterPlmPartExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_Plm_Part_Edit)]
        public async Task CreateOrEdit(CreateOrEditMasterPlmPartDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMasterPlmPartDto input)
        {
            var mainObj = ObjectMapper.Map<MasterPlmPart>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMasterPlmPartDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_Plm_Part_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MasterPlmPartDto>> GetAll(GetMasterPlmPartInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartName), e => e.PartName.Contains(input.PartName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartCd), e => e.PartCd.Contains(input.PartCd))

                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MasterPlmPartDto
                         {
                             Id = o.Id,
                             PartName = o.PartName,
                             PartCd = o.PartCd,
                             R = o.R,
                             G = o.G,
                             B = o.B,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MasterPlmPartDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetPartToExcel()
        {
            var query = from o in _repo.GetAll()
                        select new MasterPlmPartDto
                        {
                            Id = o.Id,
                            PartName = o.PartName,
                            PartCd = o.PartCd,
                            R = o.R,
                            G = o.G,
                            B = o.B,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MasterPlmPartConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}