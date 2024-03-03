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
using prod.Master.Painting.BmpPartType.Dto;
using prod.Master.Painting.Dto;
using prod.Master.Painting.Exporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.Painting
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_Painting_BmpPartList)]
    public class MstPtsBmpPartTypeAppService : prodAppServiceBase, IMstPtsBmpPartTypeAppService
    {
        private readonly IDapperRepository<MstPtsBmpPartType, long> _dapperRepo;
        private readonly IRepository<MstPtsBmpPartType, long> _repo;
        private readonly IMstPtsBmpPartTypeExcelExporter _calendarListExcelExporter;

        public IMstPtsBmpPartTypeExcelExporter CalendarListExcelExporter => _calendarListExcelExporter;

        public MstPtsBmpPartTypeAppService(IRepository<MstPtsBmpPartType, long> repo,
                                         IDapperRepository<MstPtsBmpPartType, long> dapperRepo,
                                        IMstPtsBmpPartTypeExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_Painting_BmpPartList_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstPtsBmpPartTypeDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstPtsBmpPartTypeDto input)
        {
            var mainObj = ObjectMapper.Map<MstPtsBmpPartType>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstPtsBmpPartTypeDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_Painting_BmpPartList_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstPtsBmpPartTypeDto>> GetAll(GetMstPtsBmpPartTypeInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartType), e => e.PartType.Contains(input.PartType))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartTypeName), e => e.PartTypeName.Contains(input.PartTypeName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstPtsBmpPartTypeDto
                         {
                             Id = o.Id,
                             PartType = o.PartType,
                             PartTypeName = o.PartTypeName,
                             ProdLine = o.ProdLine,
                             Sorting = o.Sorting,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstPtsBmpPartTypeDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetBmpPartTypeToExcel(MstPtsBmpPartTypeExportInput input)
        {
            var query = from o in _repo.GetAll()
                        select new MstPtsBmpPartTypeDto
                        {
                            Id = o.Id,
                            PartType = o.PartType,
                            PartTypeName = o.PartTypeName,
                            ProdLine = o.ProdLine,
                            Sorting = o.Sorting,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return CalendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstPtsBmpPartTypeConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
